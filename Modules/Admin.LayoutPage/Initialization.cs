using Admin.LayoutPage.Database;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using ComponentUILibrary.Models;
using static Admin.LayoutPage.Constants.MenuConstants;
using Steam.Core.Utilities.STeamHelper;
using Admin.LayoutPage.Constants;
using Steam.Core.Base.Constant;
using Admin.LayoutPage.Services;
using Steam.Infrastructure.Repository;

namespace Admin.LayoutPage
{
    public static class Initialization
    {

        public static void AddModuleAdminLayoutPage(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            //IConfigurationRoot configuration =
            //new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            //services.Configure<List<MultilangModel>>(configuration.GetSection("SystemConfig"));
            //services.AddOptions();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IRepository<FooterItem>>(provider =>
      new Repository<FooterItem>(
          provider.GetRequiredService<LayoutPageContext>(),
          provider.GetRequiredService<ILoggerHelper>(),
          provider.GetRequiredService<IFileHelper>()
      ));
            services.AddScoped<IRepository<FooterPage>>(provider =>
new Repository<FooterPage>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));      
            services.AddScoped<IRepository<HeaderPage>>(provider =>
new Repository<HeaderPage>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
)); 
            services.AddScoped<IRepository<HomePage>>(provider =>
new Repository<HomePage>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));    
            services.AddScoped<IRepository<Menu>>(provider =>
new Repository<Menu>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));   
            services.AddScoped<IRepository<MenuItemStyle>>(provider =>
new Repository<MenuItemStyle>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
            services.AddScoped<IRepository<MenuStyle>>(provider =>
new Repository<MenuStyle>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
  services.AddScoped<IRepository<QuickToolBar>>(provider =>
new Repository<QuickToolBar>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
            services.AddScoped<IRepository<QuickToolBarItem>>(provider =>
new Repository<QuickToolBarItem>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
     services.AddScoped<IRepository<Slider>>(provider =>
new Repository<Slider>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));

             services.AddScoped<IRepository<SliderItem>>(provider =>
new Repository<SliderItem>(
provider.GetRequiredService<LayoutPageContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));

            services.AddScoped<IRepositoryConfig<FooterPageConfig>>(provider =>
                new RepositoryConfig<FooterPageConfig>(provider.GetRequiredService<LayoutPageContext>()));  
            
            services.AddScoped<IRepositoryConfig<HeaderPageConfig>>(provider =>
                new RepositoryConfig<HeaderPageConfig>(provider.GetRequiredService<LayoutPageContext>()));    

            services.AddScoped<IRepositoryConfig<HomePageConfig>>(provider =>
                new RepositoryConfig<HomePageConfig>(provider.GetRequiredService<LayoutPageContext>()));

            services.AddScoped<IRepositoryConfig<MenuConfig>>(provider =>
                new RepositoryConfig<MenuConfig>(provider.GetRequiredService<LayoutPageContext>()));

            services.AddScoped<IRepositoryConfig<MenuStyleConfig>>(provider =>
                new RepositoryConfig<MenuStyleConfig>(provider.GetRequiredService<LayoutPageContext>()));

            services.AddScoped<IRepositoryConfig<QuickToolBarConfig>>(provider =>
                new RepositoryConfig<QuickToolBarConfig>(provider.GetRequiredService<LayoutPageContext>()));

            services.AddScoped<IRepositoryConfig<SliderConfig>>(provider =>
                new RepositoryConfig<SliderConfig>(provider.GetRequiredService<LayoutPageContext>()));



            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IHeaderPageService, HeaderPageService>();
            services.AddScoped<IMenuStyleService, MenuStyleService>();
            services.AddScoped<IHomePageService, HomePageService>();
            services.AddScoped<IFooterPageService, FooterPageService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IQuickToolBarService, QuickToolBarService>();

            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;
            // "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;LayoutPage ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<LayoutPageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseRouteAdminLayoutPage(this IApplicationBuilder app)
        {
            //app.UseComponentLibraryScripts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "AdminMenu",
               pattern: "{controller=Menu}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "AdminHeaderPage",
                pattern: "{controller=HeaderPage}/{action=Index}/{id?}"); 
                endpoints.MapControllerRoute(
                name: "AdminMenuStyle",
                pattern: "{controller=MenuStyle}/{action=Index}/{id?}");      
                endpoints.MapControllerRoute(
                name: "AdminHomePage",
                pattern: "{controller=HomePage}/{action=Index}/{id?}"); 
                endpoints.MapControllerRoute(
                name: "AdminFooterPage",
                pattern: "{controller=FooterPage}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "AdminSlider",
                pattern: "{controller=Slider}/{action=Index}/{id?}");        
                endpoints.MapControllerRoute(
                name: "AdminQuickToolBar",
                pattern: "{controller=QuickToolBar}/{action=Index}/{id?}");
            });
          
            //new FileHelper().CreateFolder("/wwwroot"+ LayoutPageConstants.StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot"+ LayoutPageConstants.StaticPath.Asset.ImageThumb);

        }
    }
}
