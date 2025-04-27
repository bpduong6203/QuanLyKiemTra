using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class BoCauHoi
{
    [Key]
    [Required]
    public string Id { get; set; }
    public string TenBoCauHoi { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public virtual ICollection<BoCauHoi_KeHoach> BoCauHoi_KeHoach { get; set; }
    public virtual ICollection<CTBoCauHoi> CTBoCauHois { get; set; }
}