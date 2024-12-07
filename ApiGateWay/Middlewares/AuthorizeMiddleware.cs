
using Newtonsoft.Json;
using Steam.Core.Base.Constant;

namespace ApiGateWay.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                string Path = httpContext.Request.Path.ToString();
                if (Path != null)
                {
                    //check public api
                    if (httpContext.Request.Path.ToString().Contains("/api/") ||httpContext.Request.Path.ToString().Contains("/swagger/"))
                    {
                        await _next(httpContext);
                        return;
                    }
                    else
                    {
                        var jsonResponse = new
                        {
                            message = "No permission to access this resource",
                            status = 403 
                        };

                        httpContext.Response.ContentType = "application/json";

                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(jsonResponse));

                        //await _next(httpContext);
                        return;
                    }
  
                }
                else
                {
          

                }
            }
            catch (Exception ex)
            {

            }
            await _next(httpContext);
        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<AuthorizeMiddleware>();
        }
    }
}
