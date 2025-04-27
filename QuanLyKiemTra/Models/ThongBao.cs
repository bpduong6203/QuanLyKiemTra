namespace QuanLyKiemTra.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ThongBao
    {
        [Key]
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
    }
}