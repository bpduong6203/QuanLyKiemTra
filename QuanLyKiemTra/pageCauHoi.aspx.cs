using QuanLyKiemTra.Models;
using System;
using System.Collections.Generic;

namespace QuanLyKiemTra
{
    public partial class pageCauHoi : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        // ViewModel để nhận dữ liệu từ form
        public class QuestionViewModel
        {
            public string NoiDung { get; set; }
            public bool DapAn { get; set; }
            public string linkTaiLieu { get; set; }
            public string ndGiaiTrinh { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Thêm Câu Hỏi";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra hợp lệ
                if (!Page.IsValid)
                {
                    lblMessage.Text = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Tạo bộ câu hỏi mới
                var boCauHoi = new BoCauHoi
                {
                    Id = Guid.NewGuid().ToString(),
                    TenBoCauHoi = txtTenBoCauHoi.Text,
                    NgayTao = DateTime.Now
                };

                // Lấy danh sách câu hỏi từ form
                var questions = new List<QuestionViewModel>();
                for (int i = 0; Request.Form[$"Questions[{i}].NoiDung"] != null; i++)
                {
                    questions.Add(new QuestionViewModel
                    {
                        NoiDung = Request.Form[$"Questions[{i}].NoiDung"],
                        DapAn = bool.Parse(Request.Form[$"Questions[{i}].DapAn"]),
                        linkTaiLieu = Request.Form[$"Questions[{i}].linkTaiLieu"],
                        ndGiaiTrinh = Request.Form[$"Questions[{i}].ndGiaiTrinh"]
                    });
                }

                // Lưu câu hỏi và liên kết
                foreach (var q in questions)
                {
                    var cauHoi = new CauHoi
                    {
                        Id = Guid.NewGuid().ToString(),
                        NoiDung = q.NoiDung,
                        DapAn = q.DapAn,
                        linkTaiLieu = q.linkTaiLieu,
                        ndGiaiTrinh = q.ndGiaiTrinh,
                        NgayTao = DateTime.Now
                    };

                    var ctBoCauHoi = new CTBoCauHoi
                    {
                        Id = Guid.NewGuid().ToString(),
                        BoCauHoiId = boCauHoi.Id,
                        CauHoiId = cauHoi.Id
                    };

                    db.CauHois.Add(cauHoi);
                    db.CTBoCauHois.Add(ctBoCauHoi);
                }

                db.BoCauHois.Add(boCauHoi);
                db.SaveChanges();

                lblMessage.Text = "Lưu bộ câu hỏi thành công!";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;

                // Xóa form sau khi lưu
                txtTenBoCauHoi.Text = "";
                // Có thể thêm logic để reset danh sách câu hỏi
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Lỗi: " + ex.Message;
                lblMessage.CssClass = "error-message";
                lblMessage.Visible = true;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            db.Dispose();
        }
    }
}