using QuanLyKiemTra.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pagePhanCong : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Trang phân công";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }
                LoadKeHoach();
                LoadThanhVien();
                LoadDaPhanCong();
            }
        }

        private void LoadKeHoach()
        {
            try
            {
                var keHoachList = db.KeHoachs.OrderBy(k => k.TenKeHoach).ToList();
                rptKeHoach.DataSource = keHoachList;
                rptKeHoach.DataBind();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách kế hoạch: {ex.Message}");
            }
        }

        protected void rptKeHoach_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeHoach keHoach = (KeHoach)e.Item.DataItem;
                CheckBox chkKeHoach = (CheckBox)e.Item.FindControl("chkKeHoach");
                chkKeHoach.Checked = hfSelectedKeHoach.Value == keHoach.Id;
            }
        }

        protected void chkKeHoach_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                hfSelectedKeHoach.Value = chk.Attributes["data-id"];
            }
            else if (hfSelectedKeHoach.Value == chk.Attributes["data-id"])
            {
                hfSelectedKeHoach.Value = "";
            }

            LoadKeHoach();
            LoadThanhVien();
            LoadDaPhanCong();
        }

        private void LoadDaPhanCong()
        {
            try
            {
                if (!string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    var daPhanCong = db.PhanCong_Users
                        .Include(p => p.NguoiDung)
                        .Where(p => p.KeHoachID == hfSelectedKeHoach.Value)
                        .ToList();
                    gvDaPhanCong.DataSource = daPhanCong;
                    gvDaPhanCong.DataBind();
                }
                else
                {
                    gvDaPhanCong.DataSource = null;
                    gvDaPhanCong.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách đã phân công: {ex.Message}");
            }
        }

        private void LoadThanhVien()
        {
            try
            {
                if (!string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    var thanhVienRole = db.Roles.FirstOrDefault(r => r.Ten == "ThanhVien");
                    if (thanhVienRole != null)
                    {
                        var thanhVienList = db.NguoiDungs
                            .Where(u => u.RoleID == thanhVienRole.Id)
                            .Select(u => new
                            {
                                u.Id,
                                u.HoTen,
                                ChucVu = u.RoleID != null ? db.Roles.FirstOrDefault(r => r.Id == u.RoleID).Ten : "Không có vai trò"
                            })
                            .OrderBy(u => u.HoTen)
                            .ToList();

                        gvThanhVien.DataSource = thanhVienList;
                        gvThanhVien.DataBind();
                    }
                    else
                    {
                        ShowError("Không tìm thấy vai trò ThanhVien!");
                        gvThanhVien.DataSource = null;
                        gvThanhVien.DataBind();
                    }
                }
                else
                {
                    gvThanhVien.DataSource = null;
                    gvThanhVien.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tải danh sách người dùng: {ex.Message}");
            }
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfSelectedKeHoach.Value))
                {
                    ShowError("Vui lòng chọn kế hoạch!");
                    return;
                }

                bool hasSelected = false;
                foreach (GridViewRow row in gvThanhVien.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        hasSelected = true;
                        break;
                    }
                }

                if (!hasSelected)
                {
                    ShowError("Vui lòng chọn ít nhất một thành viên!");
                    return;
                }

                var keHoach = db.KeHoachs.FirstOrDefault(k => k.Id == hfSelectedKeHoach.Value);
                if (keHoach == null)
                {
                    ShowError("Kế hoạch không tồn tại!");
                    return;
                }

                foreach (GridViewRow row in gvThanhVien.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string userId = gvThanhVien.DataKeys[row.RowIndex].Value.ToString();
                        TextBox txtNhiemVu = (TextBox)row.FindControl("txtNhiemVu");
                        string noiDungCV = txtNhiemVu.Text.Trim();

                        var phanCong = new PhanCong_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            KeHoachID = hfSelectedKeHoach.Value,
                            UserID = userId,
                            NoiDungCV = noiDungCV,
                            ngayTao = DateTime.Now
                        };
                        db.PhanCong_Users.Add(phanCong);

                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            KeHoachID = hfSelectedKeHoach.Value,
                            UserID = userId,
                            NoiDung = $"Bạn được phân công nhiệm vụ '{noiDungCV}' trong kế hoạch '{keHoach.TenKeHoach}'.",
                            NgayTao = DateTime.Now,
                            redirectUrl = $"/chi-tiet-ke-hoach/{hfSelectedKeHoach.Value}",
                            DaXem = false
                        };
                        db.ThongBao_Users.Add(thongBao);
                    }
                }

                db.SaveChanges();
                ShowSuccess("Lưu phân công và gửi thông báo thành công!");
                LoadKeHoach();
                LoadThanhVien();
                LoadDaPhanCong();
                Response.Redirect("phan-cong");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lưu phân công: {ex.Message}");
            }
        }

        protected void btnExportPhanCong_Click(object sender, EventArgs e)
        {
            // Thêm logic xuất văn bản phân công (giữ nguyên)
        }

        protected void gvThanhVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Thêm logic nếu cần (giữ nguyên)
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