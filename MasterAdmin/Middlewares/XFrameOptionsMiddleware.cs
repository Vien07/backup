using Admin.Authorization.Database;
using System.IdentityModel.Tokens.Jwt;

using Steam.Core.Base.Constant;

namespace MasterAdmin
{
    public class XFrameOptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _allowedOrigin;

        public XFrameOptionsMiddleware(RequestDelegate next, string allowedOrigin)
        {
            _next = next;
            _allowedOrigin = allowedOrigin;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("X-Frame-Options", $"ALLOW-FROM {_allowedOrigin}");
            await _next(context);
        }
    }

    public static class XFrameOptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseXFrameOptionsMiddleware(this IApplicationBuilder builder, string allowedOrigin)
        {
            return builder.UseMiddleware<XFrameOptionsMiddleware>(allowedOrigin);
        }
    }
}
