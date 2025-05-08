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

        <div class="form-group-row">
            <!-- Form bên trái -->
            <div class="form-groups">
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

                <!-- Quyết định kiểm tra -->
                <div class="form-group">
                    <asp:Label ID="lblQuyetDinh" runat="server" Text="Quyết định kiểm tra" CssClass="form-label" />
                    <div class="file-upload-group">
                        <asp:FileUpload ID="fuQuyetDinh" runat="server" CssClass="form-input xxl" Accept=".doc,.docx,.pdf" />
                    </div>
                    <asp:Button ID="btnExportQuyetDinh" runat="server" Text="Xuất quyết định" CssClass="btn-primary lg" OnClick="btnExportQuyetDinh_Click" OnClientClick="return confirm('Bạn có chắc muốn lưu kế hoạch này?');" />
                </div>
            </div>

            <!-- Danh sách đơn vị bên phải -->
            <div class="form-groups lg">
                <h5>Danh sách đơn vị</h5>
                <input type="text" id="txtSearchUnit" class="search-bar" placeholder="Tìm kiếm đơn vị..." />
                <button type="button" id="btnSortUnit" class="sort-button">Sắp xếp A-Z</button>
                <asp:HiddenField ID="hfSelectedDonVi" runat="server" />
                <asp:Repeater ID="rptDonVi" runat="server" OnItemDataBound="rptDonVi_ItemDataBound">
                    <ItemTemplate>
                        <div class="unit-item" data-name='<%# Eval("TenDonVi") %>'>
                            <asp:CheckBox ID="chkDonVi" runat="server" CssClass="unit-checkbox" 
                                data-id='<%# Eval("Id") %>' data-name='<%# Eval("TenDonVi") %>' 
                                OnCheckedChanged="chkDonVi_CheckedChanged" AutoPostBack="true" />
                            <asp:Label ID="lblTenDonVi" runat="server" Text='<%# Eval("TenDonVi") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="divider"></div>
    </div>

    <script src="<%= ResolveUrl("~/style/search.js") %>"></script>
</asp:Content>