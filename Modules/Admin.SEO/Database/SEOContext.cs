using Microsoft.EntityFrameworkCore;
using Admin.SEO.Models;
using Admin.SEO.Constants;

namespace Admin.SEO.Database
{
    public class SEOContext : DbContext
    {
        public SEOContext(DbContextOptions<SEOContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.SEOConfig> SEOConfigs { get; set; }
        public virtual DbSet<Database.SEO> SEOs { get; set; }
        public virtual DbSet<Database.SEO_Files> SEO_Files { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SEOContext).Assembly);
            SeedSEOConfig(modelBuilder);

        }
        protected void SeedSEOConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<SEOConfig>().HasData(
                 new SEOConfig
                 {
                     Pid = ++pid,
                     Key = SEOConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Admin.MaxHeight,
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
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Admin.ThumbHeight,
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
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Admin.ThumbWidth,
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
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Admin.PageSize,
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
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Website.PageSize,
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
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Website.PreSlug,
                        Value = "/pre-slug/",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }       ,
                    new SEOConfig
                    {
                        Pid = ++pid,
                        Key = SEOConstants.Config.Admin.Modules,
                        Value = "",
                        DefaultValue = "",
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