using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CauHoi
{
    [Key]
    [Required]
    public string Id { get; set; }
    public string NoiDung { get; set; }
    public bool DapAn { get; set; }
    public string linkTaiLieu { get; set; }
    public string ndGiaiTrinh { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public virtual ICollection<CTBoCauHoi> CTBoCauHois { get; set; }
    public virtual ICollection<DapAn> DapAns { get; set; }
}