<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pageCauHoi.aspx.cs" Inherits="QuanLyKiemTra.pageCauHoi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            // Thêm câu hỏi mới
            $("#btnAddQuestion").click(function () {
                var questionCount = $(".question-block").length + 1;
                var questionHtml = `
                    <div class="question-block">
                        <button type="button" class="btn btn-danger btn-sm btn-remove">Xóa</button>
                        <div class="form-group">
                            <label class="form-label">Câu hỏi ${questionCount}</label>
                            <input class="form-input" name="Questions[${questionCount - 1}].NoiDung" rows="3" 
                                placeholder="Nhập nội dung câu hỏi" required></textarea>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Đáp án</label>
                            <div>
                                <input type="radio" name="Questions[${questionCount - 1}].DapAn" value="true" required> Đúng
                                <input type="radio" name="Questions[${questionCount - 1}].DapAn" value="false" class="ms-3"> Sai
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="form-label">Liên kết tài liệu</label>
                            <input type="url" class="form-input" name="Questions[${questionCount - 1}].linkTaiLieu" placeholder="Nhập URL tài liệu" />
                        </div>
                        <div class="form-group">
                            <label class="form-label">Giải trình</label>
                            <input class="form-input" name="Questions[${questionCount - 1}].ndGiaiTrinh" rows="3" 
                                placeholder="Nhập giải trình"></textarea>
                        </div>
                    </div>`;
                $("#questionContainer").append(questionHtml);
            });

            // Xóa câu hỏi
            $(document).on("click", ".btn-remove", function () {
                $(this).closest(".question-block").remove();
                // Cập nhật lại số thứ tự câu hỏi
                $(".question-block").each(function (index) {
                    $(this).find(".form-label").first().text(`Câu hỏi ${index + 1}`);
                    $(this).find("textarea, input[type='radio'], input[type='url']").each(function () {
                        var oldName = $(this).attr("name");
                        if (oldName) {
                            var newName = oldName.replace(/Questions\[\d+\]/, `Questions[${index}]`);
                            $(this).attr("name", newName);
                        }
                    });
                    // Cập nhật hidden inputs cho câu hỏi mặc định (nếu có)
                    $(this).find("input[type='hidden']").each(function () {
                        var oldId = $(this).attr("id");
                        if (oldId) {
                            var newId = oldId.replace(/Questions_\d+/, `Questions_${index}`);
                            $(this).attr("id", newId);
                            $(this).attr("name", oldId.includes("NoiDung") ?
                                `Questions[${index}].NoiDung` : `Questions[${index}].ndGiaiTrinh`);
                        }
                    });
                });
            });

            // Hàm cập nhật giá trị hidden input từ asp:TextBox
            window.updateHiddenInput = function (textbox, hiddenId) {
                $("#" + hiddenId).val($(textbox).val());
            };
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="container">
        <h4>Tạo bộ câu hỏi mới</h4>
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
        <div id="questionContainer">
            <!-- Câu hỏi đầu tiên (mặc định) -->
            <div class="question-block">
                <button type="button" class="btn btn-danger btn-sm btn-remove">Xóa</button>
                <div class="form-group">
                    <label class="form-label">Câu hỏi 1</label>
                    <input type="hidden" name="Questions[0].NoiDung" id="Questions_0_NoiDung" />
                    <asp:TextBox runat="server" ID="txtNoiDung_0" CssClass="form-input" Rows="3" 
                        placeholder="Nhập nội dung câu hỏi" onblur="updateHiddenInput(this, 'Questions_0_NoiDung')" required />
                </div>
                <div class="form-group">
                    <label class="form-label">Đáp án</label>
                    <div>
                        <input type="radio" name="Questions[0].DapAn" value="true" required> Đúng
                        <input type="radio" name="Questions[0].DapAn" value="false" class="ms-3"> Sai
                    </div>
                </div>
                <div class="form-group">
                    <label class="form-label">Liên kết tài liệu</label>
                    <input type="url" class="form-input" name="Questions[0].linkTaiLieu" placeholder="Nhập URL tài liệu" />
                </div>
                <div class="form-group">
                    <label class="form-label">Giải trình</label>
                    <input type="hidden" name="Questions[0].ndGiaiTrinh" id="Questions_0_ndGiaiTrinh" />
                    <asp:TextBox runat="server" ID="txtNdGiaiTrinh_0" CssClass="form-input" Rows="3" 
                        placeholder="Nhập giải trình" onblur="updateHiddenInput(this, 'Questions_0_ndGiaiTrinh')" />
                </div>
            </div>
        </div>

        <!-- Nút thêm câu hỏi -->
        <button type="button" id="btnAddQuestion" class="btn btn-primary">Thêm câu hỏi mới</button>

        <!-- Nút lưu -->
        <asp:Button ID="btnSave" runat="server" Text="Lưu bộ câu hỏi" CssClass="btn btn-success" OnClick="btnSave_Click" />
    </div>
</asp:Content>