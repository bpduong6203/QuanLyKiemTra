<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKiemTra.aspx.cs" Inherits="QuanLyKiemTra.pageKiemTraTrucTuyen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Danh sách kế hoạch kiểm tra</h4>
        <div class="form-group">
            <asp:GridView ID="gvKeHoach" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                DataKeyNames="Id" OnRowCommand="gvKeHoach_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TenKeHoach" HeaderText="Tên Kế Hoạch" />
                    <asp:BoundField DataField="DonVi.TenDonVi" HeaderText="Tên Đơn Vị" />
                    <asp:BoundField DataField="NguoiDung.HoTen" HeaderText="Người Tạo" />
                    <asp:TemplateField HeaderText="Hành Động">
                        <ItemTemplate>
                            <asp:Button ID="btnXemChiTiet" runat="server" Text="Xem Chi Tiết" CssClass="custom-btn btn-info" 
                                CommandName="XemChiTiet" CommandArgument='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>