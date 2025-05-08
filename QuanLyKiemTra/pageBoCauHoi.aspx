<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageBoCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageBoCauHoi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="container">
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Thêm mới bộ câu hỏi -->
        <div class="mb-3">
            <asp:Button ID="btnAddBoCauHoi" runat="server" Text="Thêm bộ câu hỏi" CssClass="btn-primary lg" OnClientClick="openBoCauHoiModal(false); return false;" />
        </div>

        <!-- Bảng danh sách bộ câu hỏi -->
        <div class="form-group">
            <asp:GridView ID="gvBoCauHoi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvBoCauHoi_RowCommand" EnableViewState="true">
                <Columns>
                    <asp:BoundField DataField="TenBoCauHoi" HeaderText="Tên bộ câu hỏi" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditBoCauHoi" runat="server" CommandName="EditBoCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-warning" ToolTip="Sửa" CausesValidation="false">
                                <i class="fas fa-edit"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDeleteBoCauHoi" runat="server" CommandName="DeleteBoCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa bộ câu hỏi này?');" CausesValidation="false">
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
                            <input type="text" id="txtSearchQuestion" class="search-bar" placeholder="Tìm kiếm câu hỏi..." />
                            <button type="button" id="btnSortQuestion" class="sort-button">Sắp xếp A-Z</button>
                            <asp:HiddenField ID="hfSelectedQuestions" runat="server" />
                            <div class="question-list" style="max-height: 300px; overflow-y: auto;">
                                <asp:Repeater ID="rptSelectCauHoi" runat="server" OnItemDataBound="rptSelectCauHoi_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="question-item" data-name='<%# HttpUtility.HtmlEncode(Eval("NoiDung").ToString()) %>'>
                                            <asp:CheckBox ID="chkSelectCauHoi" runat="server" CssClass="question-checkbox"
                                                data-id='<%# Eval("Id") %>' data-name='<%# HttpUtility.HtmlEncode(Eval("NoiDung").ToString()) %>' />
                                            <div class="question-details">
                                                <asp:Label ID="lblNoiDung" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("NoiDung").ToString()) %>' />
                                                <span><%# (bool)Eval("DapAn") ? "Đúng" : "Sai" %></span>
                                                <span><%# Eval("NgayTao", "{0:dd/MM/yyyy}") %></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary sm" data-bs-dismiss="modal">Đóng</button>
                        <asp:Button ID="btnSaveBoCauHoi" runat="server" Text="Lưu" CssClass="btn btn-primary sm" OnClick="btnSaveBoCauHoi_Click" />
                    </div>
                </div>
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        <script src="<%= ResolveUrl("~/style/search.js") %>"></script>

        <script>
            function openBoCauHoiModal(isEdit) {
                try {
                    // Đóng modal nếu đang mở
                    var modalElement = document.getElementById('boCauHoiModal');
                    if (modalElement && bootstrap.Modal.getInstance(modalElement)) {
                        bootstrap.Modal.getInstance(modalElement).hide();
                    }

                    // Reset form nếu không phải chế độ chỉnh sửa
                    var hfBoCauHoiId = document.getElementById('<%= hfBoCauHoiId.ClientID %>');
                    var txtTenBoCauHoi = document.getElementById('<%= txtTenBoCauHoi.ClientID %>');
                    var txtSearchQuestion = document.getElementById('txtSearchQuestion');
                    var hfSelectedQuestions = document.getElementById('<%= hfSelectedQuestions.ClientID %>');

                    if (!hfBoCauHoiId || !txtTenBoCauHoi || !txtSearchQuestion || !hfSelectedQuestions) {
                        console.error('Một hoặc nhiều phần tử không tồn tại.');
                        return;
                    }

                    if (!isEdit) {
                        hfBoCauHoiId.value = '';
                        txtTenBoCauHoi.value = '';
                        txtSearchQuestion.value = '';
                        hfSelectedQuestions.value = '';
                        $('.question-checkbox').prop('checked', false);
                    }

                    // Cập nhật tiêu đề modal và nút Lưu
                    var modalTitle = document.getElementById('boCauHoiModalLabel');
                    modalTitle.innerText = isEdit ? 'Cập nhật bộ câu hỏi' : 'Thêm bộ câu hỏi';
                    var btnSave = document.getElementById('<%= btnSaveBoCauHoi.ClientID %>');
                    btnSave.innerText = isEdit ? 'Cập nhật' : 'Lưu';

                    // Mở modal
                    if (!modalElement) {
                        console.error('Modal element not found');
                        return;
                    }
                    var modal = new bootstrap.Modal(modalElement);
                    modal.show();

                    // Debug
                    console.log('Modal opened, isEdit:', isEdit);
                } catch (error) {
                    console.error('Lỗi khi mở modal:', error);
                    alert('Đã xảy ra lỗi khi mở form. Vui lòng thử lại.');
                }
            }

            // Debug sự kiện click của nút Sửa và Xóa
            document.addEventListener('DOMContentLoaded', function () {
                var editButtons = document.querySelectorAll('.btn-warning');
                var deleteButtons = document.querySelectorAll('.btn-danger');
                editButtons.forEach(function (button) {
                    button.addEventListener('click', function (e) {
                        console.log('Nút Sửa được nhấn, ID:', button.getAttribute('data-arg') || button.getAttribute('data-__EVENTARGUMENT'));
                    });
                });
                deleteButtons.forEach(function (button) {
                    button.addEventListener('click', function (e) {
                        console.log('Nút Xóa được nhấn, ID:', button.getAttribute('data-arg') || button.getAttribute('data-__EVENTARGUMENT'));
                    });
                });

                // Cập nhật hfSelectedQuestions khi checkbox thay đổi
                $('.question-checkbox').on('change', function () {
                    var selectedIds = $('.question-checkbox:checked').map(function () {
                        return $(this).data('id');
                    }).get();
                    $('#<%= hfSelectedQuestions.ClientID %>').val(selectedIds.join(','));
                });
            });
        </script>
    </div>
</asp:Content>