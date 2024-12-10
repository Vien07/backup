using CMS.Mapping;
using CMS.Middleware;
using CMS.Services.WebsiteServices;
using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CMS
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration,
                        IWebHostEnvironment environment)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
                options.AutomaticAuthentication = true;

            });

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;

            });

            services.AddMvc(options => { });

            services.AddDbContextPool<DBContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddDirectoryBrowser();// allow file

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();
            services.AddScheduler();

            services.AddModules();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();


            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
            services.AddResponsiveFileManager(options =>
            {
                options.UploadDirectory = "/media/";
                options.CurrentPath = "../media/";
                options.ThumbsBasePath = "../img/thumbs/";
                options.MaxSizeUpload = 32;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/");
                app.UseHsts();
            }

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 404)
                {
                    ctx.Request.Path = "/error/";
                    await next();
                }
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(60)
                    };
                }
            });

            app.UseSession();

            app.UseRequestCulture();

            app.UseBizMacRoutes();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapControllerRoute(name: "MyArea", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaAdmin", areaName: "Admin", pattern: "/b-admin/{controller=Admin}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaDashboard", areaName: "Dashboard", pattern: "/b-admin/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaConfigurations", areaName: "Configurations", pattern: "/b-admin/{controller=GeneralConfiguration}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaContact", areaName: "Contact", pattern: "/b-admin/{controller=Contact}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaNews", areaName: "News", pattern: "/b-admin/{controller=News}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaGallery", areaName: "Gallery", pattern: "/b-admin/{controller=Gallery}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaDiscountCode", areaName: "DiscountCode", pattern: "/b-admin/{controller=DiscountCode}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaFAQ", areaName: "FAQ", pattern: "/b-admin/{controller=FAQ}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaFeature", areaName: "Feature", pattern: "/b-admin/{controller=Feature}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaAbout", areaName: "About", pattern: "/b-admin/{controller=About}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaPartner", areaName: "Partner", pattern: "/b-admin/{controller=Partner}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaConvenience", areaName: "Convenience", pattern: "/b-admin/{controller=Convenience}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaTranslation", areaName: "Translation", pattern: "/b-admin/{controller=Translation}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaBanner", areaName: "Banner", pattern: "/b-admin/{controller=Banner}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaSlide", areaName: "Slide", pattern: "/b-admin/{controller=Slide}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaCalendar", areaName: "Calendar", pattern: "/b-admin/{controller=Calendar}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaHomePage", areaName: "HomePage", pattern: "/b-admin/{controller=HomePage}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaTrash", areaName: "Trash", pattern: "/b-admin/{controller=Trash}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaCustomer", areaName: "Customer", pattern: "/b-admin/{controller=Customer}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaAdvertisement", areaName: "Advertisement", pattern: "/b-admin/{controller=Advertisement}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaPopup", areaName: "Popup", pattern: "/b-admin/{controller=Popup}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaProduct", areaName: "Product", pattern: "/b-admin/{controller=Product}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaOrder", areaName: "Order", pattern: "/b-admin/{controller=Order}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaRecruitment", areaName: "Recruitment", pattern: "/b-admin/{controller=Recruitment}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaComment", areaName: "Comment", pattern: "/b-admin/{controller=Comment}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaPromotion", areaName: "Promotion", pattern: "/b-admin/{controller=Promotion}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "MyAreaShared", areaName: "Shared", pattern: "/b-admin/{controller=GeneralConfiguration}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "sitemap.xml",
                pattern: "{controller=Shared}/{action=Sitemap}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseResponsiveFileManager();

            app.ApplicationServices.UseScheduler(scheduler =>
            {
                scheduler.Schedule<DomainRequestServices>().EveryMinute().Weekday().Weekend();
            });

        }
    }
}