namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableKetQuaKiemTra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KetQuaKiemTras",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        BoCauHoiId = c.String(maxLength: 128),
                        KeHoachId = c.String(maxLength: 128),
                        TrangThai = c.String(),
                        ThoiGianBatDau = c.DateTime(),
                        ThoiGianHoanThanh = c.DateTime(),
                        ThoiGianLam = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoCauHois", t => t.BoCauHoiId)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachId)
                .ForeignKey("dbo.NguoiDungs", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BoCauHoiId)
                .Index(t => t.KeHoachId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KetQuaKiemTras", "UserId", "dbo.NguoiDungs");
            DropForeignKey("dbo.KetQuaKiemTras", "KeHoachId", "dbo.KeHoaches");
            DropForeignKey("dbo.KetQuaKiemTras", "BoCauHoiId", "dbo.BoCauHois");
            DropIndex("dbo.KetQuaKiemTras", new[] { "KeHoachId" });
            DropIndex("dbo.KetQuaKiemTras", new[] { "BoCauHoiId" });
            DropIndex("dbo.KetQuaKiemTras", new[] { "UserId" });
            DropTable("dbo.KetQuaKiemTras");
        }
    }
}
