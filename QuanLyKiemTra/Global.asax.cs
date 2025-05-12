using System;
using System.Web;
using System.Web.Routing;
using System.Web.UI;

namespace QuanLyKiemTra
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Đăng ký jQuery cho ScriptManager
            if (ScriptManager.ScriptResourceMapping.GetDefinition("jquery") == null)
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
                {
                    Path = "~/Scripts/jquery-3.7.1.min.js",
                    DebugPath = "~/Scripts/jquery-3.7.1.js",
                    CdnPath = "https://code.jquery.com/jquery-3.7.1.min.js",
                    CdnDebugPath = "https://code.jquery.com/jquery-3.7.1.js"
                });
            }
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.AbsolutePath == "/") Response.Redirect("~/dang-nhap");
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            // Nhóm các định tuyến đơn giản
            var simpleRoutes = new[]
            {
                new { Name = "TaoKeHoach", Url = "tao-ke-hoach", File = "~/pageKeHoach.aspx" },
                new { Name = "PhanCong", Url = "phan-cong", File = "~/pagePhanCong.aspx" },
                new { Name = "KiemTra", Url = "danh-sach-ke-hoach", File = "~/pageKiemTra.aspx" },
                new { Name = "GiaiTrinh", Url = "danh-sach-giai-trinh", File = "~/pageGiaiTrinh.aspx" },
                new { Name = "KetLuan", Url = "ket-luan-kiem-tra", File = "~/pageKetLuan.aspx" },
                new { Name = "NguoiDung", Url = "danh-sach-nguoi-dung", File = "~/pageNguoiDung.aspx" },
                new { Name = "CauHoi", Url = "danh-sach-cau-hoi", File = "~/pageCauHoi.aspx" },
                new { Name = "BoCauHoi", Url = "danh-sach-bo-cau-hoi", File = "~/pageBoCauHoi.aspx" },
                new { Name = "DonVi", Url = "danh-sach-don-vi", File = "~/pageDonVi.aspx" },
                new { Name = "DangNhap", Url = "dang-nhap", File = "~/pageLogin.aspx" },
                new { Name = "DangKy", Url = "dang-ky", File = "~/pageRegister.aspx" },
                new { Name = "CaNhan", Url = "ca-nhan", File = "~/pageCaNhan.aspx" },
                new { Name = "CapNhatCaNhan", Url = "cai-dat-tai-khoan", File = "~/pageCapNhatCaNhan.aspx" },
                new { Name = "DangXuat", Url = "dang-xuat", File = "~/pageLogout.aspx" },
                new { Name = "ThongBao", Url = "thong-bao", File = "~/pageThongBao.aspx" }
            };

            foreach (var route in simpleRoutes)
            {
                routes.MapPageRoute(route.Name, route.Url, route.File);
            }

            // Định tuyến có tham số Id và ràng buộc GUID
            var guidConstraint = new RouteValueDictionary
            {
                { "Id", @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}" }
            };

            routes.MapPageRoute(
                "ChiTietKeHoach",
                "chi-tiet-ke-hoach/{Id}",
                "~/pageCTKeHoach.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "YeuCauGiaiTrinh",
                "chi-tiet-giai-trinh/{Id}",
                "~/pageCTYeuCauGiaiTrinh.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "ChinhSuaBoCauHoi",
                "chinh-sua-bo-cau-hoi/{Id}",
                "~/pageEditBoCauHoi.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "DanhGiaKiemTra",
                "kiem-tra-ke-hoach/{Id}",
                "~/viewCauHoi/pageDanhGiaKiemTra.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "DanhSachDApAn",
                "ket-qua-kiem-tra/{Id}",
                "~/viewCauHoi/pageKetQuaKiemTra.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "DanhSachKQ",
                "danh-sach-ket-qua/{Id}",
                "~/viewCauHoi/pageDanhSachKetQua.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );

            routes.MapPageRoute(
                "CTKetQuaKiemTra",
                "chi-tiet-ket-qua-kiem-tra/{Id}",
                "~/viewCauHoi/pageKetQuaKiemTraChiTiet.aspx",
                true,
                new RouteValueDictionary(),
                guidConstraint
            );
        }
    }
}