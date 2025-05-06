using QuanLyKiemTra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageGiaiTrinh : System.Web.UI.Page
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

                // Lấy username từ session
                string username = Session["Username"].ToString();
                LoadGiaiTrinhList(username);
            }
        }

        private void LoadGiaiTrinhList(string username)
        {
            try
            {
                // Lấy thông tin người dùng dựa trên username
                var nguoiDung = db.NguoiDungs
                    .Include("Roles")
                    .FirstOrDefault(u => u.username == username);

                if (nguoiDung == null)
                {
                    Response.Write("<script>alert('Không tìm thấy thông tin người dùng!');</script>");
                    return;
                }

                // Kiểm tra vai trò của người dùng
                bool isTruongDoanOrThanhVien = nguoiDung.Roles != null &&
                    (nguoiDung.Roles.Ten == "TruongDoan" || nguoiDung.Roles.Ten == "ThanhVien");

                // Lấy danh sách giải trình
                var giaiTrinhList = db.GiaiTrinhs
                    .Include("NguoiYeuCau")
                    .Include("KeHoach")
                    .Include("GiaiTrinhFiles")
                    .AsQueryable();

                // Nếu không phải TruongDoan hoặc ThanhVien (tức là DonVi), lọc theo NguoiGiaiTrinhID
                if (!isTruongDoanOrThanhVien)
                {
                    giaiTrinhList = giaiTrinhList.Where(g => g.NguoiGiaiTrinhID == nguoiDung.Id);
                }

                var result = giaiTrinhList.ToList();

                if (result.Any())
                {
                    // Khởi tạo GiaiTrinhFiles nếu null để tránh lỗi
                    foreach (var giaiTrinh in result)
                    {
                        if (giaiTrinh.GiaiTrinhFiles == null)
                        {
                            giaiTrinh.GiaiTrinhFiles = new List<GiaiTrinhFile>();
                        }
                    }

                    gvGiaiTrinhList.DataSource = result;
                    gvGiaiTrinhList.DataBind();
                }
                else
                {
                    gvGiaiTrinhList.Visible = false;
                    Response.Write("<script>alert('Không có yêu cầu giải trình nào!');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách giải trình: {ex.Message}');</script>");
            }
        }

        protected void gvGiaiTrinhList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemChiTiet")
            {
                string giaiTrinhId = e.CommandArgument.ToString();
                // Thêm log để kiểm tra
                System.Diagnostics.Debug.WriteLine($"Chuyển hướng tới: ~/chi-tiet-giai-trinh/{giaiTrinhId}");
                Response.Redirect($"~/chi-tiet-giai-trinh/{giaiTrinhId}");
            }
        }
    }
}