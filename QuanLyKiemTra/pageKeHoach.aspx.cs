using QuanLyKiemTra.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKeHoach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
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
                    ddlDonVi.DataSource = donVis;
                    ddlDonVi.DataTextField = "TenDonVi";
                    ddlDonVi.DataValueField = "Id";
                    ddlDonVi.DataBind();
                    ddlDonVi.Items.Insert(0, new ListItem("-- Chọn đơn vị --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách đơn vị: {ex.Message}");
            }
        }

        protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlDonVi.SelectedValue))
            {
                ShowError("Vui lòng chọn đơn vị kiểm tra.");
            }
            else
            {
                lblMessage.Visible = false; // Ẩn thông báo nếu đã chọn đơn vị
            }
        }

        protected void btnExportQuyetDinh_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrWhiteSpace(txtTenKeHoach.Text) || string.IsNullOrWhiteSpace(txtNgayBatDau.Text) ||
                    string.IsNullOrWhiteSpace(txtNgayKetThuc.Text) || string.IsNullOrEmpty(ddlDonVi.SelectedValue))
                {
                    ShowError("Vui lòng điền đầy đủ tên kế hoạch, ngày bắt đầu, ngày kết thúc và chọn đơn vị.");
                    return;
                }

                if (!fuQuyetDinh.HasFile)
                {
                    ShowError("Vui lòng tải lên file Quyết định kiểm tra.");
                    return;
                }

                // Kiểm tra định dạng ngày
                if (!DateTime.TryParse(txtNgayBatDau.Text, out DateTime ngayBatDau))
                {
                    ShowError("Ngày bắt đầu không hợp lệ.");
                    return;
                }

                if (!DateTime.TryParse(txtNgayKetThuc.Text, out DateTime ngayKetThuc))
                {
                    ShowError("Ngày kết thúc không hợp lệ.");
                    return;
                }

                if (ngayKetThuc < ngayBatDau)
                {
                    ShowError("Ngày kết thúc phải sau ngày bắt đầu.");
                    return;
                }

                // Kiểm tra định dạng file
                string[] allowedExtensions = { ".doc", ".docx", ".pdf" };
                string fileExtension = Path.GetExtension(fuQuyetDinh.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ShowError("Chỉ chấp nhận file .doc, .docx hoặc .pdf.");
                    return;
                }

                using (var context = new MyDbContext())
                {
                    // Kiểm tra người dùng
                    string username = Session["Username"]?.ToString();
                    var user = context.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (user == null)
                    {
                        ShowError("Không tìm thấy người dùng hiện tại. Vui lòng đăng nhập lại.");
                        return;
                    }

                    // Kiểm tra đơn vị
                    string donViId = ddlDonVi.SelectedValue;
                    if (!context.DonVis.Any(d => d.Id == donViId))
                    {
                        ShowError("Đơn vị được chọn không hợp lệ.");
                        return;
                    }

                    // Tạo kế hoạch
                    var keHoach = new KeHoach
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenKeHoach = txtTenKeHoach.Text.Trim(),
                        UserId = user.Id,
                        DonViID = donViId,
                        NgayBatDau = ngayBatDau,
                        NgayKetThuc = ngayKetThuc,
                        GhiChu = txtGhiChu.Text.Trim()
                    };

                    // Lưu file quyết định
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    fuQuyetDinh.SaveAs(filePath);

                    // Tạo biên bản kiểm tra
                    var bienBan = new BienBanKiemTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        tenBBKT = $"Quyết định kiểm tra cho {ddlDonVi.SelectedItem.Text}",
                        linkfile = "~/Uploads/" + fileName,
                        NgayTao = DateTime.Now
                    };
                    keHoach.BienBanID = bienBan.Id;

                    // Lưu vào database
                    context.KeHoachs.Add(keHoach);
                    context.BienBanKiemTras.Add(bienBan);

                    // Gửi thông báo đến người dùng trong đơn vị
                    var users = context.NguoiDungs.Where(u => u.DonViID == donViId).ToList();
                    foreach (var nguoiDung in users)
                    {
                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserID = nguoiDung.Id, // Sửa từ user.Id thành nguoiDung.Id để gửi đúng người nhận
                            KeHoachID = keHoach.Id,
                            NoiDung = $"Quyết định kiểm tra: {keHoach.TenKeHoach}.",
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        context.ThongBao_Users.Add(thongBao);
                    }

                    context.SaveChanges();
                    ShowSuccess("Kế hoạch đã được lưu và thông báo đã được gửi thành công!");

                    // Reset form sau khi lưu thành công
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu kế hoạch: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu kế hoạch: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            txtTenKeHoach.Text = "";
            txtNgayBatDau.Text = "";
            txtNgayKetThuc.Text = "";
            txtGhiChu.Text = "";
            ddlDonVi.SelectedIndex = 0;
            lblMessage.Visible = false;
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