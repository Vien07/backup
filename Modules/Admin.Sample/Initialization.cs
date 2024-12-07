using Admin.Sample.Database;
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
using Admin.Sample.Constants;
using static Admin.Sample.Constants.SampleConstants;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;
using Admin.Sample.Models;
using AutoMapper;

namespace Admin.Sample
{
    public static class Initialization
    {

        public static void AddModuleAdminSample(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddDbContextPool<SampleContext>(options =>
      options.UseSqlServer(DefaultConnectionDatabase));


            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();


            // Register the generic repository
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Additional service registrations
            // Register repositories using factory methods
            services.AddScoped<IRepository<SampleDetail>>(provider =>
                new Repository<SampleDetail>(
                    provider.GetRequiredService<SampleContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));

            services.AddScoped<IRepository<Sample_Files>>(provider =>
                new Repository<Sample_Files>(
                    provider.GetRequiredService<SampleContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));




            services.AddScoped<IRepositoryConfig<SampleConfig>>(provider =>
                new RepositoryConfig<SampleConfig>(provider.GetRequiredService<SampleContext>()));

            services.AddScoped<ISampleService, SampleService>();


            services.AddSingleton<IMapper>(AutoMapperConfig.Initialize());

        }

        public static void UseRouteAdminSample(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminSample",
              pattern: "{controller=Sample}/{action=Index}/{id?}");
            }); app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "ApiAdminSample",
              pattern: "{controller=ApiSample}/{action=Index}/{id?}");
            });
            //app.UseComponentLibraryScripts();

            var absolutePath = Directory.GetCurrentDirectory();
            string path = Path.Combine(absolutePath + "/wwwroot", SampleConstants.StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
    }
}
