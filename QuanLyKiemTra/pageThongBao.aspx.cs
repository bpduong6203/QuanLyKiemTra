using QuanLyKiemTra.Models;
using System;
using System.IO;
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
                LoadInspectionPlans();
            }
        }


        private void LoadThongBaoList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    string username = Session["Username"]?.ToString();
                    var user = context.NguoiDungs.FirstOrDefault(u => u.username == username);
                    if (user == null)
                    {
                        ShowError("Không tìm thấy người dùng hiện tại. Vui lòng đăng nhập lại.");
                        return;
                    }

                    var thongBaos = context.ThongBao_Users
                        .Where(t => t.UserID == user.Id)
                        .Select(t => new
                        {
                            t.Id,
                            TenKeHoach = t.KeHoach != null ? t.KeHoach.TenKeHoach : "Chưa có kế hoạch",
                            TenDonVi = t.KeHoach != null && t.KeHoach.DonVi != null ? t.KeHoach.DonVi.TenDonVi : "Không xác định",
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

        private void LoadInspectionPlans()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var plans = context.KeHoachs
                        .OrderBy(k => k.TenKeHoach)
                        .Select(k => new { k.Id, k.TenKeHoach })
                        .ToList();

                    ddlInspectionPlans.DataSource = plans;
                    ddlInspectionPlans.DataTextField = "TenKeHoach";
                    ddlInspectionPlans.DataValueField = "Id";
                    ddlInspectionPlans.DataBind();
                    ddlInspectionPlans.Items.Insert(0, new ListItem("-- Chọn kế hoạch --", ""));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách kế hoạch: {ex.Message}");
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
                            LoadThongBaoList(); // Tải lại danh sách
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

        protected void ddlInspectionPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlInspectionPlans.SelectedValue))
            {
                ShowError("Vui lòng chọn kế hoạch kiểm tra.");
            }
        }

        protected void btnSendNotification_Click(object sender, EventArgs e)
        {
            try
            {
                string keHoachId = ddlInspectionPlans.SelectedValue;
                if (string.IsNullOrEmpty(keHoachId))
                {
                    ShowError("Vui lòng chọn kế hoạch kiểm tra.");
                    return;
                }

                using (var context = new MyDbContext())
                {
                    var keHoach = context.KeHoachs.FirstOrDefault(k => k.Id == keHoachId);
                    if (keHoach == null)
                    {
                        ShowError("Kế hoạch không tồn tại.");
                        return;
                    }

                    // Gửi thông báo đến người dùng của đơn vị
                    var users = context.NguoiDungs
                        .Where(u => u.DonViID == keHoach.DonViID)
                        .ToList();

                    foreach (var user in users)
                    {
                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserID = user.Id,
                            KeHoachID = keHoachId,
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        context.ThongBao_Users.Add(thongBao);
                    }

                    context.SaveChanges();
                    ShowSuccess("Gửi thông báo thành công!");
                    LoadThongBaoList();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi gửi thông báo: {ex.Message}");
            }
        }

        protected void btnExportBienBan_Click(object sender, EventArgs e)
        {
            try
            {
                string keHoachId = ddlInspectionPlans.SelectedValue;
                if (string.IsNullOrEmpty(keHoachId))
                {
                    ShowError("Vui lòng chọn kế hoạch kiểm tra.");
                    return;
                }

                using (var context = new MyDbContext())
                {
                    var keHoach = context.KeHoachs
                        .FirstOrDefault(k => k.Id == keHoachId);
                    if (keHoach == null || string.IsNullOrEmpty(keHoach.BienBanID))
                    {
                        ShowError("Kế hoạch không có biên bản liên kết.");
                        return;
                    }

                    var bienBan = context.BienBanKiemTras
                        .FirstOrDefault(b => b.Id == keHoach.BienBanID);
                    if (bienBan == null || string.IsNullOrEmpty(bienBan.linkfile))
                    {
                        ShowError("Không tìm thấy file biên bản.");
                        return;
                    }

                    string filePath = Server.MapPath(bienBan.linkfile);
                    if (!File.Exists(filePath))
                    {
                        ShowError("File biên bản không tồn tại trên server.");
                        return;
                    }

                    string fileName = Path.GetFileName(bienBan.linkfile);
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi xuất biên bản: {ex.Message}");
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