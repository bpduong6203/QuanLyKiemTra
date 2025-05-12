<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageDanhSachKetQua.aspx.cs" Inherits="QuanLyKiemTra.viewCauHoi.pageDanhSachKetQua" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h4>Danh Sách Kết Quả Bài Kiểm Tra</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false" />
        <asp:GridView ID="gvKetQua" runat="server" AutoGenerateColumns="False" CssClass="grid-view" OnRowCommand="gvKetQua_RowCommand">
            <Columns>
                <asp:BoundField DataField="HoTen" HeaderText="Họ Tên" />
                <asp:BoundField DataField="ThoiGianNop" HeaderText="Thời Gian Nộp" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="SoCauDung" HeaderText="Số Câu Đúng" />
                <asp:BoundField DataField="TongSoCau" HeaderText="Tổng Số Câu" />
                <asp:BoundField DataField="Diem" HeaderText="Điểm" DataFormatString="{0:F2}" />
                <asp:BoundField DataField="TrangThai" HeaderText="Trạng Thái" />
                <asp:TemplateField HeaderText="Hành Động">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlXemChiTiet" runat="server" CssClass="btn btn-info btn-sm" NavigateUrl='<%# "~/chi-tiet-ket-qua-kiem-tra/" + Eval("KetQuaKiemTraId") %>' title="Xem chi tiết">
                            <i class="fas fa-eye"></i>
                        </asp:HyperLink>
                        <asp:LinkButton ID="btnLamLai" runat="server" CssClass="btn btn-warning btn-sm" CommandName="LamLai" CommandArgument='<%# Eval("UserId") + "," + Eval("BoCauHoiId") %>' Visible='<%# GetRole() == "TruongDoan" && Eval("TrangThai").ToString() == "DaHoanThanh" %>' OnClientClick="return confirm('Bạn có chắc muốn cho phép người dùng này làm lại bài kiểm tra?');" title="Cho phép làm lại">
                            <i class="fas fa-redo"></i>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="text-center">Chưa có kết quả nào cho bộ câu hỏi này.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>