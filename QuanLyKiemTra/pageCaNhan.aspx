<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCaNhan.aspx.cs" Inherits="QuanLyKiemTra.pageCaNhan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/profile.css") %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="form-container">
        <!-- Thẻ bên trái -->
        <div class="profile-card">
            <div class="profile-title">
                <h4>Thông Tin Cá Nhân</h4>
                <p>Quản lý thông tin tài khoản của bạn</p>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
            <div class="form-column">
                <div class="form-group">
                    <asp:Label ID="Label7" runat="server" Text="Họ và tên: Nguyen Van A" CssClass="form-label" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label8" runat="server" Text="Số điện thoại: 0123456789" CssClass="form-label" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label9" runat="server" Text="Địa chỉ: 123 street" CssClass="form-label" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label10" runat="server" Text="Đơn vị: Vien" CssClass="form-label" />
                </div>
                <div class="form-group">
                    <asp:Label ID="Label11" runat="server" Text="Chức vụ: Vien" CssClass="form-label" />
                </div>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
        </div>

        <!-- Thẻ bên phải -->
        <div class="profile-card2">
            <div class="profile-title">
                <h4>Danh Sách Kế Hoạch</h4>
                <p>Các kế hoạch đã làm, giải trình, tài liệu của bạn</p>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
            <div class="form-column">
                <!-- Bộ lọc -->
                <div class="filter-group">
                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="dropdown-custom" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="Tất cả trạng thái" />
                        <asp:ListItem Value="HoanThanh" Text="Hoàn thành" />
                        <asp:ListItem Value="DangXuLy" Text="Đang xử lý" />
                        <asp:ListItem Value="ChuaBatDau" Text="Chưa bắt đầu" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlTypeFilter" runat="server" CssClass="dropdown-custom" AutoPostBack="true" OnSelectedIndexChanged="ddlTypeFilter_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="Tất cả loại" />
                        <asp:ListItem Value="KeHoach" Text="Kế hoạch" />
                        <asp:ListItem Value="GiaiTrinh" Text="Giải trình" />
                        <asp:ListItem Value="TaiLieu" Text="Tài liệu" />
                    </asp:DropDownList>
                </div>

                <!-- Bảng danh sách -->
                <asp:GridView ID="gvKeHoach" runat="server" AutoGenerateColumns="False" CssClass="grid-view" AllowPaging="True" PageSize="5" OnPageIndexChanging="gvKeHoach_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="TenKeHoach" HeaderText="Tên Kế Hoạch" />
                        <asp:BoundField DataField="TrangThai" HeaderText="Trạng Thái" />
                        <asp:BoundField DataField="LoaiTaiLieu" HeaderText="Loại Tài Liệu" />
                        <asp:BoundField DataField="NgayTao" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Tài Liệu">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTaiLieu" runat="server" Text="Tải xuống" CommandArgument='<%# Eval("TaiLieuUrl") %>' OnClick="lnkTaiLieu_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="grid-pager" />
                </asp:GridView>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
        </div>
    </div>
</asp:Content>