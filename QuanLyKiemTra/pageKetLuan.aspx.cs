using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKetLuanKiemTra : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Trang quản lý Kết Luận Kiểm Tra";
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                LoadKeHoachList();
            }
        }

        private void LoadKeHoachList()
        {
            try
            {
                string username = Session["Username"].ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);

                if (nguoiDung == null)
                {
                    Response.Write("<script>alert('Không tìm thấy thông tin người dùng!');</script>");
                    return;
                }

                var keHoachQuery = db.KeHoachs
                    .Include("NguoiDung")
                    .Include("DonVi")
                    .AsQueryable();

                // Lọc theo vai trò
                bool isTruongDoanOrThanhVien = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");
                if (!isTruongDoanOrThanhVien)
                {
                    keHoachQuery = keHoachQuery.Where(k => k.DonViID == nguoiDung.DonViID);
                }

                // Lọc theo tìm kiếm (nếu có)
                string searchTerm = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    keHoachQuery = keHoachQuery.Where(k => k.TenKeHoach.Contains(searchTerm));
                }

                // Sắp xếp theo NgayKetThuc giảm dần
                var keHoachList = keHoachQuery
                    .OrderByDescending(k => k.NgayKetThuc)
                    .ToList();

                if (keHoachList.Any())
                {
                    gvKeHoachList.DataSource = keHoachList;
                    gvKeHoachList.DataBind();
                }
                else
                {
                    gvKeHoachList.Visible = false;
                    Response.Write("<script>alert('Không có kế hoạch nào!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách kế hoạch: {ex.Message}');</script>");
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadKeHoachList();
        }

        protected void gvKeHoachList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XacNhanHoanThanh")
            {
                string keHoachId = e.CommandArgument.ToString();
                ConfirmCompletion(keHoachId);
            }
        }

        protected string GetKeHoachStatus(string keHoachId)
        {
            var giaiTrinhs = db.GiaiTrinhs
                .Where(g => g.KeHoachID == keHoachId)
                .ToList();
            if (!giaiTrinhs.Any())
            {
                return "Chưa có giải trình";
            }
            if (giaiTrinhs.All(g => g.TrangThaiTongThe == "Hoàn Thành"))
            {
                return "Hoàn Thành";
            }
            return "Chưa Hoàn Thành";
        }

        protected string GetStatusCssClass(string status)
        {
            switch (status)
            {
                case "Chưa có giải trình":
                    return "status-danger";
                case "Chưa Hoàn Thành":
                    return "status-warning";
                case "Hoàn Thành":
                    return "status-success";
                default:
                    return "";
            }
        }

        protected bool CanConfirmCompletion(string keHoachId)
        {
            string username = Session["Username"].ToString();
            var nguoiDung = db.NguoiDungs
                .Include("Roles")
                .FirstOrDefault(u => u.username == username);
            bool hasRights = nguoiDung.Roles != null &&
                (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

            if (!hasRights) return false;

            var giaiTrinhs = db.GiaiTrinhs
                .Where(g => g.KeHoachID == keHoachId)
                .ToList();
            string status = GetKeHoachStatus(keHoachId);
            if (status == "Hoàn Thành" || status == "Chưa có giải trình") return false;

            return giaiTrinhs.All(g => g.CTNoiDung_GiaiTrinhs.All(ct => ct.NDGiaiTrinh.TrangThai == "Đã Đạt"));
        }

        private void ConfirmCompletion(string keHoachId)
        {
            try
            {
                var giaiTrinhs = db.GiaiTrinhs
                    .Where(g => g.KeHoachID == keHoachId)
                    .ToList();
                foreach (var giaiTrinh in giaiTrinhs)
                {
                    giaiTrinh.TrangThaiTongThe = "Hoàn Thành";
                }
                db.SaveChanges();
                LoadKeHoachList();
                Response.Write("<script>alert('Xác nhận hoàn thành thành công!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi xác nhận hoàn thành: {ex.Message}');</script>");
            }
        }
    }
}