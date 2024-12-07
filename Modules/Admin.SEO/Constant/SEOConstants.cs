
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.SEO.Constants
{

    public static class SEOConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "SEO";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.SEO/commands/";
            public static string PathMeta = "/wwwroot/meta/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/SEO/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/SEO/Edit";
        }
        public class Config
        {

            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
                public const string ThumbWidth = TabName + "ThumbWidth";
                public const string ThumbHeight = TabName + "ThumbHeight";
                public const string MaxHeight = TabName + "MaxHeight";
                public const string MaxWidth = TabName + "MaxWidth";
                public const string Config1 = TabName + "Config1";
                public const string Modules = TabName + "Modules";
            }
            public class Website
            {
                public const string TabName = "Website_";

                public const string PreSlug = TabName + "PreSlug";
                public const string PageSize = TabName + "PageSize";
            }

        }
        public class StaticPath
        {
            public class Meta
            {
                public static string MetaFile = ModuleInfo.PathMeta+ "meta.html";
            }
            public class PartialView
            {
                #region Index
                public const string ViewPath_Index = "/Views/SEO/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion


                public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
            }
            public class Asset
            {
                public static string Image = SystemInfo.PathFileManager + "/SEO/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/SEO/img/";

            }

        }
    }
}
