using Steam.Core.LocalizeManagement.Database;
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
using Steam.Core.LocalizeManagement.Constants;
using static Steam.Core.LocalizeManagement.Constants.LocalizeManagementConstants;
using Steam.Core.Base.Constant;
using Steam.Core.LocalizeManagement.Services;
using Steam.Infrastructure.Repository;

namespace Steam.Core.LocalizeManagement
{
    public static class Initialization
    {

        public static void AddModuleAdminLocalizeManagement(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IRepository<Database.LocalizationResource>>(provider =>
  new Repository<Database.LocalizationResource>(
      provider.GetRequiredService<LocalizeManagementContext>(),
      provider.GetRequiredService<ILoggerHelper>(),
      provider.GetRequiredService<IFileHelper>()
     ));       
            
            services.AddScoped<IRepository<Database.LocalizeCulture>>(provider =>
  new Repository<Database.LocalizeCulture>(
      provider.GetRequiredService<LocalizeManagementContext>(),
      provider.GetRequiredService<ILoggerHelper>(),
      provider.GetRequiredService<IFileHelper>()
     ));      
            
            services.AddScoped<IRepository<Database.LocalizeManagement>>(provider =>
  new Repository<Database.LocalizeManagement>(
      provider.GetRequiredService<LocalizeManagementContext>(),
      provider.GetRequiredService<ILoggerHelper>(),
      provider.GetRequiredService<IFileHelper>()
     ));   
            
            services.AddScoped<IRepository<Database.LocalizedContentProperty>>(provider =>
  new Repository<Database.LocalizedContentProperty>(
      provider.GetRequiredService<LocalizeManagementContext>(),
      provider.GetRequiredService<ILoggerHelper>(),
      provider.GetRequiredService<IFileHelper>()
     ));

            services.AddScoped<IRepositoryConfig<LocalizeManagementConfig>>(provider =>
                new RepositoryConfig<LocalizeManagementConfig>(provider.GetRequiredService<LocalizeManagementContext>()));

            services.AddScoped<IRepositoryConfig<LocalizeCultureConfig>>(provider =>
    new RepositoryConfig<LocalizeCultureConfig>(provider.GetRequiredService<LocalizeManagementContext>()));

            services.AddScoped<IRepositoryConfig<LocalizedContentPropertyConfig>>(provider =>
new RepositoryConfig<LocalizedContentPropertyConfig>(provider.GetRequiredService<LocalizeManagementContext>()));
            services.AddScoped<ILocalizeCultureService, LocalizeCultureService>();
            services.AddScoped<ILocalizeManagementService, LocalizeManagementService>();
            services.AddScoped<ILocalizedContentPropertyService, LocalizedContentPropertyService>();

            services.AddScoped<IContentLocalizationService, ContentLocalizationService>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper,ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<LocalizeManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteAdminLocalizeManagement(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminLocalizeManagement",
              pattern: "{controller=LocalizeManagement}/{action=Index}/{view?}");
            });

            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
    }
}
