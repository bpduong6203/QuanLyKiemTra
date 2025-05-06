using System;

namespace QuanLyKiemTra
{
    public partial class pageLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Xóa session
            Session.Clear(); // Xóa tất cả session
            Session.Abandon(); // Kết thúc session hiện tại

            // Chuyển hướng đến trang đăng nhập
            Response.Redirect("dang-nhap");
        }
    }
}