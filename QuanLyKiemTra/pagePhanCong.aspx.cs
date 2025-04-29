using QuanLyKiemTra.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pagePhanCong : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("pageLogin.aspx");
                }
                LoadKeHoach();
                LoadThanhVien();
                LoadDaPhanCong();
            }
        }

        private void LoadDaPhanCong()
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlKeHoach.SelectedValue))
                {
                    var daPhanCong = db.PhanCong_Users
                        .Where(p => p.KeHoachID == ddlKeHoach.SelectedValue)
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
                Response.Write($"<script>alert('Lỗi khi tải danh sách đã phân công: {ex.Message}');</script>");
            }
        }

        private void LoadKeHoach()
        {
            try
            {
                var keHoachList = db.KeHoachs.ToList();
                ddlKeHoach.DataSource = keHoachList;
                ddlKeHoach.DataTextField = "TenKeHoach";
                ddlKeHoach.DataValueField = "Id";
                ddlKeHoach.DataBind();
                ddlKeHoach.Items.Insert(0, new ListItem("-- Chọn kế hoạch --", ""));
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách kế hoạch: {ex.Message}');</script>");
            }
        }

        private void LoadThanhVien()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    if (!string.IsNullOrEmpty(ddlKeHoach.SelectedValue))
                    {
                        // Lấy RoleID của vai trò ThanhVien
                        var thanhVienRole = context.Roles.FirstOrDefault(r => r.Ten == "ThanhVien");
                        if (thanhVienRole != null)
                        {
                            var thanhVienList = context.NguoiDungs
                                .Include("Roles")
                                .Where(u => u.RoleID == thanhVienRole.Id)
                                .Select(u => new
                                {
                                    u.Id,
                                    u.HoTen,
                                    ChucVu = u.Roles != null ? u.Roles.Ten : "Không có vai trò"
                                })
                                .OrderBy(u => u.HoTen)
                                .ToList();

                            // Debug
                            Response.Write($"Số thành viên ThanhVien: {thanhVienList.Count}<br/>");

                            gvThanhVien.DataSource = thanhVienList;
                            gvThanhVien.DataBind();
                        }
                        else
                        {
                            Response.Write("<script>alert('Không tìm thấy vai trò ThanhVien!');</script>");
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
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi tải danh sách người dùng: {ex.Message}');</script>");
                System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}\n{ex.StackTrace}");
                gvThanhVien.DataSource = null;
                gvThanhVien.DataBind();
            }
        }

        protected void ddlKeHoach_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadThanhVien();
            LoadDaPhanCong();
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlKeHoach.SelectedValue))
                {
                    Response.Write("<script>alert('Vui lòng chọn kế hoạch!');</script>");
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
                    Response.Write("<script>alert('Vui lòng chọn ít nhất một thành viên!');</script>");
                    return;
                }

                var keHoach = db.KeHoachs.FirstOrDefault(k => k.Id == ddlKeHoach.SelectedValue);
                if (keHoach == null)
                {
                    Response.Write("<script>alert('Kế hoạch không tồn tại!');</script>");
                    return;
                }

                foreach (GridViewRow row in gvThanhVien.Rows)
                {
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        string userId = gvThanhVien.DataKeys[row.RowIndex].Value.ToString();
                        TextBox txtNhiemVu = (TextBox)row.FindControl("txtNhiemVu");
                        string noiDungCV = txtNhiemVu.Text;

                        var phanCong = new PhanCong_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            KeHoachID = ddlKeHoach.SelectedValue,
                            UserID = userId,
                            NoiDungCV = noiDungCV,
                            ngayTao = DateTime.Now
                        };
                        db.PhanCong_Users.Add(phanCong);

                        var thongBao = new ThongBao_User
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserID = userId,
                            NoiDung = $"Bạn được phân công nhiệm vụ '{noiDungCV}' trong kế hoạch '{keHoach.TenKeHoach}'.",
                            NgayTao = DateTime.Now,
                            DaXem = false
                        };
                        db.ThongBao_Users.Add(thongBao);
                    }
                }

                db.SaveChanges();
                Response.Write("<script>alert('Lưu phân công và gửi thông báo thành công!');</script>");
                LoadThanhVien();
                LoadDaPhanCong();
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Lỗi khi lưu phân công: {ex.Message}');</script>");
            }
        }

        protected void btnExportPhanCong_Click(object sender, EventArgs e)
        {
            // Thêm logic xuất văn bản phân công
        }

        protected void gvThanhVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Thêm logic nếu cần
        }
    }

}