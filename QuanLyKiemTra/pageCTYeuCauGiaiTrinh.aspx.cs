using QuanLyKiemTra.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCTYeuCauGiaiTrinh : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
                }

                // Lấy ID giải trình từ query string
                string giaiTrinhId = Request.QueryString["Id"];
                if (string.IsNullOrEmpty(giaiTrinhId))
                {
                    Response.Redirect("pageGiaiTrinh.aspx");
                }

                // Lấy username từ session
                string username = Session["Username"].ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);
                if (nguoiDung == null)
                {
                    Response.Write("<script>alert('Không tìm thấy thông tin người dùng!');</script>");
                    return;
                }

                var giaiTrinh = db.GiaiTrinhs
                    .Include("NguoiYeuCau")
                    .Include("NguoiGiaiTrinh")
                    .FirstOrDefault(g => g.Id == giaiTrinhId);

                // Kiểm soát vai trò
                if (giaiTrinh != null)
                {
                    bool hasEvaluationRights = nguoiDung.Roles != null &&
                        (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");
                    pnlGuiGiaiTrinh.Visible = (giaiTrinh.NguoiGiaiTrinhID == nguoiDung.Id);
                    pnlThemSuaFileMau.Visible = hasEvaluationRights; // Chỉ hiển thị cho TruongDoan, ThanhVien
                    LoadGiaiTrinh(giaiTrinhId, nguoiDung);
                    LoadNDGiaiTrinh(giaiTrinhId, nguoiDung, hasEvaluationRights);
                }
            }
        }

        private void LoadGiaiTrinh(string giaiTrinhId, NguoiDung nguoiDung)
        {
            try
            {
                var giaiTrinh = db.GiaiTrinhs
                    .Include("NguoiYeuCau")
                    .Include("GiaiTrinhFiles")
                    .FirstOrDefault(g => g.Id == giaiTrinhId);

                if (giaiTrinh == null)
                {
                    Response.Write("<script>alert('Không tìm thấy giải trình!');</script>");
                    Response.Redirect("pageGiaiTrinh.aspx");
                    return;
                }

                // Hiển thị thông tin giải trình
                lblNguoiYeuCauValue.Text = giaiTrinh.NguoiYeuCau?.HoTen ?? "Không xác định";
                lblNgayTaoValue.Text = giaiTrinh.NgayTao.ToString("dd/MM/yyyy");

                // Hiển thị danh sách file mẫu
                if (giaiTrinh.GiaiTrinhFiles != null && giaiTrinh.GiaiTrinhFiles.Any())
                {
                    rptFiles.DataSource = giaiTrinh.GiaiTrinhFiles;
                    rptFiles.DataBind();
                    hlFileMau.Visible = false;
                }
                else
                {
                    hlFileMau.Visible = false;
                    rptFiles.Visible = false;
                }

                // Hiển thị trạng thái tổng thể
                lblTrangThaiTongTheValue.Text = giaiTrinh.TrangThaiTongThe;
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải thông tin giải trình: {ex.Message}');</script>");
            }
        }

        private void LoadNDGiaiTrinh(string giaiTrinhId, NguoiDung nguoiDung, bool hasEvaluationRights)
        {
            try
            {
                var ndGiaiTrinhList = db.CTNoiDung_GiaiTrinhs
                    .Include("NDGiaiTrinh")
                    .Where(ct => ct.GiaiTrinhID == giaiTrinhId)
                    .Select(ct => ct.NDGiaiTrinh)
                    .OrderByDescending(nd => nd.NgayTao)
                    .ToList();

                if (ndGiaiTrinhList.Any())
                {
                    gvNDGiaiTrinh.DataSource = ndGiaiTrinhList;
                    gvNDGiaiTrinh.DataBind();
                    gvNDGiaiTrinh.Columns[4].Visible = hasEvaluationRights;
                }
                else
                {
                    gvNDGiaiTrinh.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách nội dung giải trình: {ex.Message}');</script>");
            }
        }

        protected void btnThemFileMau_Click(object sender, EventArgs e)
        {
            try
            {
                string giaiTrinhId = Request.QueryString["Id"];
                var giaiTrinh = db.GiaiTrinhs
                    .Include("GiaiTrinhFiles")
                    .FirstOrDefault(g => g.Id == giaiTrinhId);
                if (giaiTrinh == null)
                {
                    Response.Write("<script>alert('Không tìm thấy giải trình!');</script>");
                    return;
                }

                // Kiểm tra quyền
                string username = Session["Username"].ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);
                bool hasEvaluationRights = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

                if (!hasEvaluationRights)
                {
                    Response.Write("<script>alert('Bạn không có quyền thêm file mẫu!');</script>");
                    return;
                }

                // Kiểm tra file
                if (!fuFileMau.HasFiles)
                {
                    Response.Write("<script>alert('Vui lòng chọn ít nhất một file mẫu!');</script>");
                    return;
                }

                // Lưu các file mẫu
                foreach (HttpPostedFile file in fuFileMau.PostedFiles)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    file.SaveAs(filePath);

                    var giaiTrinhFile = new GiaiTrinhFile
                    {
                        Id = Guid.NewGuid().ToString(),
                        GiaiTrinhID = giaiTrinhId,
                        LinkFile = "~/Uploads/" + fileName,
                        FileName = fileName,
                        NgayTao = DateTime.Now
                    };
                    db.GiaiTrinhFiles.Add(giaiTrinhFile);
                }

                db.SaveChanges();

                // Làm mới danh sách file mẫu
                LoadGiaiTrinh(giaiTrinhId, nguoiDung);
                Response.Write("<script>alert('Thêm file mẫu thành công!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi thêm file mẫu: {ex.Message}');</script>");
            }
        }

        protected void btnGuiGiaiTrinh_Click(object sender, EventArgs e)
        {
            try
            {
                string giaiTrinhId = Request.QueryString["Id"];
                var giaiTrinh = db.GiaiTrinhs
                    .Include("NguoiGiaiTrinh")
                    .Include("KeHoach")
                    .FirstOrDefault(g => g.Id == giaiTrinhId);
                if (giaiTrinh == null) return;

                // Kiểm tra file
                if (!fuFileGiaiTrinh.HasFiles)
                {
                    Response.Write("<script>alert('Vui lòng chọn ít nhất một file giải trình!');</script>");
                    return;
                }

                // Lấy username từ session
                string username = Session["Username"].ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);

                // Tạo NDGiaiTrinh mới
                var ndGiaiTrinh = new NDGiaiTrinh
                {
                    Id = Guid.NewGuid().ToString(),
                    NoiDung = txtNoiDungGiaiTrinh.Text,
                    NgayTao = DateTime.Now,
                    DaXem = false,
                    TrangThai = "Chờ Đánh Giá"
                };
                db.NDGiaiTrinhs.Add(ndGiaiTrinh);

                // Lưu các file giải trình
                foreach (HttpPostedFile file in fuFileGiaiTrinh.PostedFiles)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    file.SaveAs(filePath);

                    // Lưu vào linkfile của NDGiaiTrinh (chỉ lưu file đầu tiên cho đơn giản)
                    if (string.IsNullOrEmpty(ndGiaiTrinh.linkfile))
                    {
                        ndGiaiTrinh.linkfile = "~/Uploads/" + fileName;
                        ndGiaiTrinh.FileName = fileName;
                    }
                }

                // Liên kết với GiaiTrinh
                var ctNoiDung = new CTNoiDung_GiaiTrinh
                {
                    Id = Guid.NewGuid().ToString(),
                    NDGiaiTrinhID = ndGiaiTrinh.Id,
                    GiaiTrinhID = giaiTrinhId
                };
                db.CTNoiDung_GiaiTrinhs.Add(ctNoiDung);

                // Cập nhật trạng thái tổng thể
                giaiTrinh.TrangThaiTongThe = "Chờ Đánh Giá";

                // Tạo thông báo cho người yêu cầu
                var nguoiYeuCau = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.Id == giaiTrinh.NguoiYeuCauID);
                if (nguoiYeuCau != null && nguoiYeuCau.Roles != null)
                {
                    var thongBao = new ThongBao_User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserID = giaiTrinh.NguoiYeuCauID,
                        KeHoachID = giaiTrinh.KeHoachID,
                        NoiDung = $"Bạn có giải trình mới từ {giaiTrinh.NguoiGiaiTrinh.HoTen} cho kế hoạch {giaiTrinh.KeHoach.TenKeHoach}",
                        NgayTao = DateTime.Now,
                        DaXem = false
                    };
                    db.ThongBao_Users.Add(thongBao);
                }

                db.SaveChanges();

                // Làm mới danh sách
                bool hasEvaluationRights = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");
                LoadNDGiaiTrinh(giaiTrinhId, nguoiDung, hasEvaluationRights);
                txtNoiDungGiaiTrinh.Text = string.Empty;
                LoadGiaiTrinh(giaiTrinhId, nguoiDung);
                Response.Write("<script>alert('Gửi giải trình thành công!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi gửi giải trình: {ex.Message}');</script>");
            }
        }

        protected void gvNDGiaiTrinh_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ndGiaiTrinhId = e.CommandArgument.ToString();
            var ndGiaiTrinh = db.NDGiaiTrinhs.FirstOrDefault(nd => nd.Id == ndGiaiTrinhId);
            if (ndGiaiTrinh == null) return;

            // Lấy username từ session
            string username = Session["Username"].ToString();
            var nguoiDung = db.NguoiDungs
                .Include("Roles")
                .FirstOrDefault(u => u.username == username);
            string giaiTrinhId = Request.QueryString["Id"];
            var giaiTrinh = db.GiaiTrinhs
                .Include("NguoiYeuCau")
                .FirstOrDefault(g => g.Id == giaiTrinhId);

            bool hasEvaluationRights = nguoiDung.Roles != null &&
                (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

            if (e.CommandName == "XacNhanDat" && hasEvaluationRights)
            {
                ndGiaiTrinh.TrangThai = "Đã Đạt";
                ndGiaiTrinh.NguoiDanhGiaID = nguoiDung.Id;
                ndGiaiTrinh.NgayDanhGia = DateTime.Now;
                db.SaveChanges();

                // Cập nhật trạng thái tổng thể nếu tất cả đều đạt
                if (giaiTrinh.CTNoiDung_GiaiTrinhs.All(ct => ct.NDGiaiTrinh.TrangThai == "Đã Đạt"))
                {
                    giaiTrinh.TrangThaiTongThe = "Hoàn Thành"; // Sửa lỗi ở đây
                    db.SaveChanges();
                }

                Response.Write("<script>alert('Xác nhận đạt thành công!');</script>");
            }
            else if (e.CommandName == "YeuCauChinhSua" && hasEvaluationRights)
            {
                // Hiển thị form yêu cầu chỉnh sửa
                ViewState["NDGiaiTrinhId"] = ndGiaiTrinhId;
                pnlYeuCauChinhSua.Visible = true;
                return;
            }
            else if (e.CommandName == "Xoa" && hasEvaluationRights)
            {
                // Xóa các bản ghi liên quan trong CTNoiDung_GiaiTrinh
                var ctNoiDungs = db.CTNoiDung_GiaiTrinhs
                    .Where(ct => ct.NDGiaiTrinhID == ndGiaiTrinhId)
                    .ToList();
                foreach (var ct in ctNoiDungs)
                {
                    db.CTNoiDung_GiaiTrinhs.Remove(ct);
                }

                // Xóa NDGiaiTrinh
                db.NDGiaiTrinhs.Remove(ndGiaiTrinh);
                db.SaveChanges();

                // Cập nhật trạng thái tổng thể nếu không còn nội dung giải trình nào
                if (!db.CTNoiDung_GiaiTrinhs.Any(ct => ct.GiaiTrinhID == giaiTrinhId))
                {
                    giaiTrinh.TrangThaiTongThe = "Chờ Giải Trình";
                    db.SaveChanges();
                }

                Response.Write("<script>alert('Xóa nội dung giải trình thành công!');</script>");
            }

            // Làm mới danh sách và thông tin giải trình
            LoadNDGiaiTrinh(giaiTrinhId, nguoiDung, hasEvaluationRights);
            LoadGiaiTrinh(giaiTrinhId, nguoiDung);
        }

        protected void btnGuiYeuCauChinhSua_Click(object sender, EventArgs e)
        {
            try
            {
                string ndGiaiTrinhId = ViewState["NDGiaiTrinhId"]?.ToString();
                var ndGiaiTrinh = db.NDGiaiTrinhs.FirstOrDefault(nd => nd.Id == ndGiaiTrinhId);
                if (ndGiaiTrinh == null) return;

                // Lấy username từ session
                string username = Session["Username"].ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);
                bool hasEvaluationRights = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

                if (hasEvaluationRights)
                {
                    // Cập nhật NDGiaiTrinh hiện tại
                    ndGiaiTrinh.NoiDung = txtGhiChuChinhSua.Text;
                    ndGiaiTrinh.TrangThai = "Chưa Đạt";
                    ndGiaiTrinh.NguoiDanhGiaID = nguoiDung.Id;
                    ndGiaiTrinh.NgayTao = DateTime.Now;
                    ndGiaiTrinh.DaXem = false;

                    // Cập nhật trạng thái tổng thể của GiaiTrinh
                    string giaiTrinhId = Request.QueryString["Id"];
                    var giaiTrinh = db.GiaiTrinhs
                        .Include("NguoiGiaiTrinh")
                        .Include("KeHoach")
                        .FirstOrDefault(g => g.Id == giaiTrinhId);
                    if (giaiTrinh != null)
                    {
                        giaiTrinh.TrangThaiTongThe = "Chờ Giải Trình";
                    }

                    // Tạo thông báo cho người giải trình
                    if (giaiTrinh != null && giaiTrinh.NguoiGiaiTrinhID != null)
                    {
                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserID = giaiTrinh.NguoiGiaiTrinhID,
                            KeHoachID = giaiTrinh.KeHoachID,
                            NoiDung = $"Bạn có yêu cầu chỉnh sửa từ {nguoiDung.HoTen} cho kế hoạch {giaiTrinh.KeHoach.TenKeHoach}",
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        db.ThongBao_Users.Add(thongBao);
                    }

                    db.SaveChanges();

                    // Ẩn form và làm mới danh sách
                    pnlYeuCauChinhSua.Visible = false;
                    txtGhiChuChinhSua.Text = string.Empty;
                    LoadNDGiaiTrinh(giaiTrinhId, nguoiDung, hasEvaluationRights);
                    LoadGiaiTrinh(giaiTrinhId, nguoiDung);
                    Response.Write("<script>alert('Gửi yêu cầu chỉnh sửa thành công!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi gửi yêu cầu chỉnh sửa: {ex.Message}');</script>");
            }
        }

        protected void btnHuyChinhSua_Click(object sender, EventArgs e)
        {
            pnlYeuCauChinhSua.Visible = false;
            txtGhiChuChinhSua.Text = string.Empty;
        }

        protected bool HasEvaluationRights()
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username)) return false;

            var nguoiDung = db.NguoiDungs
                .Include("Roles")
                .FirstOrDefault(u => u.username == username);

            return nguoiDung?.Roles != null &&
                   (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");
        }

        protected void rptFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "XoaFile")
            {
                string fileId = e.CommandArgument.ToString();
                string giaiTrinhId = Request.QueryString["Id"];

                // Tách giá trị Session["Username"] để tránh ?. trong lambda
                string username = Session["Username"]?.ToString();
                if (string.IsNullOrEmpty(username))
                {
                    Response.Write("<script>alert('Phiên đăng nhập không hợp lệ!');</script>");
                    return;
                }

                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);

                if (nguoiDung?.Roles == null || !(nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien"))
                {
                    Response.Write("<script>alert('Bạn không có quyền xóa file mẫu!');</script>");
                    return;
                }

                try
                {
                    var file = db.GiaiTrinhFiles.FirstOrDefault(f => f.Id == fileId && f.GiaiTrinhID == giaiTrinhId);
                    if (file != null)
                    {
                        // Xóa file vật lý nếu tồn tại
                        string filePath = Server.MapPath(file.LinkFile);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        // Xóa bản ghi trong cơ sở dữ liệu
                        db.GiaiTrinhFiles.Remove(file);
                        db.SaveChanges();

                        // Làm mới danh sách file mẫu
                        LoadGiaiTrinh(giaiTrinhId, nguoiDung);
                        Response.Write("<script>alert('Xóa file mẫu thành công!');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Không tìm thấy file mẫu!');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Lỗi khi xóa file mẫu: {ex.Message}');</script>");
                }
            }
        }

        private string FormatFileName(string originalFileName, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(originalFileName)) return "Không có tên file";

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);

            if (fileNameWithoutExtension.Length > maxLength)
            {
                fileNameWithoutExtension = fileNameWithoutExtension.Substring(0, maxLength - 3) + "...";
            }

            fileNameWithoutExtension = System.Text.RegularExpressions.Regex.Replace(fileNameWithoutExtension, @"[^a-zA-Z0-9\s]", "");

            return fileNameWithoutExtension.Trim();
        }
    }
}