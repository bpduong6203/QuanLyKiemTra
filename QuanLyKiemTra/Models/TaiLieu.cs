namespace QuanLyKiemTra.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TaiLieu
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string TenTaiLieu { get; set; }
        public string linkfile { get; set; }
        public DateTime NgayTao { get; set; }
        public string LoaiTaiLieu { get; set; }

        public virtual ICollection<CTTaiLieu_KeHoach> CTTaiLieu_KeHoach { get; set; }
    }
}