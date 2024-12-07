using Admin.Course.Database;
using Common;
using Common.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using ComponentUILibrary;
using Microsoft.Extensions.Configuration;
using ComponentUILibrary.Models;

namespace Admin.Course
{
    public static class Extensions
    {
        public static void AddModuleAdminCourse(this IServiceCollection services)
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            string DefaultConnectionDatabase = conf["SystemConfig:DefaultConnectionDatabase"].ToString();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddModuleCommon();
            services.AddComponentLibraryViews();
            string DefaultConnection = DefaultConnectionDatabase;
            services.AddDbContext<CourseContext>(options => options.UseSqlServer(DefaultConnection), ServiceLifetime.Transient);
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Course/Edit", "/Course/Edit/{id}");
                options.Conventions.AddPageRoute("/Course/Lecture", "/Course/Lecture/{id}");
                options.Conventions.AddPageRoute("/Lecture/Edit", "/Lecture/Edit/{id}");
                options.Conventions.AddPageRoute("/Member/Edit", "/Member/Edit/{id}");
                options.Conventions.AddPageRoute("/Member/CourseManagement", "/Member/CourseManagement/{id}");
            });
        }
        public static void UseStaticFileCourse(this IApplicationBuilder app)
        {
            var absolutePath = Directory.GetCurrentDirectory();
            string path = Path.Combine(absolutePath, "wwwroot/course/img");

            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
