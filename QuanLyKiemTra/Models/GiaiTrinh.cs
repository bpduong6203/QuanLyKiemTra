using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GiaiTrinh
{
    [Key]
    [Required]
    public string Id { get; set; }
    [ForeignKey("KeHoach")]
    public string KeHoachID { get; set; }
    [ForeignKey("NguoiYeuCau")]
    public string NguoiYeuCauID { get; set; }
    [ForeignKey("NguoiGiaiTrinh")]
    public string NguoiGiaiTrinhID { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public virtual NguoiDung NguoiYeuCau { get; set; }
    public virtual NguoiDung NguoiGiaiTrinh { get; set; }
    public virtual KeHoach KeHoach { get; set; }

    public virtual ICollection<CTNoiDung_GiaiTrinh> CTNoiDung_GiaiTrinhs { get; set; }
}