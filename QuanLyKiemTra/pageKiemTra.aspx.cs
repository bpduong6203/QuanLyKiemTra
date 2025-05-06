using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKiemTra : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                LoadKeHoach();
            }
        }

        private void LoadKeHoach()
        {
            try
            {
                // Lấy thông tin người dùng dựa trên username
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .Include("DonVi")
                    .FirstOrDefault(u => u.username == username);

                if (nguoiDung == null)
                {
                    Response.Write("<script>alert('Không tìm thấy thông tin người dùng!');</script>");
                    return;
                }

                // Kiểm tra vai trò của người dùng
                bool isTruongDoanOrThanhVien = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

                // Lấy danh sách kế hoạch
                var keHoachList = db.KeHoachs
                    .Include("DonVi")
                    .Include("NguoiDung")
                    .AsQueryable();

                // Nếu không phải TruongDoan hoặc ThanhVien (tức là DonVi), lọc theo DonViID
                if (!isTruongDoanOrThanhVien)
                {
                    if (!string.IsNullOrEmpty(nguoiDung.DonViID))
                    {
                        keHoachList = keHoachList.Where(k => k.DonViID == nguoiDung.DonViID);
                    }
                    else
                    {
                        // Nếu DonVi không có DonViID, không hiển thị kế hoạch
                        keHoachList = keHoachList.Where(k => false);
                    }
                }

                var result = keHoachList
                    .Select(k => new
                    {
                        k.Id,
                        k.TenKeHoach,
                        DonVi = k.DonVi,
                        NguoiDung = k.NguoiDung
                    })
                    .ToList();

                if (result.Any())
                {
                    gvKeHoach.DataSource = result;
                    gvKeHoach.DataBind();
                    gvKeHoach.Visible = true;
                }
                else
                {
                    gvKeHoach.Visible = false;
                    Response.Write("<script>alert('Không có kế hoạch nào để hiển thị!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách kế hoạch: {ex.Message}');</script>");
            }
        }

        protected void gvKeHoach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemChiTiet")
            {
                string keHoachId = e.CommandArgument.ToString();
                Response.Redirect($"~/chi-tiet-ke-hoach/{keHoachId}");
            }
        }
    }
}