using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra.viewCauHoi
{
    public partial class pageKetQuaKiemTraChiTiet : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Kết quả kiểm tra chi tiết";
            if (!IsPostBack)
            {
                Page.Title = "Kết quả kiểm tra chi tiết";
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                // Lấy KetQuaKiemTraId từ URL
                string ketQuaKiemTraId = RouteData.Values["Id"]?.ToString();

                if (string.IsNullOrEmpty(ketQuaKiemTraId))
                {
                    lblMessage.Text = "Thiếu thông tin kết quả kiểm tra!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Tìm KetQuaKiemTra
                var ketQua = db.KetQuaKiemTras
                    .Include("NguoiDung")
                    .Include("BoCauHoi")
                    .FirstOrDefault(k => k.Id == ketQuaKiemTraId);
                if (ketQua == null)
                {
                    lblMessage.Text = "Không tìm thấy kết quả kiểm tra!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra quyền
                string role = GetRole();
                string username = Session["Username"]?.ToString();
                var currentUser = db.NguoiDungs.FirstOrDefault(u => u.username == username);

                if (role != "TruongDoan" && (currentUser == null || currentUser.Id != ketQua.UserId))
                {
                    lblMessage.Text = "Bạn không có quyền xem kết quả này!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Nếu là TruongDoan, kiểm tra xem UserId thuộc kế hoạch
                if (role == "TruongDoan")
                {
                    var keHoach = db.BoCauHoi_KeHoachs
                        .FirstOrDefault(b => b.BoCauHoiId == ketQua.BoCauHoiId);
                    if (keHoach == null || !db.PhanCong_Users.Any(p => p.KeHoachID == keHoach.KeHoachId && p.UserID == ketQua.UserId))
                    {
                        lblMessage.Text = "Người dùng không thuộc kế hoạch của bộ câu hỏi này!";
                        lblMessage.CssClass = "message-label error-message";
                        lblMessage.Visible = true;
                        return;
                    }
                }

                // Cập nhật link quay lại
                hlQuayLai.NavigateUrl = $"~/danh-sach-ket-qua/{ketQua.BoCauHoiId}";

                LoadKetQua(ketQua);
            }
        }

        private void LoadKetQua(KetQuaKiemTra ketQua)
        {
            try
            {
                // Kiểm tra trạng thái bài kiểm tra
                if (ketQua.TrangThai != "DaHoanThanh")
                {
                    lblMessage.Text = "Bài kiểm tra chưa được hoàn thành!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                string boCauHoiId = ketQua.BoCauHoiId;
                string userId = ketQua.UserId;

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

                // Kết hợp dữ liệu
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
                var nguoiDung = ketQua.NguoiDung;
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
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tải kết quả: {ex.Message}\nStackTrace: {ex.StackTrace}");
                lblMessage.Text = $"Lỗi khi tải kết quả: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void rptKetQua_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Không cần logic bổ sung vì giao diện đã xử lý
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