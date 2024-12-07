using Admin.PostsCategory.Database;

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
using Admin.PostsCategory.Constants;
using Steam.Core.Utilities.STeamHelper;
using static Admin.PostsCategory.Constants.PostsCategoryConstants;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Steam.Infrastructure.Repository;
using Admin.SEO;
using Admin.SEO.Services;
using Admin.TemplatePage;
using Admin.PostsCategory.Services;

namespace Admin.PostsCategory
{
    public static class Initialization
    {

        public static void AddModuleAdminPostsCategory(this IServiceCollection services)
        {

            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddScoped<IRepository<Database.PostsCategory>>(provider =>
   new Repository<Database.PostsCategory>(
       provider.GetRequiredService<PostsCategoryContext>(),
       provider.GetRequiredService<ILoggerHelper>(),
       provider.GetRequiredService<IFileHelper>()
   ));
            services.AddScoped<IRepository<PostsCategory_Files>>(provider =>
             new Repository<PostsCategory_Files>(
                 provider.GetRequiredService<PostsCategoryContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
             ));

            services.AddScoped<IRepositoryConfig<PostsCategoryConfig>>(provider =>
                new RepositoryConfig<PostsCategoryConfig>(provider.GetRequiredService<PostsCategoryContext>()));

            services.AddModuleAdminTemplatePage();

            services.AddScoped<IPostsCategoryService, PostsCategoryService>();
            services.AddDbContext<PostsCategoryContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseRoutePostsCategory(this IApplicationBuilder app)
        {
            //app.UseComponentLibraryScripts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "AdminPostsCategory",
              pattern: "{controller=PostsCategory}/{action=Index}/{id?}");
            });
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.Image);
            //new FileHelper().CreateFolder("/wwwroot" + StaticPath.Asset.ImageThumb);
        }

        public static void AddAPIAdminPostsCategory(this IServiceCollection services)
        {

            string DefaultConnectionDatabase = SystemInfo.DefaultConnectionDatabase;

               services.AddScoped<IRepository<Database.PostsCategory>>(provider =>
                new Repository<Database.PostsCategory>(
                provider.GetRequiredService<PostsCategoryContext>(),
                provider.GetRequiredService<ILoggerHelper>(),
                provider.GetRequiredService<IFileHelper>()
                ));
             services.AddScoped<IRepository<PostsCategory_Files>>(provider =>
             new Repository<PostsCategory_Files>(
                 provider.GetRequiredService<PostsCategoryContext>(),
                 provider.GetRequiredService<ILoggerHelper>(),
                 provider.GetRequiredService<IFileHelper>()
             ));

            services.AddScoped<IRepositoryConfig<PostsCategoryConfig>>(provider =>
                new RepositoryConfig<PostsCategoryConfig>(provider.GetRequiredService<PostsCategoryContext>()));

            services.AddScoped<IPostsCategoryService, PostsCategoryService>();

            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<PostsCategoryContext>(
                                    options => options.UseSqlServer(DefaultConnection),
                                    ServiceLifetime.Transient
                                    );


        }

        public static void UseAPIPostsCategory(this IApplicationBuilder app)
        {


        }
    }
}
