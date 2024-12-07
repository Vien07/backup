using Admin.PostsManagement.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using Steam.Core.Utilities.STeamHelper;
using Admin.PostsManagement.Constants;
using Admin.PostsManagement.ApiControllers;
using Steam.Core.Base;
using static Admin.PostsCategory.Constants.PostsCategoryConstants;
using Steam.Core.Base.Constant;
using Admin.PostsManagement.Services;
using Steam.Infrastructure.Repository;
using Admin.SEO;
using Admin.SEO.Services;
using Admin.PostsCategory;

namespace Admin.PostsManagement
{
    public static class Initialization
    {

        public static void AddModuleAdminPostsManagement(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");



            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddScoped<IRepository<Database.PostsManagement>>(provider =>
              new Repository<Database.PostsManagement>(
                  provider.GetRequiredService<PostsManagementContext>(),
                  provider.GetRequiredService<ILoggerHelper>(),
                  provider.GetRequiredService<IFileHelper>()
              ));
             services.AddScoped<IRepository<Database.PostsManagement_Files>>(provider =>
             new Repository<Database.PostsManagement_Files>(
                 provider.GetRequiredService<PostsManagementContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
                ));

            services.AddScoped<IRepositoryConfig<PostsManagementConfig>>(provider =>
                new RepositoryConfig<PostsManagementConfig>(provider.GetRequiredService<PostsManagementContext>()));
            //services.AddScoped<ISEOService, SEOService>();
            services.AddModuleAdminSEO();
            services.AddModuleAdminPostsCategory();

            services.AddScoped<IPostsManagementService, PostsManagementService>();
            //services.AddScoped<IApiPostsManagementService, ApiPostsManagementService>();

            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;PostsManagement ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<PostsManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );

        }
        public static void UseRoutePostsManagement(this IApplicationBuilder app)
        {




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminPostsManagement",
              pattern: "{controller=PostsManagement}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                 name: "AdminPostsManagementApi",
                 pattern: "{controller=ApiPostsManagement}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
        public static void AddAPIAdminPostsManagement(this IServiceCollection services)
        {
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddScoped<IRepository<Database.PostsManagement>>(provider =>
              new Repository<Database.PostsManagement>(
                  provider.GetRequiredService<PostsManagementContext>(),
                  provider.GetRequiredService<ILoggerHelper>(),
                  provider.GetRequiredService<IFileHelper>()
              ));
            services.AddScoped<IRepository<Database.PostsManagement_Files>>(provider =>
            new Repository<Database.PostsManagement_Files>(
                provider.GetRequiredService<PostsManagementContext>(),
                provider.GetRequiredService<ILoggerHelper>(),
                provider.GetRequiredService<IFileHelper>()
               ));

            services.AddScoped<IRepositoryConfig<PostsManagementConfig>>(provider =>
                new RepositoryConfig<PostsManagementConfig>(provider.GetRequiredService<PostsManagementContext>()));

            services.AddScoped<IApiPostsManagementService, ApiPostsManagementService>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;

            services.AddDbContext<PostsManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );

        }
        public static void UseRouteAPIPostsManagement(this IApplicationBuilder app)
        {

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "AdminPostsManagementApi",
                 pattern: "{controller=ApiPostsManagement}/{action=Index}/{id?}");
            });

        }

    }
}
