using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsManagement.Constants
{
    public static class PostsManagementConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "PostsManagement";
            public static string ModuleCode = "PostsManagement";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.PostsManagement/commands/";

        }
        public static class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/PostsManagement/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/PostsManagement/Edit";
        }
        public static class Config
        {

            public static class Admin
            {
                public const string TabName = "Admin_";
                public const string PageSize = TabName + "PageSize";
                public const string ThumbWidth = TabName + "ThumbWidth";
                public const string ThumbHeight = TabName + "ThumbHeight";
                public const string MaxHeight = TabName + "MaxHeight";
                public const string MaxWidth = TabName + "MaxWidth";
                public const string PostType = TabName + "PostType";
            }
            public static class Website
            {
                public const string TabName = "Website_";
                public const string PreSlug = TabName + "PreSlug";
                public const string PageSize = TabName + "PageSize";
            }

        }
        public static class StaticPath
        {

            public static class PartialView
            {
                #region Index
                public const string ViewPath_Index = "/Views/PostsManagement/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/PostsManagement/Edit";
                public const string Edit = "/Views/PostsManagement/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                public const string Edit_TabContent_Policy = $"{ViewPath_Edit}/_TabContent_Policy.cshtml";
                public const string Edit_ModalPickLanguage = $"{ViewPath_Edit}/_ModalPickLanguage.cshtml";

                #endregion
                //public const string _PartialPageTitle = $"{ViewPath_Edit}/_PageTitle.cshtml";
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
            public static class Asset
            {
                public static string Image = SystemInfo.PathFileManager + "/PostsManagement/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/PostsManagement/img/";
            }

        }
    }
}
