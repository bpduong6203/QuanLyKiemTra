<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCTYeuCauGiaiTrinh.aspx.cs" Inherits="QuanLyKiemTra.pageCTYeuCauGiaiTrinh" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="container">
        <h4>Chi Tiết Yêu Cầu Giải Trình</h4>

        <!-- Thông tin giải trình -->
        <div class="form-group">
            <h5>Thông Tin Yêu Cầu</h5>
            <div class="form-row">
                <asp:Label ID="lblNguoiYeuCau" runat="server" CssClass="form-labels" Text="Người Yêu Cầu: " />
                <asp:Label ID="lblNguoiYeuCauValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblNgayTao" runat="server" CssClass="form-labels" Text="Ngày Tạo: " />
                <asp:Label ID="lblNgayTaoValue" runat="server" CssClass="form-value" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblFileMau" runat="server" CssClass="form-labels" Text="File Mẫu: " />
                <asp:Repeater ID="rptFiles" runat="server" OnItemCommand="rptFiles_ItemCommand">
                    <ItemTemplate>
                        <div class="file-block">
                            <asp:HyperLink ID="hlFileMau" runat="server" Text='<%# Eval("FileName") %>' NavigateUrl='<%# Eval("LinkFile") %>' Target="_blank" CssClass="form-link" />
                            <asp:LinkButton ID="btnXoaFile" runat="server" Text="X" CssClass="delete-btn" CommandName="XoaFile" CommandArgument='<%# Eval("Id") %>' Visible='<%# HasEvaluationRights() %>' OnClientClick="return confirm('Bạn có chắc muốn xóa file này?');" />
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblNoFiles" runat="server" Text="Không có file mẫu" Visible='<%# rptFiles.Items.Count == 0 %>' />
                    </FooterTemplate>
                </asp:Repeater>
                <asp:HyperLink ID="hlFileMau" runat="server" Text="Tải xuống" Target="_blank" CssClass="form-link" Visible="false" />
            </div>
            <div class="form-row">
                <asp:Label ID="lblTrangThaiTongThe" runat="server" CssClass="form-labels" Text="Trạng Thái Tổng Thể: " />
                <asp:Label ID="lblTrangThaiTongTheValue" runat="server" CssClass="form-value" />
            </div>
        </div>

        <!-- Form thêm/sửa file mẫu (chỉ cho TruongDoan, ThanhVien) -->
        <asp:Panel ID="pnlThemSuaFileMau" runat="server" CssClass="form-group" Visible="false">
            <h5>Thêm File Mẫu</h5>
            <asp:Label ID="lblFileMauUpload" runat="server" CssClass="form-labels" Text="Chọn File Mẫu: " />
            <div class="form-row">
                <asp:FileUpload ID="fuFileMau" runat="server" CssClass="form-input lg" Accept=".doc,.docx,.pdf" AllowMultiple="true" />
            </div>
            <asp:Button ID="btnThemFileMau" runat="server" Text="Thêm File Mẫu" CssClass="btn btn-primary lg" OnClick="btnThemFileMau_Click" />
        </asp:Panel>

        <!-- Form gửi giải trình -->
        <asp:Panel ID="pnlGuiGiaiTrinh" runat="server" CssClass="form-group" Visible="false">
            <h5>Gửi Giải Trình</h5>
            <asp:Label ID="lblNoiDungGiaiTrinh" runat="server" CssClass="form-labels" Text="Nội Dung Giải Trình: " />
            <div class="form-row">
                <asp:TextBox ID="txtNoiDungGiaiTrinh" runat="server" CssClass="form-input xxl" TextMode="MultiLine" Rows="3" />
            </div>
            <asp:Label ID="lblFileGiaiTrinh" runat="server" CssClass="form-labels" Text="File Giải Trình: " />
            <div class="form-row">
                <asp:FileUpload ID="fuFileGiaiTrinh" runat="server" CssClass="form-input lg" Accept=".doc,.docx,.pdf" AllowMultiple="true" />
            </div>
            <asp:Button ID="btnGuiGiaiTrinh" runat="server" Text="Gửi Giải Trình" CssClass="btn btn-primary lg" OnClick="btnGuiGiaiTrinh_Click" />
        </asp:Panel>

        <!-- Danh sách nội dung giải trình -->
        <div class="form-group">
            <h5>Lịch Sử Giải Trình</h5>
            <asp:GridView ID="gvNDGiaiTrinh" runat="server" AutoGenerateColumns="False" CssClass="grid-view" 
                DataKeyNames="Id" OnRowCommand="gvNDGiaiTrinh_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NoiDung" HeaderText="Nội Dung" />
                    <asp:TemplateField HeaderText="File Giải Trình">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlFileND" runat="server" Text='<%# Eval("FileName") ?? "Tải xuống" %>' NavigateUrl='<%# Eval("linkfile") %>' Target="_blank" Visible='<%# !string.IsNullOrEmpty(Eval("linkfile")?.ToString()) %>' />
                            <asp:Label ID="lblNoFileND" runat="server" Text="Không có file" Visible='<%# string.IsNullOrEmpty(Eval("linkfile")?.ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày Gửi" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="TrangThai" HeaderText="Trạng Thái" />
                    <asp:TemplateField HeaderText="Hành Động" Visible="false">
                        <ItemTemplate>
                            <asp:Button ID="btnXacNhanDat" runat="server" Text="Xác Nhận Đạt" CssClass="custom-btn btn-info" 
                                CommandName="XacNhanDat" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("TrangThai").ToString() == "Chờ Đánh Giá" %>' />
                            <asp:Button ID="btnYeuCauChinhSua" runat="server" Text="Yêu Cầu Chỉnh Sửa" CssClass="custom-btn btn-warning" 
                                CommandName="YeuCauChinhSua" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("TrangThai").ToString() == "Chờ Đánh Giá" %>' />
                            <asp:Button ID="btnXoa" runat="server" Text="Xóa" CssClass="custom-btn btn-danger" 
                                CommandName="Xoa" CommandArgument='<%# Eval("Id") %>' Visible='<%# Eval("TrangThai").ToString() == "Chưa Đạt" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Form yêu cầu chỉnh sửa -->
        <asp:Panel ID="pnlYeuCauChinhSua" runat="server" CssClass="form-group" Visible="false">
            <h5>Yêu Cầu Chỉnh Sửa</h5>
            <asp:Label ID="lblGhiChuChinhSua" runat="server" CssClass="form-labels" Text="Ghi Chú Chỉnh Sửa: " />
            <div class="form-row">
                <asp:TextBox ID="txtGhiChuChinhSua" runat="server" CssClass="form-input xxl" TextMode="MultiLine" Rows="3" />
            </div>
            <asp:Button ID="btnGuiYeuCauChinhSua" runat="server" Text="Gửi Yêu Cầu Chỉnh Sửa" CssClass="btn btn-primary lg" OnClick="btnGuiYeuCauChinhSua_Click" />
            <asp:Button ID="btnHuyChinhSua" runat="server" Text="Hủy" CssClass="btn btn-secondary lg" OnClick="btnHuyChinhSua_Click" />
        </asp:Panel>
    </div>
</asp:Content>