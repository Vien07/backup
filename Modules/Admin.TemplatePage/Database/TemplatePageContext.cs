using Microsoft.EntityFrameworkCore;
using Admin.TemplatePage.Models;
using Admin.TemplatePage.Constants;
namespace Admin.TemplatePage.Database
{
    public class TemplatePageContext : DbContext
    {
        public TemplatePageContext(DbContextOptions<TemplatePageContext> options) : base(options)
        {

        }


        public virtual DbSet<TemplatePageConfig> TemplatePageConfigs { get; set; }
        public virtual DbSet<TemplatePage> TemplatePages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemplatePage>(entity =>
            entity.HasIndex(e => e.Url).IsUnique()
                     );
  
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplatePageContext).Assembly);
            SeedTemplatePageConfig(modelBuilder);

        }
        protected void SeedTemplatePageConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<TemplatePageConfig>().HasData(
                 new TemplatePageConfig
                 {
                     Pid = ++pid,
                     Key = TemplatePageConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Admin.MaxHeight,
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
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Admin.ThumbHeight,
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
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Admin.ThumbWidth,
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
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Admin.PageSize,
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
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Website.PageSize,
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
                    new TemplatePageConfig
                    {
                        Pid = ++pid,
                        Key = TemplatePageConstants.Config.Website.PreSlug,
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