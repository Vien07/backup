using Admin.HomePage.Database;
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
using Steam.Core.Common.STeamHelper;
using Admin.HomePage.Constants;

namespace Admin.HomePage
{
    public static class Initialization
    {

        public static void AddModuleAdminHomePage(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IHomePageRepository, HomePageRepository>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper,ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;HomePage ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<HomePageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseRouteAdminHomePage(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminHomePage",
              pattern: "{controller=HomePage}/{action=Index}/{id?}");
            });
            //app.UseComponentLibraryScripts();

            var absolutePath = Directory.GetCurrentDirectory();    
        }
    }
}
