using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using Admin.Course.Database;
using Microsoft.EntityFrameworkCore;
using Admin.WebSetting.Constants;
using Steam.Core.Utilities.STeamHelper;
using static Admin.WebSetting.Constants.WebSettingConstants;
using Steam.Core.Base.Constant;
using Admin.WebSetting.Services;
using Steam.Infrastructure.Repository;
using Admin.WebSetting.Database;

namespace Admin.WebSetting
{
    public static class Extensions
    {
        public static void AddModuleAdminWebSetting(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

             services.AddScoped<IRepository<WebsiteConfiguration>>(provider =>
              new Repository<WebsiteConfiguration>(
                  provider.GetRequiredService<WebsiteConfigurationContext>(),
                  provider.GetRequiredService<ILoggerHelper>(),
                  provider.GetRequiredService<IFileHelper>()
              ));
            services.AddScoped<IRepositoryConfig<WebsiteConfiguration>>(provider =>
                new RepositoryConfig<WebsiteConfiguration>(provider.GetRequiredService<WebsiteConfigurationContext>()));



            services.AddComponentLibraryViews();
            services.AddScoped<IWebSettingService, WebSettingService>();

            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<WebsiteConfigurationContext>(options => options.UseSqlServer(DefaultConnection), ServiceLifetime.Transient);

        }
        public static void UseRouteWebSetting(this IApplicationBuilder app)
        {

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminWebsetting",
              pattern: "{controller=WebSetting}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageDefault);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.FontsAddon);
        }
    }
}
