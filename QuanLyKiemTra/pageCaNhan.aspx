<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="pageCaNhan.aspx.cs" Inherits="QuanLyKiemTra.pageCaNhan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%=ResolveUrl("~/css/profile.css") %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <div class="profile-card">
            <!-- Tiêu đề -->
            <div class="profile-title">
                <h2>Thông Tin Cá Nhân</h2>
                <p>Quản lý thông tin tài khoản của bạn</p>
            </div>

            <!-- Nội dung chính -->
            <div class="form-container">
                <!-- Cột trái: Thông tin cá nhân -->
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblFullName" runat="server" Text="Họ và tên" CssClass="form-label" />
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-input" Placeholder="Họ và tên" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPhone" runat="server" Text="Số điện thoại" CssClass="form-label" />
                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input" Placeholder="Số điện thoại" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblAddress" runat="server" Text="Địa chỉ" CssClass="form-label" />
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-input" Placeholder="Địa chỉ" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblUnit" runat="server" Text="Đơn vị" CssClass="form-label" />
                        <asp:TextBox ID="txtUnit" runat="server" CssClass="form-input" Placeholder="Đơn vị" ReadOnly="true" />
                    </div>
                </div>

                <!-- Cột phải: Đổi mật khẩu -->
                <div class="form-column">
                    <div class="form-group">
                        <asp:Label ID="lblCurrentPassword" runat="server" Text="Mật khẩu hiện tại" CssClass="form-label" />
                        <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu hiện tại" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblNewPassword" runat="server" Text="Mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu mới" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblConfirmNewPassword" runat="server" Text="Xác nhận mật khẩu mới" CssClass="form-label" />
                        <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập lại mật khẩu mới" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnChangePassword" runat="server" Text="Đổi Mật Khẩu" CssClass="btn-primary" />
                    </div>
                </div>
            </div>
        </div>
</asp:Content>