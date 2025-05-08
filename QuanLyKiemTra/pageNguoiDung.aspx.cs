using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageNguoiDung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Trang quản lý Người Dùng";
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                LoadNguoiDungList();
                LoadRoles();
                LoadDonVis();
            }
        }

        private void LoadNguoiDungList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var nguoiDungs = context.NguoiDungs
                        .Include("Roles")
                        .Include("DonVi")
                        .OrderBy(u => u.username)
                        .ToList();
                    gvNguoiDung.DataSource = nguoiDungs;
                    gvNguoiDung.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách người dùng: {ex.Message}");
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var roles = context.Roles.OrderBy(r => r.Ten).ToList();
                    ddlRole.DataSource = roles;
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new ListItem("--- Chọn vai trò ---", ""));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách vai trò: {ex.Message}");
            }
        }

        private void LoadDonVis()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var donVis = context.DonVis.OrderBy(d => d.TenDonVi).ToList();
                    ddlDonVi.DataSource = donVis;
                    ddlDonVi.DataBind();
                    ddlDonVi.Items.Insert(0, new ListItem("--- Chọn đơn vị ---", ""));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách đơn vị: {ex.Message}");
            }
        }

        protected void btnSaveNguoiDung_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            // Kiểm tra dữ liệu (server-side)
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Tên đăng nhập là bắt buộc.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ddlRole.SelectedValue))
            {
                ShowError("Vai trò là bắt buộc.");
                return;
            }

            try
            {
                using (var context = new MyDbContext())
                {
                    NguoiDung nguoiDung;
                    bool isEdit = !string.IsNullOrWhiteSpace(hfNguoiDungId.Value);

                    if (isEdit)
                    {
                        // Chế độ chỉnh sửa
                        nguoiDung = context.NguoiDungs.Find(hfNguoiDungId.Value);
                        if (nguoiDung == null)
                        {
                            ShowError("Người dùng không tồn tại.");
                            return;
                        }

                        // Kiểm tra username trùng (trừ chính người dùng đang sửa)
                        if (context.NguoiDungs.Any(u => u.username == txtUsername.Text && u.Id != hfNguoiDungId.Value))
                        {
                            ShowError("Tên đăng nhập đã tồn tại.");
                            return;
                        }
                    }
                    else
                    {
                        // Chế độ thêm mới
                        if (string.IsNullOrWhiteSpace(txtPassword.Text))
                        {
                            ShowError("Mật khẩu là bắt buộc khi đăng ký.");
                            return;
                        }

                        if (context.NguoiDungs.Any(u => u.username == txtUsername.Text))
                        {
                            ShowError("Tên đăng nhập đã tồn tại.");
                            return;
                        }

                        nguoiDung = new NguoiDung
                        {
                            Id = Guid.NewGuid().ToString(),
                            username = txtUsername.Text,
                            password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                            NgayTao = DateTime.Now
                        };
                        context.NguoiDungs.Add(nguoiDung);
                    }

                    // Cập nhật thông tin người dùng
                    nguoiDung.username = txtUsername.Text;
                    nguoiDung.HoTen = txtHoTen.Text;
                    nguoiDung.Email = txtEmail.Text;
                    nguoiDung.SoDienThoai = txtSoDienThoai.Text;
                    nguoiDung.DiaChi = txtDiaChi.Text;
                    nguoiDung.RoleID = ddlRole.SelectedValue;
                    nguoiDung.DonViID = string.IsNullOrEmpty(ddlDonVi.SelectedValue) ? null : ddlDonVi.SelectedValue;

                    // Cập nhật mật khẩu nếu có (chỉ trong chế độ chỉnh sửa)
                    if (isEdit && !string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        nguoiDung.password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                    }

                    context.SaveChanges();

                    ShowSuccess(isEdit ? "Cập nhật người dùng thành công!" : "Đăng ký tài khoản thành công!");
                    LoadNguoiDungList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu người dùng: {ex.Message}\n{ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ShowError($"Lỗi khi lưu người dùng: {ex.Message}");
            }
        }

        protected void gvNguoiDung_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string nguoiDungId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    if (e.CommandName == "DeleteNguoiDung")
                    {
                        var nguoiDung = context.NguoiDungs.Find(nguoiDungId);
                        if (nguoiDung == null)
                        {
                            ShowError("Người dùng không tồn tại.");
                            return;
                        }

                        // Kiểm tra ràng buộc khóa ngoại
                        if (context.DapAns.Any(d => d.Id == nguoiDungId) ||
                            context.ThongBao_Users.Any(t => t.Id == nguoiDungId) ||
                            context.PhanCong_Users.Any(p => p.Id == nguoiDungId) ||
                            context.GiaiTrinhs.Any(g => g.NguoiYeuCauID == nguoiDungId || g.NguoiGiaiTrinhID == nguoiDungId))
                        {
                            ShowError("Không thể xóa người dùng vì có dữ liệu liên quan.");
                            return;
                        }

                        context.NguoiDungs.Remove(nguoiDung);
                        context.SaveChanges();

                        ShowSuccess("Xóa người dùng thành công!");
                        LoadNguoiDungList();
                    }
                    else if (e.CommandName == "EditNguoiDung")
                    {
                        var nguoiDung = context.NguoiDungs
                            .Include("Roles")
                            .Include("DonVi")
                            .FirstOrDefault(u => u.Id == nguoiDungId);
                        if (nguoiDung != null)
                        {
                            txtUsername.Text = nguoiDung.username;
                            txtPassword.Text = string.Empty;
                            txtHoTen.Text = nguoiDung.HoTen;
                            txtEmail.Text = nguoiDung.Email;
                            txtSoDienThoai.Text = nguoiDung.SoDienThoai;
                            txtDiaChi.Text = nguoiDung.DiaChi;
                            ddlRole.SelectedValue = nguoiDung.RoleID;
                            ddlDonVi.SelectedValue = nguoiDung.DonViID ?? string.Empty;
                            hfNguoiDungId.Value = nguoiDung.Id;

                            ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openAddModal(true);", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xử lý hành động: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "error-message";
            lblMessage.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "success-message";
            lblMessage.Visible = true;
        }
    }
}