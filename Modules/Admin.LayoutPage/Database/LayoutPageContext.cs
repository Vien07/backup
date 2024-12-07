using Microsoft.EntityFrameworkCore;
using Admin.LayoutPage.Models;
using static Admin.LayoutPage.Constants.MenuConstants;
using Admin.LayoutPage.Constants;

namespace Admin.LayoutPage.Database
{
    public class LayoutPageContext : DbContext
    {
        public LayoutPageContext(DbContextOptions<LayoutPageContext> options) : base(options)
        {

        }
        #region header

        public virtual DbSet<Database.HeaderPage> HeaderPages { get; set; }
        public virtual DbSet<Database.HeaderPageConfig> HeaderPageConfigs { get; set; }
        #endregion
        #region menu
        public virtual DbSet<Database.MenuStyle> MenuStyles { get; set; }
        public virtual DbSet<Database.MenuStyleConfig> MenuStyleConfigs { get; set; }
        public virtual DbSet<Database.MenuConfig> MenuConfigs { get; set; }
        public virtual DbSet<Database.Menu> Menus { get; set; }
        public virtual DbSet<Database.MenuItemStyle> MenuItemStyles { get; set; }

        #endregion
        #region homepage
        public virtual DbSet<Database.HomePageConfig> HomePageConfigs { get; set; }
        public virtual DbSet<Database.HomePage> HomePages { get; set; }
        #endregion
        #region footpage
        public virtual DbSet<Database.FooterPage> FooterPages { get; set; }
        public virtual DbSet<Database.FooterPageConfig> FooterPageConfigs { get; set; }

        public virtual DbSet<Database.FooterItem> FooterItems { get; set; }

        #endregion
        #region slider
        public virtual DbSet<Database.Slider> Sliders { get; set; }
        public virtual DbSet<Database.SliderConfig> SliderConfigs { get; set; }

        public virtual DbSet<Database.SliderItem> SliderItems { get; set; }

        #endregion
        #region QuickToolBar
        public virtual DbSet<Database.QuickToolBar> QuickToolBars { get; set; }
        public virtual DbSet<Database.QuickToolBarConfig> QuickToolBarConfigs { get; set; }

        public virtual DbSet<Database.QuickToolBarItem> QuickToolBarItems { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LayoutPageContext).Assembly);
            SeedMenuConfig(modelBuilder);
            SeedHeaderConfig(modelBuilder);
            SeedFooterConfig(modelBuilder);
            SeedHomePageConfig(modelBuilder);
            SeedSliderConfig(modelBuilder);
            SeedQuickToolBarConfig(modelBuilder);

        }
        protected void SeedMenuConfig(ModelBuilder modelBuilder)
        {
            int id = 0;
            modelBuilder.Entity<MenuConfig>().HasData(
                 new MenuConfig
                 {
                     Pid = ++id,
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
                        Pid = ++id,
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
            int id = 0;
            modelBuilder.Entity<HeaderPageConfig>().HasData(
                 new HeaderPageConfig
                 {
                     Pid = ++id,
                     Key = HeaderPageConstants.Config.Admin.PageSize,
                     Value = "100",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 }
                 ,
                      new HeaderPageConfig
                      {
                          Pid = ++id,
                          Key = HeaderPageConstants.Config.Website.ApiUpdateHeader,
                          Value = "{host}/api/ApiMasterPage/UpdateHeader",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }
                      ,
                      new HeaderPageConfig
                      {
                          Pid = ++id,
                          Key = HeaderPageConstants.Config.Website.ApiRevertHeader,
                          Value = "{host}/api/ApiMasterPage/RevertHeader",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }         ,
                      new HeaderPageConfig
                      {
                          Pid = ++id,
                          Key = HeaderPageConstants.Config.Website.AlwaysShowTop,
                          Value = "",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }      ,
                      new HeaderPageConfig
                      {
                          Pid = ++id,
                          Key = HeaderPageConstants.Config.Website.ApiPreviewPage,
                          Value = "{host}/home/preview?previewType=header",
                          DefaultValue = "{host}/home/preview?previewType=header",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }   ,
                      new HeaderPageConfig
                      {
                          Pid = ++id,
                          Key = HeaderPageConstants.Config.Website.ApiUpdatePreviewHeader,
                          Value = "{host}/api/ApiMasterPage/UpdatePreviewHeader",
                          DefaultValue = "{host}/api/ApiMasterPage/UpdatePreviewHeader",
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
        protected void SeedMenuStyleConfig(ModelBuilder modelBuilder)
        {
            int id = 0;

            modelBuilder.Entity<MenuStyleConfig>().HasData(
                 new MenuStyleConfig
                 {
                     Pid = ++id,
                     Key = Admin.LayoutPage.Constants.MenuStyleConstants.Config.Admin.PageSize,
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
        protected void SeedFooterConfig(ModelBuilder modelBuilder)
        {
            int id = 0;
            modelBuilder.Entity<FooterPageConfig>().HasData(
                 new FooterPageConfig
                 {
                     Pid = ++id,
                     Key = FooterPageConstants.Config.Admin.PageSize,
                     Value = "100",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 }
                 ,
                      new FooterPageConfig
                      {
                          Pid = ++id,
                          Key = FooterPageConstants.Config.Website.ApiUpdateFooterPage,
                          Value = "",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }
                      ,
                      new FooterPageConfig
                      {
                          Pid = ++id,
                          Key = FooterPageConstants.Config.Website.ApiRevertFooterPage,
                          Value = "",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }  ,
                      new FooterPageConfig
                      {
                          Pid = ++id,
                          Key = FooterPageConstants.Config.Website.ApiPreviewPage,
                          Value = "{host}/home/preview?previewType=footer",
                          DefaultValue = "{host}/home/preview?previewType=footer",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      },
                      new FooterPageConfig
                      {
                          Pid = ++id,
                          Key = FooterPageConstants.Config.Website.ApiUpdatePreviewFooterPage,
                          Value = "{host}/api/ApiMasterPage/UpdatePreviewFooter",
                          DefaultValue = "{host}/api/ApiMasterPage/UpdatePreviewFooter",
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
        protected void SeedHomePageConfig(ModelBuilder modelBuilder)
        {
            int id = 0;

            modelBuilder.Entity<HomePageConfig>().HasData(
                 new HomePageConfig
                 {
                     Pid = ++id,
                     Key = HomePageConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new HomePageConfig
                    {
                        Pid = ++id,
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
                        Pid = ++id,
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
                        Pid = ++id,
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
                        Pid = ++id,
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
                        Pid = ++id,
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
                          Pid = ++id,
                          Key = HomePageConstants.Config.Website.ApiUpdateHomePage,
                          Value = "{host}/api/ApiMasterPage/UpdateHomePage",
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
                          Pid = ++id,
                          Key = HomePageConstants.Config.Website.ApiRevertHomePage,
                          Value = "{host}/api/ApiMasterPage/RevertHomePage",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      },
                      new HomePageConfig
                      {
                          Pid = ++id,
                          Key = HomePageConstants.Config.Website.ApiPreviewPage,
                          Value = "{host}/home/preview?previewType=layout",
                          DefaultValue = "{host}/home/preview?previewType=layout",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      },
                      new HomePageConfig
                      {
                          Pid = ++id,
                          Key = HomePageConstants.Config.Website.ApiUpdatePreviewHomePage,
                          Value = "{host}/api/ApiMasterPage/UpdatePreviewHomePage",
                          DefaultValue = "{host}/api/ApiMasterPage/UpdatePreviewHomePage",
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
        protected void SeedSliderConfig(ModelBuilder modelBuilder)
        {
            int id = 0;


            modelBuilder.Entity<SliderConfig>().HasData(
                 new SliderConfig
                 {
                     Pid = ++id,
                     Key = SliderConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new SliderConfig
                    {
                        Pid = ++id,
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
                        Pid = ++id,
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
                        Pid = ++id,
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
                        Pid = ++id,
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
                          Pid = ++id,
                          Key = SliderConstants.Config.Website.ApiUpdateSlider,
                          Value = "{host}/api/ApiMasterPage/UpdateSlider",
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
                          Pid = ++id,
                          Key = SliderConstants.Config.Website.ApiRevertSlider,
                          Value = "{host}/api/ApiMasterPage/RevertSlider",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }      ,
                      new SliderConfig
                      {
                          Pid = ++id,
                          Key = SliderConstants.Config.Website.ApiPreviewPage,
                          Value = "{host}/home/preview?previewType=slider",
                          DefaultValue = "{host}/home/preview?previewType=slider",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      },
                      new SliderConfig
                      {
                          Pid = ++id,
                          Key = SliderConstants.Config.Website.ApiUpdatePreviewSlider,
                          Value = "{host}/api/ApiMasterPage/UpdatePreviewSlider",
                          DefaultValue = "{host}/api/ApiMasterPage/UpdatePreviewSlider",
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
        protected void SeedQuickToolBarConfig(ModelBuilder modelBuilder)
        {
            int id = 0;


            modelBuilder.Entity<QuickToolBarConfig>().HasData(
                    new QuickToolBarConfig
                    {
                        Pid = ++id,
                        Key = QuickToolBarConstants.Config.Admin.PageSize,
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

                      new QuickToolBarConfig
                      {
                          Pid = ++id,
                          Key = QuickToolBarConstants.Config.Website.ApiUpdateQuickToolBar,
                          Value = "{host}/api/ApiMasterPage/UpdateQuickToolBar",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }
                      ,
                      new QuickToolBarConfig
                      {
                          Pid = ++id,
                          Key = QuickToolBarConstants.Config.Website.ApiRevertQuickToolBar,
                          Value = "{host}/api/ApiMasterPage/RevertQuickToolBar",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      }   ,
                      new QuickToolBarConfig
                      {
                          Pid = ++id,
                          Key = QuickToolBarConstants.Config.Website.ApiPreviewPage,
                          Value = "{host}/home/preview?previewType=tooltip",
                          DefaultValue = "{host}/home/preview?previewType=tooltip",
                          Group = "",
                          CreateUser = "admin",
                          CreateDate = DateTime.Now,
                          LastLogin = DateTime.Now,
                          UpdateUser = "admin",
                          UpdateDate = DateTime.Now,
                          Type = "Admin"
                      } ,
                      new QuickToolBarConfig
                      {
                          Pid = ++id,
                          Key = QuickToolBarConstants.Config.Website.ApiUpdatePreviewQuickToolBar,
                          Value = "{host}/api/ApiMasterPage/UpdatePreviewQuickTool",
                          DefaultValue = "{host}/api/ApiMasterPage/UpdatePreviewQuickTool",
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