

using Microsoft.EntityFrameworkCore;
using ComponentUILibrary;
using Admin.LogManagement.Database;
using Steam.Core.Common.STeamHelper;
using Admin.LogManagement.Constants;

namespace Admin.LogManagement
{
    public static class Extensions
    {


        public static void AddModuleAdminLogManagement(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();


            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<ILogManagementRepository, LogManagementRepository>();

            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;Numerology ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<LogManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }
        public static void UseRouteLogManagement(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminLogManagementManagement",
              pattern: "{controller=LogManagementManagement}/{action=Index}/{id?}");
            });

            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
    }
}
