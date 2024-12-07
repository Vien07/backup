using Microsoft.EntityFrameworkCore;
using Admin.FooterPage.Models;
using static Admin.FooterPage.Constants.FooterPageConstants;

namespace Admin.FooterPage.Database
{
    public class FooterPageContext : DbContext
    {
        public FooterPageContext(DbContextOptions<FooterPageContext> options) : base(options)
        {

        }



        public virtual DbSet<Database.FooterPage> FooterPages { get; set; }
        public virtual DbSet<Database.FooterPageConfig> FooterPageConfigs { get; set; }

        public virtual DbSet<Database.FooterItem> FooterItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FooterPageContext).Assembly);
            SeedHeaderConfig(modelBuilder);

        }
        protected void SeedHeaderConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FooterPageConfig>().HasData(
                 new FooterPageConfig
                 {
                     Pid = 1,
                     Key = Admin.FooterPage.Constants.FooterPageConstants.Config.Admin.PageSize,
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
                 //     new FooterPageConfig
                 //     {
                 //         Pid = 2,
                 //         Key = Admin.FooterPage.Constants.FooterPageConstants.Config.Website.ApiUpdateHeader,
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
                 //     new FooterPageConfig
                 //     {
                 //         Pid = 2,
                 //         Key = Admin.FooterPage.Constants.FooterPageConstants.Config.Website.ApiRevertHeader,
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


    }
}