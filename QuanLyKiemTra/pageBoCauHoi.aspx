<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageBoCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageBoCauHoi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h4>Danh sách bộ câu hỏi</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="mb-3" Visible="false" />

        <!-- Thanh tìm kiếm và nút sắp xếp -->
        <div class="search-container d-flex align-items-center mb-4">
            <asp:TextBox ID="txtSearchQuestion" runat="server" CssClass="form-input me-2" Placeholder="Tìm kiếm bộ câu hỏi..." />
            <asp:Button ID="btnAddNew" runat="server" Text="Thêm mới" CssClass="btn btn-success" OnClick="btnAddNew_Click" />
        </div>

        <!-- Hidden field lưu ID bộ câu hỏi được chọn -->
        <asp:HiddenField ID="hfSelectedQuestions" runat="server" />

        <!-- Danh sách bộ câu hỏi -->
        <div class="question-list">
            <asp:GridView ID="gvBoCauHoi" runat="server" AutoGenerateColumns="False" CssClass="grid-view"
                DataKeyNames="Id" OnRowCommand="gvBoCauHoi_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Chọn">
                        <ItemTemplate>
                            <input type="checkbox" class="question-checkbox" data-id='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TenBoCauHoi" HeaderText="Tên bộ câu hỏi" />
                    <asp:BoundField DataField="NgayTao" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Hành động">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditBoCauHoi" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-primary btn-sm">
                                <i class="fas fa-edit"></i> Chỉnh sửa
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>