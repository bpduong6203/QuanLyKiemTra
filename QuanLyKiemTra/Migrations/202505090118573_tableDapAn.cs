namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableDapAn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DapAns", "BoCauHoiId", c => c.String(maxLength: 128));
            AddColumn("dbo.DapAns", "NgayTraLoi", c => c.DateTime(nullable: false));
            CreateIndex("dbo.DapAns", "BoCauHoiId");
            AddForeignKey("dbo.DapAns", "BoCauHoiId", "dbo.BoCauHois", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DapAns", "BoCauHoiId", "dbo.BoCauHois");
            DropIndex("dbo.DapAns", new[] { "BoCauHoiId" });
            DropColumn("dbo.DapAns", "NgayTraLoi");
            DropColumn("dbo.DapAns", "BoCauHoiId");
        }
    }
}
