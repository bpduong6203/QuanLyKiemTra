using System;

namespace QuanLyKiemTra
{
    public partial class pageCapNhatCaNhan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Điều kiện để chọn Master Page
            if (Request.QueryString["master"] == "CaNhan")
            {
                this.MasterPageFile = "~/CaNhan.Master";
            }
            else
            {
                this.MasterPageFile = "~/Site1.Master";
            }
        }
    }
}