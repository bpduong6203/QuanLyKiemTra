using QuanLyKiemTra.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCTKeHoach : System.Web.UI.Page
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

                // Lấy Id kế hoạch từ query string
                string keHoachId = Request.QueryString["Id"];
                if (string.IsNullOrEmpty(keHoachId))
                {
                    Response.Write("<script>alert('Không tìm thấy kế hoạch!');</script>");
                    return;
                }

                LoadKeHoach(keHoachId);
                LoadThanhVienDonVi(keHoachId);
                CheckGiaiTrinh(keHoachId);
                LoadBoCauHoi();
            }
        }

        private void LoadKeHoach(string keHoachId)
        {
            try
            {
                var keHoach = db.KeHoachs
                    .Include("DonVi")
                    .Include("NguoiDung")
                    .Include("BienBanKiemTra")
                    .FirstOrDefault(k => k.Id == keHoachId);

                if (keHoach == null)
                {
                    Response.Write("<script>alert('Kế hoạch không tồn tại!');</script>");
                    return;
                }

                // Hiển thị thông tin kế hoạch
                lblTenKeHoachValue.Text = keHoach.TenKeHoach;
                lblNgayBatDauValue.Text = keHoach.NgayBatDau.ToString("dd/MM/yyyy");
                lblNgayKetThucValue.Text = keHoach.NgayKetThuc.ToString("dd/MM/yyyy");
                lblGhiChuValue.Text = keHoach.GhiChu ?? "Không có ghi chú";

                // Hiển thị thông tin đơn vị
                lblTenDonViValue.Text = keHoach.DonVi?.TenDonVi ?? "Không có thông tin";
                lblDiaChiDonViValue.Text = keHoach.DonVi?.DiaChi ?? "Không có thông tin";
                lblSoDienThoaiDonViValue.Text = keHoach.DonVi?.SoDienThoai ?? "Không có thông tin";
                lblEmailDonViValue.Text = keHoach.DonVi?.Email ?? "Không có thông tin";
                lblNguoiDaiDienValue.Text = keHoach.DonVi?.NguoiDaiDien ?? "Không có thông tin";
                lblChucVuNguoiDaiDienValue.Text = keHoach.DonVi?.ChucVuNguoiDaiDien ?? "Không có thông tin";

                // Hiển thị người tạo
                lblNguoiTaoValue.Text = keHoach.NguoiDung?.HoTen ?? "Không có thông tin";

                // Hiển thị biên bản kiểm tra
                if (!string.IsNullOrEmpty(keHoach.BienBanID) && keHoach.BienBanKiemTra != null)
                {
                    lblTenBienBanValue.Text = keHoach.BienBanKiemTra.tenBBKT;
                    hlLinkBienBan.NavigateUrl = keHoach.BienBanKiemTra.linkfile;
                    pnlBienBan.Visible = true;
                    lblNoBienBan.Visible = false;
                }
                else
                {
                    pnlBienBan.Visible = false;
                    lblNoBienBan.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải thông tin kế hoạch: {ex.Message}');</script>");
            }
        }

        private void LoadThanhVienDonVi(string keHoachId)
        {
            try
            {
                var keHoach = db.KeHoachs.FirstOrDefault(k => k.Id == keHoachId);
                if (keHoach == null) return;

                var thanhVienList = db.NguoiDungs
                    .Where(u => u.DonViID == keHoach.DonViID)
                    .Select(u => new
                    {
                        u.HoTen,
                        u.Email,
                        u.SoDienThoai,
                        u.DiaChi
                    })
                    .ToList();

                gvThanhVienDonVi.DataSource = thanhVienList;
                gvThanhVienDonVi.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách thành viên đơn vị: {ex.Message}');</script>");
            }
        }

        private void CheckGiaiTrinh(string keHoachId)
        {
            try
            {
                var giaiTrinhList = db.GiaiTrinhs
                    .Include("NguoiYeuCau")
                    .Include("NguoiGiaiTrinh")
                    .Where(g => g.KeHoachID == keHoachId)
                    .ToList();

                if (giaiTrinhList.Any())
                {
                    // Nếu đã có giải trình, hiển thị GridView
                    gvGiaiTrinh.DataSource = giaiTrinhList;
                    gvGiaiTrinh.DataBind();
                    gvGiaiTrinh.Visible = true;
                    pnlGiaiTrinh.Visible = false;
                }
                else
                {
                    // Nếu chưa có giải trình, hiển thị panel để yêu cầu
                    gvGiaiTrinh.Visible = false;
                    pnlGiaiTrinh.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi kiểm tra bảng giải trình: {ex.Message}');</script>");
            }
        }

        protected void btnYeuCauGiaiTrinh_Click(object sender, EventArgs e)
        {
            try
            {
                string keHoachId = Request.QueryString["Id"];
                var keHoach = db.KeHoachs.Include("DonVi").FirstOrDefault(k => k.Id == keHoachId);
                if (keHoach == null)
                {
                    Response.Write("<script>alert('Kế hoạch không tồn tại!');</script>");
                    return;
                }

                // Lấy người dùng hiện tại (người yêu cầu)
                string username = Session["Username"]?.ToString();
                var nguoiYeuCau = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                if (nguoiYeuCau == null)
                {
                    Response.Write("<script>alert('Không tìm thấy thông tin người dùng!');</script>");
                    return;
                }

                // Lấy danh sách tất cả thành viên của đơn vị
                var thanhVienDonVi = db.NguoiDungs
                    .Where(u => u.DonViID == keHoach.DonViID)
                    .ToList();

                if (!thanhVienDonVi.Any())
                {
                    Response.Write("<script>alert('Không tìm thấy thành viên nào trong đơn vị!');</script>");
                    return;
                }

                // Xử lý file mẫu nếu có
                string linkFile = null;
                if (fuFileMau.HasFile)
                {
                    string fileName = Path.GetFileName(fuFileMau.FileName);
                    string uploadPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);
                    fuFileMau.SaveAs(filePath);
                    linkFile = $"~/Uploads/{fileName}";
                }

                // Tạo bản ghi giải trình cho từng thành viên
                foreach (var thanhVien in thanhVienDonVi)
                {
                    var giaiTrinh = new GiaiTrinh
                    {
                        Id = Guid.NewGuid().ToString(),
                        KeHoachID = keHoachId,
                        NguoiYeuCauID = nguoiYeuCau.Id,
                        NguoiGiaiTrinhID = thanhVien.Id,
                        NgayTao = DateTime.Now,
                        linkFile = linkFile // Lưu đường dẫn file mẫu
                    };
                    db.GiaiTrinhs.Add(giaiTrinh);

                    var thongBao = new ThongBao_User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserID = thanhVien.Id,
                        KeHoachID = keHoachId,
                        NoiDung = $"Yêu cầu giải trình từ {nguoiYeuCau.HoTen} cho kế hoạch '{keHoach.TenKeHoach}'.",
                        NgayTao = DateTime.Now,
                        DaXem = false
                    };
                    db.ThongBao_Users.Add(thongBao);
                }

                db.SaveChanges();

                // Ẩn panel sau khi yêu cầu giải trình
                pnlGiaiTrinh.Visible = false;
                Response.Write("<script>alert('Yêu cầu giải trình đã được gửi đến các thành viên của đơn vị!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi yêu cầu giải trình: {ex.Message}');</script>");
            }
        }

        private void LoadBoCauHoi()
        {
            try
            {
                var boCauHoiList = db.BoCauHois.ToList();
                ddlBoCauHoi.DataSource = boCauHoiList;
                ddlBoCauHoi.DataTextField = "TenBoCauHoi";
                ddlBoCauHoi.DataValueField = "Id";
                ddlBoCauHoi.DataBind();
                ddlBoCauHoi.Items.Insert(0, new ListItem("-- Chọn bộ câu hỏi --", ""));
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách bộ câu hỏi: {ex.Message}');</script>");
            }
        }

        protected void btnThemBoCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                string keHoachId = Request.QueryString["Id"];
                var keHoach = db.KeHoachs.FirstOrDefault(k => k.Id == keHoachId);
                if (keHoach == null)
                {
                    Response.Write("<script>alert('Kế hoạch không tồn tại!');</script>");
                    return;
                }

                if (string.IsNullOrEmpty(ddlBoCauHoi.SelectedValue))
                {
                    Response.Write("<script>alert('Vui lòng chọn bộ câu hỏi!');</script>");
                    return;
                }

                if (!int.TryParse(txtThoiGianLam.Text, out int thoiGianLam) || thoiGianLam <= 0)
                {
                    Response.Write("<script>alert('Thời gian làm không hợp lệ!');</script>");
                    return;
                }

                // Tạo bản ghi BoCauHoi_KeHoach
                var boCauHoiKeHoach = new BoCauHoi_KeHoach
                {
                    Id = Guid.NewGuid().ToString(),
                    BoCauHoiId = ddlBoCauHoi.SelectedValue,
                    KeHoachId = keHoachId,
                    ThoiGianLam = thoiGianLam
                };

                db.BoCauHoi_KeHoachs.Add(boCauHoiKeHoach);

                // Gửi thông báo đến các thành viên được phân công
                var thanhVienPhanCong = db.PhanCong_Users
                    .Where(p => p.KeHoachID == keHoachId)
                    .ToList();

                foreach (var thanhVien in thanhVienPhanCong)
                {
                    var thongBao = new ThongBao_User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserID = thanhVien.UserID,
                        KeHoachID = keHoachId,
                        NoiDung = $"Bộ câu hỏi '{ddlBoCauHoi.SelectedItem.Text}' đã được thêm vào kế hoạch '{keHoach.TenKeHoach}'.",
                        NgayTao = DateTime.Now,
                        DaXem = false
                    };
                    db.ThongBao_Users.Add(thongBao);
                }

                db.SaveChanges();
                Response.Write("<script>alert('Thêm bộ câu hỏi và gửi thông báo thành công!');</script>");

                // Reset form
                ddlBoCauHoi.SelectedIndex = 0;
                txtThoiGianLam.Text = "";
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi thêm bộ câu hỏi: {ex.Message}');</script>");
            }
        }
    }
}