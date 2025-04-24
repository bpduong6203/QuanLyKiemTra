using System;
using System.Data;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCaNhan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Điều kiện để chọn Master Page
            if (Request.QueryString["master"] == "CaNhan")
            {
                this.MasterPageFile = "~/CaNhan.Master";
            }
            else
            {
                this.MasterPageFile = "~/Site1.Master";
            }
        }

        private void BindGridView(string statusFilter = "", string typeFilter = "")
        {
            // Giả lập dữ liệu (thay bằng truy vấn database thực tế)
            DataTable dt = new DataTable();
            dt.Columns.Add("TenKeHoach");
            dt.Columns.Add("TrangThai");
            dt.Columns.Add("LoaiTaiLieu");
            dt.Columns.Add("NgayTao", typeof(DateTime));
            dt.Columns.Add("TaiLieuUrl");

            // Thêm dữ liệu mẫu
            dt.Rows.Add("Kế hoạch kiểm tra Q1", "Hoàn thành", "Kế hoạch", DateTime.Now.AddDays(-10), "~/files/kehoach1.pdf");
            dt.Rows.Add("Giải trình lỗi", "Đang xử lý", "Giải trình", DateTime.Now.AddDays(-5), "~/files/giaitinh1.docx");
            dt.Rows.Add("Tài liệu hướng dẫn", "Chưa bắt đầu", "Tài liệu", DateTime.Now, "~/files/tailieu1.pdf");
            dt.Rows.Add("Tài liệu hướng dẫn", "Chưa bắt đầu", "Tài liệu", DateTime.Now, "~/files/tailieu1.pdf");
            dt.Rows.Add("Tài liệu hướng dẫn", "Chưa bắt đầu", "Tài liệu", DateTime.Now, "~/files/tailieu1.pdf");
            dt.Rows.Add("Kế hoạch kiểm tra Q1", "Hoàn thành", "Kế hoạch", DateTime.Now.AddDays(-10), "~/files/kehoach1.pdf");
            dt.Rows.Add("Kế hoạch kiểm tra Q1", "Hoàn thành", "Kế hoạch", DateTime.Now.AddDays(-10), "~/files/kehoach1.pdf");

            // Lọc dữ liệu
            DataView dv = dt.DefaultView;
            if (!string.IsNullOrEmpty(statusFilter))
                dv.RowFilter += $"TrangThai = '{statusFilter}'";
            if (!string.IsNullOrEmpty(typeFilter))
                dv.RowFilter += (dv.RowFilter.Length > 0 ? " AND " : "") + $"LoaiTaiLieu = '{typeFilter}'";

            gvKeHoach.DataSource = dv;
            gvKeHoach.DataBind();
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(ddlStatusFilter.SelectedValue, ddlTypeFilter.SelectedValue);
        }

        protected void ddlTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridView(ddlStatusFilter.SelectedValue, ddlTypeFilter.SelectedValue);
        }

        protected void gvKeHoach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKeHoach.PageIndex = e.NewPageIndex;
            BindGridView(ddlStatusFilter.SelectedValue, ddlTypeFilter.SelectedValue);
        }

        protected void lnkTaiLieu_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string fileUrl = lnk.CommandArgument;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                Response.Redirect(fileUrl); // Hoặc logic tải file thực tế
            }
        }

    }
}