using Microsoft.EntityFrameworkCore;
using Steam.Core.LocalizeManagement.Models;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.LocalizeManagement.Database;

namespace Steam.Core.LocalizeManagement.Database
{
    public class LocalizeManagementContext : DbContext
    {
        public LocalizeManagementContext(DbContextOptions<LocalizeManagementContext> options) : base(options)
        {

        }


        public virtual DbSet<LocalizeCulture> LocalizeCulture { get; set; }
        public virtual DbSet<LocalizeCultureConfig> LocalizeCultureConfigs { get; set; }
        public virtual DbSet<LocalizedContentProperty> LocalizedContentProperties { get; set; }
        public virtual DbSet<LocalizeManagementConfig> LocalizeManagementConfigs { get; set; }
        public virtual DbSet<LocalizedContentPropertyConfig> LocalizedContentPropertyConfigs { get; set; }
        public virtual DbSet<LocalizeManagement> LocalizeManagement { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizeCulture>(entity =>
            entity.HasIndex(e => e.LangCode).IsUnique()
                     );
            modelBuilder.Entity<LocalizeCulture>(entity =>
            entity.HasIndex(e => e.SlugKey).IsUnique()
            );
            modelBuilder.Entity<LocalizeManagement>(entity =>
            entity.HasIndex(e => e.Key).IsUnique()
                     );
            modelBuilder.Entity<LocalizeManagementConfig>(entity =>
                entity.HasIndex(e => e.Key).IsUnique()
                 );
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalizeManagementContext).Assembly);
            SeedLocalizeManagementConfig(modelBuilder);
            SeedLocalizeCultureConfig(modelBuilder);
            SeedLocalizeManagementystem(modelBuilder);

        }
        protected void SeedLocalizeManagementConfig(ModelBuilder modelBuilder)
        {
            int id = 0;
            modelBuilder.Entity<LocalizeManagementConfig>().HasData(
                 new LocalizeManagementConfig
                 {
                     Pid = ++id,
                     Key = LocalizeManagementConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Steam.Core"
                 },
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Admin.MaxHeight,
                        Value = "350",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Steam.Core"
                    }
                    ,
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Admin.ThumbHeight,
                        Value = "285",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Steam.Core"
                    }
                      ,
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Admin.ThumbWidth,
                        Value = "456",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Steam.Core"
                    }
                      ,
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Steam.Core"
                    }
                          ,
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Website.PageSize,
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
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Website.PreSlug,
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
        protected void SeedLocalizeCultureConfig(ModelBuilder modelBuilder)
        {
            int id = 0;
            modelBuilder.Entity<LocalizeCultureConfig>().HasData(
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Steam.Core"
                    }
                          ,
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Website.PageSize,
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
                    new LocalizeManagementConfig
                    {
                        Pid = ++id,
                        Key = LocalizeManagementConstants.Config.Website.PreSlug,
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
        protected void SeedLocalizeManagementystem(ModelBuilder modelBuilder)
        {
            int id = 0;

            modelBuilder.Entity<LocalizeManagement>().HasData(
                 new LocalizeManagement
                 {
                     Pid = ++id,
                     Title = "MENU",
                     Key = "{{MENU}}",
                     Description = "",
                     Group = "Header",
                     Value = "",
                     isSystemKey = true,
                     isMedia = false,
                     MediaPath = "",
                     Images = "",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     CreateUser = "admin"
                 },
                  new LocalizeManagement
                  {
                      Pid = ++id,
                      Title = "MENU_ITEM",
                      Key = "{{MENU_ITEM}}",
                      Description = "",
                      Group = "Header",
                      Value = "",
                      isSystemKey = true,
                      isMedia = false,
                      MediaPath = "",
                      Images = "",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      CreateUser = "admin"
                  },
                 new LocalizeManagement
                 {
                     Pid = ++id,
                     Title = "MENU_NAME",
                     Key = "{{MENU_NAME}}",
                     Description = "",
                     Group = "Header",
                     Value = "",
                     isSystemKey = true,
                     isMedia = false,
                     MediaPath = "",
                     Images = "",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     CreateUser = "admin"
                 },
                new LocalizeManagement
                {
                    Pid = ++id,
                    Title = "MENU_URL",
                    Key = "{{MENU_URL}}",
                    Description = "",
                    Group = "Header",
                    Value = "",
                    isSystemKey = true,
                    isMedia = false,
                    MediaPath = "",
                    Images = "",
                    CreateDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    UpdateUser = "admin",
                    UpdateDate = DateTime.Now,
                    CreateUser = "admin"
                },
                   new LocalizeManagement
                   {
                       Pid = ++id,
                       Title = "SECTION_ITEMS",
                       Key = "{{SECTION_ITEMS}}",
                       Description = "",
                       Group = "HomePage",
                       Value = "",
                       isSystemKey = true,
                       isMedia = false,
                       MediaPath = "",
                       Images = "",
                       CreateDate = DateTime.Now,
                       LastLogin = DateTime.Now,
                       UpdateUser = "admin",
                       UpdateDate = DateTime.Now,
                       CreateUser = "admin"
                   },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SECTION_TABS_NAME",
                        Key = "{{SECTION_TABS_NAME}}",
                        Description = "",
                        Group = "HomePage",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SECTION_TABS",
                        Key = "{{SECTION_TABS}}",
                        Description = "",
                        Group = "HomePage",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SECTIONITEM_CHILD",
                        Key = "{{SECTIONITEM_CHILD}}",
                        Description = "",
                        Group = "HomePage",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SLIDERITEM_MEDIA",
                        Key = "{{SLIDERITEM_MEDIA}}",
                        Description = "",
                        Group = "Slider",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SLIDERITEM_TITLE",
                        Key = "{{SLIDERITEM_TITLE}}",
                        Description = "",
                        Group = "Slider",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SLIDERITEM_DESCRIPTION",
                        Key = "{{SLIDERITEM_DESCRIPTION}}",
                        Description = "",
                        Group = "Slider",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "SLIDER_ITEMS",
                        Key = "{{SLIDER_ITEMS}}",
                        Description = "",
                        Group = "Slider",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    },
                    new LocalizeManagement
                    {
                        Pid = ++id,
                        Title = "FOOTER_PLUGIN",
                        Key = "{{FOOTER_PLUGIN}}",
                        Description = "",
                        Group = "Footer",
                        Value = "",
                        isSystemKey = true,
                        isMedia = false,
                        MediaPath = "",
                        Images = "",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        CreateUser = "admin"
                    }
             ); ;

            #region custom
            modelBuilder.Entity<LocalizeManagement>().HasData(
             new LocalizeManagement
             {
                 Pid = ++id,
                 Title = "SECTION_ITEM_NAME",
                 Key = "{{SECTION_ITEM_NAME}}",
                 Description = "",
                 Group = "Product",
                 Value = "",
                 isSystemKey = false,
                 isMedia = false,
                 MediaPath = "",
                 Images = "",
                 CreateDate = DateTime.Now,
                 LastLogin = DateTime.Now,
                 UpdateUser = "admin",
                 UpdateDate = DateTime.Now,
                 CreateUser = "admin"
             }, new LocalizeManagement
             {
                 Pid = ++id,
                 Title = "SECTION_ITEM_IMAGE",
                 Key = "{{SECTION_ITEM_IMAGE}}",
                 Description = "",
                 Group = "Product",
                 Value = "",
                 isSystemKey = false,
                 isMedia = false,
                 MediaPath = "",
                 Images = "",
                 CreateDate = DateTime.Now,
                 LastLogin = DateTime.Now,
                 UpdateUser = "admin",
                 UpdateDate = DateTime.Now,
                 CreateUser = "admin"
             }, new LocalizeManagement
             {
                 Pid = ++id,
                 Title = "SECTION_ITEM_PRICE",
                 Key = "{{SECTION_ITEM_PRICE}}",
                 Description = "",
                 Group = "Product",
                 Value = "",
                 isSystemKey = false,
                 isMedia = false,
                 MediaPath = "",
                 Images = "",
                 CreateDate = DateTime.Now,
                 LastLogin = DateTime.Now,
                 UpdateUser = "admin",
                 UpdateDate = DateTime.Now,
                 CreateUser = "admin"
             }

         ); ;
            #endregion

        }
        protected void SeedWebsiteKeyCustom(ModelBuilder modelBuilder)
        {
            int id = 0;



        }
    }
}