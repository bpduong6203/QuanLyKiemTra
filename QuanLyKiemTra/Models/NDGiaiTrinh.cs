namespace QuanLyKiemTra.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class NDGiaiTrinh
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string FileName { get; set; }
        public string NoiDung { get; set; }
        public string linkfile { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool DaXem { get; set; }
        public string TrangThai { get; set; } = "Chờ Đánh Giá";
        [ForeignKey("NguoiDanhGia")]
        public string NguoiDanhGiaID { get; set; }
        public DateTime? NgayDanhGia { get; set; }

        public virtual NguoiDung NguoiDanhGia { get; set; }
        public virtual ICollection<CTNoiDung_GiaiTrinh> CTNoiDung_GiaiTrinhs { get; set; }
    }
}