<%@ Page Title="" Language="C#" MasterPageFile="~/siteAuth.Master" AutoEventWireup="true" CodeBehind="pageRegister.aspx.cs" Inherits="QuanLyKiemTra.pageRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/auth.css") %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-container">
        <div class="register-card">
            <!-- Tiêu đề -->
            <div class="login-title">
                <h2>Đăng Ký</h2>
                <p>Tạo tài khoản mới để sử dụng hệ thống</p>
            </div>

            <!-- Form đăng ký -->
            <div class="form-container">
                <!-- Cột trái: Thông tin người dùng -->
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblFullName" runat="server" Text="Họ và tên" CssClass="form-label" />
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-input" Placeholder="Nhập họ và tên" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPhone" runat="server" Text="Số điện thoại" CssClass="form-label" />
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input" Placeholder="Nhập số điện thoại" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblAddress" runat="server" Text="Địa chỉ" CssClass="form-label" />
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-input" Placeholder="Nhập nhập địa chỉ" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblUnit" runat="server" Text="Chọn đơn vị" CssClass="form-label" />
                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-input">
                            <asp:ListItem Text="--- Chọn đơn vị ---" Value="" />
                            <asp:ListItem Text="Đơn vị 1" Value="Unit1" />
                            <asp:ListItem Text="Đơn vị 2" Value="Unit2" />
                            <asp:ListItem Text="Đơn vị 3" Value="Unit3" />
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Cột phải: Thông tin đăng nhập -->
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblUsername" runat="server" Text="Tên đăng nhập" CssClass="form-label" />
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" Placeholder="Nhập tên đăng nhập" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPassword" runat="server" Text="Mật khẩu" CssClass="form-label" />
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblConfirmPassword" runat="server" Text="Xác nhận mật khẩu" CssClass="form-label" />
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập lại mật khẩu" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnRegister" runat="server" Text="Đăng Ký" style="margin-top: 30px" CssClass="btn-primary" />
                    </div>

                </div>


            </div>

            <!-- Nút đăng ký -->

            <div class="form-footer">
                <a>Bạn đã có tài khoản?</a>
                <a href="pageLogin.aspx">Đăng nhập</a>
            </div>
        </div>
    </div>
</asp:Content>
