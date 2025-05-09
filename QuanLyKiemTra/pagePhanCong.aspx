<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pagePhanCong.aspx.cs" Inherits="QuanLyKiemTra.pagePhanCong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <!-- Tiêu đề -->
        <div class="profile-title">
            <h4>Phân Công Kế Hoạch Kiểm Tra</h4>
            <p>Quản lý phân công thành viên cho kế hoạch kiểm tra</p>
        </div>
        <div class="divider"></div>

        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False" />

        <div class="form-group-row">
            <!-- Form bên trái -->
            <div class="form-groups lg">
                <!-- Bước 2: Nhập đề cương và tài liệu liên quan -->
                <asp:Label ID="lblDeCuong" runat="server" Text="Đề cương kiểm tra" CssClass="form-label" />
                <div class="form-group">
                    <asp:FileUpload ID="fuDeCuong" runat="server" CssClass="form-input xxl" Accept=".doc,.docx,.pdf" />
                </div>
                <asp:Label ID="lblTaiLieu" runat="server" Text="Tài liệu liên quan" CssClass="form-label" />
                <div class="form-group">
                    <asp:FileUpload ID="fuTaiLieu" runat="server" CssClass="form-input xxl" AllowMultiple="true" Accept=".doc,.docx,.pdf" />
                </div>

                <!-- Bước 4: Xuất văn bản phân công -->
                <div class="form-group">
                    <asp:Button ID="btnExportPhanCong" runat="server" Text="Xuất Văn Bản Phân Công" CssClass="btn btn-primary md m" OnClick="btnExportPhanCong_Click" />
                </div>
                <!-- Bước 5: Lưu kế hoạch -->
                <div class="form-group">
                    <asp:Button ID="btnSavePlan" runat="server" Text="Lưu Phân Công" CssClass="btn btn-primary md m" OnClick="btnSavePlan_Click" />
                </div>
            </div>

            <!-- Danh sách kế hoạch bên phải -->
            <div class="form-groups lg">
                <h5>Danh sách kế hoạch</h5>
                <input type="text" id="txtSearchPlan" class="search-bar" placeholder="Tìm kiếm kế hoạch..." />
                <button type="button" id="btnSortPlan" class="sort-button btn">Sắp xếp A-Z</button>
                <asp:HiddenField ID="hfSelectedKeHoach" runat="server" />
                <asp:Repeater ID="rptKeHoach" runat="server" OnItemDataBound="rptKeHoach_ItemDataBound">
                    <ItemTemplate>
                        <div class="plan-item" data-name='<%# Eval("TenKeHoach") %>'>
                            <asp:CheckBox ID="chkKeHoach" runat="server" CssClass="plan-checkbox" 
                                data-id='<%# Eval("Id") %>' data-name='<%# Eval("TenKeHoach") %>' 
                                OnCheckedChanged="chkKeHoach_CheckedChanged" AutoPostBack="true" />
                            <asp:Label ID="lblTenKeHoach" runat="server" Text='<%# Eval("TenKeHoach") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
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
        <div class="divider"></div>
    </div>

    <script src="<%= ResolveUrl("~/style/search.js") %>"></script>
</asp:Content>