

using Microsoft.EntityFrameworkCore;
using ComponentUILibrary;
using Admin.MemberManagement.Database;
using Steam.Core.Utilities.STeamHelper;
using Admin.MemberManagement.Constants;
using Admin.EmailManagement;
using Steam.Core.Base.Constant;
using Admin.MemberManagement.Services;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;

namespace Admin.MemberManagement
{
    public static class Extensions
    {


        public static void AddModuleAdminMemberManagement(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");


            services.AddScoped<IRepository<Database.Feedback>>(provider =>
                 new Repository<Database.Feedback>(
                     provider.GetRequiredService<MemberManagementContext>(),
                     provider.GetRequiredService<ILoggerHelper>(),
                     provider.GetRequiredService<IFileHelper>()
                 ));

             services.AddScoped<IRepository<Database.Feedback_Files>>(provider =>
             new Repository<Database.Feedback_Files>(
                 provider.GetRequiredService<MemberManagementContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
                ));

             services.AddScoped<IRepository<Database.MemberManagement>>(provider =>
            new Repository<Database.MemberManagement>(
            provider.GetRequiredService<MemberManagementContext>(),
            provider.GetRequiredService<ILoggerHelper>(),
            provider.GetRequiredService<IFileHelper>()
            ));

                services.AddScoped<IRepository<Database.MemberManagement_Files>>(provider =>
                new Repository<Database.MemberManagement_Files>(
                provider.GetRequiredService<MemberManagementContext>(),
                provider.GetRequiredService<ILoggerHelper>(),
                provider.GetRequiredService<IFileHelper>()
               ));


            services.AddScoped<IRepositoryConfig<FeedbackConfig>>(provider =>
             new RepositoryConfig<FeedbackConfig>(provider.GetRequiredService<MemberManagementContext>()));

            services.AddScoped<IRepositoryConfig<MemberManagementConfig>>(provider =>
             new RepositoryConfig<MemberManagementConfig>(provider.GetRequiredService<MemberManagementContext>()));


            services.AddScoped<ISEOService, SEOService>();

            services.AddScoped<IMemberManagementService, MemberManagementService>();
            services.AddScoped<IApiMemberManagementService, ApiMemberManagementService>();
            services.AddScoped<IFeedbackService, FeedbackService>();


            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;Numerology ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<MemberManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }
        public static void UseRouteMemberManagement(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminMemberManagementManagement",
              pattern: "{controller=MemberManagementManagement}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminMemberManagementFeedback",
              pattern: "{controller=Feedback}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }

        public static void AddAPIAdminMemberManagement(this IServiceCollection services)
        {
            //IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;


            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddScoped<IMailHelper, MailHelper>();


            services.AddScoped<IRepository<Database.Feedback>>(provider =>
           new Repository<Database.Feedback>(
               provider.GetRequiredService<MemberManagementContext>(),
               provider.GetRequiredService<ILoggerHelper>(),
               provider.GetRequiredService<IFileHelper>()
           ));

            services.AddScoped<IRepository<Database.Feedback_Files>>(provider =>
            new Repository<Database.Feedback_Files>(
                provider.GetRequiredService<MemberManagementContext>(),
                provider.GetRequiredService<ILoggerHelper>(),
                provider.GetRequiredService<IFileHelper>()
               ));

            services.AddScoped<IRepository<Database.MemberManagement>>(provider =>
           new Repository<Database.MemberManagement>(
           provider.GetRequiredService<MemberManagementContext>(),
           provider.GetRequiredService<ILoggerHelper>(),
           provider.GetRequiredService<IFileHelper>()
           ));

            services.AddScoped<IRepository<Database.MemberManagement_Files>>(provider =>
            new Repository<Database.MemberManagement_Files>(
            provider.GetRequiredService<MemberManagementContext>(),
            provider.GetRequiredService<ILoggerHelper>(),
            provider.GetRequiredService<IFileHelper>()
           ));


            services.AddScoped<IRepositoryConfig<FeedbackConfig>>(provider =>
             new RepositoryConfig<FeedbackConfig>(provider.GetRequiredService<MemberManagementContext>()));

            services.AddScoped<IRepositoryConfig<MemberManagementConfig>>(provider =>
             new RepositoryConfig<MemberManagementConfig>(provider.GetRequiredService<MemberManagementContext>()));

            services.AddScoped<IMemberManagementService, MemberManagementService>();
            services.AddScoped<IApiMemberManagementService, ApiMemberManagementService>();

            string DefaultConnection = DefaultConnectionDatabase;// "Server=cdchost.ddns.net,1433;Database=cms_db;Integrated Security=False;Numerology ID=sa;Password=123456A@a;";
                                                                 //string DefaultConnection=  "Server=localhost;Database=CMS;Integrated Security=true";
            services.AddDbContext<MemberManagementContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );



        }
        public static void UseAPIMemberManagement(this IApplicationBuilder app)
        {

        }
    }
}
