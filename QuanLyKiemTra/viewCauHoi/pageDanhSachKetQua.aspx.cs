using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra.viewCauHoi
{
    public partial class pageDanhSachKetQua : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Danh sách kết quả kiểm tra";
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                // Kiểm tra quyền
                string role = GetRole();
                if (role != "TruongDoan" && role != "ThanhVien")
                {
                    lblMessage.Text = "Bạn không có quyền xem danh sách kết quả!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Lấy BoCauHoiId từ route
                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                if (string.IsNullOrEmpty(boCauHoiId))
                {
                    lblMessage.Text = "Không tìm thấy bộ câu hỏi!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                LoadKetQua(boCauHoiId);
            }
        }

        private void LoadKetQua(string boCauHoiId)
        {
            try
            {
                string role = GetRole();
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);

                // Lấy danh sách kết quả
                var ketQuaList = db.KetQuaKiemTras
                    .Include("NguoiDung")
                    .Where(k => k.BoCauHoiId == boCauHoiId)
                    .ToList();

                // Lọc theo vai trò
                if (role == "ThanhVien" && nguoiDung != null)
                {
                    ketQuaList = ketQuaList
                        .Where(k => k.UserId == nguoiDung.Id)
                        .ToList();
                }

                // Tính điểm và tạo danh sách hiển thị
                var displayList = ketQuaList
                    .Select(k => new
                    {
                        KetQuaKiemTraId = k.Id,
                        k.UserId,
                        k.BoCauHoiId,
                        HoTen = k.NguoiDung?.HoTen ?? "Không xác định",
                        ThoiGianNop = k.ThoiGianHoanThanh,
                        TrangThai = k.TrangThai,
                        SoCauDung = db.DapAns
                            .Count(d => d.UserId == k.UserId && d.BoCauHoiId == boCauHoiId &&
                                        d.DapAnTraLoi == db.CauHois
                                            .Join(db.CTBoCauHois,
                                                  c => c.Id,
                                                  ct => ct.CauHoiId,
                                                  (c, ct) => new { CauHoi = c, ct.BoCauHoiId })
                                            .Where(ct => ct.BoCauHoiId == boCauHoiId)
                                            .Select(ct => ct.CauHoi)
                                            .FirstOrDefault(c => c.Id == d.CauHoiId).DapAn),
                        TongSoCau = db.CTBoCauHois.Count(ct => ct.BoCauHoiId == boCauHoiId),
                    })
                    .Select(k => new
                    {
                        k.KetQuaKiemTraId,
                        k.UserId,
                        k.BoCauHoiId,
                        k.HoTen,
                        k.ThoiGianNop,
                        k.TrangThai,
                        k.SoCauDung,
                        k.TongSoCau,
                        Diem = k.TongSoCau > 0 ? (double)k.SoCauDung / k.TongSoCau * 10 : 0
                    })
                    .ToList();

                gvKetQua.DataSource = displayList;
                gvKetQua.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi tải danh sách kết quả: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void gvKetQua_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LamLai")
            {
                try
                {
                    string[] args = e.CommandArgument.ToString().Split(',');
                    string userId = args[0];
                    string boCauHoiId = args[1];

                    // Kiểm tra quyền
                    if (GetRole() != "TruongDoan")
                    {
                        lblMessage.Text = "Bạn không có quyền thực hiện thao tác này!";
                        lblMessage.CssClass = "message-label error-message";
                        lblMessage.Visible = true;
                        return;
                    }

                    // Tìm kết quả kiểm tra
                    var ketQua = db.KetQuaKiemTras
                        .FirstOrDefault(k => k.UserId == userId && k.BoCauHoiId == boCauHoiId);
                    if (ketQua == null)
                    {
                        lblMessage.Text = "Không tìm thấy kết quả bài kiểm tra!";
                        lblMessage.CssClass = "message-label error-message";
                        lblMessage.Visible = true;
                        return;
                    }

                    // Xóa đáp án
                    var dapAns = db.DapAns
                        .Where(d => d.UserId == userId && d.BoCauHoiId == boCauHoiId)
                        .ToList();
                    db.DapAns.RemoveRange(dapAns);

                    // Cập nhật trạng thái
                    ketQua.TrangThai = "CanLamLai";
                    ketQua.ThoiGianHoanThanh = null;
                    db.SaveChanges();

                    lblMessage.Text = "Đã cho phép người dùng làm lại bài kiểm tra!";
                    lblMessage.CssClass = "message-label success-message";
                    lblMessage.Visible = true;

                    LoadKetQua(boCauHoiId);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Lỗi khi xử lý: {ex.Message}";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                }
            }
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