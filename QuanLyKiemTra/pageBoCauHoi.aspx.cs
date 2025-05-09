using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageBoCauHoi : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Danh sách bộ câu hỏi";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }
                LoadBoCauHoi();
            }
        }

        private void LoadBoCauHoi()
        {
            try
            {
                var boCauHoiList = db.BoCauHois
                    .Select(b => new
                    {
                        b.Id,
                        b.TenBoCauHoi,
                        b.NgayTao
                    })
                    .ToList();

                gvBoCauHoi.DataSource = boCauHoiList;
                gvBoCauHoi.DataBind();

                // Thêm class và data-name cho mỗi hàng
                foreach (GridViewRow row in gvBoCauHoi.Rows)
                {
                    var tenBoCauHoi = row.Cells[1].Text;
                    row.Attributes["class"] = "question-item";
                    row.Attributes["data-name"] = Server.HtmlDecode(tenBoCauHoi);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Lỗi khi tải danh sách: " + ex.Message;
                lblMessage.CssClass = "error-message";
                lblMessage.Visible = true;
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/danh-sach-cau-hoi");
        }

        protected void gvBoCauHoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditBoCauHoi")
            {
                string boCauHoiId = e.CommandArgument.ToString();
                Response.Redirect($"~/chinh-sua-bo-cau-hoi/{boCauHoiId}");
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            db.Dispose();
        }
    }
}