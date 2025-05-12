namespace QuanLyKiemTra.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class KetQuaKiemTra
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [ForeignKey("NguoiDung")]
        public string UserId { get; set; }
        [ForeignKey("BoCauHoi")]
        public string BoCauHoiId { get; set; }
        [ForeignKey("KeHoach")]
        public string KeHoachId { get; set; }
        public string TrangThai { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianHoanThanh { get; set; }
        public int? ThoiGianLam { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual BoCauHoi BoCauHoi { get; set; }
        public virtual KeHoach KeHoach { get; set; }
    }
}