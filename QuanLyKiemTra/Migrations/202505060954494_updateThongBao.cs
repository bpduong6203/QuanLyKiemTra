namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateThongBao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThongBao_User", "redirectUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThongBao_User", "redirectUrl");
        }
    }
}
