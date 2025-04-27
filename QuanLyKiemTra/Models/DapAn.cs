using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DapAn
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("CauHoi")]
    public string CauHoiId { get; set; }
    public string NoiDung { get; set; }
    public bool DapAnTraLoi { get; set; }
    public string CauTraLoiPhu { get; set; }
    [ForeignKey("NguoiDung")]
    public string UserId { get; set; }
    virtual public CauHoi CauHoi { get; set; }
    virtual public NguoiDung NguoiDung { get; set; }
}