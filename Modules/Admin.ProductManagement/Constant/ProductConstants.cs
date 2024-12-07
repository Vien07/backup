using Steam.Core.Base.Constant;

namespace Admin.ProductManagement.Constants
{
    public static class ProductConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Product";
            public static string ModuleCode = "ProductPost";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.ProductManagement/";
        }

        public static class ConfigAsset
        {
            public static string Image = SystemInfo.PathFileManager + "/Product/img/";
            public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/Product/img/";

        }

        public static class ConfigRoute
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/Product";
            public static string PageEdit = SystemInfo.VirtualFolder + "/Product/Edit";
        }

        public static class ConfigAdmin
        {
            public const string TabName = "Admin_";

            public const string PageSize = TabName + "PageSize";
            public const string ThumbWidth = TabName + "ThumbWidth";
            public const string ThumbHeight = TabName + "ThumbHeight";
            public const string MaxHeight = TabName + "MaxHeight";
            public const string MaxWidth = TabName + "MaxWidth";
            public const string ProductTypes = TabName + "Config1";
        }

        public static class ConfigWebsite
        {
            public const string TabName = "Website_";
            public const string PreSlug = TabName + "PreSlug";
            public const string PageSize = TabName + "PageSize";
        }

        public static class ConfigPartial
        {
            #region index
            public const string ViewPath_Index = "/Views/Product/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            public const string Index_TableProductChildren = $"{ViewPath_Index}/_TableProductChildren.cshtml";
            public const string Index_ModalSyncMisa = $"{ViewPath_Index}/_ModalSyncMisa.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/Product/Edit";
            public const string Edit = "/Views/MisaApiTracker/Edit.cshtml";
            public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
            public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
            public const string Edit_TabProductDetail = $"{ViewPath_Edit}/_TabProductDetail.cshtml";
            public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
            public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
            public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
            public const string Edit_TabPost = $"{ViewPath_Edit}/_TabPost.cshtml";
            public const string Edit_TabPostTable = $"{ViewPath_Edit}/_TabPostTable.cshtml";
            #endregion



            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
        }

        public static class ConfigCategory
        {
            public const string ExtenalViewslUrl = "/Views/ProductCategoryIntegrate";
            public const string CategorySingle = ExtenalViewslUrl + "/TabCategorySingle.cshtml";
            public const string CategoryMulti = ExtenalViewslUrl + "/TabCategoryMulti.cshtml";
        }

        public static class ConfigSEO
        {
            public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
            public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
        }
        public class StaticPath
        {

            public static class Asset
            {
                public static string Image = SystemInfo.PathFileManager + "/Product/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/Product/img/_thumb/";
            }

        }
    }
}
