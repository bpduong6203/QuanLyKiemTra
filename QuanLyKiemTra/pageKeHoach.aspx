<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageKeHoach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/style.css") %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <!-- Tiêu đề -->
        <div class="profile-title">
            <h4>Tạo Kế Hoạch Kiểm Tra</h4>
            <p>Quản lý và tạo kế hoạch kiểm tra cho các đơn vị</p>
        </div>
        <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>

        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False"></asp:Label>

        <!-- Bước 1: Chọn đơn vị kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblDonVi" runat="server" Text="Chọn đơn vị kiểm tra:" CssClass="form-label" AssociatedControlID="ddlDonVi" />
            <asp:DropDownList ID="ddlDonVi" runat="server" CssClass="dropdown-custom" AutoPostBack="true" OnSelectedIndexChanged="ddlDonVi_SelectedIndexChanged">
                <asp:ListItem Text="-- Chọn đơn vị --" Value="" />
                <asp:ListItem Text="Đơn vị 1" Value="1" />
                <asp:ListItem Text="Đơn vị 2" Value="2" />
                <asp:ListItem Text="Đơn vị 3" Value="3" />
            </asp:DropDownList>
        </div>

        <!-- Bước 2: Xuất và nhập quyết định kiểm tra -->
        <div class="form-group">
            <asp:Label ID="lblQuyetDinh" runat="server" Text="Quyết định kiểm tra:" CssClass="form-label" />
            <div class="file-upload-group">
                <asp:Button ID="btnExportQuyetDinh" runat="server" Text="Xuất Quyết Định" CssClass="btn-primary" OnClick="btnExportQuyetDinh_Click" />
                <asp:FileUpload ID="fuQuyetDinh" runat="server" CssClass="file-upload" />
            </div>
        </div>

        <!-- Bước 3: Nhập đề cương và tài liệu liên quan -->
        <div class="form-group">
            <asp:FileUpload ID="fuDeCuong" runat="server" CssClass="file-upload" />
            <asp:Label ID="lblDeCuong" runat="server" Text="Đề cương kiểm tra" CssClass="form-label" />
        </div>
        <div class="form-group">
            <asp:FileUpload ID="fuTaiLieu" runat="server" CssClass="file-upload" AllowMultiple="true" />
            <asp:Label ID="lblTaiLieu" runat="server" Text="Tài liệu liên quan" CssClass="form-label" />
        </div>

        <!-- Bước 4: Chọn thành viên và phân công nhiệm vụ -->
        <div class="form-group">
            <asp:Label ID="lblThanhVien" runat="server" Text="Phân công thành viên:" CssClass="form-label" />
            <asp:GridView ID="gvThanhVien" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                DataKeyNames="MaThanhVien" OnRowCommand="gvThanhVien_RowCommand">
                <Columns>
                    <asp:BoundField DataField="HoTen" HeaderText="Họ và Tên" />
                    <asp:BoundField DataField="ChucVu" HeaderText="Chức Vụ" />
                    <asp:TemplateField HeaderText="Nhiệm Vụ">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNhiemVu" runat="server" CssClass="form-input" Placeholder="Nhập nhiệm vụ" />
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
            <asp:Button ID="btnExportPhanCong" runat="server" Text="Xuất Văn Bản Phân Công" CssClass="btn-primary" OnClick="btnExportPhanCong_Click" />
        </div>

        <!-- Nút lưu kế hoạch -->
        <div class="form-group">
            <asp:Button ID="btnSavePlan" runat="server" Text="Lưu Kế Hoạch" CssClass="btn-primary" OnClick="btnSavePlan_Click" />
        </div>
    </div>
</asp:Content>