using Admin.HeaderPage.Database;

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
using static Admin.HeaderPage.Constants.MenuConstants;

namespace Admin.HeaderPage
{
    public static class Initialization
    {

        public static void AddModuleAdminHeaderPage(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            //IConfigurationRoot configuration =
            //new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            //services.Configure<List<MultilangModel>>(configuration.GetSection("SystemConfig"));
            //services.AddOptions();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IHeaderPageRepository, HeaderPageRepository>();
            services.AddScoped<IMenuStyleRepository, MenuStyleRepository>();
            //services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            //services.AddScoped<Common.Helper.IViewRendererHelper, Common.Helper.ViewRendererHelper>();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;HeaderPage ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<HeaderPageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseRouteAdminHeaderPage(this IApplicationBuilder app)
        {
            //app.UseComponentLibraryScripts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminMenu",
               pattern: "{controller=HeaderPage}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "AdminHeaderPage",
                pattern: "{controller=HeaderPage}/{action=Index}/{id?}"); 
                endpoints.MapControllerRoute(
                name: "AdminMenuStyle",
                pattern: "{controller=MenuStyle}/{action=Index}/{id?}");
            });

            var absolutePath = Directory.GetCurrentDirectory();
            string path = Path.Combine(absolutePath + "/wwwroot", StaticPath.Asset.Image);

            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(path);

            }
            string pathThumb = Path.Combine(absolutePath + "/wwwroot", StaticPath.Asset.ImageThumb);

            if (Directory.Exists(pathThumb))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(pathThumb);

            }
        }
    }
}
