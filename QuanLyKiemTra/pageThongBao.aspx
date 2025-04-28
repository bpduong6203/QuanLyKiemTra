<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageThongBao.aspx.cs" Inherits="QuanLyKiemTra.pageThongBao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/style.css") %>' />
    <style>
        .bien-ban-link { 
            color: #4a90e2; 
            text-decoration: none; 
        }
        .bien-ban-link:hover { 
            text-decoration: underline; 
        }

        .box {
            margin: 10px 0px 10px 0px;
        }

        .btn-confirm {
            margin: 10px 0 10px 0;
            width: 100%;
            padding: 5px;
            font-size: 16px;
            font-weight: 600;
            color: #ffffff;
            background-color: #475569;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .btn-confirm:hover {
            background-color: #64748b;
        }

        .error-message {
            color: #dc3545;
            font-weight: bold;
        }

        .success-message {
            color: #28a745;
            font-weight: bold;
        }

        .grid-view {
            width: 100%;
            border-collapse: collapse;
        }

        .grid-view th, .grid-view td {
            padding: 10px;
            border: 1px solid #dee2e6;
            text-align: left;
        }

        .grid-view th {
            background-color: #f8f9fa;
        }
    </style>
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

        <!-- Chọn kế hoạch kiểm tra -->
        <div class="form-group box">
            <asp:Label ID="lblSelectPlan" runat="server" Text="Chọn kế hoạch kiểm tra:" CssClass="form-label" AssociatedControlID="ddlInspectionPlans" />
            <asp:DropDownList ID="ddlInspectionPlans" runat="server" CssClass="dropdown-custom btn-medium" AutoPostBack="true" OnSelectedIndexChanged="ddlInspectionPlans_SelectedIndexChanged">
                <asp:ListItem Text="-- Chọn kế hoạch --" Value="" />
            </asp:DropDownList>
        </div>

        <!-- Gửi thông báo kiểm tra -->
        <div class="form-group">
            <asp:Button ID="btnSendNotification" runat="server" Text="Gửi Thông Báo" CssClass="btn-primary btn-medium" OnClick="btnSendNotification_Click" />
        </div>

        <!-- Xuất biên bản thông báo kiểm tra -->
        <div class="form-group">
            <asp:Button ID="btnExportBienBan" runat="server" Text="Xuất Biên Bản" CssClass="btn-secondary btn-medium" OnClick="btnExportBienBan_Click" />
        </div>
    </div>
</asp:Content>