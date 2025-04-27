using System.ComponentModel.DataAnnotations;

public class TienDo
{
    [Key]
    public string Id { get; set; }
    public int DaXong { get; set; }
    public int ChuaXong { get; set; }
}