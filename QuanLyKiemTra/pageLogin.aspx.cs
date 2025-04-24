using System;

namespace QuanLyKiemTra
{
    public partial class pageLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (username == "admin" && password == "12345")
            {
                Session["UserLoggedIn"] = true;
                Response.Redirect("pageCaNhan.aspx");
            }
            else
            {
                lblUsername.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
            }
        }

    }
}