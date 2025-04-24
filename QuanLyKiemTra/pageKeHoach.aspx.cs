using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageKeHoach : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxListMembers.Items.Clear();

            string selectedUnit = DropDownList1.SelectedValue;

            Dictionary<string, List<string>> unitMembers = new Dictionary<string, List<string>>()
            {
                { "1", new List<string>{ "Nguyễn Văn A", "Trần Thị B", "Lê Văn C" } },
                { "2", new List<string>{ "Phạm Thị X", "Hoàng Văn Y", "Đỗ Thị Z" } },
                { "3", new List<string>{ "Vũ Văn M", "Lê Thị N", "Trịnh Văn O" } }
            };

            if (unitMembers.ContainsKey(selectedUnit))
            {
                foreach (var member in unitMembers[selectedUnit])
                {
                    CheckBoxListMembers.Items.Add(new ListItem(member));
                }
            }
        }
    }
}