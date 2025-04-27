namespace QuanLyKiemTra.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BoCauHoi_KeHoach
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [ForeignKey("BoCauHoi")]
        public string BoCauHoiId { get; set; }
        [ForeignKey("KeHoach")]
        public string KeHoachId { get; set; }
        public int SoLuong { get; set; }
        public int ThoiGianLam { get; set; }
        public virtual BoCauHoi BoCauHoi { get; set; }
        public virtual KeHoach KeHoach { get; set; }
    }
}