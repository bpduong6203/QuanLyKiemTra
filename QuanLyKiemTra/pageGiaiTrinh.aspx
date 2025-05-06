<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageGiaiTrinh.aspx.cs" Inherits="QuanLyKiemTra.pageGiaiTrinh" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Danh Sách Yêu Cầu Giải Trình</h4>
        <div class="form-group">
            <asp:GridView ID="gvGiaiTrinhList" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                DataKeyNames="Id" OnRowCommand="gvGiaiTrinhList_RowCommand">
                <Columns>
                    <asp:BoundField DataField="KeHoach.TenKeHoach" HeaderText="Tên Kế Hoạch" NullDisplayText="Không có kế hoạch" />
                    <asp:BoundField DataField="NguoiYeuCau.HoTen" HeaderText="Người Yêu Cầu" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy}" />
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