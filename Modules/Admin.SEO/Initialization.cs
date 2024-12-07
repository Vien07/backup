using Admin.SEO.Database;
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
using Admin.SEO.Constants;
using static Admin.SEO.Constants.SEOConstants;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;
namespace Admin.SEO
{
    public static class Initialization
    {

        public static void AddModuleAdminSEO(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();

            services.AddScoped<IRepository<Database.SEO>>(provider =>
               new Repository<Database.SEO>(
                   provider.GetRequiredService<SEOContext>(),
                   provider.GetRequiredService<ILoggerHelper>(),
                   provider.GetRequiredService<IFileHelper>()
               ));
            services.AddScoped<IRepository<SEO_Files>>(provider =>
             new Repository<SEO_Files>(
                 provider.GetRequiredService<SEOContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
             ));

            services.AddScoped<IRepositoryConfig<SEOConfig>>(provider =>
                new RepositoryConfig<SEOConfig>(provider.GetRequiredService<SEOContext>()));
            services.AddScoped<ISEOService, SEOService>();

            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<SEOContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteSEO(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminSEO",
              pattern: "{controller=SEO}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminSEO",
              pattern: "{controller=SEOIntegrateSEOIntegrate}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
        public static void AddAPIAdminSEO(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IRepository<Database.SEO>>(provider =>
               new Repository<Database.SEO>(
                   provider.GetRequiredService<SEOContext>(),
                   provider.GetRequiredService<ILoggerHelper>(),
                   provider.GetRequiredService<IFileHelper>()
               ));
            services.AddScoped<IRepository<SEO_Files>>(provider =>
             new Repository<SEO_Files>(
                 provider.GetRequiredService<SEOContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
             ));

            services.AddScoped<IRepositoryConfig<SEOConfig>>(provider =>
                new RepositoryConfig<SEOConfig>(provider.GetRequiredService<SEOContext>()));
            services.AddScoped<ISEOService, SEOService>();
            services.AddScoped<IFileHelper, FileHelper>();

            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<SEOContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseAPISEO(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminSEO",
              pattern: "{controller=SEO}/{action=Index}/{id?}");
            });


        }
    }
}
