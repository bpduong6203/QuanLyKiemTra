using System;
using System.ComponentModel.DataAnnotations;

public class BienBanKiemTra
{
    [Key]
    [Required]
    public string Id { get; set; }
    public string tenBBKT { get; set; }
    public string linkfile { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
}