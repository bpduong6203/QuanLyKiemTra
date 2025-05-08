using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageDonVi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Trang quản lý Đơn Vị";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                LoadDonViList();
            }
        }

        private void LoadDonViList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var donVis = context.DonVis.OrderBy(d => d.TenDonVi).ToList();
                    gvDonVi.DataSource = donVis;
                    gvDonVi.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách đơn vị: {ex.Message}");
            }
        }

        protected void btnSaveDonVi_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (string.IsNullOrWhiteSpace(txtTenDonVi.Text))
            {
                ShowError("Tên đơn vị là bắt buộc.");
                return;
            }

            try
            {
                using (var context = new MyDbContext())
                {
                    DonVi donVi;
                    bool isEdit = !string.IsNullOrWhiteSpace(hfDonViId.Value);

                    if (isEdit)
                    {
                        // Chế độ chỉnh sửa
                        donVi = context.DonVis.Find(hfDonViId.Value);
                        if (donVi == null)
                        {
                            ShowError("Đơn vị không tồn tại.");
                            return;
                        }
                    }
                    else
                    {
                        // Chế độ thêm mới
                        donVi = new DonVi
                        {
                            Id = Guid.NewGuid().ToString(),
                            NguoiTao = Session["Username"]?.ToString() ?? "System",
                            NgayTao = DateTime.Now
                        };
                        context.DonVis.Add(donVi);
                    }

                    // Cập nhật thông tin đơn vị
                    donVi.TenDonVi = txtTenDonVi.Text;
                    donVi.DiaChi = txtDiaChi.Text;
                    donVi.SoDienThoai = txtSoDienThoai.Text;
                    donVi.Email = txtEmail.Text;
                    donVi.NguoiDaiDien = txtNguoiDaiDien.Text;
                    donVi.ChucVuNguoiDaiDien = txtChucVuNguoiDaiDien.Text;

                    context.SaveChanges();

                    ShowSuccess(isEdit ? "Cập nhật đơn vị thành công!" : "Thêm đơn vị thành công!");
                    LoadDonViList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu đơn vị: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu đơn vị: {ex.Message}");
            }
        }

        protected void gvDonVi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string donViId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    if (e.CommandName == "DeleteDonVi")
                    {
                        var donVi = context.DonVis.Find(donViId);
                        if (donVi == null)
                        {
                            ShowError("Đơn vị không tồn tại.");
                            return;
                        }

                        // Kiểm tra xem đơn vị có được sử dụng trong NguoiDungs không
                        if (context.NguoiDungs.Any(u => u.DonViID == donViId))
                        {
                            ShowError("Không thể xóa đơn vị vì đã có người dùng liên kết.");
                            return;
                        }

                        context.DonVis.Remove(donVi);
                        context.SaveChanges();

                        ShowSuccess("Xóa đơn vị thành công!");
                        LoadDonViList();
                    }
                    else if (e.CommandName == "EditDonVi")
                    {
                        var donVi = context.DonVis.Find(donViId);
                        if (donVi != null)
                        {
                            txtTenDonVi.Text = donVi.TenDonVi;
                            txtDiaChi.Text = donVi.DiaChi;
                            txtSoDienThoai.Text = donVi.SoDienThoai;
                            txtEmail.Text = donVi.Email;
                            txtNguoiDaiDien.Text = donVi.NguoiDaiDien;
                            txtChucVuNguoiDaiDien.Text = donVi.ChucVuNguoiDaiDien;
                            hfDonViId.Value = donVi.Id;

                            // Mở modal với chế độ chỉnh sửa
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