<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageEditBoCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageEditBoCauHoi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            // Thêm câu hỏi mới
            $("#btnAddQuestion").click(function () {
                var questionCount = $(".question-block").length;
                var questionHtml = `
                <div class="question-block">
                    <button type="button" class="btn btn-danger btn-sm btn-remove">Xóa</button>
                    <div class="form-group">
                        <label class="form-label">Câu hỏi ${questionCount + 1}</label>
                        <input class="form-input" name="Questions[${questionCount}].NoiDung" rows="3" 
                            placeholder="Nhập nội dung câu hỏi" required></input>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Đáp án</label>
                        <div>
                            <input type="radio" name="Questions[${questionCount}].DapAn" value="true" required> Đúng
                            <input type="radio" name="Questions[${questionCount}].DapAn" value="false" class="ms-3"> Sai
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Liên kết tài liệu</label>
                        <input type="url" class="form-input" name="Questions[${questionCount}].linkTaiLieu" placeholder="Nhập URL tài liệu" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Giải trình</label>
                        <input class="form-input" name="Questions[${questionCount}].ndGiaiTrinh" rows="3" 
                            placeholder="Nhập giải trình"></input>
                    </div>
                </div>`;
                $("#questionContainer").append(questionHtml);
            });

            // Xóa câu hỏi
            $(document).on("click", ".btn-remove", function () {
                if (confirm("Bạn có chắc muốn xóa câu hỏi này?")) {
                    $(this).closest(".question-block").remove();
                    // Cập nhật lại số thứ tự câu hỏi
                    $(".question-block").each(function (index) {
                        $(this).find(".form-label").first().text(`Câu hỏi ${index + 1}`);
                        $(this).find("textarea, input[type='radio'], input[type='url'], input[type='hidden']").each(function () {
                            var oldName = $(this).attr("name");
                            if (oldName) {
                                var newName = oldName.replace(/Questions\[\d+\]/, `Questions[${index}]`);
                                $(this).attr("name", newName);
                            }
                        });
                        // Cập nhật hidden inputs
                        $(this).find("input[type='hidden']").each(function () {
                            var oldId = $(this).attr("id");
                            if (oldId) {
                                var newId = oldId.replace(/Questions_\d+/, `Questions_${index}`);
                                $(this).attr("id", newId);
                                var newName = oldId.includes("NoiDung") ?
                                    `Questions[${index}].NoiDung` : `Questions[${index}].ndGiaiTrinh`;
                                $(this).attr("name", newName);
                            }
                        });
                    });
                }
            });

            // Hàm cập nhật giá trị hidden input từ asp:TextBox
            window.updateHiddenInput = function (textbox, hiddenId) {
                $("#" + hiddenId).val($(textbox).val());
            };

            // Debug trước khi submit form
            $("form").on("submit", function () {
                console.log("Danh sách input trong form:");
                $("#questionContainer .question-block").each(function (index) {
                    console.log(`Câu hỏi ${index + 1}:`);
                    $(this).find("textarea, input").each(function () {
                        console.log(`Name: ${$(this).attr("name")}, Value: ${$(this).val()}`);
                    });
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h4>Chỉnh sửa bộ câu hỏi</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="mb-3" Visible="false" />

        <!-- Thông tin bộ câu hỏi -->
        <div class="mb-4">
            <div class="form-group">
                <label for="txtTenBoCauHoi" class="form-label">Tên bộ câu hỏi</label>
                <asp:TextBox ID="txtTenBoCauHoi" runat="server" CssClass="form-input lg" Placeholder="Nhập tên bộ câu hỏi" />
                <asp:RequiredFieldValidator ID="rfvTenBoCauHoi" runat="server" ControlToValidate="txtTenBoCauHoi"
                    ErrorMessage="Tên bộ câu hỏi là bắt buộc" CssClass="error-message" Display="Dynamic" />
            </div>
        </div>

        <!-- Danh sách câu hỏi -->
        <asp:Repeater ID="rptQuestions" runat="server">
            <ItemTemplate>
                <div class="question-block">
                    <button type="button" class="btn btn-danger btn-sm btn-remove">Xóa</button>
                    <input type="hidden" name="Questions[<%# Container.ItemIndex %>].Id" value='<%# Eval("Id") %>' />
                    <div class="form-group">
                        <label class="form-label">Câu hỏi <%# Container.ItemIndex + 1 %></label>
                        <input type="hidden" name="Questions[<%# Container.ItemIndex %>].NoiDung" id='Questions_<%# Container.ItemIndex %>_NoiDung' value='<%# Eval("NoiDung") %>' />
                        <asp:TextBox runat="server" ID="txtNoiDung" CssClass="form-input" Rows="3"
                            Text='<%# Eval("NoiDung") %>' onblur='<%# $"updateHiddenInput(this, \"Questions_{Container.ItemIndex}_NoiDung\")" %>' required />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Đáp án</label>
                        <div>
                            <input type="radio" name="Questions[<%# Container.ItemIndex %>].DapAn" value="true" 
                                <%# Eval("DapAn").ToString() == "True" ? "checked" : "" %> required> Đúng
                            <input type="radio" name="Questions[<%# Container.ItemIndex %>].DapAn" value="false" class="ms-3" 
                                <%# Eval("DapAn").ToString() == "False" ? "checked" : "" %>> Sai
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Liên kết tài liệu</label>
                        <input type="url" class="form-input" name="Questions[<%# Container.ItemIndex %>].linkTaiLieu" 
                            value='<%# Eval("linkTaiLieu") %>' placeholder="Nhập URL tài liệu" />
                    </div>
                    <div class="form-group">
                        <label class="form-label">Giải trình</label>
                        <input type="hidden" name="Questions[<%# Container.ItemIndex %>].ndGiaiTrinh" id='Questions_<%# Container.ItemIndex %>_ndGiaiTrinh' value='<%# Eval("ndGiaiTrinh") %>' />
                        <asp:TextBox runat="server" ID="txtNdGiaiTrinh" CssClass="form-input" Rows="3"
                            Text='<%# Eval("ndGiaiTrinh") %>' onblur='<%# $"updateHiddenInput(this, \"Questions_{Container.ItemIndex}_ndGiaiTrinh\")" %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div id="questionContainer"></div>

        <!-- Nút thêm câu hỏi -->
        <button type="button" id="btnAddQuestion" class="btn btn-primary">Thêm câu hỏi mới</button>

        <!-- Nút lưu -->
        <asp:Button ID="btnSave" runat="server" Text="Lưu bộ câu hỏi" CssClass="btn btn-success" OnClick="btnSave_Click" />
    </div>
</asp:Content>