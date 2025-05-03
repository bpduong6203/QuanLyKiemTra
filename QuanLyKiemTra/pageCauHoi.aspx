<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageCauHoi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />


    <div class="container">
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Thêm câu hỏi -->
        <div class="mb-3">
            <asp:Button ID="btnAddCauHoi" runat="server" Text="Thêm câu hỏi" CssClass="btn btn-primary btn-large" OnClientClick="openCauHoiModal(); return false;" />
        </div>

        <!-- Bảng danh sách câu hỏi -->
        <div class="form-group">
            <asp:GridView ID="gvCauHoi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvCauHoi_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NoiDung" HeaderText="Nội dung câu hỏi" />
                    <asp:TemplateField HeaderText="Đáp án">
                        <ItemTemplate>
                            <%# (bool)Eval("DapAn") ? "Đúng" : "Sai" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="linkTaiLieu" HeaderText="Link tài liệu" />
                    <asp:BoundField DataField="ndGiaiTrinh" HeaderText="Giải trình" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditCauHoi" runat="server" CommandName="EditCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-warning" ToolTip="Sửa">
                            <i class="fas fa-edit"></i>
                        </asp:LinkButton>
                            <asp:LinkButton ID="btnDeleteCauHoi" runat="server" CommandName="DeleteCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa câu hỏi này?');">
                            <i class="fas fa-trash"></i>
                        </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Modal Thêm/Sửa câu hỏi -->
        <div class="modal fade" id="cauHoiModal" tabindex="-1" aria-labelledby="cauHoiModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="cauHoiModalLabel">Thêm câu hỏi</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hfCauHoiId" runat="server" />
                        <div class="mb-3">
                            <label for="txtNoiDung" class="form-label">Nội dung câu hỏi</label>
                            <asp:TextBox ID="txtNoiDung" runat="server" CssClass="form-input" Placeholder="Nhập nội dung câu hỏi" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvNoiDung" runat="server" ControlToValidate="txtNoiDung"
                                ErrorMessage="Nội dung câu hỏi là bắt buộc" CssClass="error-message" Display="Dynamic" />
                        </div>
                        <div class="mb-3">
                            <label for="ddlDapAn" class="form-label">Đáp án</label>
                            <asp:DropDownList ID="ddlDapAn" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Đúng" Value="true" />
                                <asp:ListItem Text="Sai" Value="false" />
                            </asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label for="txtLinkTaiLieu" class="form-label">Link tài liệu</label>
                            <asp:TextBox ID="txtLinkTaiLieu" runat="server" CssClass="form-input" Placeholder="Nhập link tài liệu" />
                        </div>
                        <div class="mb-3">
                            <label for="txtNdGiaiTrinh" class="form-label">Giải trình</label>
                            <asp:TextBox ID="txtNdGiaiTrinh" runat="server" CssClass="form-input" Placeholder="Nhập giải trình" TextMode="MultiLine" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary btn-small" data-bs-dismiss="modal">Đóng</button>
                        <asp:Button ID="btnSaveCauHoi" runat="server" Text="Lưu" CssClass="btn btn-primary btn-small" OnClick="btnSaveCauHoi_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Bootstrap JS -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        <script>
            function openCauHoiModal() {
                var hfCauHoiId = document.getElementById('<%= hfCauHoiId.ClientID %>');
            var txtNoiDung = document.getElementById('<%= txtNoiDung.ClientID %>');
            var ddlDapAn = document.getElementById('<%= ddlDapAn.ClientID %>');
            var txtLinkTaiLieu = document.getElementById('<%= txtLinkTaiLieu.ClientID %>');
            var txtNdGiaiTrinh = document.getElementById('<%= txtNdGiaiTrinh.ClientID %>');

                if (!hfCauHoiId) {
                    console.error('HiddenField hfCauHoiId không tồn tại.');
                    return;
                }

                var cauHoiId = hfCauHoiId.value;
                document.getElementById('cauHoiModalLabel').innerText = cauHoiId ? 'Cập nhật câu hỏi' : 'Thêm câu hỏi';

                if (!cauHoiId && txtNoiDung && ddlDapAn && txtLinkTaiLieu && txtNdGiaiTrinh) {
                    txtNoiDung.value = '';
                    ddlDapAn.value = 'true';
                    txtLinkTaiLieu.value = '';
                    txtNdGiaiTrinh.value = '';
                }

                var modal = new bootstrap.Modal(document.getElementById('cauHoiModal'));
                modal.show();
            }
    </script>
    </div>

</asp:Content>
