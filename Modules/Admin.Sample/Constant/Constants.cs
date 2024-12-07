
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Sample.Constants
{
    public static class SampleConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Sample";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.Sample/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/Sample";
                public static string PageEdit = SystemInfo.VirtualFolder + "/Sample/Edit";
            }
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
                const string ViewPath_Index = "/Views/Sample/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/Sample/Edit";
                public const string Edit = "/Views/Sample/Edit.cshtml";
                #endregion
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public static class ExtenalViews
            {
                public static class Category
                {
                    public const string ExtenalViewslUrl = "/Views/PostsCategoryIntegrate";
                    public const string CategorySingle = ExtenalViewslUrl + "/TabCategorySingle.cshtml";
                    public const string CategoryMulti = ExtenalViewslUrl + "/TabCategoryMulti.cshtml";
                }
                public static class SEO
                {
                    public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                    public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
                }


            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/Sample/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/Sample/img/";
            }

        }
    }
}
