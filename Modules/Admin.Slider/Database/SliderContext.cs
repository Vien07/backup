using Microsoft.EntityFrameworkCore;
using Admin.Slider.Models;
using Admin.Slider.Constants;
namespace Admin.Slider.Database
{
    public class SliderContext : DbContext
    {
        public SliderContext(DbContextOptions<SliderContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.SliderConfig> SliderConfigs { get; set; }
        public virtual DbSet<Database.Slider> Sliders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SliderContext).Assembly);
            SeedSliderConfig(modelBuilder);

        }
        protected void SeedSliderConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SliderConfig>().HasData(
                 new SliderConfig
                 {
                     Pid = 1,
                     Key = SliderConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new SliderConfig
                    {
                        Pid = 2,
                        Key = SliderConstants.Config.Admin.MaxHeight,
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
                    new SliderConfig
                    {
                        Pid = 3,
                        Key = SliderConstants.Config.Admin.ThumbHeight,
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
                    new SliderConfig
                    {
                        Pid = 4,
                        Key = SliderConstants.Config.Admin.ThumbWidth,
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
                    new SliderConfig
                    {
                        Pid = 5,
                        Key = SliderConstants.Config.Admin.PageSize,
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
                    new SliderConfig
                    {
                        Pid = 6,
                        Key = SliderConstants.Config.Website.PageSize,
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
                    new SliderConfig
                    {
                        Pid = 7,
                        Key = SliderConstants.Config.Website.PreSlug,
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