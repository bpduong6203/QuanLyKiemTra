namespace QuanLyKiemTra.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class NguoiDung
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }

        [ForeignKey("Roles")]
        public string RoleID { get; set; }
        public virtual Roles Roles { get; set; }

        [ForeignKey("DonVi")]
        public string DonViID { get; set; }
        public virtual DonVi DonVi { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public virtual ICollection<DapAn> DapAns { get; set; }
        public virtual ICollection<ThongBao_User> ThongBao_Users { get; set; }
        public virtual ICollection<PhanCong_User> PhanCong_Users { get; set; }
        public virtual ICollection<GiaiTrinh> GiaiTrinhs { get; set; }
    }
}

