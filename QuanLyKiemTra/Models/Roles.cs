namespace QuanLyKiemTra.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Roles
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
    }
}