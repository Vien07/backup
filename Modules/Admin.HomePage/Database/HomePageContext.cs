using Microsoft.EntityFrameworkCore;
using Admin.HomePage.Models;
using Admin.HomePage.Constants;
namespace Admin.HomePage.Database
{
    public class HomePageContext : DbContext
    {
        public HomePageContext(DbContextOptions<HomePageContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.HomePageConfig> HomePageConfigs { get; set; }
        public virtual DbSet<Database.HomePage> HomePages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomePageContext).Assembly);
            SeedHomePageConfig(modelBuilder);

        }
        protected void SeedHomePageConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HomePageConfig>().HasData(
                 new HomePageConfig
                 {
                     Pid = 1,
                     Key = HomePageConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new HomePageConfig
                    {
                        Pid = 2,
                        Key = HomePageConstants.Config.Admin.MaxHeight,
                        Value = "350",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                    ,
                    new HomePageConfig
                    {
                        Pid = 3,
                        Key = HomePageConstants.Config.Admin.ThumbHeight,
                        Value = "285",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                      ,
                    new HomePageConfig
                    {
                        Pid = 4,
                        Key = HomePageConstants.Config.Admin.ThumbWidth,
                        Value = "456",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                      ,
                    new HomePageConfig
                    {
                        Pid = 5,
                        Key = HomePageConstants.Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                          ,
                    new HomePageConfig
                    {
                        Pid = 6,
                        Key = HomePageConstants.Config.Website.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }
                             ,
                    new HomePageConfig
                    {
                        Pid = 7,
                        Key = HomePageConstants.Config.Website.PreSlug,
                        Value = "/pre-slug/",
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


    }
}