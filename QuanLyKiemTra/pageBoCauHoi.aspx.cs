using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageBoCauHoi : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
                }

                System.Diagnostics.Debug.WriteLine("Page_Load: Bắt đầu tải dữ liệu...");
                LoadBoCauHoiList();
                LoadCauHoiForSelection();
                System.Diagnostics.Debug.WriteLine("Page_Load: Hoàn tất tải dữ liệu.");
            }
        }

        private void LoadBoCauHoiList()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LoadBoCauHoiList: Bắt đầu tải danh sách bộ câu hỏi...");
                var boCauHoiList = db.BoCauHois
                    .OrderBy(b => b.TenBoCauHoi)
                    .ToList();
                System.Diagnostics.Debug.WriteLine($"LoadBoCauHoiList: Số bộ câu hỏi: {boCauHoiList.Count}");
                foreach (var boCauHoi in boCauHoiList)
                {
                    System.Diagnostics.Debug.WriteLine($"ID: {boCauHoi.Id}, Tên: {boCauHoi.TenBoCauHoi}, Ngày tạo: {boCauHoi.NgayTao}");
                }
                gvBoCauHoi.DataSource = boCauHoiList;
                gvBoCauHoi.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadBoCauHoiList Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi tải danh sách bộ câu hỏi: {ex.Message}");
            }
        }

        private void LoadCauHoiForSelection()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LoadCauHoiForSelection: Bắt đầu tải danh sách câu hỏi để chọn...");
                var query = db.CauHois.AsQueryable();

                // Tìm kiếm
                if (!string.IsNullOrEmpty(txtSearchCauHoi.Text))
                {
                    System.Diagnostics.Debug.WriteLine($"Tìm kiếm: {txtSearchCauHoi.Text}");
                    query = query.Where(c => c.NoiDung.Contains(txtSearchCauHoi.Text));
                }

                // Lọc theo ngày tạo
                if (!string.IsNullOrEmpty(txtNgayTaoFrom.Text))
                {
                    if (DateTime.TryParseExact(txtNgayTaoFrom.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fromDate))
                    {
                        System.Diagnostics.Debug.WriteLine($"Lọc từ ngày: {fromDate}");
                        query = query.Where(c => c.NgayTao >= fromDate);
                    }
                }
                if (!string.IsNullOrEmpty(txtNgayTaoTo.Text))
                {
                    if (DateTime.TryParseExact(txtNgayTaoTo.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime toDate))
                    {
                        toDate = toDate.AddDays(1).AddTicks(-1); // Bao gồm cả ngày kết thúc
                        System.Diagnostics.Debug.WriteLine($"Lọc đến ngày: {toDate}");
                        query = query.Where(c => c.NgayTao <= toDate);
                    }
                }

                var cauHoiList = query
                    .OrderBy(c => c.NoiDung)
                    .ToList();
                System.Diagnostics.Debug.WriteLine($"LoadCauHoiForSelection: Số câu hỏi: {cauHoiList.Count}");
                gvSelectCauHoi.DataSource = cauHoiList;
                gvSelectCauHoi.DataBind();

                // Đánh dấu các câu hỏi đã được chọn trong bộ câu hỏi (nếu đang sửa)
                if (!string.IsNullOrEmpty(hfBoCauHoiId.Value))
                {
                    var selectedCauHoiIds = db.CTBoCauHois
                        .Where(ct => ct.BoCauHoiId == hfBoCauHoiId.Value)
                        .Select(ct => ct.CauHoiId)
                        .ToList();
                    System.Diagnostics.Debug.WriteLine($"LoadCauHoiForSelection: Số câu hỏi đã chọn: {selectedCauHoiIds.Count}");

                    foreach (GridViewRow row in gvSelectCauHoi.Rows)
                    {
                        var chkSelect = (CheckBox)row.FindControl("chkSelectCauHoi");
                        var cauHoiId = gvSelectCauHoi.DataKeys[row.RowIndex].Value.ToString();
                        chkSelect.Checked = selectedCauHoiIds.Contains(cauHoiId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCauHoiForSelection Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi tải danh sách câu hỏi để chọn: {ex.Message}");
            }
        }

        protected void txtSearchCauHoi_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtSearchCauHoi_TextChanged: Tìm kiếm câu hỏi...");
            LoadCauHoiForSelection();
        }

        protected void txtNgayTaoFilter_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("txtNgayTaoFilter_TextChanged: Lọc câu hỏi theo ngày tạo...");
            LoadCauHoiForSelection();
        }

        protected void btnSaveBoCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("btnSaveBoCauHoi_Click: Page không hợp lệ.");
                    ShowError("Vui lòng kiểm tra lại dữ liệu nhập.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("btnSaveBoCauHoi_Click: Bắt đầu lưu bộ câu hỏi...");
                System.Diagnostics.Debug.WriteLine($"Tên bộ câu hỏi: {txtTenBoCauHoi.Text}");

                using (var context = new MyDbContext())
                {
                    BoCauHoi boCauHoi;
                    string boCauHoiId = hfBoCauHoiId.Value;
                    System.Diagnostics.Debug.WriteLine($"Bộ câu hỏi ID: {boCauHoiId}");

                    if (string.IsNullOrEmpty(boCauHoiId))
                    {
                        // Thêm mới
                        System.Diagnostics.Debug.WriteLine("Thêm bộ câu hỏi mới...");
                        boCauHoi = new BoCauHoi
                        {
                            Id = Guid.NewGuid().ToString(),
                            TenBoCauHoi = txtTenBoCauHoi.Text,
                            NgayTao = DateTime.Now
                        };
                        context.BoCauHois.Add(boCauHoi);
                    }
                    else
                    {
                        // Cập nhật
                        System.Diagnostics.Debug.WriteLine("Cập nhật bộ câu hỏi...");
                        boCauHoi = context.BoCauHois.Find(boCauHoiId);
                        if (boCauHoi == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Bộ câu hỏi không tồn tại.");
                            ShowError("Bộ câu hỏi không tồn tại.");
                            return;
                        }
                        boCauHoi.TenBoCauHoi = txtTenBoCauHoi.Text;
                    }

                    // Lưu danh sách câu hỏi được chọn
                    var selectedCauHoiIds = new System.Collections.Generic.List<string>();
                    foreach (GridViewRow row in gvSelectCauHoi.Rows)
                    {
                        var chkSelect = (CheckBox)row.FindControl("chkSelectCauHoi");
                        if (chkSelect.Checked)
                        {
                            var cauHoiId = gvSelectCauHoi.DataKeys[row.RowIndex].Value.ToString();
                            selectedCauHoiIds.Add(cauHoiId);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine($"btnSaveBoCauHoi_Click: Số câu hỏi được chọn: {selectedCauHoiIds.Count}");

                    // Xóa các câu hỏi cũ trong CTBoCauHoi
                    if (!string.IsNullOrEmpty(boCauHoiId))
                    {
                        var oldCTBoCauHois = context.CTBoCauHois.Where(ct => ct.BoCauHoiId == boCauHoiId).ToList();
                        System.Diagnostics.Debug.WriteLine($"Xóa {oldCTBoCauHois.Count} liên kết cũ...");
                        context.CTBoCauHois.RemoveRange(oldCTBoCauHois);
                    }

                    // Thêm các câu hỏi mới vào CTBoCauHoi
                    foreach (var cauHoiId in selectedCauHoiIds)
                    {
                        context.CTBoCauHois.Add(new CTBoCauHoi
                        {
                            Id = Guid.NewGuid().ToString(),
                            BoCauHoiId = boCauHoi.Id,
                            CauHoiId = cauHoiId
                        });
                    }
                    System.Diagnostics.Debug.WriteLine($"Thêm {selectedCauHoiIds.Count} liên kết mới...");

                    context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Lưu thành công.");
                    ShowSuccess(string.IsNullOrEmpty(boCauHoiId) ? "Thêm bộ câu hỏi thành công!" : "Cập nhật bộ câu hỏi thành công!");
                    LoadBoCauHoiList();
                    LoadCauHoiForSelection();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"btnSaveBoCauHoi_Click Lỗi: {ex.Message}\n{ex.StackTrace}");
                ShowError($"Lỗi khi lưu bộ câu hỏi: {ex.Message}");
            }
        }

        protected void gvBoCauHoi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"gvBoCauHoi_RowCommand: CommandName={e.CommandName}, CommandArgument={e.CommandArgument}");
                string boCauHoiId = e.CommandArgument.ToString();
                using (var context = new MyDbContext())
                {
                    if (e.CommandName == "EditBoCauHoi")
                    {
                        var boCauHoi = context.BoCauHois.Find(boCauHoiId);
                        if (boCauHoi != null)
                        {
                            txtTenBoCauHoi.Text = boCauHoi.TenBoCauHoi;
                            hfBoCauHoiId.Value = boCauHoi.Id;
                            LoadCauHoiForSelection();
                            ScriptManager.RegisterStartupScript(this, GetType(), "openModal", "openBoCauHoiModal();", true);
                        }
                    }
                    else if (e.CommandName == "DeleteBoCauHoi")
                    {
                        var boCauHoi = context.BoCauHois.Find(boCauHoiId);
                        if (boCauHoi == null)
                        {
                            ShowError("Bộ câu hỏi không tồn tại.");
                            return;
                        }

                        // Kiểm tra ràng buộc khóa ngoại
                        if (context.BoCauHoi_KeHoachs.Any(bk => bk.BoCauHoiId == boCauHoiId))
                        {
                            ShowError("Không thể xóa bộ câu hỏi vì đã được sử dụng trong kế hoạch.");
                            return;
                        }

                        // Xóa các bản ghi liên quan trong CTBoCauHoi
                        var ctBoCauHois = context.CTBoCauHois.Where(ct => ct.BoCauHoiId == boCauHoiId).ToList();
                        context.CTBoCauHois.RemoveRange(ctBoCauHois);

                        context.BoCauHois.Remove(boCauHoi);
                        context.SaveChanges();
                        ShowSuccess("Xóa bộ câu hỏi thành công!");
                        LoadBoCauHoiList();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"gvBoCauHoi_RowCommand Lỗi: {ex.Message}\n{ex.StackTrace}");
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