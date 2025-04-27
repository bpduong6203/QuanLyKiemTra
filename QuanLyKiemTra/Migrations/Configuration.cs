namespace QuanLyKiemTra.Migrations
{
    using QuanLyKiemTra.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate(
                    r => r.Id,
                    new Roles { Id = Guid.NewGuid().ToString(), Ten = "TruongDoan", MoTa = "" },
                    new Roles { Id = Guid.NewGuid().ToString(), Ten = "ThanhVien", MoTa = "" },
                    new Roles { Id = Guid.NewGuid().ToString(), Ten = "DonVi", MoTa = "" }
                );
            }
        }
    }
}
