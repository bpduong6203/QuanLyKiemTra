<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKiemTra.aspx.cs" Inherits="QuanLyKiemTra.pageKiemTraTrucTuyen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="~/style.css" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Danh Sách Nguồn Nhận Yêu Cầu Giải Trình</h4>

        <!-- Danh sách nguồn nhận yêu cầu giải trình -->
        <asp:GridView ID="gvExplanationSources" runat="server" CssClass="grid-view" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="SourceName" HeaderText="Tên Nguồn" />
                <asp:BoundField DataField="ContactInfo" HeaderText="Thông Tin Liên Hệ" />
                <asp:BoundField DataField="Status" HeaderText="Trạng Thái" />
            </Columns>
        </asp:GridView>
    </div>

    <div class="container">
        <h4>Nội Dung Yêu Cầu Giải Trình</h4>

        <!-- Danh sách nội dung yêu cầu giải trình -->
        <asp:GridView ID="gvExplanationContent" runat="server" CssClass="grid-view btn-large" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="RequestID" HeaderText="Mã Yêu Cầu" />
                <asp:BoundField DataField="Content" HeaderText="Nội Dung" />
                <asp:BoundField DataField="DateSent" HeaderText="Ngày Gửi" />
                <asp:BoundField DataField="Status" HeaderText="Trạng Thái" />
            </Columns>
        </asp:GridView>
    </div>

    <div class="container">
        <h4>Kiểm Tra Trực Tuyến</h4>

        <!-- Chọn kế hoạch kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblSelectPlan" runat="server" Text="Chọn kế hoạch kiểm tra:" CssClass="form-label" AssociatedControlID="ddlInspectionPlans" />
            <asp:DropDownList ID="ddlInspectionPlans" runat="server" CssClass="dropdown-custom btn-large">
                <asp:ListItem Text="Kế hoạch kiểm tra 1" Value="1" />
                <asp:ListItem Text="Kế hoạch kiểm tra 2" Value="2" />
                <asp:ListItem Text="Kế hoạch kiểm tra 3" Value="3" />
            </asp:DropDownList>
        </div>

        <!-- Xem trước nội dung kiểm tra -->
        <div class="form-group">
            <asp:Button ID="btnPreviewInspection" runat="server" Text="Nội Dung Kiểm Tra" CssClass="btn-primary btn-medium"/>
        </div>


        <!-- Yêu cầu gửi giải trình -->
        <div class="form-group">
            <asp:Button ID="btnRequestExplanation" runat="server" Text="Yêu Cầu Giải Trình" CssClass="btn-secondary btn-medium" />
        </div>
    </div>
</asp:Content>