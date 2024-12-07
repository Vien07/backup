
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsCategory.Constants
{
    public static class PostsCategoryConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "PostsCategory";
            public static string ModuleCode = "PostsCategory";
            public static string PathCommand = SystemInfo.VirtualFolder+"/_content/Admin.PostsCategory/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/PostsCategory/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/PostsCategory/Edit";
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
            }
            public class Website
            {
                public const string TabName = "Website_";

                //public const string AlwaysShowTop = TabName + "AlwaysShowTop";
                public const string PreSlug = TabName + "PreSlug";
                public const string ApiUpdateRewritePostUrl = TabName + "ApiUpdateRewritePostUrl";

            }

        }
        public class StaticPath
        {

            public static class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/PostsCategory/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/PostsCategory/Edit";
                public const string Edit = "/Views/PostsCategory/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                #endregion


                public const string _PartialUrl = "/Views/PostsCategory/_Partial/";
                public const string _PartialModel = "_PartialModal.cshtml";
                public const string _PartialTable = "_PartialTable.cshtml";
                public const string _PartialModalConfig = "_PartialModalConfig.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

                public const string ViewPath = "/Views/PostsCategory";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";


                //public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public static class ExtenalViews
            {
                public static class SEO
                {
                    public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                    public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
                }


            }
            public static class Asset
            {
                public static string Image = SystemInfo.PathFileManager + "/PostsCategory/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/PostsCategory/img/";
            }

        }
    }
}
