﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCTKeHoach.aspx.cs" Inherits="QuanLyKiemTra.pageCTKeHoach" %>

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
            <!-- Danh sách tài liệu/đề cương -->
            <div class="form-groups lg">
                <asp:Label ID="lblDocuments" runat="server" Text="Tài liệu và đề cương" CssClass="form-label" />
                <asp:Repeater ID="rptDocuments" runat="server">
                    <ItemTemplate>
                        <div class="file-block">
                            <span class="file-type">
                                <i class='<%# GetLoaiTaiLieuIcon(Eval("LoaiTaiLieu")) %>'></i>
                                <%# GetLoaiTaiLieuText(Eval("LoaiTaiLieu")) %>
                            </span>
                            <asp:HyperLink ID="hlFileMau" runat="server" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("TenTaiLieu").ToString()) %>' 
                                NavigateUrl='<%# Eval("linkfile") %>' 
                                Target="_blank" 
                                CssClass="form-link" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Biên bản kiểm tra -->
            <div class="form-groups lg">
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

        <div class="form-group-row">

            <!-- Yêu cầu giải trình -->
            <div class="form-groups lg">
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
                                <asp:LinkButton ID="btnXoaFile" runat="server" Text="X" CssClass="btn-detele btn-danger" CommandName="XoaFile" CommandArgument='<%# Eval("Id") %>' Visible='<%# HasEvaluationRights() %>' OnClientClick="return confirm('Bạn có chắc muốn xóa file này?');" />
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
            <div class="form-groups lg">
                <h5>Quản Lý Bộ Câu Hỏi</h5>
                <asp:Panel ID="pnlThemBoCauHoi" runat="server" Visible='<%# GetRole() == "TruongDoan" %>'>
                    <div>
                        <asp:Label ID="lblBoCauHoi" runat="server" Text="Bộ Câu Hỏi" CssClass="form-labels" />
                        <asp:DropDownList ID="ddlBoCauHoi" runat="server" CssClass="dropdown-custom lg">
                            <asp:ListItem Value="" Text="-- Chọn bộ câu hỏi --" />
                        </asp:DropDownList><br />
                        <asp:Label ID="lblThoiGianLam" runat="server" Text="Thời Gian Làm (phút)" CssClass="form-labels" />
                        <asp:TextBox ID="txtThoiGianLam" runat="server" CssClass="form-input xs m" TextMode="Number" /><br />
                        <asp:Button ID="btnThemBoCauHoi" runat="server" Text="Thêm Bộ Câu Hỏi" CssClass="btn btn-primary lg" OnClick="btnThemBoCauHoi_Click" />
                    </div>
                </asp:Panel>
                <h6 class="mt-3">Danh Sách Bộ Câu Hỏi Đã Thêm</h6>
                <asp:GridView ID="gvBoCauHoi" runat="server" AutoGenerateColumns="False" CssClass="grid-view" OnRowCommand="gvBoCauHoi_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="TenBoCauHoi" HeaderText="Tên Bộ Câu Hỏi" />
                        <asp:BoundField DataField="ThoiGianLam" HeaderText="Thời Gian (phút)" />
                        <asp:TemplateField HeaderText="Hành Động">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnXoa" runat="server" CssClass="btn btn-danger btn-sm" CommandName="Xoa" CommandArgument='<%# Eval("Id") %>' Visible='<%# GetRole() == "TruongDoan" %>' OnClientClick="return confirm('Bạn có chắc muốn xóa bộ câu hỏi này?');" title="Xóa">
                                    <i class="fas fa-trash"></i>
                                </asp:LinkButton>
                                <asp:HyperLink ID="hlXemDanhSach" runat="server" CssClass="btn btn-info btn-sm" NavigateUrl='<%# "~/danh-sach-ket-qua/" + Eval("BoCauHoiId") %>' Visible='<%# GetRole() == "TruongDoan" || GetRole() == "ThanhVien" %>' title="Xem danh sách">
                                    <i class="fas fa-list"></i>
                                </asp:HyperLink>
                                <asp:HyperLink ID="hlThucHienKiemTra" runat="server" Text="Làm kiểm tra" CssClass="btn btn-success btn-sm" NavigateUrl='<%# "~/kiem-tra-ke-hoach/" + Eval("BoCauHoiId") %>' Visible='<%# GetRole() == "ThanhVien" || GetRole() == "DonVi" %>' />
                                <asp:HyperLink ID="hlXemKetQua" runat="server" CssClass="btn btn-primary btn-sm" NavigateUrl='<%# "~/ket-qua-kiem-tra/" + Eval("BoCauHoiId") %>' Visible='<%# (GetRole() == "ThanhVien" || GetRole() == "DonVi") && DaHoanThanh(Eval("BoCauHoiId").ToString()) %>' title="Xem kết quả">
                                    <i class="fas fa-eye"></i>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="text-center">Chưa có bộ câu hỏi nào được thêm.</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>
