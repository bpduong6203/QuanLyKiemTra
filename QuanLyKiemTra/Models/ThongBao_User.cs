namespace QuanLyKiemTra.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ThongBao_User
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [ForeignKey("NguoiDung")]
        public string UserID { get; set; }
        [ForeignKey("KeHoach")]
        public string KeHoachID { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool DaXem { get; set; }
        virtual public NguoiDung NguoiDung { get; set; }
        virtual public KeHoach KeHoach { get; set; }
    }
}