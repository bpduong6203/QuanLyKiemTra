using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageThongBao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
                }

                LoadThongBaoList();
            }
        }

        private void LoadThongBaoList()
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

                    var thongBaos = context.ThongBao_Users
                        .Where(t => t.UserID == user.Id)
                        .Select(t => new
                        {
                            t.Id,
                            TenKeHoach = t.KeHoach != null ? t.KeHoach.TenKeHoach : "Chưa có kế hoạch",
                            TenDonVi = t.KeHoach != null && t.KeHoach.DonVi != null ? t.KeHoach.DonVi.TenDonVi : "Không xác định",
                            t.NoiDung,
                            t.NgayTao,
                            t.DaXem,
                            t.redirectUrl
                        })
                        .OrderByDescending(t => t.NgayTao)
                        .ToList();

                    gvThongBao.DataSource = thongBaos;
                    gvThongBao.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách thông báo: {ex.Message}");
            }
        }

        protected void gvThongBao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string thongBaoId = gvThongBao.DataKeys[rowIndex]["Id"].ToString();

                using (var context = new MyDbContext())
                {
                    var thongBao = context.ThongBao_Users.FirstOrDefault(t => t.Id == thongBaoId);
                    if (thongBao == null)
                    {
                        ShowError("Thông báo không tồn tại.");
                        return;
                    }

                    if (e.CommandName == "Redirect")
                    {
                        // Đánh dấu đã xem nếu chưa xem
                        if (!thongBao.DaXem)
                        {
                            thongBao.DaXem = true;
                            context.SaveChanges();
                            ShowSuccess("Xác nhận đã xem thành công!");
                        }

                        // Chuyển hướng đến redirectUrl nếu có
                        if (!string.IsNullOrEmpty(thongBao.redirectUrl))
                        {
                            Response.Redirect(thongBao.redirectUrl);
                        }
                        else
                        {
                            ShowError("Không có liên kết để chuyển hướng.");
                        }
                    }
                }

                LoadThongBaoList();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        protected void gvThongBao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvThongBao.PageIndex = e.NewPageIndex;
            LoadThongBaoList();
        }

        protected void gvThongBao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool daXem = (bool)DataBinder.Eval(e.Row.DataItem, "DaXem");
                e.Row.CssClass = daXem ? "read" : "unread";
            }
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
            else
                return dateTime.ToString("dd/MM/yyyy HH:mm");
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
    }
}