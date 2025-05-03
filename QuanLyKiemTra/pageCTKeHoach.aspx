<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCTKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageCTKeHoach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Chi Tiết Kế Hoạch Kiểm Tra</h4>

        <!-- Thông tin kế hoạch -->
        <div class="form-groups">
            <h5>Thông Tin Kế Hoạch</h5>
            <div class="form-row">
                <asp:Label ID="lblTenKeHoach" runat="server" CssClass="form-labels" Text="Tên Kế Hoạch: " />
                <asp:Label ID="lblTenKeHoachValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblNgayBatDau" runat="server" CssClass="form-labels" Text="Ngày Bắt Đầu: " />
                <asp:Label ID="lblNgayBatDauValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblNgayKetThuc" runat="server" CssClass="form-labels" Text="Ngày Kết Thúc: " />
                <asp:Label ID="lblNgayKetThucValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblGhiChu" runat="server" CssClass="form-labels" Text="Ghi Chú: " />
                <asp:Label ID="lblGhiChuValue" runat="server" CssClass="form-value" />
            </div>
        </div>

        <!-- Thông tin đơn vị -->
        <div class="form-groups">
            <h5>Thông Tin Đơn Vị</h5>
            <div class="form-row">
                <asp:Label ID="lblTenDonVi" runat="server" CssClass="form-labels" Text="Tên Đơn Vị: " />
                <asp:Label ID="lblTenDonViValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblDiaChiDonVi" runat="server" CssClass="form-labels" Text="Địa Chỉ: " />
                <asp:Label ID="lblDiaChiDonViValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblSoDienThoaiDonVi" runat="server" CssClass="form-labels" Text="Số Điện Thoại: " />
                <asp:Label ID="lblSoDienThoaiDonViValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblEmailDonVi" runat="server" CssClass="form-labels" Text="Email: " />
                <asp:Label ID="lblEmailDonViValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblNguoiDaiDien" runat="server" CssClass="form-labels" Text="Người Đại Diện: " />
                <asp:Label ID="lblNguoiDaiDienValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblChucVuNguoiDaiDien" runat="server" CssClass="form-labels" Text="Chức Vụ Người Đại Diện: " />
                <asp:Label ID="lblChucVuNguoiDaiDienValue" runat="server" CssClass="form-value" />
            </div>
        </div>

        <div class="form-group-row">
            <!-- Người tạo kế hoạch -->
            <div class="form-groups">
                <h5>Người Tạo Kế Hoạch</h5>
                <div class="form-row">
                    <asp:Label ID="lblNguoiTao" runat="server" CssClass="form-labels" Text="Người Tạo: " />
                    <asp:Label ID="lblNguoiTaoValue" runat="server" CssClass="form-value" />
                </div>
            </div>

            <!-- Biên bản kiểm tra -->
            <div class="form-groups">
                <h5>Biên Bản Kiểm Tra</h5>
                <asp:Panel ID="pnlBienBan" runat="server" Visible="false">
                    <div class="form-row">
                        <asp:Label ID="lblTenBienBan" runat="server" CssClass="form-labels" Text="Tên Biên Bản: " />
                        <asp:Label ID="lblTenBienBanValue" runat="server" CssClass="form-value" />
                    </div>
                    <div class="form-row">
                        <asp:Label ID="lblLinkBienBan" runat="server" CssClass="form-labels" Text="File Biên Bản: " />
                        <asp:HyperLink ID="hlLinkBienBan" runat="server" Text="Tải xuống" Target="_blank" CssClass="form-link" />
                    </div>
                </asp:Panel>
                <asp:Label ID="lblNoBienBan" runat="server" Text="Chưa có biên bản kiểm tra." CssClass="message-label warning-message" Visible="true" />
            </div>
        </div>

        <!-- Thành viên đơn vị -->
        <div class="form-group">
            <h5>Thành Viên Đơn Vị</h5>
            <asp:GridView ID="gvThanhVienDonVi" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                <Columns>
                    <asp:BoundField DataField="HoTen" HeaderText="Họ và Tên" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="SoDienThoai" HeaderText="Số Điện Thoại" />
                    <asp:BoundField DataField="DiaChi" HeaderText="Địa Chỉ" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Yêu cầu giải trình -->
        <div class="form-group">
            <h5>Bảng Giải Trình</h5>
            <asp:Panel ID="pnlGiaiTrinh" runat="server" Visible="false">
                <asp:Label ID="lblGiaiTrinhMessage" runat="server" Text="Kế hoạch chưa có giải trình." CssClass="message-label warning-message" /><br />
                <asp:Label ID="lblFileMau" runat="server" Text="File mẫu giải trình (Word/PDF): " CssClass="form-labels" />
                <asp:FileUpload ID="fuFileMau" runat="server" CssClass="form-input btn-large" Accept=".doc,.docx,.pdf" /><br />
                <asp:Button ID="btnYeuCauGiaiTrinh" runat="server" Text="Yêu Cầu Giải Trình" CssClass="btn btn-primary btn-large" OnClick="btnYeuCauGiaiTrinh_Click" />
            </asp:Panel>
            <asp:GridView ID="gvGiaiTrinh" runat="server" AutoGenerateColumns="False" CssClass="grid-view" Visible="false">
                <Columns>
                    <asp:BoundField DataField="NguoiYeuCau.HoTen" HeaderText="Người Yêu Cầu" />
                    <asp:BoundField DataField="NguoiGiaiTrinh.HoTen" HeaderText="Người Giải Trình" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày Tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="File Mẫu">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlLinkFile" runat="server" Text="Tải xuống" NavigateUrl='<%# Eval("linkFile") %>' Target="_blank" Visible='<%# !string.IsNullOrEmpty(Eval("linkFile")?.ToString()) %>' />
                            <asp:Label ID="lblNoFile" runat="server" Text="Không có file" Visible='<%# string.IsNullOrEmpty(Eval("linkFile")?.ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Chọn bộ câu hỏi -->
        <div class="form-groups">
            <h5>Chọn Bộ Câu Hỏi</h5>
            <div>
                <asp:Label ID="lblBoCauHoi" runat="server" Text="Bộ Câu Hỏi" CssClass="form-labels" />
                <asp:DropDownList ID="ddlBoCauHoi" runat="server" CssClass="dropdown-custom btn-large">
                    <asp:ListItem Value="" Text="-- Chọn bộ câu hỏi --" />
                </asp:DropDownList><br />
                <asp:Label ID="lblThoiGianLam" runat="server" Text="Thời Gian Làm (phút)" CssClass="form-labels" />
                <asp:TextBox ID="txtThoiGianLam" runat="server" CssClass="form-input btn-medium" TextMode="Number" /><br />
                <asp:Button ID="btnThemBoCauHoi" runat="server" Text="Thêm Bộ Câu Hỏi" CssClass="btn btn-primary btn-large" OnClick="btnThemBoCauHoi_Click" />
            </div>
        </div>
    </div>
</asp:Content>
