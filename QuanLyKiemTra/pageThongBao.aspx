<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageThongBao.aspx.cs" Inherits="QuanLyKiemTra.pageThongBao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="~/style.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h2>Danh Sách Nguồn Nhận Thông Báo Kiểm Tra</h2>

        <!-- Danh sách nguồn nhận thông báo kiểm tra -->
        <asp:GridView ID="gvNotificationSources" runat="server" CssClass="grid-view" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="SourceName" HeaderText="Tên Nguồn" />
                <asp:BoundField DataField="ContactInfo" HeaderText="Thông Tin Liên Hệ" />
                <asp:BoundField DataField="Status" HeaderText="Trạng Thái" />
            </Columns>
        </asp:GridView>
    </div>
    <div class="container">
        <h2>Thông Báo Kiểm Tra</h2>

        <!-- Chọn kế hoạch kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblSelectPlan" runat="server" Text="Chọn kế hoạch kiểm tra:" CssClass="form-label" AssociatedControlID="ddlInspectionPlans" />
            <asp:DropDownList ID="ddlInspectionPlans" runat="server" CssClass="dropdown-custom">
                <asp:ListItem Text="Kế hoạch kiểm tra 1" Value="1" />
                <asp:ListItem Text="Kế hoạch kiểm tra 2" Value="2" />
                <asp:ListItem Text="Kế hoạch kiểm tra 3" Value="3" />
            </asp:DropDownList>
        </div>

        <!-- Gửi thông báo kiểm tra -->
        <div class="form-group">
            <button class="btn-custom">Gửi Thông Báo Kiểm Tra</button>
        </div>

        <!-- Xuất biên bản thông báo kiểm tra -->
        <div class="form-group">
            <button class="btn-custom btn-secondary-custom">Xuất Biên Bản Thông Báo Kiểm Tra</button>
        </div>
    </div>
</asp:Content>

