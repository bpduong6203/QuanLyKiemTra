namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NDGiaiTrinh_GiaiTrinh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GiaiTrinhs", "TrangThaiTongThe", c => c.String());
            AddColumn("dbo.NDGiaiTrinhs", "TrangThai", c => c.String());
            AddColumn("dbo.NDGiaiTrinhs", "NguoiDanhGiaID", c => c.String(maxLength: 128));
            AddColumn("dbo.NDGiaiTrinhs", "NgayDanhGia", c => c.DateTime());
            CreateIndex("dbo.NDGiaiTrinhs", "NguoiDanhGiaID");
            AddForeignKey("dbo.NDGiaiTrinhs", "NguoiDanhGiaID", "dbo.NguoiDungs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NDGiaiTrinhs", "NguoiDanhGiaID", "dbo.NguoiDungs");
            DropIndex("dbo.NDGiaiTrinhs", new[] { "NguoiDanhGiaID" });
            DropColumn("dbo.NDGiaiTrinhs", "NgayDanhGia");
            DropColumn("dbo.NDGiaiTrinhs", "NguoiDanhGiaID");
            DropColumn("dbo.NDGiaiTrinhs", "TrangThai");
            DropColumn("dbo.GiaiTrinhs", "TrangThaiTongThe");
        }
    }
}
