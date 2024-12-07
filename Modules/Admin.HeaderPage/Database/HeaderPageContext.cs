using Microsoft.EntityFrameworkCore;
using Admin.HeaderPage.Models;
using static Admin.HeaderPage.Constants.MenuConstants;

namespace Admin.HeaderPage.Database
{
    public class HeaderPageContext : DbContext
    {
        public HeaderPageContext(DbContextOptions<HeaderPageContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.MenuConfig> MenuConfigs { get; set; }
        public virtual DbSet<Database.Menu> Menus { get; set; }
        public virtual DbSet<Database.Menu_Files> Menu_Files { get; set; }

        public virtual DbSet<Database.HeaderPage> HeaderPages { get; set; }
        public virtual DbSet<Database.HeaderPageConfig> HeaderPageConfigs { get; set; }

        public virtual DbSet<Database.MenuStyle> MenuStyles { get; set; }
        public virtual DbSet<Database.MenuStyleConfig> MenuStyleConfigs { get; set; }

        public virtual DbSet<Database.MenuItemStyle> MenuItemStyles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HeaderPageContext).Assembly);
            SeedMenuConfig(modelBuilder);
            SeedHeaderConfig(modelBuilder);

        }
        protected void SeedMenuConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MenuConfig>().HasData(
                 new MenuConfig
                 {
                     Pid = 1,
                     Key = Config.Admin.PageSize,
                     Value = "100",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new MenuConfig
                    {
                        Pid = 2,
                        Key = Config.Website.AlwaysShowTop,
                        Value = "1",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }

             ); ;


        }
        protected void SeedHeaderConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HeaderPageConfig>().HasData(
                 new HeaderPageConfig
                 {
                     Pid = 1,
                     Key = Admin.HeaderPage.Constants.HeaderPageConstants.Config.Admin.PageSize,
                     Value = "100",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 }
                 //,
                 //     new HeaderPageConfig
                 //     {
                 //         Pid = 2,
                 //         Key = Admin.HeaderPage.Constants.HeaderPageConstants.Config.Website.ApiUpdateHeader,
                 //         Value = "",
                 //         Group = "",
                 //         CreateUser = "admin",
                 //         CreateDate = DateTime.Now,
                 //         LastLogin = DateTime.Now,
                 //         UpdateUser = "admin",
                 //         UpdateDate = DateTime.Now,
                 //         Type = "Admin"
                 //     }
                 //     ,
                 //     new HeaderPageConfig
                 //     {
                 //         Pid = 2,
                 //         Key = Admin.HeaderPage.Constants.HeaderPageConstants.Config.Website.ApiRevertHeader,
                 //         Value = "",
                 //         Group = "",
                 //         CreateUser = "admin",
                 //         CreateDate = DateTime.Now,
                 //         LastLogin = DateTime.Now,
                 //         UpdateUser = "admin",
                 //         UpdateDate = DateTime.Now,
                 //         Type = "Admin"
                 //     }
             ); ;


        }
        protected void SeedMenuStyleConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MenuStyleConfig>().HasData(
                 new MenuStyleConfig
                 {
                     Pid = 1,
                     Key = Admin.HeaderPage.Constants.MenuStyleConstants.Config.Admin.PageSize,
                     Value = "100",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 }

             ); ;


        }

    }
}