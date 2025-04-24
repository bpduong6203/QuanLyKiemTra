<%@ Page Title="" Language="C#" MasterPageFile="~/siteAuth.Master" AutoEventWireup="true" CodeBehind="pageLogin.aspx.cs" Inherits="QuanLyKiemTra.pageLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/auth.css") %>'/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-container">
        <div class="login-card">
            <!-- Tiêu đề -->
            <div class="login-title">
                <h2>Đăng Nhập</h2>
                <p>Chào mừng bạn trở lại!</p>
            </div>

            <!-- Form đăng nhập -->
            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" Text="Tên đăng nhập" CssClass="form-label" />
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" Placeholder="Nhập tên đăng nhập" />
            </div>
            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" Text="Mật khẩu" CssClass="form-label" />
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu" />
            </div>

            <!-- Nút đăng nhập và đăng ký -->
            <div class="form-group">
                <asp:Button ID="btnLogin" runat="server" Text="Đăng Nhập" CssClass="btn-primary" OnClick="btnLogin_Click" />
            </div>

            <!-- Liên kết Quên mật khẩu -->
            <div class="form-footer">
                <a href="#">Quên mật khẩu?</a>
            </div>

            <div class="form-footer">
                <a>Bạn chưa có tài khoản?</a>
                <a href="pageRegister.aspx">Đăng ký</a>
            </div>
        </div>
    </div>
</asp:Content>