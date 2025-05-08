using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCauHoi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Trang quản lý Câu Hỏi";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                LoadCauHoiList();
            }
        }

        private void LoadCauHoiList()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var cauHois = context.CauHois.OrderBy(c => c.NgayTao).ToList();
                    gvCauHoi.DataSource = cauHois;
                    gvCauHoi.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách câu hỏi: {ex.Message}");
            }
        }

        protected void btnSaveCauHoi_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (string.IsNullOrWhiteSpace(txtNoiDung.Text))
            {
                ShowError("Nội dung câu hỏi là bắt buộc.");
                return;
            }

            try
            {
                using (var context = new MyDbContext())
                {
                    CauHoi cauHoi;
                    bool isEdit = !string.IsNullOrWhiteSpace(hfCauHoiId.Value);

                    if (isEdit)
                    {
                        // Chế độ chỉnh sửa
                        cauHoi = context.CauHois.Find(hfCauHoiId.Value);
                        if (cauHoi == null)
                        {
                            ShowError("Câu hỏi không tồn tại.");
                            return;
                        }
                    }
                    else
                    {
                        // Chế độ thêm mới
                        cauHoi = new CauHoi
                        {
                            Id = Guid.NewGuid().ToString(),
                            NgayTao = DateTime.Now
                        };
                        context.CauHois.Add(cauHoi);
                    }

                    // Cập nhật thông tin câu hỏi
                    cauHoi.NoiDung = txtNoiDung.Text;
                    cauHoi.DapAn = chkDapAn.Checked;
                    cauHoi.linkTaiLieu = txtLinkTaiLieu.Text;
                    cauHoi.ndGiaiTrinh = txtNdGiaiTrinh.Text;

                    context.SaveChanges();

                    ShowSuccess(isEdit ? "Cập nhật câu hỏi thành công!" : "Thêm câu hỏi thành công!");
                    LoadCauHoiList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu câu hỏi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu câu hỏi: {ex.Message}");
            }
        }

        protected void gvCauHoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string cauHoiId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    if (e.CommandName == "DeleteCauHoi")
                    {
                        var cauHoi = context.CauHois.Find(cauHoiId);
                        if (cauHoi == null)
                        {
                            ShowError("Câu hỏi không tồn tại.");
                            return;
                        }

                        // Kiểm tra xem câu hỏi có được sử dụng trong CTBoCauHoi không
                        if (context.CTBoCauHois.Any(ct => ct.CauHoiId == cauHoiId))
                        {
                            ShowError("Không thể xóa câu hỏi vì đã được sử dụng trong bộ câu hỏi.");
                            return;
                        }

                        context.CauHois.Remove(cauHoi);
                        context.SaveChanges();

                        ShowSuccess("Xóa câu hỏi thành công!");
                        LoadCauHoiList();
                    }
                    else if (e.CommandName == "EditCauHoi")
                    {
                        var cauHoi = context.CauHois.Find(cauHoiId);
                        if (cauHoi != null)
                        {
                            txtNoiDung.Text = cauHoi.NoiDung;
                            chkDapAn.Checked = cauHoi.DapAn;
                            txtLinkTaiLieu.Text = cauHoi.linkTaiLieu;
                            txtNdGiaiTrinh.Text = cauHoi.ndGiaiTrinh;
                            hfCauHoiId.Value = cauHoi.Id;

                            // Mở modal với chế độ chỉnh sửa
                            ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openAddModal(true);", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi xử lý hành động: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi: {ex.Message}");
            }
        }

        protected void gvCauHoi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCauHoi.PageIndex = e.NewPageIndex;
            LoadCauHoiList();
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