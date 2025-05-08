<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageCauHoi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="table-container">
        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Thêm mới -->
        <div class="mb-3">
            <asp:Button ID="btnAddNew" runat="server" Text="Thêm mới" CssClass="btn-primary lg" OnClientClick="openAddModal(); return false;" />
        </div>

        <!-- Bảng danh sách câu hỏi -->
        <div class="form-groups">
            <asp:GridView ID="gvCauHoi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvCauHoi_RowCommand" AllowPaging="true" PageSize="10"
                OnPageIndexChanging="gvCauHoi_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="NoiDung" HeaderText="Nội dung" />
                    <asp:TemplateField HeaderText="Đáp án">
                        <ItemTemplate>
                            <%# (bool)Eval("DapAn") ? "Đúng" : "Sai" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="linkTaiLieu" HeaderText="Link tài liệu" />
                    <asp:BoundField DataField="ndGiaiTrinh" HeaderText="Nội dung giải trình" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-warning" ToolTip="Sửa">
                                <i class="fas fa-edit"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa câu hỏi này?');">
                                <i class="fas fa-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="grid-pager" />
                <PagerSettings Mode="NumericFirstLast" FirstPageText="Đầu" LastPageText="Cuối" PreviousPageText="Trước" NextPageText="Sau" />
            </asp:GridView>
        </div>
    </div>

    <!-- Modal Thêm câu hỏi -->
    <div class="modal fade" id="addCauHoiModal" tabindex="-1" aria-labelledby="addCauHoiModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addCauHoiModalLabel">Thêm câu hỏi mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfCauHoiId" runat="server" />
                    <div class="mb-3">
                        <label for="txtNoiDung" class="form-label">Nội dung</label>
                        <asp:TextBox ID="txtNoiDung" runat="server" CssClass="form-input" Placeholder="Nhập nội dung câu hỏi" TextMode="MultiLine" />
                    </div>
                    <div class="mb-3">
                        <label for="chkDapAn" class="form-label">Đáp án</label>
                        <div class="switch-container">
                            <asp:CheckBox ID="chkDapAn" runat="server" CssClass="switch-checkbox" />
                            <label class="switch">
                                <input type="checkbox" id="switchInput" onchange="updateStatusText()">
                                <span class="slider"></span>
                            </label>
                            <span class="status-text" id="statusText">Sai</span>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="txtLinkTaiLieu" class="form-label">Link tài liệu</label>
                        <asp:TextBox ID="txtLinkTaiLieu" runat="server" CssClass="form-input" Placeholder="Nhập link tài liệu" />
                    </div>
                    <div class="mb-3">
                        <label for="txtNdGiaiTrinh" class="form-label">Nội dung giải trình</label>
                        <asp:TextBox ID="txtNdGiaiTrinh" runat="server" CssClass="form-input" Placeholder="Nhập nội dung giải trình" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn-secondary sm" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnSaveCauHoi" runat="server" Text="Lưu" CssClass="btn-primary sm" OnClick="btnSaveCauHoi_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS và Popper.js -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function openAddModal(isEdit) {
            // Reset form
            if (!isEdit) {
                document.getElementById('<%= txtNoiDung.ClientID %>').value = '';
                document.getElementById('<%= chkDapAn.ClientID %>').checked = false;
                document.getElementById('switchInput').checked = false;
                document.getElementById('statusText').innerText = 'Sai';
                document.getElementById('<%= txtLinkTaiLieu.ClientID %>').value = '';
                document.getElementById('<%= txtNdGiaiTrinh.ClientID %>').value = '';
                document.getElementById('<%= hfCauHoiId.ClientID %>').value = '';
            } else {
                // Đồng bộ switch và status text khi chỉnh sửa
                var chkDapAn = document.getElementById('<%= chkDapAn.ClientID %>');
                document.getElementById('switchInput').checked = chkDapAn.checked;
                document.getElementById('statusText').innerText = chkDapAn.checked ? 'Đúng' : 'Sai';
            }

            // Cập nhật tiêu đề modal
            var modalTitle = document.getElementById('addCauHoiModalLabel');
            modalTitle.innerText = isEdit ? 'Chỉnh sửa câu hỏi' : 'Thêm câu hỏi mới';

            // Cập nhật văn bản nút Lưu
            var btnSave = document.getElementById('<%= btnSaveCauHoi.ClientID %>');
            btnSave.innerText = isEdit ? 'Cập nhật' : 'Lưu';

            // Mở modal
            var modal = new bootstrap.Modal(document.getElementById('addCauHoiModal'));
            modal.show();
        }

        function updateStatusText() {
            var switchInput = document.getElementById('switchInput');
            var statusText = document.getElementById('statusText');
            var chkDapAn = document.getElementById('<%= chkDapAn.ClientID %>');
            statusText.innerText = switchInput.checked ? 'Đúng' : 'Sai';
            chkDapAn.checked = switchInput.checked; 
        }
    </script>
</asp:Content>