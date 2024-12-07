using Admin.FooterPage.Database;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using static Admin.FooterPage.Constants.FooterPageConstants;

namespace Admin.FooterPage
{
    public static class Initialization
    {

        public static void AddModuleAdminFooterPage(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            //IConfigurationRoot configuration =
            //new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            //services.Configure<List<MultilangModel>>(configuration.GetSection("SystemConfig"));
            //services.AddOptions();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


            services.AddScoped<IFooterPageRepository, FooterPageRepository>();
            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<FooterPageContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseRouteAdminFooterPage(this IApplicationBuilder app)
        {
            //app.UseComponentLibraryScripts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminMenu",
               pattern: "{controller=FooterPage}/{action=Index}/{id?}");
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
