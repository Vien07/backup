
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace ApiGateWay.Builder
{


    public static class SwaggerBuilder
    {

        public static void AddSwagger4Steam(this IServiceCollection services)
        {
           
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<PublicApiFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API For Website", Version = "v1" });

            });

        }

        public static void UseSwagger4Steam(this IApplicationBuilder app)
        {


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }
    }
    public class PublicApiFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {


        void Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var routesToKeep = swaggerDoc.Paths
                     .Where(pathItem => pathItem.Key.Contains("/api/")) // Customize your filtering logic here
                     .ToList();

            foreach (var path in swaggerDoc.Paths.Keys.ToList())
            {
                var checkRoute = routesToKeep.Where(p => p.Key == path).FirstOrDefault();
                if (checkRoute.Key == null)
                {
                    swaggerDoc.Paths.Remove(path);

                }

            }
        }
    }
}