<%@ Page Title="Danh sách thông báo" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="pageThongBao.aspx.cs" Inherits="QuanLyKiemTra.pageThongBao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .grid-view tr.unread {
            background-color: #e9ecef; 
        }
        .grid-view tr.read {
            background-color: #f8f9fa; 
        }

        .grid-view tr {
            cursor: pointer;
        }
        .grid-view tr:hover {
            background-color: #a4a4a4;
        }

        .grid-view td a {
            display: block; 
            color: inherit; 
            text-decoration: none;
        }
        .grid-view td a:hover {
            text-decoration: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h2>Danh sách thông báo</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="False" />

        <asp:GridView ID="gvThongBao" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="grid-view" AllowPaging="True" PageSize="10"
            OnRowCommand="gvThongBao_RowCommand" OnPageIndexChanging="gvThongBao_PageIndexChanging"
            OnRowDataBound="gvThongBao_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Tên kế hoạch">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTenKeHoach" runat="server" CommandName="Redirect" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# Eval("TenKeHoach") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Đơn vị">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTenDonVi" runat="server" CommandName="Redirect" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# Eval("TenDonVi") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nội dung">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNoiDung" runat="server" CommandName="Redirect" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# Eval("NoiDung") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày gửi">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNgayTao" runat="server" CommandName="Redirect" CommandArgument='<%# Container.DataItemIndex %>'
                            Text='<%# FormatTimeAgo((DateTime)Eval("NgayTao")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="Đầu" LastPageText="Cuối" />
        </asp:GridView>
    </div>
</asp:Content>