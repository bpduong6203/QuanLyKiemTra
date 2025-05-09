<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCTKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageCTKeHoach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Chi Tiết Kế Hoạch Kiểm Tra</h4>

        <!-- Thông tin kế hoạch -->
        <div class="form-group-row">
            <!-- Thông tin kế hoạch -->
            <div class="form-groups lg">
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
                <div class="form-row">
                    <asp:Label ID="lblNguoiTao" runat="server" CssClass="form-labels" Text="Người Tạo: " />
                    <asp:Label ID="lblNguoiTaoValue" runat="server" CssClass="form-value" />
                </div>
            </div>

            <!-- Thông tin đơn vị -->
            <div class="form-groups lg">
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
                    <asp:Label ID="lblChucVuNguoiDaiDien" runat="server" CssClass="form-labels" Text="Chức Vụ: " />
                    <asp:Label ID="lblChucVuNguoiDaiDienValue" runat="server" CssClass="form-value" />
                </div>
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

        <!-- Thành viên đơn vị -->
        <div class="form-groups">
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
        <div class="form-group-row">
            <!-- Yêu cầu giải trình -->
            <div class="form-groups xl">
                <h5>Bảng Giải Trình</h5>
                <asp:Panel ID="pnlGiaiTrinh" runat="server" Visible="false">
                    <asp:Label ID="lblGiaiTrinhMessage" runat="server" Text="Kế hoạch chưa có giải trình." CssClass="message-label warning-message" /><br />
                    <asp:Label ID="lblFileMauUpload" runat="server" CssClass="form-labels" Text="Chọn File Mẫu: " />
                    <div class="form-row">
                        <asp:FileUpload ID="fuFileMau" runat="server" CssClass="form-input lg" Accept=".doc,.docx,.pdf" AllowMultiple="true" Visible='<%# HasEvaluationRights() %>' />
                    </div>
                    <asp:Button ID="btnYeuCauGiaiTrinh" runat="server" Text="Yêu Cầu Giải Trình" CssClass="btn btn-primary lg" OnClick="btnYeuCauGiaiTrinh_Click" Visible='<%# HasEvaluationRights() %>' />
                </asp:Panel>
                <div class="form-row">
                    <asp:Label ID="lblFileMau" runat="server" CssClass="form-labels" Text="File Mẫu: " />
                    <asp:Repeater ID="rptFiles" runat="server" OnItemCommand="rptFiles_ItemCommand">
                        <ItemTemplate>
                            <div class="file-block">
                                <asp:HyperLink ID="hlFileMau" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("FileName").ToString()) %>' NavigateUrl='<%# Eval("LinkFile") %>' Target="_blank" CssClass="form-link" />
                                <asp:LinkButton ID="btnXoaFile" runat="server" Text="X" CssClass="delete-btn" CommandName="XoaFile" CommandArgument='<%# Eval("Id") %>' Visible='<%# HasEvaluationRights() %>' OnClientClick="return confirm('Bạn có chắc muốn xóa file này?');" />
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblNoFiles" runat="server" Text="Không có file mẫu" Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>' CssClass="message-label" />
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <asp:Button ID="btnXemChiTiet" runat="server" Text="Xem Chi Tiết" CssClass="btn btn-info" OnClick="btnXemChiTiet_Click" Visible="false" />
            </div>

            <!-- Chọn bộ câu hỏi -->
            <div class="form-groups md">
                <h5>Chọn Bộ Câu Hỏi</h5>
                <div>
                    <asp:Label ID="lblBoCauHoi" runat="server" Text="Bộ Câu Hỏi" CssClass="form-labels" />
                    <asp:DropDownList ID="ddlBoCauHoi" runat="server" CssClass="dropdown-custom lg">
                        <asp:ListItem Value="" Text="-- Chọn bộ câu hỏi --" />
                    </asp:DropDownList><br />
                    <asp:Label ID="lblThoiGianLam" runat="server" Text="Thời Gian Làm (phút)" CssClass="form-labels" />
                    <asp:TextBox ID="txtThoiGianLam" runat="server" CssClass="form-input sm" TextMode="Number" /><br />
                    <asp:Button ID="btnThemBoCauHoi" runat="server" Text="Thêm Bộ Câu Hỏi" CssClass="btn btn-primary lg" OnClick="btnThemBoCauHoi_Click" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>