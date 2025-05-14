using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private MyDbContext db = new MyDbContext();
        protected int NotificationCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
                    LoadNotifications();
                }
                else
                {
                    Response.Redirect("dang-nhap");
                }

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
                            t.NgayTao,
                            t.redirectUrl
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
                // Ghi log lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi tải thông báo: {ex.Message}");
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
            try
            {
                string thongBaoId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    var thongBao = context.ThongBao_Users.FirstOrDefault(t => t.Id == thongBaoId);
                    if (thongBao == null)
                    {
                        // Hiển thị thông báo lỗi bằng toastr
                        hfMessageType.Value = "error";
                        hfMessage.Value = "Thông báo không tồn tại.";
                        return;
                    }

                    if (e.CommandName == "Redirect")
                    {
                        // Đánh dấu thông báo là đã xem
                        if (!thongBao.DaXem)
                        {
                            thongBao.DaXem = true;
                            context.SaveChanges();
                        }

                        // Chuyển hướng đến redirectUrl
                        if (!string.IsNullOrEmpty(thongBao.redirectUrl))
                        {
                            Response.Redirect(thongBao.redirectUrl);
                        }
                        else
                        {
                            // Hiển thị thông báo lỗi nếu redirectUrl rỗng
                            hfMessageType.Value = "error";
                            hfMessage.Value = "Không tìm thấy liên kết để chuyển hướng.";
                        }
                    }
                }

                // Làm mới danh sách thông báo
                LoadNotifications();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi xử lý thông báo: {ex.Message}");
                hfMessageType.Value = "error";
                hfMessage.Value = "Đã xảy ra lỗi khi xử lý thông báo.";
            }
        }

        // Phương thức kiểm tra vai trò
        protected bool IsRoleAuthorized(params string[] allowedRoles)
        {
            if (Session["Username"] == null) return false;

            string username = Session["Username"].ToString();
            var user = db.NguoiDungs.FirstOrDefault(u => u.username == username);
            if (user == null || user.RoleID == null) return false;

            var role = db.Roles.FirstOrDefault(r => r.Id == user.RoleID);
            if (role == null) return false;

            return allowedRoles.Contains(role.Ten);
        }
    }
}