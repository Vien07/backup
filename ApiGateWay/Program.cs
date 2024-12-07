using Admin.Course.Database;

using Admin.WebSetting;

using ApiGateWay.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Utilities;
using Admin.SEO;
using ApiGateWay.Services;
using ApiGateWay.Middlewares;
using Admin.PostsCategory.Database;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using Admin.WebSetting.Database;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddXmlSerializerFormatters()
        .AddXmlDataContractSerializerFormatters();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddScoped<IRepository<WebsiteConfiguration>>(provider =>
 new Repository<WebsiteConfiguration>(
     provider.GetRequiredService<WebsiteConfigurationContext>(),
     provider.GetRequiredService<ILoggerHelper>(),
     provider.GetRequiredService<IFileHelper>()
 ));

builder.Services.AddScoped<IRepositoryConfig<WebsiteConfiguration>>(provider =>
    new RepositoryConfig<WebsiteConfiguration>(provider.GetRequiredService<WebsiteConfigurationContext>()));
builder.Services.AddAPI4Steams();

//builder.Services.AddScoped<IWebSettingRepository, WebSettingRepository>();
builder.Services.AddScoped<IApiWebsiteService, ApiWebsiteService>();
string DefaultConnection = DefaultConnectionDatabase;
builder.Services.AddDbContext<WebsiteConfigurationContext>(options => options.UseSqlServer(DefaultConnection), ServiceLifetime.Transient);
//builder.Services.AddModuleCommon();
builder.Services.AddSwagger4Steam();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<AuthorizeMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger4Steam();

//app.MapControllers();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger4Steam();
//}//app.UseRoutePostsManagement();
app.UseRouting();
app.UseAPI4Steams();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//  name: "AdminWebsetting",
//  pattern: "{controller=WebSetting}/{action=Index}/{id?}");
//});

app.Run();
