<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKetLuan.aspx.cs" Inherits="QuanLyKiemTra.pageKetLuanKiemTra" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Danh Sách Kế Hoạch</h4>
        <div class="form-group">
            <div class="form-row">
                <asp:Label ID="lblSearch" runat="server" Text="Tìm kiếm: " CssClass="form-label" />
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-input lg" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
            </div>
            <asp:GridView ID="gvKeHoachList" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                DataKeyNames="Id" OnRowCommand="gvKeHoachList_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TenKeHoach" HeaderText="Tên Kế Hoạch" />
                    <asp:BoundField DataField="NguoiDung.HoTen" HeaderText="Người Tạo" NullDisplayText="Không xác định" />
                    <asp:BoundField DataField="DonVi.TenDonVi" HeaderText="Đơn Vị" NullDisplayText="Không xác định" />
                    <asp:BoundField DataField="NgayBatDau" HeaderText="Ngày Bắt Đầu" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="NgayKetThuc" HeaderText="Ngày Kết Thúc" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Trạng Thái">
                        <ItemTemplate>
                            <asp:Label ID="lblTrangThai" runat="server" Text='<%# GetKeHoachStatus(Eval("Id").ToString()) %>'
                                CssClass='<%# GetStatusCssClass(GetKeHoachStatus(Eval("Id").ToString())) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Hành Động">
                        <ItemTemplate>
                            <asp:Button ID="btnXacNhanHoanThanh" runat="server" Text="Xác nhận đã hoàn thành" CssClass="custom-btn btn-info" 
                                CommandName="XacNhanHoanThanh" CommandArgument='<%# Eval("Id") %>' Visible='<%# CanConfirmCompletion(Eval("Id").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>