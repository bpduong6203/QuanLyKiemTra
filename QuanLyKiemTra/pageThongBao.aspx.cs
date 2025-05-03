
using QuanLyKiemTra.Models;
using System;
using System.Linq;
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
                    var thongBaos = context.ThongBao_Users
                        .Select(t => new
                        {
                            t.Id,
                            TenKeHoach = t.KeHoach != null ? t.KeHoach.TenKeHoach : "Chưa có kế hoạch",
                            TenDonVi = t.KeHoach != null && t.KeHoach.DonVi != null ? t.KeHoach.DonVi.TenDonVi : "Không xác định",
                            ChucVuNguoiDaiDien = t.KeHoach != null && t.KeHoach.DonVi != null ? t.KeHoach.DonVi.ChucVuNguoiDaiDien : "Không xác định",
                            t.NoiDung,
                            t.NgayTao,
                            t.DaXem,
                            LinkFile = t.KeHoach != null && t.KeHoach.BienBanKiemTra != null ? t.KeHoach.BienBanKiemTra.linkfile : null
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
            if (e.CommandName == "ConfirmView")
            {
                try
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string thongBaoId = gvThongBao.DataKeys[rowIndex]["Id"].ToString();

                    using (var context = new MyDbContext())
                    {
                        var thongBao = context.ThongBao_Users.FirstOrDefault(t => t.Id == thongBaoId);
                        if (thongBao != null && !thongBao.DaXem)
                        {
                            thongBao.DaXem = true;
                            context.SaveChanges();
                            ShowSuccess("Xác nhận đã xem thành công!");
                            LoadThongBaoList();
                        }
                        else
                        {
                            ShowError("Thông báo không tồn tại hoặc đã được xác nhận.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Lỗi khi xác nhận đã xem: {ex.Message}");
                }
            }
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