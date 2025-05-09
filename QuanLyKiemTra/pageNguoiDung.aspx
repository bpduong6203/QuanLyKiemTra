<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageNguoiDung.aspx.cs" Inherits="QuanLyKiemTra.pageNguoiDung" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="table-container">
        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Đăng ký tài khoản -->
        <div class="mb-3">
            <asp:Button ID="btnAddNew" runat="server" Text="Đăng ký tài khoản" CssClass="m btn btn-primary md" OnClientClick="openAddModal(false); return false;" />
        </div>

        <!-- Bảng danh sách người dùng -->
        <div class="form-groups">
            <asp:GridView ID="gvNguoiDung" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvNguoiDung_RowCommand" EnableViewState="true">
                <Columns>
                    <asp:BoundField DataField="username" HeaderText="Tên đăng nhập" />
                    <asp:BoundField DataField="HoTen" HeaderText="Họ tên" />
                    <asp:TemplateField HeaderText="Vai trò">
                        <ItemTemplate>
                            <%# Eval("Roles.Ten") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Đơn vị">
                        <ItemTemplate>
                            <%# Eval("DonVi.TenDonVi") ?? "Chưa gán" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditNguoiDung" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-warning" ToolTip="Sửa" CausesValidation="false">
                                <i class="fas fa-edit"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteNguoiDung" CommandArgument='<%# Eval("Id") %>'
                                CssClass="custom-btn btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa người dùng này?');" CausesValidation="false">
                                <i class="fas fa-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal Đăng ký/Cập nhật người dùng -->
    <div class="modal fade" id="addNguoiDungModal" tabindex="-1" aria-labelledby="addNguoiDungModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addNguoiDungModalLabel">Đăng ký tài khoản</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfNguoiDungId" runat="server" />
                    <div class="mb-3">
                        <label for="txtUsername" class="form-label">Tên đăng nhập</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" Placeholder="Nhập tên đăng nhập" />
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                            ErrorMessage="Tên đăng nhập là bắt buộc" CssClass="error-message" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <label for="txtPassword" class="form-label">Mật khẩu</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" Placeholder="Nhập mật khẩu" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="Mật khẩu là bắt buộc" CssClass="error-message" Display="Dynamic" Enabled="false" />
                    </div>
                    <div class="mb-3">
                        <label for="txtHoTen" class="form-label">Họ tên</label>
                        <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-input" Placeholder="Nhập họ tên" />
                    </div>
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" Placeholder="Nhập email" TextMode="Email" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Email không hợp lệ" CssClass="error-message" Display="Dynamic"
                            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" />
                    </div>
                    <div class="mb-3">
                        <label for="txtSoDienThoai" class="form-label">Số điện thoại</label>
                        <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-input" Placeholder="Nhập số điện thoại" />
                    </div>
                    <div class="mb-3">
                        <label for="txtDiaChi" class="form-label">Địa chỉ</label>
                        <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-input" Placeholder="Nhập địa chỉ" />
                    </div>
                    <div class="mb-3">
                        <label for="ddlRole" class="form-label">Vai trò</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select" DataTextField="Ten" DataValueField="Id">
                            <asp:ListItem Text="--- Chọn vai trò ---" Value="" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
                            ErrorMessage="Vai trò là bắt buộc" CssClass="error-message" Display="Dynamic" InitialValue="" />
                    </div>
                    <div class="mb-3">
                        <label for="ddlDonVi" class="form-label">Đơn vị</label>
                        <asp:DropDownList ID="ddlDonVi" runat="server" CssClass="form-select" DataTextField="TenDonVi" DataValueField="Id">
                            <asp:ListItem Text="--- Chọn đơn vị ---" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary xs" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnSaveNguoiDung" runat="server" Text="Lưu" CssClass="btn btn-primary xs" OnClick="btnSaveNguoiDung_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS và Popper.js -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function openAddModal(isEdit) {
            try {
                // Đóng modal nếu đang mở
                var modalElement = document.getElementById('addNguoiDungModal');
                if (modalElement && bootstrap.Modal.getInstance(modalElement)) {
                    bootstrap.Modal.getInstance(modalElement).hide();
                }

                // Reset form nếu không phải chế độ chỉnh sửa
                if (!isEdit) {
                    document.getElementById('<%= txtUsername.ClientID %>').value = '';
                    document.getElementById('<%= txtPassword.ClientID %>').value = '';
                    document.getElementById('<%= txtHoTen.ClientID %>').value = '';
                    document.getElementById('<%= txtEmail.ClientID %>').value = '';
                    document.getElementById('<%= txtSoDienThoai.ClientID %>').value = '';
                    document.getElementById('<%= txtDiaChi.ClientID %>').value = '';
                    document.getElementById('<%= ddlRole.ClientID %>').value = '';
                    document.getElementById('<%= ddlDonVi.ClientID %>').value = '';
                    document.getElementById('<%= hfNguoiDungId.ClientID %>').value = '';

                    // Kích hoạt validator cho mật khẩu khi thêm mới
                    document.getElementById('<%= rfvPassword.ClientID %>').enabled = true;
                } else {
                    // Vô hiệu hóa validator cho mật khẩu khi chỉnh sửa
                    document.getElementById('<%= rfvPassword.ClientID %>').enabled = false;
                }

                // Cập nhật tiêu đề modal
                var modalTitle = document.getElementById('addNguoiDungModalLabel');
                modalTitle.innerText = isEdit ? 'Cập nhật người dùng' : 'Đăng ký tài khoản';

                // Cập nhật văn bản nút Lưu
                var btnSave = document.getElementById('<%= btnSaveNguoiDung.ClientID %>');
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
        });
    </script>
</asp:Content>