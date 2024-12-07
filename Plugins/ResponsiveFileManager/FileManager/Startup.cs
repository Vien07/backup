using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steam.Core.FileManager.Helper;
using Steam.Core.FileManager.Model;

namespace Steam.Core.FileManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            // Add application services.

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddResponsiveFileManager(options =>
            {
                options.UploadDirectory = "/FileStorage/Storage/";
                options.CurrentPath = "../FileStorage/Storage/";
                options.ThumbsBasePath = "../FileStorage/thumbs/";
                options.MaxSizeUpload = 32;
            });
            services.Configure<AdminLogin>(Configuration.GetSection("AdminLogin"));
            services.AddScoped<IFileHelper, FileHelper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
        
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSession();

            app.UseDefaultFiles();
            app.UseStaticFiles(); // shortcut for HostEnvironment.WebRootFileProvider

            app.UseResponsiveFileManager();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}