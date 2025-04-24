<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCapNhatCaNhan.aspx.cs" Inherits="QuanLyKiemTra.pageCapNhatCaNhan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/profile.css") %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <div class="form-container">
        <!-- Thẻ bên trái -->
        <div class="profile-card">
            <div class="profile-title">
                <h4>Thông Tin Cá Nhân</h4>
                <p>Quản lý thông tin tài khoản của bạn</p>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
                            <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Họ và tên" CssClass="form-label" />
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-input" Placeholder="Nhập họ và tên" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Số điện thoại" CssClass="form-label" />
                        <asp:TextBox ID="TextBox5" runat="server" CssClass="form-input" Placeholder="Nhập số điện thoại" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Địa chỉ" CssClass="form-label" />
                        <asp:TextBox ID="TextBox6" runat="server" CssClass="form-input" Placeholder="Nhập địa chỉ" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="Đơn vị" CssClass="form-label" />
                        <asp:TextBox ID="TextBox7" runat="server" CssClass="form-input" Placeholder="Nhập đơn vị" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Chức vụ" CssClass="form-label" />
                        <asp:TextBox ID="TextBox8" runat="server" CssClass="form-input" Placeholder="Nhập chức vụ" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="Button2" runat="server" Text="Cập Nhật Thông Tin" CssClass="btn-primary"/>
                    </div>
                </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
        </div>

        <div class="profile-card">
            <div class="profile-title">
                <h4>Thông Tin Cá Nhân</h4>
                <p>Quản lý thông tin tài khoản của bạn</p>
            </div>
                            <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Mật khẩu hiện tại" CssClass="form-label" />
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu hiện tại" />

                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu mới" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Xác nhận mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập lại mật khẩu mới" />

                    </div>
                    <div class="form-group">
                        <asp:Button ID="Button1" runat="server" Text="Đổi Mật Khẩu" CssClass="btn-primary" />
                    </div>
                </div>
        </div>
    </div>
</asp:Content>