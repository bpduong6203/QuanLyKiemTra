using QuanLyKiemTra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra
{
    public partial class pageEditBoCauHoi : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Chỉnh sửa bộ câu hỏi";
            if (!IsPostBack)
            {
                if (Session["Username"] == null)
                {
                    Response.Redirect("dang-nhap");
                }

                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                if (string.IsNullOrEmpty(boCauHoiId))
                {
                    lblMessage.Text = "Không tìm thấy bộ câu hỏi.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                LoadBoCauHoi(boCauHoiId);
            }
        }

        private void LoadBoCauHoi(string boCauHoiId)
        {
            try
            {
                var boCauHoi = db.BoCauHois
                    .Where(b => b.Id == boCauHoiId)
                    .Select(b => new { b.TenBoCauHoi })
                    .FirstOrDefault();

                if (boCauHoi == null)
                {
                    lblMessage.Text = "Bộ câu hỏi không tồn tại.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                txtTenBoCauHoi.Text = boCauHoi.TenBoCauHoi;

                var questions = db.CTBoCauHois
                    .Where(ct => ct.BoCauHoiId == boCauHoiId)
                    .Join(db.CauHois,
                        ct => ct.CauHoiId,
                        c => c.Id,
                        (ct, c) => new
                        {
                            c.Id,
                            c.NoiDung,
                            c.DapAn,
                            c.linkTaiLieu,
                            c.ndGiaiTrinh
                        })
                    .ToList();

                rptQuestions.DataSource = questions;
                rptQuestions.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Lỗi khi tải bộ câu hỏi: " + ex.Message;
                lblMessage.CssClass = "error-message";
                lblMessage.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    lblMessage.Text = "Vui lòng điền đầy đủ các trường bắt buộc.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                if (string.IsNullOrEmpty(boCauHoiId))
                {
                    lblMessage.Text = "Không tìm thấy bộ câu hỏi.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                var boCauHoi = db.BoCauHois.Find(boCauHoiId);
                if (boCauHoi == null)
                {
                    lblMessage.Text = "Bộ câu hỏi không tồn tại.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;
                    return;
                }

                boCauHoi.TenBoCauHoi = txtTenBoCauHoi.Text.Trim();
                boCauHoi.NgayTao = DateTime.Now;

                // Lấy danh sách câu hỏi hiện có trong CTBoCauHoi
                var existingCTBoCauHois = db.CTBoCauHois
                    .Where(ct => ct.BoCauHoiId == boCauHoiId)
                    .ToList();
                var existingCauHoiIds = existingCTBoCauHois.Select(ct => ct.CauHoiId).ToList();

                // Danh sách câu hỏi từ form
                var formQuestions = new List<(string Id, string NoiDung, bool DapAn, string linkTaiLieu, string ndGiaiTrinh)>();
                var processedIndexes = new HashSet<string>(); // Theo dõi các chỉ số đã xử lý
                int maxIndex = -1;

                // Xác định chỉ số tối đa của Questions[X]
                foreach (var key in Request.Form.AllKeys)
                {
                    if (key.StartsWith("Questions[") && key.Contains("].NoiDung"))
                    {
                        var index = key.Split('[')[1].Split(']')[0];
                        if (int.TryParse(index, out int idx))
                        {
                            maxIndex = Math.Max(maxIndex, idx);
                        }
                    }
                }

                // Duyệt qua các chỉ số từ 0 đến maxIndex
                for (int i = 0; i <= maxIndex; i++)
                {
                    var index = i.ToString();
                    if (processedIndexes.Contains(index)) continue; // Bỏ qua nếu đã xử lý

                    var idKey = $"Questions[{index}].Id";
                    var noiDungKey = $"Questions[{index}].NoiDung";
                    var dapAnKey = $"Questions[{index}].DapAn";
                    var linkTaiLieuKey = $"Questions[{index}].linkTaiLieu";
                    var ndGiaiTrinhKey = $"Questions[{index}].ndGiaiTrinh";

                    var noiDung = Request.Form[noiDungKey];
                    if (!string.IsNullOrEmpty(noiDung))
                    {
                        var cauHoiId = Request.Form[idKey];
                        var dapAn = Request.Form[dapAnKey];
                        formQuestions.Add((
                            cauHoiId,
                            noiDung,
                            bool.TryParse(dapAn, out bool result) ? result : false,
                            Request.Form[linkTaiLieuKey],
                            Request.Form[ndGiaiTrinhKey]
                        ));
                        processedIndexes.Add(index); // Đánh dấu chỉ số đã xử lý
                    }
                }

                // Tiếp tục xử lý lưu câu hỏi như trước
                var newCTBoCauHois = new List<CTBoCauHoi>();
                foreach (var formQuestion in formQuestions)
                {
                    var cauHoiId = formQuestion.Id;
                    if (string.IsNullOrEmpty(cauHoiId)) // Câu hỏi mới
                    {
                        cauHoiId = Guid.NewGuid().ToString();
                        var newCauHoi = new CauHoi
                        {
                            Id = cauHoiId,
                            NoiDung = formQuestion.NoiDung,
                            DapAn = formQuestion.DapAn,
                            linkTaiLieu = formQuestion.linkTaiLieu,
                            ndGiaiTrinh = formQuestion.ndGiaiTrinh,
                            NgayTao = DateTime.Now
                        };
                        db.CauHois.Add(newCauHoi);

                        // Thêm liên kết CTBoCauHoi mới
                        newCTBoCauHois.Add(new CTBoCauHoi
                        {
                            Id = Guid.NewGuid().ToString(),
                            BoCauHoiId = boCauHoiId,
                            CauHoiId = cauHoiId
                        });
                    }
                    else // Câu hỏi hiện có
                    {
                        var existingCauHoi = db.CauHois.Find(cauHoiId);
                        if (existingCauHoi != null)
                        {
                            existingCauHoi.NoiDung = formQuestion.NoiDung;
                            existingCauHoi.DapAn = formQuestion.DapAn;
                            existingCauHoi.linkTaiLieu = formQuestion.linkTaiLieu;
                            existingCauHoi.ndGiaiTrinh = formQuestion.ndGiaiTrinh;
                            existingCauHoi.NgayTao = DateTime.Now;
                        }
                    }
                }

                // Xóa các liên kết CTBoCauHoi cho các câu hỏi bị xóa trong form
                var formCauHoiIds = formQuestions.Select(q => q.Id).Where(id => !string.IsNullOrEmpty(id)).ToList();
                var ctToRemove = existingCTBoCauHois
                    .Where(ct => !formCauHoiIds.Contains(ct.CauHoiId))
                    .ToList();
                db.CTBoCauHois.RemoveRange(ctToRemove);

                // Xóa CauHoi không còn liên kết với bất kỳ BoCauHoi nào
                var cauHoiToRemove = existingCauHoiIds
                    .Where(id => !formCauHoiIds.Contains(id))
                    .Where(id => !db.CTBoCauHois.Any(ct => ct.CauHoiId == id))
                    .ToList();
                var cauHoiEntitiesToRemove = db.CauHois
                    .Where(c => cauHoiToRemove.Contains(c.Id))
                    .ToList();
                db.CauHois.RemoveRange(cauHoiEntitiesToRemove);

                // Thêm liên kết CTBoCauHoi mới
                db.CTBoCauHois.AddRange(newCTBoCauHois);

                db.SaveChanges();

                lblMessage.Text = "Lưu bộ câu hỏi thành công!";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;

                Response.Redirect($"~/chinh-sua-bo-cau-hoi/{boCauHoiId}");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Lỗi khi lưu bộ câu hỏi: " + ex.Message;
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