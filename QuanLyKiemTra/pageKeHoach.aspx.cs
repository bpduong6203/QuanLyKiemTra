using System;
using System.Data;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKeHoach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindThanhVienGrid();
            }
        }

        private void BindThanhVienGrid()
        {
            // Giả lập danh sách thành viên (thay bằng truy vấn database)
            DataTable dt = new DataTable();
            dt.Columns.Add("MaThanhVien");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("ChucVu");
            dt.Rows.Add("1", "Nguyen Van A", "Trưởng nhóm");
            dt.Rows.Add("2", "Tran Thi B", "Thành viên");
            dt.Rows.Add("3", "Le Van C", "Thành viên");

            gvThanhVien.DataSource = dt;
            gvThanhVien.DataBind();
        }

        protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Logic khi chọn đơn vị (nếu cần)
            lblMessage.Text = $"Đã chọn đơn vị: {ddlDonVi.SelectedItem.Text}";
            lblMessage.CssClass = "message-label success";
            lblMessage.Visible = true;
        }

        protected void btnExportQuyetDinh_Click(object sender, EventArgs e)
        {
            // Logic xuất quyết định kiểm tra (PDF/Word)
            try
            {
                // Giả lập tạo file PDF
                lblMessage.Text = "Xuất quyết định kiểm tra thành công!";
                lblMessage.CssClass = "message-label success";
                lblMessage.Visible = true;

                // Thực tế: Tạo và gửi file về client
                // Response.ContentType = "application/pdf";
                // Response.AddHeader("Content-Disposition", "attachment;filename=QuyetDinh.pdf");
                // Response.BinaryWrite(pdfBytes);
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi xuất quyết định: {ex.Message}";
                lblMessage.CssClass = "message-label error";
                lblMessage.Visible = true;
            }
        }

        protected void btnExportPhanCong_Click(object sender, EventArgs e)
        {
            // Logic xuất văn bản phân công
            try
            {
                // Giả lập tạo file PDF
                lblMessage.Text = "Xuất văn bản phân công thành công!";
                lblMessage.CssClass = "message-label success";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi xuất phân công: {ex.Message}";
                lblMessage.CssClass = "message-label error";
                lblMessage.Visible = true;
            }
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Lưu đơn vị
                    string donVi = ddlDonVi.SelectedValue;

                    // Lưu file quyết định
                    if (fuQuyetDinh.HasFile)
                    {
                        string filePath = Server.MapPath("~/Uploads/") + fuQuyetDinh.FileName;
                        fuQuyetDinh.SaveAs(filePath);
                    }

                    // Lưu file đề cương
                    if (fuDeCuong.HasFile)
                    {
                        string filePath = Server.MapPath("~/Uploads/") + fuDeCuong.FileName;
                        fuDeCuong.SaveAs(filePath);
                    }

                    // Lưu tài liệu liên quan
                    if (fuTaiLieu.HasFiles)
                    {
                        foreach (var file in fuTaiLieu.PostedFiles)
                        {
                            string filePath = Server.MapPath("~/Uploads/") + file.FileName;
                            file.SaveAs(filePath);
                        }
                    }

                    // Lưu phân công thành viên
                    foreach (GridViewRow row in gvThanhVien.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        TextBox txtNhiemVu = (TextBox)row.FindControl("txtNhiemVu");
                        if (chkSelect.Checked)
                        {
                            string maThanhVien = gvThanhVien.DataKeys[row.RowIndex]["MaThanhVien"].ToString();
                            string nhiemVu = txtNhiemVu.Text;
                            // Lưu vào database: maThanhVien, nhiemVu
                        }
                    }

                    lblMessage.Text = "Lưu kế hoạch kiểm tra thành công!";
                    lblMessage.CssClass = "message-label success";
                    lblMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Lỗi khi lưu kế hoạch: {ex.Message}";
                    lblMessage.CssClass = "message-label error";
                    lblMessage.Visible = true;
                }
            }
        }

        protected void gvThanhVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Xử lý lệnh trong GridView nếu cần
        }
    }
}