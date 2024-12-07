
using Steam.Core.Base.Constant;

namespace MasterAdmin.Builder
{


    public static class ResponsiveFileManagerBuilder
    {

        public static void AddResponsiveFileManager4Steams(this IServiceCollection services)
        {

            //services.AddResponsiveFileManager(options =>
            //{
            //    //
            //    options.UploadDirectory = SystemInfo.PathFileManager;// "/FileStorage/Storage/";
            //    options.CurrentPath = ".." + SystemInfo.PathFileManager; ;/// FileStorage/Storage/";
            //    options.ThumbsBasePath = ".." + SystemInfo.ThumbsBasePathFileManager;// / FileStorage/thumbs/";
            //    options.MaxSizeUpload = 1024;
            //});
        }

        public static void UseResponsiveFileManager4Steams(this IApplicationBuilder app)
        {
            //app.UseResponsiveFileManager();

        }
    }
    
}