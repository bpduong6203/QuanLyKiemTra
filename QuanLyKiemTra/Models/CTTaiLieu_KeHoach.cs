using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CTTaiLieu_KeHoach
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("KeHoach")]
    public string KeHoachId { get; set; }
    [ForeignKey("TaiLieu")]
    public string TaiLieuId { get; set; }
    public virtual TaiLieu TaiLieu { get; set; }
    public virtual KeHoach KeHoach { get; set; }
}