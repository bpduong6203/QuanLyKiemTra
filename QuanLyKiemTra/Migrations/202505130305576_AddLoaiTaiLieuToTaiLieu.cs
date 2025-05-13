namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoaiTaiLieuToTaiLieu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaiLieux", "LoaiTaiLieu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaiLieux", "LoaiTaiLieu");
        }
    }
}
