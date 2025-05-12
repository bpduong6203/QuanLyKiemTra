using QuanLyKiemTra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace QuanLyKiemTra.viewCauHoi
{
    public partial class pageDanhGiaKiemTra : System.Web.UI.Page
    {
        private MyDbContext db = new MyDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra đăng nhập
                if (Session["Username"] == null)
                {
                    Response.Redirect("~/dang-nhap");
                }

                // Lấy BoCauHoiId từ route
                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                if (string.IsNullOrEmpty(boCauHoiId))
                {
                    lblMessage.Text = "Không tìm thấy bộ câu hỏi!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra quyền
                string role = GetRole();
                if (role != "ThanhVien" && role != "DonVi")
                {
                    lblMessage.Text = "Bạn không có quyền làm bài kiểm tra!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Kiểm tra trạng thái bài kiểm tra
                if (!KiemTraTrangThai(boCauHoiId))
                {
                    return;
                }

                // Tải câu hỏi
                LoadCauHoi(boCauHoiId);
            }
        }

        private bool KiemTraTrangThai(string boCauHoiId)
        {
            try
            {
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                if (nguoiDung == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return false;
                }

                var boCauHoiKeHoach = db.BoCauHoi_KeHoachs
                    .FirstOrDefault(b => b.BoCauHoiId == boCauHoiId);
                if (boCauHoiKeHoach == null)
                {
                    lblMessage.Text = "Bộ câu hỏi không thuộc kế hoạch nào!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return false;
                }

                var phanCong = db.PhanCong_Users
                    .Any(p => p.KeHoachID == boCauHoiKeHoach.KeHoachId && p.UserID == nguoiDung.Id);
                if (!phanCong)
                {
                    lblMessage.Text = "Bạn không được phân công làm bài kiểm tra này!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return false;
                }

                var ketQua = db.KetQuaKiemTras
                    .FirstOrDefault(k => k.UserId == nguoiDung.Id && k.BoCauHoiId == boCauHoiId);
                if (ketQua == null)
                {
                    ketQua = new KetQuaKiemTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = nguoiDung.Id,
                        BoCauHoiId = boCauHoiId,
                        KeHoachId = boCauHoiKeHoach.KeHoachId,
                        TrangThai = "ChuaBatDau",
                        ThoiGianBatDau = DateTime.Now,
                        ThoiGianLam = boCauHoiKeHoach.ThoiGianLam
                    };
                    db.KetQuaKiemTras.Add(ketQua);
                    db.SaveChanges();
                }

                if (ketQua.TrangThai == "DaHoanThanh")
                {
                    lblMessage.Text = "Bạn đã hoàn thành bài kiểm tra này!";
                    lblMessage.CssClass = "message-label warning-message";
                    lblMessage.Visible = true;
                    return false;
                }

                if (ketQua.TrangThai == "ChuaBatDau" || ketQua.TrangThai == "CanLamLai")
                {
                    ketQua.TrangThai = "DangLam";
                    ketQua.ThoiGianBatDau = DateTime.Now;
                    db.SaveChanges();
                }

                ViewState["ThoiGianLam"] = ketQua.ThoiGianLam;
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi kiểm tra trạng thái: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
                return false;
            }
        }

        private void LoadCauHoi(string boCauHoiId)
        {
            try
            {
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                if (nguoiDung == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Lấy danh sách câu hỏi
                var cauHoiList = db.CTBoCauHois
                    .Include("CauHoi")
                    .Where(ct => ct.BoCauHoiId == boCauHoiId)
                    .Select(ct => new
                    {
                        ct.CauHoiId,
                        ct.BoCauHoiId,
                        CauHoi = ct.CauHoi
                    })
                    .ToList()
                    .Select((ct, index) => new
                    {
                        ct.CauHoiId,
                        ct.BoCauHoiId,
                        CauHoi = ct.CauHoi,
                        STT = index + 1
                    })
                    .ToList();

                // Lấy đáp án đã lưu (nếu có)
                var dapAnList = db.DapAns
                    .Where(d => d.UserId == nguoiDung.Id && d.BoCauHoiId == boCauHoiId)
                    .ToDictionary(d => d.CauHoiId, d => d);

                ViewState["DapAnList"] = dapAnList;
                rptCauHoi.DataSource = cauHoiList;
                rptCauHoi.DataBind();
                pnlKiemTra.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi tải câu hỏi: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void rptCauHoi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = e.Item.DataItem as dynamic;
                string cauHoiId = item.CauHoiId;
                var rbDapAnTrue = e.Item.FindControl("rbDapAnTrue") as RadioButton;
                var rbDapAnFalse = e.Item.FindControl("rbDapAnFalse") as RadioButton;
                var txtCauTraLoiPhu = e.Item.FindControl("txtCauTraLoiPhu") as TextBox;

                var dapAnList = ViewState["DapAnList"] as Dictionary<string, DapAn>;
                if (dapAnList != null && dapAnList.ContainsKey(cauHoiId))
                {
                    var dapAn = dapAnList[cauHoiId];
                    if (dapAn.DapAnTraLoi)
                        rbDapAnTrue.Checked = true;
                    else
                        rbDapAnFalse.Checked = true;
                    txtCauTraLoiPhu.Text = dapAn.CauTraLoiPhu;
                }
            }
        }

        protected void btnLuuLai_Click(object sender, EventArgs e)
        {
            try
            {
                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                if (nguoiDung == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Lưu đáp án
                foreach (RepeaterItem item in rptCauHoi.Items)
                {
                    var hfCauHoiId = item.FindControl("hfCauHoiId") as HiddenField;
                    var rbDapAnTrue = item.FindControl("rbDapAnTrue") as RadioButton;
                    var rbDapAnFalse = item.FindControl("rbDapAnFalse") as RadioButton;
                    var txtCauTraLoiPhu = item.FindControl("txtCauTraLoiPhu") as TextBox;

                    string cauHoiId = hfCauHoiId.Value;
                    bool? dapAnTraLoi = null;
                    if (rbDapAnTrue.Checked)
                        dapAnTraLoi = true;
                    else if (rbDapAnFalse.Checked)
                        dapAnTraLoi = false;

                    if (dapAnTraLoi.HasValue)
                    {
                        var dapAn = db.DapAns
                            .FirstOrDefault(d => d.UserId == nguoiDung.Id && d.CauHoiId == cauHoiId && d.BoCauHoiId == boCauHoiId);
                        if (dapAn == null)
                        {
                            dapAn = new DapAn
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserId = nguoiDung.Id,
                                CauHoiId = cauHoiId,
                                BoCauHoiId = boCauHoiId,
                                NoiDung = txtCauTraLoiPhu.Text,
                                DapAnTraLoi = dapAnTraLoi.Value,
                                CauTraLoiPhu = txtCauTraLoiPhu.Text,
                                NgayTraLoi = DateTime.Now
                            };
                            db.DapAns.Add(dapAn);
                        }
                        else
                        {
                            dapAn.NoiDung = txtCauTraLoiPhu.Text;
                            dapAn.DapAnTraLoi = dapAnTraLoi.Value;
                            dapAn.CauTraLoiPhu = txtCauTraLoiPhu.Text;
                            dapAn.NgayTraLoi = DateTime.Now;
                        }
                    }
                }

                // Cập nhật trạng thái
                var ketQua = db.KetQuaKiemTras
                    .FirstOrDefault(k => k.UserId == nguoiDung.Id && k.BoCauHoiId == boCauHoiId);
                if (ketQua != null)
                {
                    ketQua.TrangThai = "DangLam";
                    db.SaveChanges();
                }

                lblMessage.Text = "Đã lưu đáp án thành công!";
                lblMessage.CssClass = "message-label success-message";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi lưu đáp án: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected void btnHoanThanh_Click(object sender, EventArgs e)
        {
            try
            {
                string boCauHoiId = RouteData.Values["Id"]?.ToString();
                string username = Session["Username"]?.ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(u => u.username == username);
                if (nguoiDung == null)
                {
                    lblMessage.Text = "Không tìm thấy thông tin người dùng!";
                    lblMessage.CssClass = "message-label error-message";
                    lblMessage.Visible = true;
                    return;
                }

                // Lưu đáp án
                btnLuuLai_Click(sender, e);

                // Cập nhật trạng thái hoàn thành
                var ketQua = db.KetQuaKiemTras
                    .FirstOrDefault(k => k.UserId == nguoiDung.Id && k.BoCauHoiId == boCauHoiId);
                if (ketQua != null)
                {
                    ketQua.TrangThai = "DaHoanThanh";
                    ketQua.ThoiGianHoanThanh = DateTime.Now;
                    db.SaveChanges();
                }

                // Chuyển hướng đến trang kết quả
                Response.Redirect($"~/ket-qua-kiem-tra/{boCauHoiId}");
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Lỗi khi nộp bài: {ex.Message}";
                lblMessage.CssClass = "message-label error-message";
                lblMessage.Visible = true;
            }
        }

        protected string GetRole()
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username)) return "";

            var nguoiDung = db.NguoiDungs
                .Include("Roles")
                .FirstOrDefault(u => u.username == username);

            return nguoiDung?.Roles?.Ten ?? "DonVi";
        }
    }
}