using Admin.EmailManagement.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Constant;
using Admin.EmailManagement.Services;
using Admin.EmailManagement.Servcies;
using Steam.Infrastructure.Repository;

namespace Admin.EmailManagement
{
    public static class Initialization
    {

        public static void AddModuleAdminEmail(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


                       services.AddScoped<IRepository<EmailTemplate>>(provider =>
               new Repository<EmailTemplate>(
                   provider.GetRequiredService<EmailContext>(),
                   provider.GetRequiredService<ILoggerHelper>(),
                   provider.GetRequiredService<IFileHelper>()
    ));

            services.AddScoped<IRepository<EmailMailBox>>(provider =>
                new Repository<EmailMailBox>(
                    provider.GetRequiredService<EmailContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));

            services.AddScoped<IRepository<EmailAdmin>>(provider =>
                new Repository<EmailAdmin>(
                    provider.GetRequiredService<EmailContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));




            services.AddScoped<IRepositoryConfig<EmailConfig>>(provider =>
                new RepositoryConfig<EmailConfig>(provider.GetRequiredService<EmailContext>()));

            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailAdminService, EmailAdminService>();
            services.AddScoped<IApiEmailManagementService, ApiEmailManagementService>();
            services.AddScoped<IEmailMailBoxService, EmailMailBoxService>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddComponentLibraryViews();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;EmailManagement ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<EmailContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseRouteAdminEmail(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminEmail",
              pattern: "{controller=EmailAdmin}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "TemplateEmail",
              pattern: "{controller=EmailTemplate}/{action=Index}/{id?}");
            });
            //app.UseComponentLibraryScripts();

            var absolutePath = Directory.GetCurrentDirectory();


            // Determine whether the directory exists.

        }

        public static void AddAPIAdminEmail(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");



            services.AddScoped<IRepository<EmailTemplate>>(provider =>
    new Repository<EmailTemplate>(
        provider.GetRequiredService<EmailContext>(),
        provider.GetRequiredService<ILoggerHelper>(),
        provider.GetRequiredService<IFileHelper>()
));

            services.AddScoped<IRepository<EmailMailBox>>(provider =>
                new Repository<EmailMailBox>(
                    provider.GetRequiredService<EmailContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));

            services.AddScoped<IRepository<EmailAdmin>>(provider =>
                new Repository<EmailAdmin>(
                    provider.GetRequiredService<EmailContext>(),
                    provider.GetRequiredService<ILoggerHelper>(),
                    provider.GetRequiredService<IFileHelper>()
                ));




            services.AddScoped<IRepositoryConfig<EmailConfig>>(provider =>
                new RepositoryConfig<EmailConfig>(provider.GetRequiredService<EmailContext>()));

            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailAdminService, EmailAdminService>();
            services.AddScoped<IApiEmailManagementService, ApiEmailManagementService>();
            services.AddScoped<IEmailMailBoxService, EmailMailBoxService>();
            string DefaultConnection = DefaultConnectionDatabase;

            services.AddDbContext<EmailContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }

        public static void UseAPIAdminEmail(this IApplicationBuilder app)
        {

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminEmail",
              pattern: "{controller=ApiEmailManagement}/{action=Index}/{id?}");
            });


        }
    }
}
