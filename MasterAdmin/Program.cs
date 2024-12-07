using Admin.PostsCategory;
using Admin.SEO;
using Admin.WebSetting;

//using Admin.Authorization;
using Admin.Sample;

using Admin.PostsManagement;
using MasterAdmin;
using Admin.Authorization;
using MasterAdmin.Builder;
using Steam.Core.Base.Models;
using Steam.Core.Base.Constant;
using System.Media;
using Admin.MemberManagement;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MasterAdmin.Repository;
using Steam.Core.Base;
using Microsoft.AspNetCore.Builder;
using Admin.ProductManagement;
using Admin.Sample.Database;
using Admin.DashBoard.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
//string CDNHOST = configuration["SystemConfig:ThemesUrl"].ToString();
// Add services to the container.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddModules4Steams();

builder.Services.AddScoped<IApiWebsiteRepository, ApiWebsiteRepository>();
builder.Services.AddTransient<MultiLangService>();
builder.Services.AddScoped<IDashBoardRepository, DashBoardRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication();
builder.Services.AddSwagger4Steam();
builder.Services.AddDbContext<DashBoardContext>(
            options => options.UseSqlServer(SystemInfo.DefaultConnectionDatabase),
            ServiceLifetime.Transient
            );
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

//

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

//builder.Services.AddResponsiveFileManager4Steams();
builder.Services.AddResponsiveFileManager(options =>
{
    //
    options.UploadDirectory = "/FileStorage/Storage/";
    options.CurrentPath = "../FileStorage/Storage/";
    options.ThumbsBasePath = "../FileStorage/thumbs/";
    options.MaxSizeUpload = 32;

});
builder.Services.AddScoped<IIdentityService, IdentityService>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHsts();



//app.UseMiddleware<AuthorizeMiddleware>();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
//app.UseXFrameOptionsMiddleware("https://localhost:51846");

app.UseDefaultFiles();
app.UseStaticFiles(); // shortcut for HostEnvironment.WebRootFileProvider
app.UseRouting();


//app.UseCors("AllowAllOrigins");

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
  name: "default",
    pattern: "{controller=Sample}/{action=Index}/{id?}");
});

//app.UseResponsiveFileManager4Steams();
app.UseResponsiveFileManager();


app.UseAddModules4Steams();
app.UseSwagger4Steam();

app.Run();

