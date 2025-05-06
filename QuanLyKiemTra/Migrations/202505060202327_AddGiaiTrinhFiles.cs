namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGiaiTrinhFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GiaiTrinhFiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        GiaiTrinhID = c.String(maxLength: 128),
                        LinkFile = c.String(nullable: false),
                        FileName = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GiaiTrinhs", t => t.GiaiTrinhID)
                .Index(t => t.GiaiTrinhID);
            
            DropColumn("dbo.GiaiTrinhs", "linkFile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GiaiTrinhs", "linkFile", c => c.String());
            DropForeignKey("dbo.GiaiTrinhFiles", "GiaiTrinhID", "dbo.GiaiTrinhs");
            DropIndex("dbo.GiaiTrinhFiles", new[] { "GiaiTrinhID" });
            DropTable("dbo.GiaiTrinhFiles");
        }
    }
}
