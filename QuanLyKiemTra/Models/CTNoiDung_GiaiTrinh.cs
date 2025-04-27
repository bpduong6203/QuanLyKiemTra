using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CTNoiDung_GiaiTrinh
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("NDGiaiTrinh")]
    public string NDGiaiTrinhID { get; set; }
    [ForeignKey("GiaiTrinh")]
    public string GiaiTrinhID { get; set; }

    public virtual NDGiaiTrinh NDGiaiTrinh { get; set; }
    public virtual GiaiTrinh GiaiTrinh { get; set; }
}