<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCaNhan.aspx.cs" Inherits="QuanLyKiemTra.pageCaNhan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .message-label { display: block; margin: 10px 0; }
        .error-message { color: red; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="form-group-row">
        <!-- Thẻ bên trái -->
        <div class="form-groups md">
            <div class="profile-title">
                <h4>Thông Tin Cá Nhân</h4>
                <p>Quản lý thông tin tài khoản của bạn</p>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message-label error-message" Visible="false" />
            <div class="form-column">
                <div class="mb-3">
                    <asp:Label ID="lblHoTen" runat="server" CssClass="form-label" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblSoDienThoai" runat="server" CssClass="form-label" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblDiaChi" runat="server" CssClass="form-label" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblDonVi" runat="server" CssClass="form-label" />
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblChucVu" runat="server" CssClass="form-label" />
                </div>
            </div>
            <div style="border-top: 2px solid #475569; width: 100%; margin: 20px 0;"></div>
        </div>
        <!-- Thẻ bên phải -->
        <div class="form-groups xl">
        </div>
    </div>
</asp:Content>