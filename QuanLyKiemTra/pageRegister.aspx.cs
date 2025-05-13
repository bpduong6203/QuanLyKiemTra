using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Đăng Ký Tài Khoản";
            if (!IsPostBack)
            {
                LoadUnits();
            }
        }

        private void LoadUnits()
        {
            using (var context = new MyDbContext())
            {
                var units = context.DonVis.ToList();
                ddlUnit.DataSource = units;
                ddlUnit.DataTextField = "TenDonVi";
                ddlUnit.DataValueField = "Id";
                ddlUnit.DataBind();

                ddlUnit.Items.Insert(0, new ListItem("--- Chọn đơn vị ---", ""));
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowError("Vui lòng điền đầy đủ thông tin bắt buộc.");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowError("Mật khẩu và xác nhận mật khẩu không khớp.");
                return;
            }

            try
            {
                using (var context = new MyDbContext())
                {
                    if (context.NguoiDungs.Any(u => u.username == txtUsername.Text))
                    {
                        ShowError("Tên đăng nhập đã tồn tại.");
                        return;
                    }

                    // Kiểm tra đơn vị (cho phép null)
                    DonVi donVi = null;
                    if (!string.IsNullOrEmpty(ddlUnit.SelectedValue))
                    {
                        donVi = context.DonVis.Find(ddlUnit.SelectedValue);
                        if (donVi == null)
                        {
                            ShowError("Đơn vị không hợp lệ.");
                            return;
                        }
                    }

                    var defaultRole = context.Roles.FirstOrDefault(r => r.Ten == "DonVi");
                    if (defaultRole == null)
                    {
                        ShowError("Vai trò mặc định 'DonVi' không tồn tại.");
                        return;
                    }

                    var user = new NguoiDung
                    {
                        Id = Guid.NewGuid().ToString(),
                        username = txtUsername.Text,
                        password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                        HoTen = txtFullName.Text,
                        SoDienThoai = txtPhone.Text,
                        DiaChi = txtAddress.Text,
                        RoleID = defaultRole.Id,
                        DonViID = donVi?.Id,
                        NgayTao = DateTime.Now
                    };

                    context.NguoiDungs.Add(user);
                    context.SaveChanges();

                    lblMessage.Text = "Đăng ký thành công! Vui lòng đăng nhập.";
                    lblMessage.CssClass = "success-message";
                    lblMessage.Visible = true;

                    Response.AddHeader("REFRESH", "2;URL=pageLogin.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi đăng ký: {ex.Message}");
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