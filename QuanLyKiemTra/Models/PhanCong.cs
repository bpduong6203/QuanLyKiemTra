namespace QuanLyKiemTra.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PhanCong
    {
        [Key]
        public string Id { get; set; }
        public string NoiDung { get; set; }
        public string linkfile { get; set; }
    }
}