using Microsoft.EntityFrameworkCore;
using Admin.WebsiteKeys.Models;
using Admin.WebsiteKeys.Constants;
namespace Admin.WebsiteKeys.Database
{
    public class WebsiteKeysContext : DbContext
    {
        public WebsiteKeysContext(DbContextOptions<WebsiteKeysContext> options) : base(options)
        {

        }


        public virtual DbSet<WebsiteKeysConfig> WebsiteKeysConfigs { get; set; }
        public virtual DbSet<WebsiteKeys> WebsiteKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebsiteKeys>(entity =>
            entity.HasIndex(e => e.Key).IsUnique()
                     );
            modelBuilder.Entity<WebsiteKeysConfig>(entity =>
                entity.HasIndex(e => e.Key).IsUnique()
                 );
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebsiteKeysContext).Assembly);
            SeedWebsiteKeysConfig(modelBuilder);
            SeedWebsiteKeySystem(modelBuilder);

        }
        protected void SeedWebsiteKeysConfig(ModelBuilder modelBuilder)
        {
            int id = 0;
            modelBuilder.Entity<WebsiteKeysConfig>().HasData(
                 new WebsiteKeysConfig
                 {
                     Pid = ++id,
                     Key = WebsiteKeysConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Admin.MaxHeight,
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
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Admin.ThumbHeight,
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
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Admin.ThumbWidth,
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
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Admin.PageSize,
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
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Website.PageSize,
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
                    new WebsiteKeysConfig
                    {
                        Pid = ++id,
                        Key = WebsiteKeysConstants.Config.Website.PreSlug,
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
        protected void SeedWebsiteKeySystem(ModelBuilder modelBuilder)
        {
            int id = 0;

            modelBuilder.Entity<WebsiteKeys>().HasData(
                 new WebsiteKeys
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
                  new WebsiteKeys
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
                 new WebsiteKeys
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
                new WebsiteKeys
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
                   new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
                    new WebsiteKeys
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
            modelBuilder.Entity<WebsiteKeys>().HasData(
             new WebsiteKeys
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
             }, new WebsiteKeys
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
             }, new WebsiteKeys
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