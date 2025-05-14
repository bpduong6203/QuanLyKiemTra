using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCongViec : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
                }

                LoadCongViecList();
            }
        }

        private void LoadCongViecList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    string username = Session["Username"].ToString();
                    var user = context.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (user == null)
                    {
                        ShowError("Người dùng không tồn tại.");
                        return;
                    }

                    var congViecs = context.PhanCong_Users
                        .Where(pc => pc.UserID == user.Id)
                        .Select(pc => new
                        {
                            pc.Id,
                            TenKeHoach = pc.KeHoach != null ? pc.KeHoach.TenKeHoach : "Chưa có kế hoạch",
                            pc.NoiDungCV,
                            pc.ngayTao,
                            pc.linkfile
                        })
                        .OrderByDescending(pc => pc.ngayTao)
                        .ToList();

                    gvCongViec.DataSource = congViecs;
                    gvCongViec.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách công việc: {ex.Message}");
            }
        }

        protected void gvCongViec_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCongViec.PageIndex = e.NewPageIndex;
            LoadCongViecList();
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

        protected string FormatTimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Vừa xong";
            else if (timeSpan.TotalHours < 1)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            else if (timeSpan.TotalDays < 1)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            else if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            else if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} tuần trước";
            else if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} tháng trước";
            else if (timeSpan.TotalDays >= 365)
                return $"{(int)(timeSpan.TotalDays / 365)} năm trước";
            else
                return dateTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
}