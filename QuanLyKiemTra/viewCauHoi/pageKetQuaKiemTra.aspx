<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageKetQuaKiemTra.aspx.cs" Inherits="QuanLyKiemTra.viewCauHoi.pageKetQuaKiemTra" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h4>Kết Quả Bài Kiểm Tra</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false" />
        <asp:Panel ID="pnlFilter" runat="server" CssClass="filter-section" Visible="false">
            <asp:DropDownList ID="ddlThanhVien" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlThanhVien_SelectedIndexChanged">
                <asp:ListItem Text="-- Chọn thành viên --" Value="" />
            </asp:DropDownList>
        </asp:Panel>
        <asp:Panel ID="pnlKetQua" runat="server" Visible="false">
            <div class="summary">
                <asp:Label ID="lblSummary" runat="server" />
            </div>
            <asp:Repeater ID="rptKetQua" runat="server" OnItemDataBound="rptKetQua_ItemDataBound">
                <ItemTemplate>
                    <div class="result-block">
                        <div class="question-content">
                            Câu hỏi <%# Container.ItemIndex + 1 %>: <%# Eval("CauHoi.NoiDung") %>
                        </div>
                        <div class="answer-content">
                            <p>
                                Đáp án: 
                                <asp:Label ID="lblDapAnNguoiDung" runat="server" CssClass="answer-status"
                                    Text='<%# Eval("DapAn") != null ? ((bool)Eval("DapAn.DapAnTraLoi") ? "Đúng" : "Sai") : "Chưa trả lời" %>' />
                                <i class='<%# Eval("DapAn") != null && ((bool)Eval("DapAn.DapAnTraLoi") == (bool)Eval("CauHoi.DapAn")) ? "fas fa-check correct" : "fas fa-times incorrect" %>'></i>
                                / <%# (bool)Eval("CauHoi.DapAn") ? "Đúng" : "Sai" %>
                            </p>
                            <asp:Panel ID="pnlCauTraLoiPhu" runat="server" Visible='<%# Eval("DapAn") != null && !string.IsNullOrEmpty(Eval("DapAn.CauTraLoiPhu").ToString()) %>'>
                                <p>Câu trả lời phụ: <%# Eval("DapAn.CauTraLoiPhu") %></p>
                            </asp:Panel>
                            <asp:Panel ID="pnlGiaiTrinh" runat="server" Visible='<%# !string.IsNullOrEmpty(Eval("CauHoi.ndGiaiTrinh").ToString()) %>'>
                                <p>Giải trình: <%# Eval("CauHoi.ndGiaiTrinh") %></p>
                            </asp:Panel>
                            <asp:HyperLink ID="hlTaiLieu" runat="server" Text="Xem tài liệu"
                                NavigateUrl='<%# Eval("CauHoi.linkTaiLieu") %>'
                                Visible='<%# !string.IsNullOrEmpty(Eval("CauHoi.linkTaiLieu").ToString()) %>'
                                CssClass="btn btn-info btn-sm" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>
</asp:Content>