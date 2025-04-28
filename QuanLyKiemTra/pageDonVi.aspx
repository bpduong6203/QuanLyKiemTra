<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageDonVi.aspx.cs" Inherits="QuanLyKiemTra.pageDonVi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="table-container">
        <!-- Thông báo -->
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <!-- Nút Thêm mới -->
        <div class="mb-3">
            <asp:Button ID="btnAddNew" runat="server" Text="Thêm mới" CssClass="btn btn-primary btn-large" OnClientClick="openAddModal(); return false;" />
        </div>

        <!-- Bảng danh sách đơn vị -->
        <div class="form-group">
            <asp:GridView ID="gvDonVi" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvDonVi_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TenDonVi" HeaderText="Tên đơn vị" />
                    <asp:BoundField DataField="DiaChi" HeaderText="Địa chỉ" />
                    <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                    <asp:BoundField DataField="NguoiDaiDien" HeaderText="Người đại diện" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditDonVi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-extra-small btn-warning" ToolTip="Sửa">
                            <i class="fas fa-edit"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteDonVi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-extra-small btn-danger" ToolTip="Xóa" OnClientClick="return confirm('Bạn có chắc muốn xóa đơn vị này?');">
                            <i class="fas fa-trash"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Modal Thêm đơn vị -->
    <div class="modal fade" id="addDonViModal" tabindex="-1" aria-labelledby="addDonViModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addDonViModalLabel">Thêm đơn vị mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfDonViId" runat="server" />
                    <div class="mb-3">
                        <label for="txtTenDonVi" class="form-label">Tên đơn vị</label>
                        <asp:TextBox ID="txtTenDonVi" runat="server" CssClass="form-control" Placeholder="Nhập tên đơn vị" />
                        <div class="mb-3">
                            <label for="txtDiaChi" class="form-label">Địa chỉ</label>
                            <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-control" Placeholder="Nhập địa chỉ" />
                        </div>
                        <div class="mb-3">
                            <label for="txtSoDienThoai" class="form-label">Số điện thoại</label>
                            <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" Placeholder="Nhập số điện thoại" />
                        </div>
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Nhập email" TextMode="Email" />
                        </div>
                        <div class="mb-3">
                            <label for="txtNguoiDaiDien" class="form-label">Người đại diện</label>
                            <asp:TextBox ID="txtNguoiDaiDien" runat="server" CssClass="form-control" Placeholder="Nhập tên người đại diện" />
                        </div>
                        <div class="mb-3">
                            <label for="txtChucVuNguoiDaiDien" class="form-label">Chức vụ</label>
                            <asp:TextBox ID="txtChucVuNguoiDaiDien" runat="server" CssClass="form-control" Placeholder="Nhập chức vụ" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-small" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnSaveDonVi" runat="server" Text="Lưu" CssClass="btn btn-primary btn-small" OnClick="btnSaveDonVi_Click" />
                </div>
            </div>
        </div>

        <!-- Bootstrap JS và Popper.js -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        <script>
            function openAddModal() {
                // Reset form
                document.getElementById('<%= txtTenDonVi.ClientID %>').value = '';
            document.getElementById('<%= txtDiaChi.ClientID %>').value = '';
            document.getElementById('<%= txtSoDienThoai.ClientID %>').value = '';
            document.getElementById('<%= txtEmail.ClientID %>').value = '';
            document.getElementById('<%= txtNguoiDaiDien.ClientID %>').value = '';
            document.getElementById('<%= txtChucVuNguoiDaiDien.ClientID %>').value = '';
            document.getElementById('<%= hfDonViId.ClientID %>').value = '';

                // Mở modal
                var modal = new bootstrap.Modal(document.getElementById('addDonViModal'));
                modal.show();
            }
        </script>
</asp:Content>
