using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;

namespace QuanLyKiemTra
{
    public partial class pageCaNhan : Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Thông tin cá nhân - Quản lý kiểm tra";
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                // Tải thông tin người dùng
                LoadUserInfo();
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                string username = Session["Username"]?.ToString();
                var user = db.NguoiDungs
                    .Include("DonVi")
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.Visible = true;
                    return;
                }

                // Gán thông tin vào Label
                lblHoTen.Text = $"Họ và tên: {user.HoTen ?? "Chưa cập nhật"}";
                lblSoDienThoai.Text = $"Số điện thoại: {user.SoDienThoai ?? "Chưa cập nhật"}";
                lblDiaChi.Text = $"Địa chỉ: {user.DiaChi ?? "Chưa cập nhật"}";
                lblDonVi.Text = $"Đơn vị: {user.DonVi?.TenDonVi ?? "Chưa cập nhật"}";
                lblChucVu.Text = $"Chức vụ: {user.Roles?.Ten ?? "Chưa cập nhật"}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tải thông tin: {ex.Message}\nStackTrace: {ex.StackTrace}");
                lblMessage.Text = $"Lỗi khi tải thông tin: {ex.Message}";
                lblMessage.Visible = true;
            }
        }
    }
}