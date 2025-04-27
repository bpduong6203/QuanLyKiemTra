using System;
using System.ComponentModel.DataAnnotations;

public class BienBanHopDoan
{
    [Key]
    public string Id { get; set; }
    public string TenHopDoan { get; set; }
    public DateTime NgayHop { get; set; }
    public string DiaDiem { get; set; }
    public string NoiDung { get; set; }
    public string ChuKy { get; set; }
}