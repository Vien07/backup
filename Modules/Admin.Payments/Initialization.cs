using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Steam.Core.Base.Constant;
using Admin.Payments.Database;
using Admin.Payments.Services;
using Admin.Payments.PayOS_Services;

namespace Admin.Payments
{
    public static class Initialization
    {

        public static void AddModuleAdminPayments(this IServiceCollection services)
        {
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //services.AddScoped<IFileHelper, FileHelper>();
            //services.AddComponentLibraryViews();
            //services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            //services.AddScoped<ILoggerHelper, LoggerHelper>();

            //services.AddScoped<IRepository<Database.PostsManagement>>(provider =>
            //  new Repository<Database.PostsManagement>(
            //      provider.GetRequiredService<PostsManagementContext>(),
            //      provider.GetRequiredService<ILoggerHelper>(),
            //      provider.GetRequiredService<IFileHelper>()
            //  ));

            //services.AddScoped<IRepository<Database.PostsManagement_Files>>(provider =>
            //new Repository<Database.PostsManagement_Files>(
            //    provider.GetRequiredService<PostsManagementContext>(),
            //    provider.GetRequiredService<ILoggerHelper>(),
            //    provider.GetRequiredService<IFileHelper>()
            //   ));

            //services.AddScoped<IRepositoryConfig<PostsManagementConfig>>(provider =>
            //    new RepositoryConfig<PostsManagementConfig>(provider.GetRequiredService<PostsManagementContext>()));

            services.AddTransient<IPayOSService, PayOSService>();
            services.AddTransient<IPaymentService, PaymentService>();

            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<PaymentContext>(options => options.UseSqlServer(DefaultConnection), ServiceLifetime.Transient);

        }
        public static void UseRoutePayments(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "AdminPayments",
                    pattern: "{controller=Payment}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //    name: "AdminPostsManagementApi",
                //    pattern: "{controller=ApiPostsManagement}/{action=Index}/{id?}");
            });
        }
        //public static void AddAPIAdminPostsManagement(this IServiceCollection services)
        //{
        //    string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;
        //    services.AddScoped<IApiPostsManagementService, ApiPostsManagementService>();
        //    services.AddScoped<ILoggerHelper, LoggerHelper>();
        //    string DefaultConnection = DefaultConnectionDatabase;

        //    services.AddDbContext<PostsManagementContext>(options => options.UseSqlServer(DefaultConnection), ServiceLifetime.Transient);
        //}
        //public static void UseRouteAPIPostsManagement(this IApplicationBuilder app)
        //{
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //         name: "AdminPostsManagementApi",
        //         pattern: "{controller=ApiPostsManagement}/{action=Index}/{id?}");
        //    });
        //}
    }
}
