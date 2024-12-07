
using Steam.Core.Base.Constant;


namespace Admin.TemplatePage.Constants
{
    public static class TemplatePageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "TemplatePage";
            public static string ModuleCode = "TemplatePage";

            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.TemplatePage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/TemplatePage/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/TemplatePage/Edit";
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

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/TemplatePage/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/TemplatePage/Edit";
                public const string Edit = "/Views/TemplatePage/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                #endregion

                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";


            }
            public static class ExtenalViews
            {
                public static class SEO
                {
                    public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                    public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
                }


            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/TemplatePage/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/TemplatePage/img/";
            }

        }
    }
}
