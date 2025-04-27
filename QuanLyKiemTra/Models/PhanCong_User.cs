using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PhanCong_User
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("KeHoach")]
    public string KeHoachID { get; set; }
    [ForeignKey("NguoiDung")]
    public string UserID { get; set; }
    public string linkfile { get; set; }
    public string NoiDungCV { get; set; }
    public DateTime ngayTao { get; set; } = DateTime.Now;

    public virtual NguoiDung NguoiDung { get; set; }
    public virtual KeHoach KeHoach { get; set; }
}