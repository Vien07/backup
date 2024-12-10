using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Middleware
{
    public class CheckValidFilemanagerPopup
    {
        private readonly RequestDelegate _next;
        public CheckValidFilemanagerPopup(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/filemanager/dialog.php")
            {

                var token = httpContext.Request.Query["token"].ToString();
                if (string.IsNullOrEmpty(token) || token != "bizmac")
                {
                    await Task.Run(async () =>
                    {
                        httpContext.Request.Path = "/error/";
                        //string html = "<h1>You do not have permission to access this resource.</h1>";
                        //httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        //await httpContext.Response.WriteAsync(html);
                        await _next(httpContext);
                    });
                }
                else
                {
                    await _next(httpContext);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckValidFilemanagerPopup>();
        }
    }
}
