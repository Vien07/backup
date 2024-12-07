using Admin.WebsiteKeys.Database;
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
using Steam.Core.Utilities.STeamHelper;
using Admin.WebsiteKeys.Constants;
using static Admin.WebsiteKeys.Constants.WebsiteKeysConstants;
using Steam.Core.Base.Constant;
using Admin.WebsiteKeys.Service;
using Steam.Infrastructure.Repository;

namespace Admin.WebsiteKeys
{
    public static class Initialization
    {

        public static void AddModuleAdminWebsiteKeys(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            services.AddScoped<IRepository<Database.WebsiteKeys>>(provider =>
              new Repository<Database.WebsiteKeys>(
                  provider.GetRequiredService<WebsiteKeysContext>(),
                  provider.GetRequiredService<ILoggerHelper>(),
                  provider.GetRequiredService<IFileHelper>()
              ));





            services.AddScoped<IRepositoryConfig<WebsiteKeysConfig>>(provider =>
                new RepositoryConfig<WebsiteKeysConfig>(provider.GetRequiredService<WebsiteKeysContext>()));
            services.AddScoped<IWebsiteKeysService, WebsiteKeysService>();

            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<WebsiteKeysContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteAdminWebsiteKeys(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminWebsiteKeys",
              pattern: "{controller=WebsiteKeys}/{action=Index}/{view?}");
            });

            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
    }
}
