using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected int NotificationCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNotifications();
                this.DataBind();
            }
        }

        private void LoadNotifications()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    string username = Session["Username"]?.ToString();
                    if (string.IsNullOrEmpty(username))
                    {
                        NotificationCount = 0;
                        rptNotifications.DataSource = null;
                        rptNotifications.DataBind();
                        return;
                    }

                    var user = context.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (user == null)
                    {
                        NotificationCount = 0;
                        rptNotifications.DataSource = null;
                        rptNotifications.DataBind();
                        return;
                    }

                    var notifications = context.ThongBao_Users
                        .Where(t => t.UserID == user.Id && !t.DaXem)
                        .Select(t => new
                        {
                            t.Id,
                            t.NoiDung,
                            t.NgayTao
                        })
                        .OrderByDescending(t => t.NgayTao)
                        .ToList();

                    NotificationCount = notifications.Count;
                    rptNotifications.DataSource = notifications;
                    rptNotifications.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                NotificationCount = 0;
                rptNotifications.DataSource = null;
                rptNotifications.DataBind();
            }
        }

        protected string FormatTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            if (timeSpan.TotalMinutes < 1)
                return "Vừa xong";
            if (timeSpan.TotalHours < 1)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalDays < 1)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            return $"{(int)timeSpan.TotalDays} ngày trước";
        }

        protected void rptNotifications_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MarkAsRead")
            {
                try
                {
                    string thongBaoId = e.CommandArgument.ToString();
                    using (var context = new MyDbContext())
                    {
                        var thongBao = context.ThongBao_Users.FirstOrDefault(t => t.Id == thongBaoId);
                        if (thongBao != null && !thongBao.DaXem)
                        {
                            thongBao.DaXem = true;
                            context.SaveChanges();
                        }
                    }
                    LoadNotifications(); // Làm mới danh sách thông báo
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                }
            }
        }
    }
}