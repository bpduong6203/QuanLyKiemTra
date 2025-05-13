using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;

namespace QuanLyKiemTra
{
    public partial class pageCapNhatCaNhan : Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                var user = db.NguoiDungs.FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Gán thông tin vào TextBox
                txtHoTen.Text = user.HoTen;
                txtEmail.Text = user.Email;
                txtSoDienThoai.Text = user.SoDienThoai;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tải thông tin: {ex.Message}\nStackTrace: {ex.StackTrace}");
                lblMessage.Text = $"Lỗi khi tải thông tin: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["Username"]?.ToString();
                var user = db.NguoiDungs.FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Cập nhật thông tin
                user.HoTen = txtHoTen.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.SoDienThoai = txtSoDienThoai.Text.Trim();

                // Kiểm tra dữ liệu
                if (string.IsNullOrEmpty(user.HoTen))
                {
                    lblMessage.Text = "Họ và tên không được để trống!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                if (!string.IsNullOrEmpty(user.Email) && !IsValidEmail(user.Email))
                {
                    lblMessage.Text = "Email không hợp lệ!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                if (!string.IsNullOrEmpty(user.SoDienThoai) && !IsValidPhoneNumber(user.SoDienThoai))
                {
                    lblMessage.Text = "Số điện thoại không hợp lệ!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra Email trùng lặp
                if (!string.IsNullOrEmpty(user.Email) && db.NguoiDungs.Any(u => u.Email == user.Email && u.Id != user.Id))
                {
                    lblMessage.Text = "Email đã được sử dụng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                db.SaveChanges();
                lblMessage.Text = "Cập nhật thông tin thành công!";
                lblMessage.CssClass = "message-label success-message";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật thông tin: {ex.Message}\nStackTrace: {ex.StackTrace}");
                lblMessage.Text = $"Lỗi khi cập nhật thông tin: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["Username"]?.ToString();
                var user = db.NguoiDungs.FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra mật khẩu hiện tại
                string matKhauHienTai = txtMatKhauHienTai.Text.Trim();
                if (!BCrypt.Net.BCrypt.Verify(matKhauHienTai, user.password))
                {
                    lblMessage.Text = "Mật khẩu hiện tại không đúng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra mật khẩu mới
                string matKhauMoi = txtMatKhauMoi.Text.Trim();
                string xacNhanMatKhau = txtXacNhanMatKhau.Text.Trim();

                if (string.IsNullOrEmpty(matKhauMoi) || matKhauMoi.Length < 6)
                {
                    lblMessage.Text = "Mật khẩu mới phải có ít nhất 6 ký tự!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                if (matKhauMoi != xacNhanMatKhau)
                {
                    lblMessage.Text = "Mật khẩu xác nhận không khớp!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Cập nhật mật khẩu
                user.password = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
                db.SaveChanges();

                // Xóa TextBox mật khẩu
                txtMatKhauHienTai.Text = "";
                txtMatKhauMoi.Text = "";
                txtXacNhanMatKhau.Text = "";

                lblMessage.Text = "Đổi mật khẩu thành công!";
                lblMessage.CssClass = "message-label success-message";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi đổi mật khẩu: {ex.Message}\nStackTrace: {ex.StackTrace}");
                lblMessage.Text = $"Lỗi khi đổi mật khẩu: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Kiểm tra định dạng số điện thoại (10-12 chữ số, có thể bắt đầu bằng +)
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?\d{10,12}$");
        }
    }
}