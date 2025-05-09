
using QuanLyKiemTra.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DapAn
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("CauHoi")]
    public string CauHoiId { get; set; }
    [ForeignKey("BoCauHoi")]
    public string BoCauHoiId { get; set; }
    public string NoiDung { get; set; }
    public bool DapAnTraLoi { get; set; }
    public string CauTraLoiPhu { get; set; }
    [ForeignKey("NguoiDung")]
    public string UserId { get; set; }
    public DateTime NgayTraLoi { get; set; } = DateTime.Now;
    public virtual CauHoi CauHoi { get; set; }
    public virtual BoCauHoi BoCauHoi { get; set; }
    public virtual NguoiDung NguoiDung { get; set; }
}