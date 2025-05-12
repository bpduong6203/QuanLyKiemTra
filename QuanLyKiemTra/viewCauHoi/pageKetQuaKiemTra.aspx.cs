using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra.viewCauHoi
{
    public partial class pageKetQuaKiemTra : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Kết quả kiểm tra";
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                string keHoachId = Request.QueryString["KeHoachId"];
                string userId = Request.QueryString["UserId"];

                string role = GetRole();
                if (role == "TruongDoan")
                {
                    LoadFilter(keHoachId);
                    LoadKetQua(boCauHoiId, userId ?? ddlThanhVien.SelectedValue);
                }
                else if (role == "ThanhVien" || role == "DonVi")
                {
                    string username = Session["Username"]?.ToString();
                    var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (nguoiDung != null)
                    {
                        LoadKetQua(boCauHoiId, nguoiDung.Id);
                    }
                    else
                    {
                        lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                        lblMessage.CssClass = "message-label error-message";
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "Bạn không có quyền xem kết quả!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                }
            }
        }

        private void LoadFilter(string keHoachId)
        {
            try
            {
                if (string.IsNullOrEmpty(keHoachId))
                {
                    lblMessage.Text = "Không tìm thấy kế hoạch!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                var thanhVienList = db.PhanCong_Users
                    .Include("NguoiDung")
                    .Where(p => p.KeHoachID == keHoachId)
                    .Select(p => p.NguoiDung)
                    .Distinct()
                    .Select(u => new { u.Id, u.HoTen })
                    .ToList();

                ddlThanhVien.DataSource = thanhVienList;
                ddlThanhVien.DataTextField = "HoTen";
                ddlThanhVien.DataValueField = "Id";
                ddlThanhVien.DataBind();
                ddlThanhVien.Items.Insert(0, new ListItem("-- Chọn thành viên --", ""));
                pnlFilter.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi tải danh sách thành viên: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        private void LoadKetQua(string boCauHoiId, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(boCauHoiId) || string.IsNullOrEmpty(userId))
                {
                    lblMessage.Text = "Thiếu thông tin bộ câu hỏi hoặc người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra trạng thái bài kiểm tra
                var ketQua = db.KetQuaKiemTras
                    .FirstOrDefault(k => k.UserId == userId && k.BoCauHoiId == boCauHoiId);
                if (ketQua == null || ketQua.TrangThai != "DaHoanThanh")
                {
                    lblMessage.Text = "Bài kiểm tra chưa được hoàn thành!";
                    lblMessage.CssClass = "message-label warning-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Lấy danh sách câu hỏi
                var cauHoiList = db.CTBoCauHois
                    .Include("CauHoi")
                    .Where(ct => ct.BoCauHoiId == boCauHoiId)
                    .Select(ct => new
                    {
                        ct.CauHoiId,
                        ct.BoCauHoiId,
                        CauHoi = ct.CauHoi
                    })
                    .ToList();

                // Lấy danh sách đáp án
                var dapAnList = db.DapAns
                    .Where(d => d.UserId == userId && d.BoCauHoiId == boCauHoiId)
                    .ToDictionary(d => d.CauHoiId, d => d);

                // Kết hợp đáp án
                var ketQuaList = cauHoiList
                    .Select(ct => new
                    {
                        ct.CauHoiId,
                        ct.BoCauHoiId,
                        CauHoi = ct.CauHoi,
                        DapAn = dapAnList.ContainsKey(ct.CauHoiId) ? dapAnList[ct.CauHoiId] : null
                    })
                    .ToList();

                // Tính điểm
                int soCauDung = ketQuaList.Count(c => c.DapAn != null && c.DapAn.DapAnTraLoi == c.CauHoi.DapAn);
                int tongSoCau = ketQuaList.Count;
                double diem = tongSoCau > 0 ? (double)soCauDung / tongSoCau * 10 : 0;

                // Hiển thị tóm tắt
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.Id == userId);
                lblSummary.Text = $"Người làm: {nguoiDung?.HoTen ?? "Không xác định"} | " +
                                 $"Số câu đúng: {soCauDung}/{tongSoCau} | " +
                                 $"Điểm: {diem:F2}/10 | " +
                                 $"Thời gian nộp: {ketQua.ThoiGianHoanThanh?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"}";

                rptKetQua.DataSource = ketQuaList;
                rptKetQua.DataBind();
                pnlKetQua.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi tải kết quả: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void rptKetQua_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Không cần thêm logic vì giao diện đã xử lý hiển thị
        }

        protected void ddlThanhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string boCauHoiId = RouteData.Values["Id"]?.ToString();
            LoadKetQua(boCauHoiId, ddlThanhVien.SelectedValue);
        }

        protected string GetRole()
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username)) return "";

            var nguoiDung = db.NguoiDungs
                .Include("Roles")
                .FirstOrDefault(u => u.username == username);

            return nguoiDung?.Roles?.Ten ?? "DonVi";
        }
    }
}