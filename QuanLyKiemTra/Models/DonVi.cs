using System;
using System.ComponentModel.DataAnnotations;
public class DonVi
{
    [Key]
    [Required]
    public string Id { get; set; }
    public string TenDonVi { get; set; }
    public string DiaChi { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string NguoiDaiDien { get; set; }
    public string ChucVuNguoiDaiDien { get; set; }
    public string NguoiTao { get; set; }
    public DateTime NgayTao { get; set; }
}