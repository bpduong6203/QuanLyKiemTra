namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableGiaiTrinh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GiaiTrinhs", "linkFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GiaiTrinhs", "linkFile");
        }
    }
}
