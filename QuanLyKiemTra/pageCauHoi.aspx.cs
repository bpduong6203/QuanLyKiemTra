using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageCauHoi : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                System.Diagnostics.Debug.WriteLine("Page_Load: Bắt đầu tải dữ liệu...");
                LoadCauHoiList();
                System.Diagnostics.Debug.WriteLine("Page_Load: Hoàn tất tải dữ liệu.");
            }
        }

        private void LoadCauHoiList()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LoadCauHoiList: Bắt đầu tải danh sách câu hỏi...");
                var cauHoiList = db.CauHois
                    .OrderBy(c => c.NoiDung)
                    .ToList();
                System.Diagnostics.Debug.WriteLine($"LoadCauHoiList: Số câu hỏi: {cauHoiList.Count}");
                foreach (var cauHoi in cauHoiList)
                {
                    System.Diagnostics.Debug.WriteLine($"ID: {cauHoi.Id}, Nội dung: {cauHoi.NoiDung}, Đáp án: {cauHoi.DapAn}, Ngày tạo: {cauHoi.NgayTao}");
                }
                gvCauHoi.DataSource = cauHoiList;
                gvCauHoi.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCauHoiList Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi tải danh sách câu hỏi: {ex.Message}");
            }
        }

        protected void btnSaveCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("btnSaveCauHoi_Click: Page không hợp lệ.");
                    ShowError("Vui lòng kiểm tra lại dữ liệu nhập.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("btnSaveCauHoi_Click: Bắt đầu lưu câu hỏi...");
                System.Diagnostics.Debug.WriteLine($"Nội dung: {txtNoiDung.Text}, Đáp án: {ddlDapAn.SelectedValue}, Link tài liệu: {txtLinkTaiLieu.Text}, Giải trình: {txtNdGiaiTrinh.Text}");

                using (var context = new MyDbContext())
                {
                    CauHoi cauHoi;
                    string cauHoiId = hfCauHoiId.Value;
                    System.Diagnostics.Debug.WriteLine($"Câu hỏi ID: {cauHoiId}");

                    if (string.IsNullOrEmpty(cauHoiId))
                    {
                        // Thêm mới
                        System.Diagnostics.Debug.WriteLine("Thêm câu hỏi mới...");
                        cauHoi = new CauHoi
                        {
                            Id = Guid.NewGuid().ToString(),
                            NoiDung = txtNoiDung.Text,
                            DapAn = bool.Parse(ddlDapAn.SelectedValue),
                            linkTaiLieu = txtLinkTaiLieu.Text,
                            ndGiaiTrinh = txtNdGiaiTrinh.Text,
                            NgayTao = DateTime.Now
                        };
                        context.CauHois.Add(cauHoi);
                    }
                    else
                    {
                        // Cập nhật
                        System.Diagnostics.Debug.WriteLine("Cập nhật câu hỏi...");
                        cauHoi = context.CauHois.Find(cauHoiId);
                        if (cauHoi == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Câu hỏi không tồn tại.");
                            ShowError("Câu hỏi không tồn tại.");
                            return;
                        }
                        cauHoi.NoiDung = txtNoiDung.Text;
                        cauHoi.DapAn = bool.Parse(ddlDapAn.SelectedValue);
                        cauHoi.linkTaiLieu = txtLinkTaiLieu.Text;
                        cauHoi.ndGiaiTrinh = txtNdGiaiTrinh.Text;
                    }

                    System.Diagnostics.Debug.WriteLine("Lưu thay đổi vào cơ sở dữ liệu...");
                    context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Lưu thành công.");
                    ShowSuccess(string.IsNullOrEmpty(cauHoiId) ? "Thêm câu hỏi thành công!" : "Cập nhật câu hỏi thành công!");
                    LoadCauHoiList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"btnSaveCauHoi_Click Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu câu hỏi: {ex.Message}");
            }
        }

        protected void gvCauHoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"gvCauHoi_RowCommand: CommandName={e.CommandName}, CommandArgument={e.CommandArgument}");
                string cauHoiId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    if (e.CommandName == "EditCauHoi")
                    {
                        var cauHoi = context.CauHois.Find(cauHoiId);
                        if (cauHoi != null)
                        {
                            txtNoiDung.Text = cauHoi.NoiDung;
                            ddlDapAn.SelectedValue = cauHoi.DapAn.ToString().ToLower();
                            txtLinkTaiLieu.Text = cauHoi.linkTaiLieu;
                            txtNdGiaiTrinh.Text = cauHoi.ndGiaiTrinh;
                            hfCauHoiId.Value = cauHoi.Id;
                            ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openCauHoiModal();", true);
                        }
                    }
                    else if (e.CommandName == "DeleteCauHoi")
                    {
                        var cauHoi = context.CauHois.Find(cauHoiId);
                        if (cauHoi == null)
                        {
                            ShowError("Câu hỏi không tồn tại.");
                            return;
                        }

                        // Kiểm tra ràng buộc khóa ngoại
                        if (context.CTBoCauHois.Any(ct => ct.CauHoiId == cauHoiId) ||
                            context.DapAns.Any(d => d.CauHoiId == cauHoiId))
                        {
                            ShowError("Không thể xóa câu hỏi vì có dữ liệu liên quan.");
                            return;
                        }

                        context.CauHois.Remove(cauHoi);
                        context.SaveChanges();
                        ShowSuccess("Xóa câu hỏi thành công!");
                        LoadCauHoiList();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"gvCauHoi_RowCommand Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi: {ex.Message}");
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