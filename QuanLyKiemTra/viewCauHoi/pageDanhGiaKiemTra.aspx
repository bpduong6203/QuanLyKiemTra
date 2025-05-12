<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageDanhGiaKiemTra.aspx.cs" Inherits="QuanLyKiemTra.viewCauHoi.pageDanhGiaKiemTra" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function startTimer(duration, display) {
            var timer = duration, minutes, seconds;
            var interval = setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);
                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;
                display.textContent = minutes + ":" + seconds;
                if (--timer < 0) {
                    clearInterval(interval);
                    document.getElementById('<%= btnHoanThanh.ClientID %>').click(); 
                }
            }, 1000);
        }
        window.onload = function () {
            var thoiGianLam = parseInt('<%= ViewState["ThoiGianLam"] ?? 0 %>') * 60; 
            var display = document.querySelector('#timer');
            if (thoiGianLam > 0) {
                startTimer(thoiGianLam, display);
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h4>Làm Bài Kiểm Tra</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label" Visible="false" />
        <asp:Panel ID="pnlKiemTra" runat="server" Visible="false">
            <div id="timer" class="timer xs"></div>
            <asp:Repeater ID="rptCauHoi" runat="server" OnItemDataBound="rptCauHoi_ItemDataBound">
                <ItemTemplate>
                    <div class="question-block">
                        <div class="question-content">
                            Câu hỏi <%# Eval("STT") %>: <%# Eval("CauHoi.NoiDung") %>
                        </div>
                        <div class="answer-option">
                            <asp:RadioButton ID="rbDapAnTrue" runat="server" GroupName='<%# "DapAn_" + Eval("CauHoiId") %>' Text="Đúng" />
                            <asp:RadioButton ID="rbDapAnFalse" runat="server" GroupName='<%# "DapAn_" + Eval("CauHoiId") %>' Text="Sai" />
                            <br />
                            <asp:Label ID="lblCauTraLoiPhu" runat="server" Text="Câu trả lời phụ (nếu có):" AssociatedControlID="txtCauTraLoiPhu" />
                            <asp:TextBox ID="txtCauTraLoiPhu" runat="server" CssClass="form-control" Rows="3" />
                            <asp:HiddenField ID="hfCauHoiId" runat="server" Value='<%# Eval("CauHoiId") %>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="action-buttons">
                <asp:Button ID="btnLuuLai" runat="server" Text="Lưu lại" CssClass="btn btn-primary" OnClick="btnLuuLai_Click" />
                <asp:Button ID="btnHoanThanh" runat="server" Text="Hoàn thành" CssClass="btn btn-success" OnClick="btnHoanThanh_Click" OnClientClick="return confirm('Bạn có chắc muốn nộp bài?');" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>