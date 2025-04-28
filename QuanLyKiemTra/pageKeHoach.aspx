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
        <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>

        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False" />

        <!-- Tên kế hoạch -->
        <div class="form-group">
            <asp:Label ID="lblTenKeHoach" runat="server" Text="Tên kế hoạch" CssClass="form-label" AssociatedControlID="txtTenKeHoach" />
            <asp:TextBox ID="txtTenKeHoach" runat="server" CssClass="form-control btn-large" Placeholder="Nhập tên kế hoạch" />
            <asp:RequiredFieldValidator ID="rfvTenKeHoach" runat="server" ControlToValidate="txtTenKeHoach"
                ErrorMessage="Tên kế hoạch là bắt buộc" CssClass="error-message" Display="Dynamic" />
        </div>

        <!-- Thời gian -->
        <div style="display: flex; justify-content: space-between; align-items: center; width: 100%;" class="btn-large">
            <div class="form-group">
                <asp:Label ID="lblNgayBatDau" runat="server" Text="Ngày bắt đầu" CssClass="form-label" AssociatedControlID="txtNgayBatDau" />
                <asp:TextBox ID="txtNgayBatDau" runat="server" CssClass="form-control btn-medium" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvNgayBatDau" runat="server" ControlToValidate="txtNgayBatDau"
                    ErrorMessage="Ngày bắt đầu là bắt buộc" CssClass="error-message" Display="Dynamic" />
            </div>
            <div class="form-group">
                <asp:Label ID="lblNgayKetThuc" runat="server" Text="Ngày kết thúc" CssClass="form-label" AssociatedControlID="txtNgayKetThuc" />
                <asp:TextBox ID="txtNgayKetThuc" runat="server" CssClass="form-control btn-medium" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvNgayKetThuc" runat="server" ControlToValidate="txtNgayKetThuc"
                    ErrorMessage="Ngày kết thúc là bắt buộc" CssClass="error-message" Display="Dynamic" />
            </div>
        </div>


        <!-- Ghi chú -->
        <div class="form-group">
            <asp:Label ID="lblGhiChu" runat="server" Text="Ghi chú" CssClass="form-label" AssociatedControlID="txtGhiChu" />
            <asp:TextBox ID="txtGhiChu" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" Placeholder="Nhập ghi chú" />
        </div>

        <!-- Bước 1: Chọn đơn vị kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblDonVi" runat="server" Text="Chọn đơn vị kiểm tra:" CssClass="form-label" AssociatedControlID="ddlDonVi" />
            <asp:DropDownList ID="ddlDonVi" runat="server" CssClass="form-select btn-large" AutoPostBack="true" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged">
                <asp:ListItem Text="-- Chọn đơn vị --" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvDonVi" runat="server" ControlToValidate="ddlDonVi"
                ErrorMessage="Đơn vị là bắt buộc" CssClass="error-message" Display="Dynamic" InitialValue="" />
        </div>

        <!-- Bước 2: Xuất và nhập quyết định kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblQuyetDinh" runat="server" Text="Quyết định kiểm tra:" CssClass="form-label" />
            <div class="file-upload-group">
                <asp:FileUpload ID="fuQuyetDinh" runat="server" CssClass="form-control btn-large" Accept=".doc,.docx,.pdf" />
                <asp:Button ID="btnExportQuyetDinh" runat="server" Text="Xuất Quyết Định" CssClass="btn btn-primary btn-large" OnClick="btnExportQuyetDinh_Click" />
            </div>
        </div>
        <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>

        <!-- Bước 3: Nhập đề cương và tài liệu liên quan -->
        <div class="form-group">
            <asp:Label ID="lblDeCuong" runat="server" Text="Đề cương kiểm tra" CssClass="form-label" />
            <asp:FileUpload ID="fuDeCuong" runat="server" CssClass="form-control btn-large" Accept=".doc,.docx,.pdf" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblTaiLieu" runat="server" Text="Tài liệu liên quan" CssClass="form-label" />
            <asp:FileUpload ID="fuTaiLieu" runat="server" CssClass="form-control btn-large" AllowMultiple="true" Accept=".doc,.docx,.pdf" />
        </div>

        <!-- Bước 4: Phân công thành viên (vai trò ThanhVien) -->
        <div class="form-group">
            <asp:Label ID="lblThanhVien" runat="server" Text="Phân công thành viên" CssClass="form-label" />
            <asp:GridView ID="gvThanhVien" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvThanhVien_RowCommand">
                <Columns>
                    <asp:BoundField DataField="HoTen" HeaderText="Họ và Tên" />
                    <asp:BoundField DataField="ChucVu" HeaderText="Chức Vụ" />
                    <asp:TemplateField HeaderText="Nhiệm Vụ">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNhiemVu" runat="server" CssClass="form-control" Placeholder="Nhập nhiệm vụ" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Chọn">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Bước 5: Xuất văn bản phân công -->
        <div class="form-group">
            <asp:Button ID="btnExportPhanCong" runat="server" Text="Xuất Văn Bản Phân Công" CssClass="btn btn-primary btn-large" OnClick="btnExportPhanCong_Click" />
        </div>

        <!-- Nút lưu kế hoạch -->
        <div class="form-group">
            <asp:Button ID="btnSavePlan" runat="server" Text="Lưu Kế Hoạch" CssClass="btn btn-primary btn-large" OnClick="btnSavePlan_Click" />
        </div>
    </div>
</asp:Content>