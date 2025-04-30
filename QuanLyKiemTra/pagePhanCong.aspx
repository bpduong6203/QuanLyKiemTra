<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pagePhanCong.aspx.cs" Inherits="QuanLyKiemTra.pagePhanCong" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <!-- Bước 1: Chọn kế hoạch -->
        <asp:Label ID="lblKeHoach" runat="server" Text="Chọn kế hoạch" CssClass="form-label" />
        <div class="form-group">
            <asp:DropDownList ID="ddlKeHoach" runat="server" CssClass="dropdown-custom btn-large" AutoPostBack="true" OnSelectedIndexChanged="ddlKeHoach_SelectedIndexChanged">
                <asp:ListItem Value="" Text="-- Chọn kế hoạch --" />
            </asp:DropDownList>
        </div>

        <!-- Bước 2: Nhập đề cương và tài liệu liên quan -->
        <asp:Label ID="lblDeCuong" runat="server" Text="Đề cương kiểm tra" CssClass="form-label" />
        <div class="form-group">

            <asp:FileUpload ID="fuDeCuong" runat="server" CssClass="form-input btn-large" Accept=".doc,.docx,.pdf" />
        </div>
        <asp:Label ID="lblTaiLieu" runat="server" Text="Tài liệu liên quan" CssClass="form-label" />
        <div class="form-group">
            <asp:FileUpload ID="fuTaiLieu" runat="server" CssClass="form-input btn-large" AllowMultiple="true" Accept=".doc,.docx,.pdf" />
        </div>

        <!-- Bước 3: Phân công thành viên (vai trò ThanhVien) -->
        <div class="form-group">
            <asp:Label ID="lblThanhVien" runat="server" Text="Phân công thành viên" CssClass="form-label" />
            <asp:GridView ID="gvThanhVien" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvThanhVien_RowCommand">
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
        <div class="form-group">
            <asp:Label ID="lblDaPhanCong" runat="server" Text="Danh sách đã phân công" CssClass="form-label" />
            <asp:GridView ID="gvDaPhanCong" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                <Columns>
                    <asp:BoundField DataField="NguoiDung.HoTen" HeaderText="Họ và Tên" />
                    <asp:BoundField DataField="NoiDungCV" HeaderText="Nhiệm Vụ" />
                    <asp:BoundField DataField="ngayTao" HeaderText="Ngày Phân Công" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
            </asp:GridView>
        </div>
        <!-- Bước 4: Xuất văn bản phân công -->
        <div class="form-group">
            <asp:Button ID="btnExportPhanCong" runat="server" Text="Xuất Văn Bản Phân Công" CssClass="btn btn-primary btn-large" OnClick="btnExportPhanCong_Click" />
        </div>

        <!-- Bước 5: Lưu kế hoạch -->
        <div class="form-group">
            <asp:Button ID="btnSavePlan" runat="server" Text="Lưu Phân Công" CssClass="btn btn-primary btn-large" OnClick="btnSavePlan_Click" />
        </div>
    </div>
</asp:Content>