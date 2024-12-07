using Admin.PostsCategory;
using Admin.SEO;
using Admin.Sample;
using Admin.PostsManagement;
using Admin.Collection;
using Admin.Authorization;
using Admin.LayoutPage;
using Admin.WebSetting;
using Admin.ProductManagement;
using Admin.EmailManagement;
using Admin.MemberManagement;
using Admin.WebsiteKeys;
using Admin.TemplatePage;
using Steam.Core.Utilities;
using Steam.Core.LocalizeManagement;
using Admin.Orders;
using Admin.Payments;

namespace MasterAdmin.Builder
{


    public static class ModulesBuilder
    {

        public static void AddModules4Steams(this IServiceCollection services)
        {

            //services.AddModuleAdminSEO();

            services.AddModuleAdminLocalizeManagement();
            services.AddModuleCommon();
            services.AddModuleAdminSample();
            services.AddModuleAdminTemplatePage();

            services.AddModuleAdminPostsCategory();

            services.AddModuleAdminPostsManagement();

            services.AddModuleAdminCollection();

            services.AddModuleAdminLayoutPage();
            services.AddModuleAdminEmail();
            services.AddModuleAdminMemberManagement();
            services.AddModuleAdminAuthorization();
            services.AddModuleAdminWebSetting();
            services.AddModuleAdminWebsiteKeys();

            //services.AddModuleAdminMisaProduct();
            services.AddModuleAdminProductManagement();
            services.AddModuleAdminOrders();
            services.AddModuleAdminPayments();

        }

        public static void UseAddModules4Steams(this IApplicationBuilder app)
        {
            app.UseRouteAdminLocalizeManagement();
            app.UseRouteAdminLayoutPage();
            app.UseRoutePostsManagement();
            app.UseRouteCollection();
            app.UseRoutePostsCategory();
            app.UseRouteSEO();
            app.UseRouteAuthorization();
            app.UseRouteAdminSample();
            app.UseRouteWebSetting();
            app.UseRouteAdminEmail();
            app.UseRouteMemberManagement();
            app.UseRouteAdminWebsiteKeys();
            app.UseRouteAdminTemplatePage();
            //app.UseRouteMisaProduct();
            app.UseRouteProductManagement();
            app.UseRouteOrders();
            app.UseRoutePayments();
        }

    }

}