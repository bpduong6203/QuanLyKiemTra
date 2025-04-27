namespace QuanLyKiemTra.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class KeHoach
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string TenKeHoach { get; set; }

        [ForeignKey("NguoiDung")]
        public string UserId { get; set; }
        [ForeignKey("DonVi")]
        public string DonViID { get; set; }

        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string GhiChu { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
        public virtual DonVi DonVi { get; set; }

        [ForeignKey("BienBanKiemTra")]
        public string BienBanID { get; set; }
        public virtual BienBanKiemTra BienBanKiemTra { get; set; }

        public virtual ICollection<CTTaiLieu_KeHoach> CTTaiLieu_KeHoach { get; set; }
        public virtual ICollection<BoCauHoi_KeHoach> BoCauHoi_KeHoach { get; set; }
        public virtual ICollection<ThongBao_User> ThongBao_Users { get; set; }
        public virtual ICollection<PhanCong_User> PhanCong_Users { get; set; }
        public virtual ICollection<GiaiTrinh> GiaiTrinhs { get; set; }
    }
}