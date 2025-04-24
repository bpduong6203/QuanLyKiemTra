<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageKeHoach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="~/style.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h2>Danh sách văn bản kiểm tra</h2>
        <asp:Label ID="lblReport" runat="server" ForeColor="Green" CssClass="form-label" />
    </div>

    <div class="container">
        <h2>Kế Hoạch Kiểm Tra</h2>

        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Chọn đơn vị kiểm tra:" CssClass="form-label" AssociatedControlID="DropDownList1" />
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdown-custom" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem Text="Đơn vị 1" Value="1" />
                <asp:ListItem Text="Đơn vị 2" Value="2" />
                <asp:ListItem Text="Đơn vị 3" Value="3" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Chọn thành viên:" CssClass="form-label" AssociatedControlID="CheckBoxListMembers" />
            <asp:CheckBoxList ID="CheckBoxListMembers" runat="server" CssClass="dropdown-custom">
            </asp:CheckBoxList>
        </div>


        <div class="form-group">
            <asp:Button ID="btnSavePlan" runat="server" Text="Lưu" CssClass="btn-custom" />
        </div>
    </div>
</asp:Content>
