using Admin.SEO;


using Admin.PostsManagement;
using Admin.Collection;

using Admin.ProductManagement;

using Admin.EmailManagement;
using Admin.MemberManagement;
using Admin.TemplatePage;
using Admin.PostsCategory;
using Steam.Core.Utilities;
namespace ApiGateWay.Builder
{


    public static class ModulesBuilder
    {

        public static void AddAPI4Steams(this IServiceCollection services)
        {
            services.AddModuleCommon();
            services.AddModuleAdminTemplatePage();
            services.AddModuleAdminSEO();
            services.AddAPIAdminPostsCategory();
            services.AddAPIAdminPostsManagement();



        }

        public static void UseAPI4Steams(this IApplicationBuilder app)
        {

            app.UseAPISEO();
            app.UseAPICollection();
            app.UseAPIPostsCategory();
            app.UseRouteAPIPostsManagement();
            app.UseApiMisaProduct();
            app.UseAPIAdminEmail();
            app.UseAPIAdminTemplatePage();
            app.UseAPIAdminEmail();
            app.UseAPIMemberManagement();

        }

    }

}