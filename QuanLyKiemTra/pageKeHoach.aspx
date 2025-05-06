<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageKeHoach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <!-- Tiêu đề -->
        <div class="profile-title">
            <h4>Tạo Kế Hoạch Kiểm Tra</h4>
            <p>Quản lý và tạo kế hoạch kiểm tra cho các đơn vị</p>
        </div>
        <div class="divider"></div>

        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False" />

        <!-- Tên kế hoạch -->
        <asp:Label ID="lblTenKeHoach" runat="server" Text="Tên kế hoạch" CssClass="form-label" AssociatedControlID="txtTenKeHoach" />
        <div class="form-group">
            <asp:TextBox ID="txtTenKeHoach" runat="server" CssClass="form-input xxl" Placeholder="Nhập tên kế hoạch" />
            <asp:RequiredFieldValidator ID="rfvTenKeHoach" runat="server" ControlToValidate="txtTenKeHoach"
                ErrorMessage="Tên kế hoạch là bắt buộc" CssClass="error-message" Display="Dynamic" />
        </div>

        <!-- Thời gian -->
        <div style="display: flex; justify-content: space-between; gap: 20px; flex-wrap: wrap;" class="xxl">
            <div class="form-group" style="flex: 1;">
                <asp:Label ID="lblNgayBatDau" runat="server" Text="Ngày bắt đầu" CssClass="form-label" AssociatedControlID="txtNgayBatDau" />
                <asp:TextBox ID="txtNgayBatDau" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvNgayBatDau" runat="server" ControlToValidate="txtNgayBatDau"
                    ErrorMessage="Ngày bắt đầu là bắt buộc" CssClass="error-message" Display="Dynamic" />
            </div>
            <div class="form-group" style="flex: 1;">
                <asp:Label ID="lblNgayKetThuc" runat="server" Text="Ngày kết thúc" CssClass="form-label" AssociatedControlID="txtNgayKetThuc" />
                <asp:TextBox ID="txtNgayKetThuc" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvNgayKetThuc" runat="server" ControlToValidate="txtNgayKetThuc"
                    ErrorMessage="Ngày kết thúc là bắt buộc" CssClass="error-message" Display="Dynamic" />
            </div>
        </div>

        <!-- Ghi chú -->
        <asp:Label ID="lblGhiChu" runat="server" Text="Ghi chú" CssClass="form-label" AssociatedControlID="txtGhiChu" />
        <div class="form-group">
            <asp:TextBox ID="txtGhiChu" runat="server" CssClass="form-input xxl" TextMode="MultiLine" Rows="4" Placeholder="Nhập ghi chú" />
        </div>

        <!-- Chọn đơn vị kiểm tra -->
        <asp:Label ID="lblDonVi" runat="server" Text="Chọn đơn vị kiểm tra" CssClass="form-label" AssociatedControlID="ddlDonVi" />
        <div class="form-group">
            <asp:DropDownList ID="ddlDonVi" runat="server" CssClass="form-select form-input xxl" AutoPostBack="true" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged">
                <asp:ListItem Text="-- Chọn đơn vị --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvDonVi" runat="server" ControlToValidate="ddlDonVi"
                ErrorMessage="Đơn vị là bắt buộc" CssClass="error-message" Display="Dynamic" InitialValue="" />
        </div>

        <!-- Quyết định kiểm tra -->

        <div class="form-group">
            <asp:Label ID="lblQuyetDinh" runat="server" Text="Quyết định kiểm tra" CssClass="form-label" />
            <div class="file-upload-group">
                <asp:FileUpload ID="fuQuyetDinh" runat="server" CssClass="form-input xxl" Accept=".doc,.docx,.pdf" />
            </div>
            <asp:Button ID="btnExportQuyetDinh" runat="server" Text="Xuất quyết định" CssClass="btn-primary lg" OnClick="btnExportQuyetDinh_Click" OnClientClick="return confirm('Bạn có chắc muốn lưu kế hoạch này?');" />
        </div>
        <div class="divider"></div>
    </div>
</asp:Content>
