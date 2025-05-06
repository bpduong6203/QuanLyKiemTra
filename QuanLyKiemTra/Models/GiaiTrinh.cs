namespace QuanLyKiemTra.Models
{
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
        public string TrangThaiTongThe { get; set; } = "Chờ Giải Trình";

        public virtual NguoiDung NguoiYeuCau { get; set; }
        public virtual NguoiDung NguoiGiaiTrinh { get; set; }
        public virtual KeHoach KeHoach { get; set; }
        public virtual ICollection<CTNoiDung_GiaiTrinh> CTNoiDung_GiaiTrinhs { get; set; }
        public virtual ICollection<GiaiTrinhFile> GiaiTrinhFiles { get; set; }
    }

    public class GiaiTrinhFile
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [ForeignKey("GiaiTrinh")]
        public string GiaiTrinhID { get; set; }
        [Required]
        public string LinkFile { get; set; }
        public string FileName { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public virtual GiaiTrinh GiaiTrinh { get; set; }
    }

}