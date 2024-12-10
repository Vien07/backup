using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

namespace CMS.Middleware
{
    public static class RouterBizmac
    {
        public static IApplicationBuilder UseBizMacRoutes(this IApplicationBuilder app)
        {
            app.UseRewriter(new RewriteOptions()

              .AddRewrite(@"search", "Search/Index", skipRemainingRules: true)

              .AddRewrite(@"^quan-ly-don-hang.html", "Customer/OrderManagement", skipRemainingRules: true)

              .AddRewrite(@"^chi-tiet-don-hang.html", "Customer/OrderDetail", skipRemainingRules: true)

              .AddRewrite(@"^tai-khoan.html", "Customer/Index", skipRemainingRules: true)

              .AddRewrite(@"^dang-nhap.html", "Customer/SignIn", skipRemainingRules: true)

              .AddRewrite(@"^dang-ky.html", "Customer/SignUp", skipRemainingRules: true)

              .AddRewrite(@"kich-hoat-tai-khoan", "Customer/Active", skipRemainingRules: true)

              .AddRewrite(@"^quen-mat-khau.html", "Customer/ForgotPassword", skipRemainingRules: true)

              .AddRewrite(@"^doi-mat-khau.html", "Customer/ChangePassword", skipRemainingRules: true)

              .AddRewrite(@"^thanh-toan.html", "Cart/Checkout", skipRemainingRules: true)

              .AddRewrite(@"^dat-hang-thanh-cong.html(.*)", "Cart/CheckoutSuccess?orderId=$1", skipRemainingRules: true)

              .AddRewrite(@"profile/(.*)", "Customer/NameCard?url=$1", skipRemainingRules: true)

              .AddRewrite(@"^lien-he.html", "Contact/Index", skipRemainingRules: true)


              .AddRewrite(@"tin-tuc/(.*).html", "News/Detail?slug=$1", skipRemainingRules: true)
              .AddRewrite(@"tin-tuc/", "News/Index", skipRemainingRules: true)


              .AddRewrite(@"tinh-nang/(.*).html", "Feature/Detail?slug=$1", skipRemainingRules: true)
              .AddRewrite(@"tinh-nang/", "Feature/Index", skipRemainingRules: true)


              .AddRewrite(@"bang-gia/", "Product/Index", skipRemainingRules: true)

              .AddRewrite(@"gioi-thieu/(.*).html", "About/Detail?slug=$1", skipRemainingRules: true)
              .AddRewrite(@"gioi-thieu/", "About/Index", skipRemainingRules: true)

              .AddRewrite(@"maintenance.html", "Shared/Maintenance", skipRemainingRules: true)

              //.AddRewrite(@"en/", "Home/Index?lang=en", skipRemainingRules: false)
              //.AddRewrite(@"recovery?key=(.*)", "b-admin/Admin/ValidateRecoveryPassword?key=$1", skipRemainingRules: false)
              );

            return app;
        }

    }
}
