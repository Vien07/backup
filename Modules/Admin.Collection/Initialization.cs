using Admin.Collection.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using Steam.Core.Utilities.STeamHelper;
using Admin.Collection.Constants;
using Admin.Collection.ApiControllers;
using Steam.Core.Base;
using Steam.Core.Base.Constant;

namespace Admin.Collection
{
    public static class Initialization
    {

        public static void AddModuleAdminCollection(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IApiCollectionRepository, ApiCollectionRepository>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper,ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;Collection ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<CollectionContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );

        }

        public static void UseRouteCollection(this IApplicationBuilder app)
        {




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminCollection",
              pattern: "{controller=Collection}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                 name: "AdminCollectionApi",
                 pattern: "{controller=ApiCollection}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + CollectionConstants.StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + CollectionConstants.StaticPath.Asset.ImageThumb);
        }
        public static void AddAPIAdminCollection(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IApiCollectionRepository, ApiCollectionRepository>();
            string DefaultConnection = DefaultConnectionDatabase;
            
            services.AddDbContext<CollectionContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );

        }

        public static void UseAPICollection(this IApplicationBuilder app)
        {

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "AdminCollectionApi",
                 pattern: "{controller=ApiCollection}/{action=Index}/{id?}");
            });

        }
    }
}
