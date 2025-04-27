using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CTBoCauHoi
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("BoCauHoi")]
    public string BoCauHoiId { get; set; }
    [ForeignKey("CauHoi")]
    public string CauHoiId { get; set; }
    public virtual BoCauHoi BoCauHoi { get; set; }
    public virtual CauHoi CauHoi { get; set; }
}
