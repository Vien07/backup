using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary
{
    public static class Extensions
    {
        public static void AddComponentLibraryViews(this IServiceCollection services)
        {
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration =
            new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
            services.AddTransient<IViewComponentHelper, DefaultViewComponentHelper>();

            services.Configure<List<MultilangModel>>(configuration.GetSection("Multilang"));
            services.AddOptions();
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.NavLangComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.TableComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.PaginationComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.Select2Component)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.NiceSelectComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.EditorTinymceComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.FileUploadFilePondComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.RadioComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.RadioButtonComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.DropdownFilterMultiSelectComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.DropzoneComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.DatePickerComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.PageTitleComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.TabSEOComponent)
                    .GetTypeInfo().Assembly));
            });      
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.ModalInfoImageComponent)
                    .GetTypeInfo().Assembly));
            });  
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.TableTreeComponent)
                    .GetTypeInfo().Assembly));
            });      
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.TreeSelectComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.FromDateToDateComponent)
                    .GetTypeInfo().Assembly));
            });
            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.ConfigModalComponent)
                    .GetTypeInfo().Assembly));
            });
        }

        public static void UseComponentLibraryScripts(this IApplicationBuilder builder)
        {
            var embeddedProvider = new EmbeddedFileProvider(typeof(ComponentUILibrary.ViewComponents.NavLangComponent)
                .GetTypeInfo().Assembly, "ComponentUILibrary.Scripts");

            builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = embeddedProvider,
                RequestPath = new PathString("/Scripts")
            });
        }
    }
}
