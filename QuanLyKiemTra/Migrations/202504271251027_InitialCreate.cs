namespace QuanLyKiemTra.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BienBanKiemTras",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        tenBBKT = c.String(),
                        linkfile = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BoCauHoi_KeHoach",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BoCauHoiId = c.String(maxLength: 128),
                        KeHoachId = c.String(maxLength: 128),
                        SoLuong = c.Int(nullable: false),
                        ThoiGianLam = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoCauHois", t => t.BoCauHoiId)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachId)
                .Index(t => t.BoCauHoiId)
                .Index(t => t.KeHoachId);
            
            CreateTable(
                "dbo.BoCauHois",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenBoCauHoi = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CTBoCauHois",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BoCauHoiId = c.String(maxLength: 128),
                        CauHoiId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoCauHois", t => t.BoCauHoiId)
                .ForeignKey("dbo.CauHois", t => t.CauHoiId)
                .Index(t => t.BoCauHoiId)
                .Index(t => t.CauHoiId);
            
            CreateTable(
                "dbo.CauHois",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NoiDung = c.String(),
                        DapAn = c.Boolean(nullable: false),
                        linkTaiLieu = c.String(),
                        ndGiaiTrinh = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DapAns",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CauHoiId = c.String(maxLength: 128),
                        NoiDung = c.String(),
                        DapAnTraLoi = c.Boolean(nullable: false),
                        CauTraLoiPhu = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CauHois", t => t.CauHoiId)
                .ForeignKey("dbo.NguoiDungs", t => t.UserId)
                .Index(t => t.CauHoiId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.NguoiDungs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        username = c.String(nullable: false),
                        password = c.String(nullable: false),
                        HoTen = c.String(),
                        Email = c.String(),
                        SoDienThoai = c.String(),
                        DiaChi = c.String(),
                        RoleID = c.String(maxLength: 128),
                        DonViID = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DonVis", t => t.DonViID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .Index(t => t.RoleID)
                .Index(t => t.DonViID);
            
            CreateTable(
                "dbo.DonVis",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenDonVi = c.String(),
                        DiaChi = c.String(),
                        SoDienThoai = c.String(),
                        Email = c.String(),
                        NguoiDaiDien = c.String(),
                        ChucVuNguoiDaiDien = c.String(),
                        NguoiTao = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GiaiTrinhs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        KeHoachID = c.String(maxLength: 128),
                        NguoiYeuCauID = c.String(maxLength: 128),
                        NguoiGiaiTrinhID = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiDung_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachID)
                .ForeignKey("dbo.NguoiDungs", t => t.NguoiGiaiTrinhID)
                .ForeignKey("dbo.NguoiDungs", t => t.NguoiYeuCauID)
                .ForeignKey("dbo.NguoiDungs", t => t.NguoiDung_Id)
                .Index(t => t.KeHoachID)
                .Index(t => t.NguoiYeuCauID)
                .Index(t => t.NguoiGiaiTrinhID)
                .Index(t => t.NguoiDung_Id);
            
            CreateTable(
                "dbo.CTNoiDung_GiaiTrinh",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NDGiaiTrinhID = c.String(maxLength: 128),
                        GiaiTrinhID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GiaiTrinhs", t => t.GiaiTrinhID)
                .ForeignKey("dbo.NDGiaiTrinhs", t => t.NDGiaiTrinhID)
                .Index(t => t.NDGiaiTrinhID)
                .Index(t => t.GiaiTrinhID);
            
            CreateTable(
                "dbo.NDGiaiTrinhs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NoiDung = c.String(),
                        linkfile = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                        DaXem = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KeHoaches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenKeHoach = c.String(),
                        UserId = c.String(maxLength: 128),
                        DonViID = c.String(maxLength: 128),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                        GhiChu = c.String(),
                        BienBanID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BienBanKiemTras", t => t.BienBanID)
                .ForeignKey("dbo.DonVis", t => t.DonViID)
                .ForeignKey("dbo.NguoiDungs", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DonViID)
                .Index(t => t.BienBanID);
            
            CreateTable(
                "dbo.CTTaiLieu_KeHoach",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        KeHoachId = c.String(maxLength: 128),
                        TaiLieuId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachId)
                .ForeignKey("dbo.TaiLieux", t => t.TaiLieuId)
                .Index(t => t.KeHoachId)
                .Index(t => t.TaiLieuId);
            
            CreateTable(
                "dbo.TaiLieux",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TenTaiLieu = c.String(),
                        linkfile = c.String(),
                        NgayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhanCong_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        KeHoachID = c.String(maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        linkfile = c.String(),
                        NoiDungCV = c.String(),
                        ngayTao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachID)
                .ForeignKey("dbo.NguoiDungs", t => t.UserID)
                .Index(t => t.KeHoachID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ThongBao_User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        KeHoachID = c.String(maxLength: 128),
                        NgayTao = c.DateTime(nullable: false),
                        DaXem = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KeHoaches", t => t.KeHoachID)
                .ForeignKey("dbo.NguoiDungs", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.KeHoachID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ten = c.String(),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ThongBaos",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TieuDe = c.String(),
                        NoiDung = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoCauHoi_KeHoach", "KeHoachId", "dbo.KeHoaches");
            DropForeignKey("dbo.BoCauHoi_KeHoach", "BoCauHoiId", "dbo.BoCauHois");
            DropForeignKey("dbo.CTBoCauHois", "CauHoiId", "dbo.CauHois");
            DropForeignKey("dbo.DapAns", "UserId", "dbo.NguoiDungs");
            DropForeignKey("dbo.NguoiDungs", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.GiaiTrinhs", "NguoiDung_Id", "dbo.NguoiDungs");
            DropForeignKey("dbo.GiaiTrinhs", "NguoiYeuCauID", "dbo.NguoiDungs");
            DropForeignKey("dbo.GiaiTrinhs", "NguoiGiaiTrinhID", "dbo.NguoiDungs");
            DropForeignKey("dbo.GiaiTrinhs", "KeHoachID", "dbo.KeHoaches");
            DropForeignKey("dbo.ThongBao_User", "UserID", "dbo.NguoiDungs");
            DropForeignKey("dbo.ThongBao_User", "KeHoachID", "dbo.KeHoaches");
            DropForeignKey("dbo.PhanCong_User", "UserID", "dbo.NguoiDungs");
            DropForeignKey("dbo.PhanCong_User", "KeHoachID", "dbo.KeHoaches");
            DropForeignKey("dbo.KeHoaches", "UserId", "dbo.NguoiDungs");
            DropForeignKey("dbo.KeHoaches", "DonViID", "dbo.DonVis");
            DropForeignKey("dbo.CTTaiLieu_KeHoach", "TaiLieuId", "dbo.TaiLieux");
            DropForeignKey("dbo.CTTaiLieu_KeHoach", "KeHoachId", "dbo.KeHoaches");
            DropForeignKey("dbo.KeHoaches", "BienBanID", "dbo.BienBanKiemTras");
            DropForeignKey("dbo.CTNoiDung_GiaiTrinh", "NDGiaiTrinhID", "dbo.NDGiaiTrinhs");
            DropForeignKey("dbo.CTNoiDung_GiaiTrinh", "GiaiTrinhID", "dbo.GiaiTrinhs");
            DropForeignKey("dbo.NguoiDungs", "DonViID", "dbo.DonVis");
            DropForeignKey("dbo.DapAns", "CauHoiId", "dbo.CauHois");
            DropForeignKey("dbo.CTBoCauHois", "BoCauHoiId", "dbo.BoCauHois");
            DropIndex("dbo.ThongBao_User", new[] { "KeHoachID" });
            DropIndex("dbo.ThongBao_User", new[] { "UserID" });
            DropIndex("dbo.PhanCong_User", new[] { "UserID" });
            DropIndex("dbo.PhanCong_User", new[] { "KeHoachID" });
            DropIndex("dbo.CTTaiLieu_KeHoach", new[] { "TaiLieuId" });
            DropIndex("dbo.CTTaiLieu_KeHoach", new[] { "KeHoachId" });
            DropIndex("dbo.KeHoaches", new[] { "BienBanID" });
            DropIndex("dbo.KeHoaches", new[] { "DonViID" });
            DropIndex("dbo.KeHoaches", new[] { "UserId" });
            DropIndex("dbo.CTNoiDung_GiaiTrinh", new[] { "GiaiTrinhID" });
            DropIndex("dbo.CTNoiDung_GiaiTrinh", new[] { "NDGiaiTrinhID" });
            DropIndex("dbo.GiaiTrinhs", new[] { "NguoiDung_Id" });
            DropIndex("dbo.GiaiTrinhs", new[] { "NguoiGiaiTrinhID" });
            DropIndex("dbo.GiaiTrinhs", new[] { "NguoiYeuCauID" });
            DropIndex("dbo.GiaiTrinhs", new[] { "KeHoachID" });
            DropIndex("dbo.NguoiDungs", new[] { "DonViID" });
            DropIndex("dbo.NguoiDungs", new[] { "RoleID" });
            DropIndex("dbo.DapAns", new[] { "UserId" });
            DropIndex("dbo.DapAns", new[] { "CauHoiId" });
            DropIndex("dbo.CTBoCauHois", new[] { "CauHoiId" });
            DropIndex("dbo.CTBoCauHois", new[] { "BoCauHoiId" });
            DropIndex("dbo.BoCauHoi_KeHoach", new[] { "KeHoachId" });
            DropIndex("dbo.BoCauHoi_KeHoach", new[] { "BoCauHoiId" });
            DropTable("dbo.ThongBaos");
            DropTable("dbo.Roles");
            DropTable("dbo.ThongBao_User");
            DropTable("dbo.PhanCong_User");
            DropTable("dbo.TaiLieux");
            DropTable("dbo.CTTaiLieu_KeHoach");
            DropTable("dbo.KeHoaches");
            DropTable("dbo.NDGiaiTrinhs");
            DropTable("dbo.CTNoiDung_GiaiTrinh");
            DropTable("dbo.GiaiTrinhs");
            DropTable("dbo.DonVis");
            DropTable("dbo.NguoiDungs");
            DropTable("dbo.DapAns");
            DropTable("dbo.CauHois");
            DropTable("dbo.CTBoCauHois");
            DropTable("dbo.BoCauHois");
            DropTable("dbo.BoCauHoi_KeHoach");
            DropTable("dbo.BienBanKiemTras");
        }
    }
}
