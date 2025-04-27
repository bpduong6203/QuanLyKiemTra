using System.Data.Entity;

namespace QuanLyKiemTra.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("name=QLKeHoach")
        {
        }
        public DbSet<BienBanKiemTra> BienBanKiemTras { get; set; }
        public DbSet<BoCauHoi> BoCauHois { get; set; }
        public DbSet<BoCauHoi_KeHoach> BoCauHoi_KeHoachs { get; set; }
        public DbSet<CauHoi> CauHois { get; set; }
        public DbSet<CTBoCauHoi> CTBoCauHois { get; set; }
        public DbSet<CTNoiDung_GiaiTrinh> CTNoiDung_GiaiTrinhs { get; set; }
        public DbSet<CTTaiLieu_KeHoach> CTTaiLieu_KeHoachs { get; set; }
        public DbSet<DapAn> DapAns { get; set; }
        public DbSet<DonVi> DonVis { get; set; }
        public DbSet<GiaiTrinh> GiaiTrinhs { get; set; }
        public DbSet<KeHoach> KeHoachs { get; set; }
        public DbSet<NDGiaiTrinh> NDGiaiTrinhs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<PhanCong_User> PhanCong_Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<TaiLieu> TaiLieus { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<ThongBao_User> ThongBao_Users { get; set; }
    }
}