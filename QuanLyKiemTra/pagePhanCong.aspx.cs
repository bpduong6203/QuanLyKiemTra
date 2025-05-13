using QuanLyKiemTra.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pagePhanCong : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Phân Công Kế Hoạch Kiểm Tra";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }
                LoadKeHoach();
                LoadPhanCong();
                LoadDocuments();
            }
        }

        private void LoadKeHoach()
        {
            try
            {
                var keHoachList = db.KeHoachs.OrderBy(k => k.TenKeHoach).ToList();
                rptKeHoach.DataSource = keHoachList;
                rptKeHoach.DataBind();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách kế hoạch: {ex.Message}");
            }
        }

        private void LoadDocuments()
        {
            try
            {
                if (!string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    var documents = db.CTTaiLieu_KeHoachs
                        .Where(ct => ct.KeHoachId == hfSelectedKeHoach.Value)
                        .Select(ct => new
                        {
                            ct.TaiLieu.Id,
                            ct.TaiLieu.TenTaiLieu,
                            ct.TaiLieu.linkfile,
                            ct.TaiLieu.LoaiTaiLieu
                        })
                        .OrderBy(d => d.LoaiTaiLieu == "DeCuong" ? 0 : 1)
                        .ThenBy(d => d.TenTaiLieu)
                        .ToList();
                    rptDocuments.DataSource = documents;
                    rptDocuments.DataBind();
                }
                else
                {
                    rptDocuments.DataSource = null;
                    rptDocuments.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách tài liệu: {ex.Message}");
            }
        }

        protected string GetLoaiTaiLieuText(object loaiTaiLieu)
        {
            string loai = loaiTaiLieu?.ToString();
            return loai == "DeCuong" ? "Đề cương" : "Tài liệu";
        }

        protected string GetLoaiTaiLieuIcon(object loaiTaiLieu)
        {
            string loai = loaiTaiLieu?.ToString();
            return loai == "DeCuong" ? "fas fa-book status-danger" : "fas fa-file-alt status-warning";
        }

        protected void rptKeHoach_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeHoach keHoach = (KeHoach)e.Item.DataItem;
                CheckBox chkKeHoach = (CheckBox)e.Item.FindControl("chkKeHoach");
                chkKeHoach.Checked = hfSelectedKeHoach.Value == keHoach.Id;
            }
        }

        protected void chkKeHoach_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                hfSelectedKeHoach.Value = chk.Attributes["data-id"];
            }
            else if (hfSelectedKeHoach.Value == chk.Attributes["data-id"])
            {
                hfSelectedKeHoach.Value = "";
            }

            LoadKeHoach();
            LoadPhanCong();
            LoadDocuments();
        }

        private void LoadPhanCong()
        {
            try
            {
                if (!string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    var thanhVienRole = db.Roles.FirstOrDefault(r => r.Ten == "ThanhVien");
                    if (thanhVienRole == null)
                    {
                        ShowError("Không tìm thấy vai trò ThanhVien!");
                        gvPhanCong.DataSource = null;
                        gvPhanCong.DataBind();
                        return;
                    }

                    var thanhVienList = from u in db.NguoiDungs
                                        where u.RoleID == thanhVienRole.Id
                                        join p in db.PhanCong_Users
                                        on new { UserID = u.Id, KeHoachID = hfSelectedKeHoach.Value }
                                        equals new { p.UserID, p.KeHoachID } into phanCong
                                        from p in phanCong.DefaultIfEmpty()
                                        select new
                                        {
                                            UserId = u.Id,
                                            HoTen = u.HoTen,
                                            ChucVu = u.RoleID != null ? db.Roles.FirstOrDefault(r => r.Id == u.RoleID).Ten : "Không có vai trò",
                                            NoiDungCV = p != null ? p.NoiDungCV : null,
                                            LinkFile = p != null ? p.linkfile : null,
                                            IsAssigned = p != null
                                        };

                    var result = thanhVienList.OrderBy(u => u.HoTen).ToList();
                    gvPhanCong.DataSource = result;
                    gvPhanCong.DataBind();
                }
                else
                {
                    gvPhanCong.DataSource = null;
                    gvPhanCong.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách phân công: {ex.Message}");
            }
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    ShowError("Vui lòng chọn kế hoạch!");
                    return;
                }

                var keHoach = db.KeHoachs.FirstOrDefault(k => k.Id == hfSelectedKeHoach.Value);
                if (keHoach == null)
                {
                    ShowError("Kế hoạch không tồn tại!");
                    return;
                }

                // Đường dẫn thư mục Uploads
                string uploadPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Xử lý đề cương (fuDeCuong)
                if (fuDeCuong.HasFile)
                {
                    if (!IsValidFile(fuDeCuong))
                    {
                        ShowError("Tệp đề cương không hợp lệ (chỉ chấp nhận .doc, .docx, .pdf, tối đa 5MB).");
                        return;
                    }

                    // Xóa đề cương cũ nếu có
                    var oldDeCuong = db.CTTaiLieu_KeHoachs
                        .Where(ct => ct.KeHoachId == keHoach.Id && ct.TaiLieu.LoaiTaiLieu == "DeCuong")
                        .Select(ct => ct.TaiLieu)
                        .FirstOrDefault();
                    if (oldDeCuong != null)
                    {
                        string oldFilePath = Server.MapPath("~/" + oldDeCuong.linkfile);
                        if (File.Exists(oldFilePath))
                            File.Delete(oldFilePath);
                        db.TaiLieus.Remove(oldDeCuong);
                        db.CTTaiLieu_KeHoachs.RemoveRange(
                            db.CTTaiLieu_KeHoachs.Where(ct => ct.TaiLieuId == oldDeCuong.Id));
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuDeCuong.FileName);
                    string filePath = Path.Combine(uploadPath, fileName);
                    fuDeCuong.SaveAs(filePath);

                    // Lưu vào TaiLieu
                    var taiLieu = new TaiLieu
                    {
                        Id = Guid.NewGuid().ToString(),
                        TenTaiLieu = fuDeCuong.FileName,
                        linkfile = $"Uploads/{fileName}",
                        NgayTao = DateTime.Now,
                        LoaiTaiLieu = "DeCuong"
                    };
                    db.TaiLieus.Add(taiLieu);

                    // Liên kết với KeHoach
                    var ctTaiLieu = new CTTaiLieu_KeHoach
                    {
                        Id = Guid.NewGuid().ToString(),
                        KeHoachId = keHoach.Id,
                        TaiLieuId = taiLieu.Id
                    };
                    db.CTTaiLieu_KeHoachs.Add(ctTaiLieu);
                }

                // Xử lý tài liệu liên quan (fuTaiLieu)
                if (fuTaiLieu.HasFiles)
                {
                    foreach (HttpPostedFile file in fuTaiLieu.PostedFiles)
                    {
                        if (!IsValidFile(file))
                        {
                            ShowError($"Tệp {file.FileName} không hợp lệ (chỉ chấp nhận .doc, .docx, .pdf, tối đa 5MB).");
                            return;
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadPath, fileName);
                        file.SaveAs(filePath);

                        // Lưu vào TaiLieu
                        var taiLieu = new TaiLieu
                        {
                            Id = Guid.NewGuid().ToString(),
                            TenTaiLieu = file.FileName,
                            linkfile = $"Uploads/{fileName}",
                            NgayTao = DateTime.Now,
                            LoaiTaiLieu = "TaiLieuLienQuan"
                        };
                        db.TaiLieus.Add(taiLieu);

                        // Liên kết với KeHoach
                        var ctTaiLieu = new CTTaiLieu_KeHoach
                        {
                            Id = Guid.NewGuid().ToString(),
                            KeHoachId = keHoach.Id,
                            TaiLieuId = taiLieu.Id
                        };
                        db.CTTaiLieu_KeHoachs.Add(ctTaiLieu);
                    }
                }

                // Xử lý phân công thành viên
                bool hasSelected = false;
                foreach (GridViewRow row in gvPhanCong.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        hasSelected = true;
                        break;
                    }
                }

                if (!hasSelected)
                {
                    ShowError("Vui lòng chọn ít nhất một thành viên!");
                    return;
                }

                foreach (GridViewRow row in gvPhanCong.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    string userId = gvPhanCong.DataKeys[row.RowIndex].Value.ToString();
                    TextBox txtNhiemVu = (TextBox)row.FindControl("txtNhiemVu");
                    FileUpload fuLinkFile = (FileUpload)row.FindControl("fuLinkFile");
                    string noiDungCV = txtNhiemVu.Text.Trim();

                    var existingPhanCong = db.PhanCong_Users
                        .FirstOrDefault(p => p.KeHoachID == hfSelectedKeHoach.Value && p.UserID == userId);

                    if (chkSelect.Checked)
                    {
                        // Upload file nếu có
                        string linkFile = existingPhanCong?.linkfile;
                        if (fuLinkFile.HasFile)
                        {
                            if (!IsValidFile(fuLinkFile))
                            {
                                ShowError($"Tệp của thành viên {gvPhanCong.Rows[row.RowIndex].Cells[0].Text} không hợp lệ (chỉ chấp nhận .doc, .docx, .pdf, tối đa 5MB).");
                                return;
                            }

                            // Xóa tệp cũ nếu có
                            if (!string.IsNullOrEmpty(linkFile))
                            {
                                string oldFilePath = Server.MapPath("~/" + linkFile);
                                if (File.Exists(oldFilePath))
                                    File.Delete(oldFilePath);
                            }

                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fuLinkFile.FileName);
                            string filePath = Path.Combine(uploadPath, fileName);
                            fuLinkFile.SaveAs(filePath);
                            linkFile = $"Uploads/{fileName}";
                        }

                        if (existingPhanCong == null)
                        {
                            // Thêm mới phân công
                            var phanCong = new PhanCong_User
                            {
                                Id = Guid.NewGuid().ToString(),
                                KeHoachID = hfSelectedKeHoach.Value,
                                UserID = userId,
                                NoiDungCV = noiDungCV,
                                linkfile = linkFile,
                                ngayTao = DateTime.Now
                            };
                            db.PhanCong_Users.Add(phanCong);

                            // Thêm thông báo
                            var thongBao = new ThongBao_User
                            {
                                Id = Guid.NewGuid().ToString(),
                                KeHoachID = hfSelectedKeHoach.Value,
                                UserID = userId,
                                NoiDung = $"Bạn được phân công nhiệm vụ '{noiDungCV}' trong kế hoạch '{keHoach.TenKeHoach}'.",
                                NgayTao = DateTime.Now,
                                redirectUrl = $"/chi-tiet-ke-hoach/{hfSelectedKeHoach.Value}",
                                DaXem = false
                            };
                            db.ThongBao_Users.Add(thongBao);
                        }
                        else
                        {
                            // Cập nhật phân công
                            existingPhanCong.NoiDungCV = noiDungCV;
                            existingPhanCong.linkfile = linkFile;
                            existingPhanCong.ngayTao = DateTime.Now;
                        }
                    }
                    else if (existingPhanCong != null)
                    {
                        // Xóa phân công và tệp nếu bỏ chọn
                        if (!string.IsNullOrEmpty(existingPhanCong.linkfile))
                        {
                            string oldFilePath = Server.MapPath("~/" + existingPhanCong.linkfile);
                            if (File.Exists(oldFilePath))
                                File.Delete(oldFilePath);
                        }
                        db.PhanCong_Users.Remove(existingPhanCong);
                    }
                }

                db.SaveChanges();
                ShowSuccess("Lưu phân công, đề cương, tài liệu và gửi thông báo thành công!");
                LoadKeHoach();
                LoadPhanCong();
                LoadDocuments();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lưu: {ex.Message}");
            }
        }

        protected void btnExportPhanCong_Click(object sender, EventArgs e)
        {
            ShowError("Chức năng xuất văn bản chưa được triển khai!");
        }

        protected void rptDocuments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "XoaFile")
            {
                try
                {
                    string taiLieuId = e.CommandArgument.ToString();
                    var taiLieu = db.TaiLieus.Find(taiLieuId);
                    if (taiLieu != null)
                    {
                        // Xóa tệp vật lý
                        string filePath = Server.MapPath("~/" + taiLieu.linkfile);
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        // Xóa bản ghi liên quan
                        db.CTTaiLieu_KeHoachs.RemoveRange(
                            db.CTTaiLieu_KeHoachs.Where(ct => ct.TaiLieuId == taiLieuId));
                        db.TaiLieus.Remove(taiLieu);

                        db.SaveChanges();
                        LoadDocuments();
                        ShowSuccess("Xóa tài liệu thành công!");
                    }
                    else
                    {
                        ShowError("Tài liệu không tồn tại!");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Lỗi khi xóa tài liệu: {ex.Message}");
                }
            }
        }

        protected void gvPhanCong_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Thêm logic nếu cần
        }

        protected bool HasEvaluationRights()
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
                return false;

            var user = db.NguoiDungs.FirstOrDefault(u => u.username == username);
            if (user == null)
                return false;

            var role = db.Roles.FirstOrDefault(r => r.Id == user.RoleID);
            return role != null && role.Ten == "TruongDoan";
        }

        private bool IsValidFile(FileUpload fileUpload)
        {
            if (!fileUpload.HasFile) return true;
            string[] allowedExtensions = { ".doc", ".docx", ".pdf" };
            string extension = Path.GetExtension(fileUpload.FileName).ToLower();
            long maxSize = 5 * 1024 * 1024; // 5MB
            return allowedExtensions.Contains(extension) && fileUpload.PostedFile.ContentLength <= maxSize;
        }

        private bool IsValidFile(HttpPostedFile file)
        {
            string[] allowedExtensions = { ".doc", ".docx", ".pdf" };
            string extension = Path.GetExtension(file.FileName).ToLower();
            long maxSize = 5 * 1024 * 1024; // 5MB
            return allowedExtensions.Contains(extension) && file.ContentLength <= maxSize;
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