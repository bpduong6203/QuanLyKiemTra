<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageBoCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageBoCauHoi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container">
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Thêm mới bộ câu hỏi -->
        <div class="mb-3">
            <asp:Button ID="btnAddBoCauHoi" runat="server" Text="Thêm bộ câu hỏi" CssClass="btn btn-primary btn-large" OnClientClick="openBoCauHoiModal(); return false;" />
        </div>

        <!-- Bảng danh sách bộ câu hỏi -->
        <div class="form-group">
            <asp:GridView ID="gvBoCauHoi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvBoCauHoi_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TenBoCauHoi" HeaderText="Tên bộ câu hỏi" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditBoCauHoi" runat="server" CommandName="EditBoCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-extra-small btn-warning" ToolTip="Sửa">
                            <i class="fas fa-edit"></i>
                        </asp:LinkButton>
                            <asp:LinkButton ID="btnDeleteBoCauHoi" runat="server" CommandName="DeleteBoCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-extra-small btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa bộ câu hỏi này?');">
                            <i class="fas fa-trash"></i>
                        </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Modal Thêm/Sửa bộ câu hỏi -->
        <div class="modal fade" id="boCauHoiModal" tabindex="-1" aria-labelledby="boCauHoiModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="boCauHoiModalLabel">Thêm bộ câu hỏi</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hfBoCauHoiId" runat="server" />
                        <div class="mb-3">
                            <label for="txtTenBoCauHoi" class="form-label">Tên bộ câu hỏi</label>
                            <asp:TextBox ID="txtTenBoCauHoi" runat="server" CssClass="form-input" Placeholder="Nhập tên bộ câu hỏi" />
                            <asp:RequiredFieldValidator ID="rfvTenBoCauHoi" runat="server" ControlToValidate="txtTenBoCauHoi"
                                ErrorMessage="Tên bộ câu hỏi là bắt buộc" CssClass="error-message" Display="Dynamic" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Danh sách câu hỏi</label>
                            <!-- Tìm kiếm -->
                            <div class="search-box">
                                <asp:TextBox ID="txtSearchCauHoi" runat="server" CssClass="form-input" Placeholder="Tìm kiếm câu hỏi..." AutoPostBack="true" OnTextChanged="txtSearchCauHoi_TextChanged" />
                            </div>
                            <!-- Lọc theo ngày tạo -->
                            <div class="filter-box date-filter">
                                <asp:TextBox ID="txtNgayTaoFrom" runat="server" CssClass="form-input btn-medium" Placeholder="Từ ngày (dd/MM/yyyy)" AutoPostBack="true" OnTextChanged="txtNgayTaoFilter_TextChanged" />
                                <asp:TextBox ID="txtNgayTaoTo" runat="server" CssClass="form-input btn-medium" Placeholder="Đến ngày (dd/MM/yyyy)" AutoPostBack="true" OnTextChanged="txtNgayTaoFilter_TextChanged" />
                            </div>
                            <!-- Bảng chọn câu hỏi -->
                            <asp:GridView ID="gvSelectCauHoi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                                DataKeyNames="Id">
                                <Columns>
                                    <asp:TemplateField HeaderText="Chọn">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectCauHoi" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NoiDung" HeaderText="Nội dung câu hỏi" />
                                    <asp:TemplateField HeaderText="Đáp án">
                                        <ItemTemplate>
                                            <%# (bool)Eval("DapAn") ? "Đúng" : "Sai" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary btn-small" data-bs-dismiss="modal">Đóng</button>
                        <asp:Button ID="btnSaveBoCauHoi" runat="server" Text="Lưu" CssClass="btn btn-primary btn-small" OnClick="btnSaveBoCauHoi_Click" />
                    </div>
                </div>
            </div>
        </div>


        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

        <script>
            function openBoCauHoiModal() {
                var hfBoCauHoiId = document.getElementById('<%= hfBoCauHoiId.ClientID %>');
            var txtTenBoCauHoi = document.getElementById('<%= txtTenBoCauHoi.ClientID %>');
            var txtSearchCauHoi = document.getElementById('<%= txtSearchCauHoi.ClientID %>');
            var txtNgayTaoFrom = document.getElementById('<%= txtNgayTaoFrom.ClientID %>');
            var txtNgayTaoTo = document.getElementById('<%= txtNgayTaoTo.ClientID %>');

                if (!hfBoCauHoiId) {
                    console.error('HiddenField hfBoCauHoiId không tồn tại.');
                    return;
                }

                var boCauHoiId = hfBoCauHoiId.value;
                document.getElementById('boCauHoiModalLabel').innerText = boCauHoiId ? 'Cập nhật bộ câu hỏi' : 'Thêm bộ câu hỏi';

                if (!boCauHoiId && txtTenBoCauHoi && txtSearchCauHoi && txtNgayTaoFrom && txtNgayTaoTo) {
                    txtTenBoCauHoi.value = '';
                    txtSearchCauHoi.value = '';
                    txtNgayTaoFrom.value = '';
                    txtNgayTaoTo.value = '';
                }

                var modal = new bootstrap.Modal(document.getElementById('boCauHoiModal'));
                modal.show();
            }
    </script>
    </div>

</asp:Content>
