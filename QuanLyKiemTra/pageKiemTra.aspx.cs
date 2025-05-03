using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKiemTraTrucTuyen : System.Web.UI.Page
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

                LoadKeHoach();
            }
        }

        private void LoadKeHoach()
        {
            try
            {
                var keHoachList = db.KeHoachs
                    .Include("DonVi")
                    .Include("NguoiDung")
                    .Select(k => new
                    {
                        k.Id,
                        k.TenKeHoach,
                        DonVi = k.DonVi,
                        NguoiDung = k.NguoiDung
                    })
                    .ToList();

                gvKeHoach.DataSource = keHoachList;
                gvKeHoach.DataBind();
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
                // Sửa tên trang chuyển hướng thành pageCTKeHoach.aspx
                Response.Redirect($"pageCTKeHoach.aspx?Id={keHoachId}");
            }
        }
    }
}