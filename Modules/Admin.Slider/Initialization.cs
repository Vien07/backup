using Admin.Slider.Database;
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
using Admin.Slider.Constants;

namespace Admin.Slider
{
    public static class Initialization
    {

        public static void AddModuleAdminSlider(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper,ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<SliderContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteAdminSlider(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminSlider",
              pattern: "{controller=Slider}/{action=Index}/{id?}");
            });

            var absolutePath = Directory.GetCurrentDirectory();
            string path = Path.Combine(absolutePath + "/wwwroot", SliderConstants.StaticPath.Asset.Image);

            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(path);

            }
            string pathThumb = Path.Combine(absolutePath+ "/wwwroot", SliderConstants.StaticPath.Asset.ImageThumb);

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
