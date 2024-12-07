using Admin.TemplatePage.Database;
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
using Admin.TemplatePage.Constants;
using static Admin.TemplatePage.Constants.TemplatePageConstants;
using Steam.Core.Base.Constant;
using Admin.TemplatePage.Services;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;
using Admin.SEO;

namespace Admin.TemplatePage
{
    public static class Initialization
    {

        public static void AddModuleAdminTemplatePage(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper,ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            services.AddScoped<IRepository<Database.TemplatePage>>(provider =>
           new Repository<Database.TemplatePage>(
               provider.GetRequiredService<TemplatePageContext>(),
               provider.GetRequiredService<ILoggerHelper>(),
               provider.GetRequiredService<IFileHelper>()
           ));

            services.AddScoped<IRepositoryConfig<TemplatePageConfig>>(provider =>
                new RepositoryConfig<TemplatePageConfig>(provider.GetRequiredService<TemplatePageContext>()));
            services.AddModuleAdminSEO();

            services.AddScoped<ITemplatePageService, TemplatePageService>();
            services.AddScoped<IApiTemplatePageService, ApiTemplatePageService>();

            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<TemplatePageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteAdminTemplatePage(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminTemplatePage",
              pattern: "{controller=TemplatePage}/{action=Index}/{id?}");
            });

            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
        public static void AddAPIAdminTemplatePage(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IRepository<Database.TemplatePage>>(provider =>
           new Repository<Database.TemplatePage>(
               provider.GetRequiredService<TemplatePageContext>(),
               provider.GetRequiredService<ILoggerHelper>(),
               provider.GetRequiredService<IFileHelper>()
           ));

            services.AddScoped<IRepositoryConfig<TemplatePageConfig>>(provider =>
                new RepositoryConfig<TemplatePageConfig>(provider.GetRequiredService<TemplatePageContext>()));
            services.AddModuleAdminSEO();

            services.AddScoped<ITemplatePageService, TemplatePageService>();
            services.AddScoped<IApiTemplatePageService, ApiTemplatePageService>();

            string DefaultConnection = DefaultConnectionDatabase;


            services.AddScoped<ILoggerHelper, LoggerHelper>();
            services.AddDbContext<TemplatePageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseAPIAdminTemplatePage(this IApplicationBuilder app)
        {



        }
    }
}
