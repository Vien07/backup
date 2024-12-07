using Microsoft.AspNetCore.Http;
using  Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Collection.Constants
{
    public static class CollectionConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Collection";
            public static string ModuleCode = "Collection";
            public static string PathCommand = SystemInfo.VirtualFolder+ "/_content/Admin.Collection/commands/";

        }
        public static class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/Collection/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/Collection/Edit";
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
                public const string Config1 = TabName + "Config1";
            }
            public static class Website
            {
                public const string TabName = "Website_";

                public const string PreSlug = TabName + "PreSlug";
                public const string PageSize = TabName + "PageSize";

                public const string ApiUpdateHeader = TabName + "ApiUpdateHeader";
                public const string ApiRevertHeader = TabName + "ApiRevertHeader";
            }

        }
        public static class StaticPath
        {

            public static class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/Collection/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/Collection/Edit";
                public const string Edit = "/Views/Collection/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                public const string Edit_TableColleciton = $"{ViewPath_Edit}/_TableCollection.cshtml";
                public const string Edit_RowTableColleciton = $"{ViewPath_Edit}/_RowTableCollection.cshtml";
                public const string Edit_TableChooseProduct = $"{ViewPath_Edit}/_TableChooseProduct.cshtml";
                public const string Edit_TabCollection = $"{ViewPath_Edit}/_TabCollection.cshtml";
                public const string Edit_ModalListProducts = $"{ViewPath_Edit}/_ModalListProducts.cshtml";
                public const string Edit_ModalChooseProducts = $"{ViewPath_Edit}/_ModalChooseProduct.cshtml";
                #endregion




                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public static class ExtenalViews
            {
                public static class Category
                {
                    public const string ExtenalViewslUrl = "/Views/PostsCategoryIntegrate";
                    public const string CategorySingle = ExtenalViewslUrl +"/TabCategorySingle.cshtml";
                    public const string CategoryMulti = ExtenalViewslUrl+ "/TabCategoryMulti.cshtml";
                }
                public static class SEO
                {
                    public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                    public const string TabSEO = ExtenalViewslUrl+ "/TabSEO.cshtml";
                }


            }
            public static class Asset
            {
                public static string Image = SystemInfo.PathFileManager+"/Collection/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/Collection/img/";
            }

        }
    }
}
