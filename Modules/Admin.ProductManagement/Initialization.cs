using Admin.ProductManagement.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ComponentUILibrary;
using Steam.Core.Utilities.STeamHelper;
using Admin.ProductManagement.Services;
using Admin.ProductManagement.Helpers;
using Admin.ProductManagement.ScheduleTask;
using Steam.Core.Base.Constant;
using Admin.ProductManagement.Repository;
using AutoMapper;
using Admin.SEO.Services;
using Steam.Infrastructure.Repository;

namespace Admin.ProductManagement
{
    public static class Initialization
    {
        public static void AddModuleAdminProductManagement(this IServiceCollection services)
        {
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContextPool<ProductManagementContext>(options => options.UseSqlServer(DefaultConnectionDatabase));

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddComponentLibraryViews();



            services.AddScoped<IRepositoryConfig<ProductCategoryConfig>>(provider =>
                new RepositoryConfig<ProductCategoryConfig>(provider.GetRequiredService<ProductManagementContext>()));


              services.AddScoped<IRepository<ProductCategory>>(provider =>
               new Repository<ProductCategory>(
                   provider.GetRequiredService<ProductManagementContext>(),
                   provider.GetRequiredService<ILoggerHelper>(),
                   provider.GetRequiredService<IFileHelper>()
               ));

            services.AddTransient(typeof(IProductManagementRepository<>), typeof(ProductManagementRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddTransient<ISEOService, SEOService>();
            services.AddTransient<IMisaApiService, MisaApiService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMisaApiTrackerService, MisaApiTrackerService>();
            services.AddTransient<IProductSpecificatyService, ProductSpecificatyService>();
            services.AddTransient<IApiProductsService, ApiProductsService>();
            services.AddTransient<IProductPolicyService, ProductPolicyService>();
            services.AddTransient<IOrderManagementService, OrderManagementService>();
            services.AddTransient<IProductCollectionService, ProductCollectionService>();

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IMisaRestHelper, MisaRestHelper>();
            services.AddScoped<IMisaApiTrackerHelper, MisaApiTrackerHelper>();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddScoped<GetMisaAccessTokenScheduleTask>();
        }

        public static void UseRouteProductManagement(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "AdminProductMamagement",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "AdminMisaProductApi",
                    pattern: "{controller=ApiMisaProduct}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "AdminOrderManagement",
                    pattern: "{controller=OrderManagement}/{action=Index}/{id?}");
            });
        }
        public static void AddApiAdminMisaProduct(this IServiceCollection services)
        {
            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<ProductManagementContext>(options => options.UseSqlServer(DefaultConnectionDatabase), ServiceLifetime.Transient);

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddComponentLibraryViews();

            services.AddTransient(typeof(IProductManagementRepository<>), typeof(ProductManagementRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IMisaApiService, MisaApiService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMisaApiTrackerService, MisaApiTrackerService>();
            services.AddTransient<IProductSpecificatyService, ProductSpecificatyService>();
            services.AddTransient<IApiProductsService, ApiProductsService>();
            services.AddTransient<IProductPolicyService, ProductPolicyService>();
            services.AddTransient<IOrderManagementService, OrderManagementService>();
            services.AddTransient<IProductCollectionService, ProductCollectionService>();

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IMisaRestHelper, MisaRestHelper>();
            services.AddScoped<IMisaApiTrackerHelper, MisaApiTrackerHelper>();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<ILoggerHelper, LoggerHelper>();

            services.AddScoped<GetMisaAccessTokenScheduleTask>();
        }

        public static void UseApiMisaProduct(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "AdminMisaProductApi",
                    pattern: "{controller=ApiMisaProduct}/{action=Index}/{id?}");
            });
        }
    }
}
