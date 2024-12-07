using Admin.Authorization.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using Admin.Authorization.Services;
using Steam.Core.Utilities.STeamHelper;
using Admin.Authorization.Constants;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization
{
    public static class Initialization
    {

        public static void AddModuleAdminAuthorization(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            //IConfigurationRoot configuration =
            //new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();

            //services.Configure<List<MultilangModel>>(configuration.GetSection("SystemConfig"));
            //services.AddOptions();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


            services.AddScoped<IRepository<Database.User_Groups>>(provider =>
new Repository<Database.User_Groups>(
provider.GetRequiredService<AuthorizationContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
            services.AddScoped<IRepository<Database.User>>(provider =>
new Repository<Database.User>(
provider.GetRequiredService<AuthorizationContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
            services.AddScoped<IRepository<Database.ModuleRole>>(provider =>
new Repository<Database.ModuleRole>(
provider.GetRequiredService<AuthorizationContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));

            services.AddScoped<IRepository<LogManagement>>(provider =>
new Repository<LogManagement>(
provider.GetRequiredService<AuthorizationContext>(),
provider.GetRequiredService<ILoggerHelper>(),
provider.GetRequiredService<IFileHelper>()
));
            services.AddScoped<IRepository<Database.GroupRole>>(provider =>
       new Repository<Database.GroupRole>(
           provider.GetRequiredService<AuthorizationContext>(),
           provider.GetRequiredService<ILoggerHelper>(),
           provider.GetRequiredService<IFileHelper>()
       ));
            services.AddScoped<IRepository<Database.Group_ModuleRole>>(provider =>
            new Repository<Database.Group_ModuleRole>(
                provider.GetRequiredService<AuthorizationContext>(),
                provider.GetRequiredService<ILoggerHelper>(),
                provider.GetRequiredService<IFileHelper>()
               ));

            services.AddScoped<IRepositoryConfig<AuthorizationConfig>>(provider =>
                new RepositoryConfig<AuthorizationConfig>(provider.GetRequiredService<AuthorizationContext>()));
            services.AddScoped<IRepositoryConfig<LogManagementConfig>>(provider =>
             new RepositoryConfig<LogManagementConfig>(provider.GetRequiredService<AuthorizationContext>()));
            services.AddScoped<IRepositoryConfig<UserConfig>>(provider =>
            new RepositoryConfig<UserConfig>(provider.GetRequiredService<AuthorizationContext>()));

            services.AddScoped<IModuleRoleService, ModuleRoleService>();
            services.AddScoped<IGroupRoleService, GroupRoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILogManagementService, LogManagementService>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddComponentLibraryViews();
            services.AddComponentLibraryViews();
            //services.AddScoped<Common.Helper.IViewRendererHelper, Common.Helper.ViewRendererHelper>();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;Authorization ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<AuthorizationContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );

        }

        public static void UseRouteAuthorization(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "AdminAuthorizationLogin",
                pattern: "{controller=Login}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                name: "AdminAuthorizationAccount",
                pattern: "{controller=Account}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                name: "AdminAuthorizationModuleRole",
                pattern: "{controller=ModuleRole}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                name: "AdminAuthorizationGroupRole",
                pattern: "{controller=GroupRole}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
               name: "AdminAuthorizationLogManagement",
               pattern: "{controller=LogManagement}/{action=Index}/{id?}"
               );
            });

            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }
    }
}
