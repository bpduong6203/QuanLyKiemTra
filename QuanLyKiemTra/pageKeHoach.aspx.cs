using QuanLyKiemTra.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
                LoadThanhVienList();
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

        private void LoadThanhVienList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var thanhViens = context.NguoiDungs
                        .Where(u => u.RoleID == "ThanhVien")
                        .Select(u => new
                        {
                            u.Id,
                            u.HoTen,
                            ChucVu = u.Roles != null ? u.Roles.Ten : "ThanhVien"
                        })
                        .OrderBy(u => u.HoTen)
                        .ToList();
                    gvThanhVien.DataSource = thanhViens;
                    gvThanhVien.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách thành viên ThanhVien: {ex.Message}");
            }
        }

        protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ kiểm tra đơn vị được chọn, không load lại danh sách ThanhVien
            if (string.IsNullOrEmpty(ddlDonVi.SelectedValue))
            {
                ShowError("Vui lòng chọn đơn vị kiểm tra.");
            }
        }

        protected void btnExportQuyetDinh_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fuQuyetDinh.HasFile)
                {
                    ShowError("Vui lòng tải lên file Quyết định kiểm tra.");
                    return;
                }

                string donViId = ddlDonVi.SelectedValue;
                if (string.IsNullOrEmpty(donViId))
                {
                    ShowError("Vui lòng chọn đơn vị kiểm tra.");
                    return;
                }

                // Kiểm tra thông tin kế hoạch
                if (string.IsNullOrWhiteSpace(txtTenKeHoach.Text) || string.IsNullOrWhiteSpace(txtNgayBatDau.Text) ||
                    string.IsNullOrWhiteSpace(txtNgayKetThuc.Text))
                {
                    ShowError("Vui lòng điền đầy đủ tên kế hoạch, ngày bắt đầu và ngày kết thúc.");
                    return;
                }

                DateTime ngayBatDau;
                if (!DateTime.TryParse(txtNgayBatDau.Text, out ngayBatDau))
                {
                    ShowError("Ngày bắt đầu không hợp lệ.");
                    return;
                }

                DateTime ngayKetThuc;
                if (!DateTime.TryParse(txtNgayKetThuc.Text, out ngayKetThuc))
                {
                    ShowError("Ngày kết thúc không hợp lệ.");
                    return;
                }

                if (ngayKetThuc < ngayBatDau)
                {
                    ShowError("Ngày kết thúc phải sau ngày bắt đầu.");
                    return;
                }

                using (var context = new MyDbContext())
                {
                    // Kiểm tra UserId
                    string username = Session["Username"]?.ToString();
                    var user = context.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (user == null)
                    {
                        ShowError("Không tìm thấy người dùng hiện tại. Vui lòng đăng nhập lại.");
                        return;
                    }

                    // Kiểm tra DonViID
                    if (!context.DonVis.Any(d => d.Id == donViId))
                    {
                        ShowError("Đơn vị được chọn không hợp lệ.");
                        return;
                    }

                    // Tạo KeHoach
                    var keHoach = new KeHoach
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenKeHoach = txtTenKeHoach.Text,
                        UserId = user.Id,
                        DonViID = donViId,
                        NgayBatDau = ngayBatDau,
                        NgayKetThuc = ngayKetThuc,
                        GhiChu = txtGhiChu.Text
                    };

                    // Lưu file Quyết định
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuQuyetDinh.FileName);
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    fuQuyetDinh.SaveAs(filePath);

                    // Tạo BienBanKiemTra
                    var bienBan = new BienBanKiemTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        tenBBKT = $"Quyết định kiểm tra cho {ddlDonVi.SelectedItem.Text}",
                        linkfile = "~/Uploads/" + fileName,
                        NgayTao = DateTime.Now
                    };
                    keHoach.BienBanID = bienBan.Id;

                    // Lưu KeHoach và BienBanKiemTra
                    context.KeHoachs.Add(keHoach);
                    context.BienBanKiemTras.Add(bienBan);
                    context.SaveChanges();

                    // Gửi thông báo đến người dùng của đơn vị
                    var users = context.NguoiDungs.Where(u => u.DonViID == donViId).ToList();
                    foreach (var nguoiDung in users)
                    {
                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserID = user.Id,
                            KeHoachID = keHoach.Id,
                            NoiDung = $"Quyết định kiểm tra: {keHoach.TenKeHoach}.",
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        context.ThongBao_Users.Add(thongBao);
                    }

                    context.SaveChanges();
                    ShowSuccess("Xuất Quyết định, lưu kế hoạch và gửi thông báo thành công!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xuất Quyết định: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi xuất Quyết định: {ex.Message}");
            }
        }
        protected void btnExportPhanCong_Click(object sender, EventArgs e)
        {
            try
            {
                string donViId = ddlDonVi.SelectedValue;
                if (string.IsNullOrEmpty(donViId))
                {
                    ShowError("Vui lòng chọn đơn vị kiểm tra.");
                    return;
                }

                // Thu thập thành viên được chọn
                var selectedMembers = new List<string>();
                foreach (GridViewRow row in gvThanhVien.Rows)
                {
                    var chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        var txtNhiemVu = row.FindControl("txtNhiemVu") as TextBox;
                        string nhiemVu = txtNhiemVu.Text;
                        string userId = gvThanhVien.DataKeys[row.RowIndex]["Id"].ToString();
                        selectedMembers.Add($"Họ tên: {row.Cells[0].Text}, Chức vụ: {row.Cells[1].Text}, Nhiệm vụ: {nhiemVu}");
                    }
                }

                if (!selectedMembers.Any())
                {
                    ShowError("Vui lòng chọn ít nhất một thành viên ThanhVien.");
                    return;
                }

                // Tạo file văn bản phân công
                string fileName = $"PhanCong_{DateTime.Now:yyyyMMddHHmmss}.txt";
                string filePath = Server.MapPath("~/Uploads/") + fileName;
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("VĂN BẢN PHÂN CÔNG");
                    writer.WriteLine($"Đơn vị kiểm tra: {ddlDonVi.SelectedItem.Text}");
                    writer.WriteLine("Danh sách thành viên ThanhVien:");
                    foreach (var member in selectedMembers)
                    {
                        writer.WriteLine(member);
                    }
                }

                // Gửi file cho client
                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                Response.TransmitFile(filePath);
                Response.End();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xuất phân công: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi xuất phân công: {ex.Message}");
            }
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu
                if (string.IsNullOrWhiteSpace(txtTenKeHoach.Text) || string.IsNullOrWhiteSpace(ddlDonVi.SelectedValue) ||
                    string.IsNullOrWhiteSpace(txtNgayBatDau.Text) || string.IsNullOrWhiteSpace(txtNgayKetThuc.Text))
                {
                    ShowError("Vui lòng điền đầy đủ thông tin bắt buộc.");
                    return;
                }

                DateTime ngayBatDau = DateTime.Parse(txtNgayBatDau.Text);
                DateTime ngayKetThuc = DateTime.Parse(txtNgayKetThuc.Text);
                if (ngayKetThuc < ngayBatDau)
                {
                    ShowError("Ngày kết thúc phải sau ngày bắt đầu.");
                    return;
                }

                using (var context = new MyDbContext())
                {
                    // Tạo KeHoach
                    var keHoach = new KeHoach
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenKeHoach = txtTenKeHoach.Text,
                        UserId = context.NguoiDungs.FirstOrDefault(u => u.username == Session["Username"].ToString())?.Id,
                        DonViID = ddlDonVi.SelectedValue,
                        NgayBatDau = ngayBatDau,
                        NgayKetThuc = ngayKetThuc,
                        GhiChu = txtGhiChu.Text
                    };

                    // Lưu file Quyết định (nếu có)
                    string bienBanId = null;
                    if (fuQuyetDinh.HasFile)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuQuyetDinh.FileName);
                        string filePath = Server.MapPath("~/Uploads/") + fileName;
                        fuQuyetDinh.SaveAs(filePath);

                        var bienBan = new BienBanKiemTra
                        {
                            Id = Guid.NewGuid().ToString(),
                            tenBBKT = $"Quyết định kiểm tra cho {txtTenKeHoach.Text}",
                            linkfile = "~/Uploads/" + fileName,
                            NgayTao = DateTime.Now
                        };
                        context.BienBanKiemTras.Add(bienBan);
                        bienBanId = bienBan.Id;
                        keHoach.BienBanID = bienBanId;
                    }

                    // Lưu file Đề cương (nếu có)
                    if (fuDeCuong.HasFile)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuDeCuong.FileName);
                        string filePath = Server.MapPath("~/Uploads/") + fileName;
                        fuDeCuong.SaveAs(filePath);
                        // Có thể lưu vào CTTaiLieu_KeHoach nếu cần
                    }

                    // Lưu file Tài liệu (nếu có)
                    if (fuTaiLieu.HasFiles)
                    {
                        foreach (HttpPostedFile file in fuTaiLieu.PostedFiles)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string filePath = Server.MapPath("~/Uploads/") + fileName;
                            file.SaveAs(filePath);
                            // Có thể lưu vào CTTaiLieu_KeHoach nếu cần
                        }
                    }

                    // Lưu phân công thành viên ThanhVien
                    var phanCongs = new List<PhanCong_User>();
                    foreach (GridViewRow row in gvThanhVien.Rows)
                    {
                        var chkSelect = row.FindControl("chkSelect") as CheckBox;
                        if (chkSelect != null && chkSelect.Checked)
                        {
                            var txtNhiemVu = row.FindControl("txtNhiemVu") as TextBox;
                            string userId = gvThanhVien.DataKeys[row.RowIndex]["Id"].ToString();
                            phanCongs.Add(new PhanCong_User
                            {
                                Id = Guid.NewGuid().ToString(),
                                KeHoachID = keHoach.Id,
                                UserID = userId,
                                NoiDungCV = txtNhiemVu.Text,
                                ngayTao = DateTime.Now
                            });
                        }
                    }

                    // Lưu vào database
                    context.KeHoachs.Add(keHoach);
                    context.PhanCong_Users.AddRange(phanCongs);

                    // Cập nhật KeHoachID cho ThongBao_User
                    var thongBaos = context.ThongBao_Users.Where(t => t.KeHoachID == null && t.NguoiDung.DonViID == keHoach.DonViID).ToList();
                    foreach (var thongBao in thongBaos)
                    {
                        thongBao.KeHoachID = keHoach.Id;
                    }

                    context.SaveChanges();
                    ShowSuccess("Lưu kế hoạch thành công!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu kế hoạch: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu kế hoạch: {ex.Message}");
            }
        }

        protected void gvThanhVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Có thể thêm logic nếu cần (ví dụ: xóa thành viên)
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