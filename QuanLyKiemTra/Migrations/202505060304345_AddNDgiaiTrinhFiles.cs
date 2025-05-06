namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNDgiaiTrinhFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NDGiaiTrinhs", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NDGiaiTrinhs", "FileName");
        }
    }
}
