using QuanLyKiemTra.Models;
using System;
using System.Linq;

namespace QuanLyKiemTra
{
    public partial class pageLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Xóa thông báo lỗi (nếu có)
                lblMessage.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nhập tên đăng nhập và mật khẩu.");
                return;
            }

            try
            {
                using (var context = new MyDbContext())
                {
                    // Tìm người dùng theo username
                    var user = context.NguoiDungs
                        .FirstOrDefault(u => u.username == txtUsername.Text);

                    if (user == null)
                    {
                        ShowError("Tên đăng nhập không tồn tại.");
                        return;
                    }

                    // Kiểm tra mật khẩu
                    if (!BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.password))
                    {
                        ShowError("Mật khẩu không đúng.");
                        return;
                    }

                    // Lưu thông tin người dùng vào Session
                    Session["UserID"] = user.Id;
                    Session["Username"] = user.username;
                    Session["FullName"] = user.HoTen;
                    Session["RoleID"] = user.RoleID;

                    // (Tùy chọn) Lấy tên vai trò
                    var role = context.Roles.FirstOrDefault(r => r.Id == user.RoleID);
                    Session["RoleName"] = role?.Ten ?? "Unknown";

                    // (Tùy chọn) Lấy tên đơn vị (nếu có)
                    if (!string.IsNullOrEmpty(user.DonViID))
                    {
                        var donVi = context.DonVis.FirstOrDefault(d => d.Id == user.DonViID);
                        Session["DonViName"] = donVi?.TenDonVi;
                    }

                    // Chuyển hướng dựa trên vai trò
                    switch (role?.Ten)
                    {
                        case "TruongDoan":
                            Response.Redirect("TruongDoanDashboard.aspx"); // Trang dành cho Trưởng đoàn
                            break;
                        case "ThanhVien":
                            Response.Redirect("ThanhVienDashboard.aspx"); // Trang dành cho Thành viên
                            break;
                        case "DonVi":
                            Response.Redirect("pageCaNhan.aspx"); // Trang dành cho Đơn vị
                            break;
                        default:
                            Response.Redirect("Default.aspx"); // Trang mặc định
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ShowError($"Đã xảy ra lỗi: {ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : "")}");
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "error-message";
            lblMessage.Visible = true;
        }
    }
}