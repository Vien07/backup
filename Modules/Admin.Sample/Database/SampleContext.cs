using Microsoft.EntityFrameworkCore;
using Admin.Sample.Models;
using Admin.Sample.Constants;
namespace Admin.Sample.Database
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.SampleConfig> SampleConfigs { get; set; }
        public virtual DbSet<Database.SampleDetail> Samples { get; set; }
        public virtual DbSet<Database.Sample_Files> Sample_Files { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SampleContext).Assembly);
            SeedSampleConfig(modelBuilder);

        }
        protected void SeedSampleConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SampleConfig>().HasData(
                 new SampleConfig
                 {
                     Pid = 1,
                     Key = SampleConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new SampleConfig
                    {
                        Pid = 2,
                        Key = SampleConstants.Config.Admin.MaxHeight,
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
                    new SampleConfig
                    {
                        Pid = 3,
                        Key = SampleConstants.Config.Admin.ThumbHeight,
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
                    new SampleConfig
                    {
                        Pid = 4,
                        Key = SampleConstants.Config.Admin.ThumbWidth,
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
                    new SampleConfig
                    {
                        Pid = 5,
                        Key = SampleConstants.Config.Admin.PageSize,
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
                    new SampleConfig
                    {
                        Pid = 6,
                        Key = SampleConstants.Config.Website.PageSize,
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
                    new SampleConfig
                    {
                        Pid = 7,
                        Key = SampleConstants.Config.Website.PreSlug,
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