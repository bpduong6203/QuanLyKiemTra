<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageThongBao.aspx.cs" Inherits="QuanLyKiemTra.pageThongBao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container">
        <h4>Thông Báo Kiểm Tra</h4>

        <!-- Thông báo lỗi hoặc thành công -->
        <asp:Label ID="lblMessage" runat="server" Visible="False" />

        <!-- Danh sách thông báo kiểm tra -->
        <asp:GridView ID="gvThongBao" runat="server" CssClass="grid-view" AutoGenerateColumns="False"
            DataKeyNames="Id" OnRowCommand="gvThongBao_RowCommand">
            <Columns>
                <asp:BoundField DataField="TenKeHoach" HeaderText="Tên Kế Hoạch" />
                <asp:BoundField DataField="TenDonVi" HeaderText="Đơn Vị" />
                <asp:BoundField DataField="NoiDung" HeaderText="Nội Dung" />
                <asp:TemplateField HeaderText="Biên Bản">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkBienBan" runat="server" NavigateUrl='<%# Eval("LinkFile") %>'
                            Text="Xem Biên Bản" CssClass="bien-ban-link"
                            Visible='<%# !string.IsNullOrEmpty(Eval("LinkFile")?.ToString()) %>' />
                        <asp:Label ID="lblNoBienBan" runat="server" Text="Không có biên bản"
                            Visible='<%# string.IsNullOrEmpty(Eval("LinkFile")?.ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trạng Thái">
                    <ItemTemplate>
                        <asp:Label ID="lblConfirmed" runat="server" Text="Chưa xem" CssClass="confirmed-text"
                            Visible='<%# !(bool)Eval("DaXem") %>' />
                        <asp:Label ID="Label1" runat="server" Text="Đã xem" CssClass="confirmed-text"
                            Visible='<%# (bool)Eval("DaXem") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>