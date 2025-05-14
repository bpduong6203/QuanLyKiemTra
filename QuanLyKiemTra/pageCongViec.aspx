<%@ Page Title="Danh sách công việc" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCongViec.aspx.cs" Inherits="QuanLyKiemTra.pageCongViec" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Danh sách công việc</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False" />

        <asp:GridView ID="gvCongViec" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="grid-view" AllowPaging="True" PageSize="10"
            OnPageIndexChanging="gvCongViec_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="TenKeHoach" HeaderText="Tên kế hoạch" />
                <asp:BoundField DataField="NoiDungCV" HeaderText="Nội dung công việc" />
                <asp:TemplateField HeaderText="Ngày tạo">
                    <ItemTemplate>
                        <%# FormatTimeAgo((DateTime)Eval("ngayTao")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tệp đính kèm">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkFile" runat="server" NavigateUrl='<%# Eval("linkfile") %>'
                            Text="Tải xuống" Target="_blank" Visible='<%# !string.IsNullOrEmpty(Eval("linkfile")?.ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="Đầu" LastPageText="Cuối" />
        </asp:GridView>
    </div>
</asp:Content>