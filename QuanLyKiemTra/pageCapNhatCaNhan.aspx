<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCapNhatCaNhan.aspx.cs" Inherits="QuanLyKiemTra.pageCapNhatCaNhan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .message-label { display: block; margin: 10px 0; }
        .error-message { color: red; }
        .success-message { color: green; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="form-container">
        <!-- Thẻ bên trái -->
        <div class="form-groups">
            <h4>Thông Tin Cá Nhân</h4>
            <p>Quản lý thông tin tài khoản của bạn</p>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false" />
        <div class="form-group-row">
            <div class="form-groups xl">
                <h4>Cập nhật thông tin người dùng</h4>
                <div class="mb-3">
                    <asp:Label ID="lblHoTen" runat="server" Text="Họ và tên" CssClass="form-label" />
                    <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-input" Placeholder="Nhập họ và tên" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="form-label" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" Placeholder="Nhập email" TextMode="Email" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblSoDienThoai" runat="server" Text="Số điện thoại" CssClass="form-label" />
                    <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-input" Placeholder="Nhập số điện thoại" TextMode="Phone" />
                </div>
                <div class="mb-3">
                    <asp:Button ID="btnCapNhat" runat="server" Text="Cập Nhật Thông Tin" CssClass="btn-primary btn md" OnClick="btnCapNhat_Click" />
                </div>
            </div>
            <div class="form-groups md">
                <h4>Thay đổi mật khẩu</h4>
                <div class="form-column">
                    <div class="mb-3">
                        <asp:Label ID="lblMatKhauHienTai" runat="server" Text="Mật khẩu hiện tại" CssClass="form-label" />
                        <asp:TextBox ID="txtMatKhauHienTai" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu hiện tại" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblMatKhauMoi" runat="server" Text="Mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="txtMatKhauMoi" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu mới" />
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblXacNhanMatKhau" runat="server" Text="Xác nhận mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="txtXacNhanMatKhau" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập lại mật khẩu mới" />
                    </div>
                    <div class="mb-3">
                        <asp:Button ID="btnDoiMatKhau" runat="server" Text="Đổi Mật Khẩu" CssClass="btn-primary btn md" OnClick="btnDoiMatKhau_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>