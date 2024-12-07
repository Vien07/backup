
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.ProductManagement.Constants
{
    public static class ProductCategoryConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "ProductCategory";
            public static string ModuleCode = "ProductCategory";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.ProductManagement/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/ProductCategory/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/ProductCategory/Edit";
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
                public const string PreSlugCate = TabName + "PreSlugCate";
                public const string PreSlugDetail = TabName + "PreSlugDetail";
                public const string Parameter = TabName + "Parameter";
                public const string CatePage = TabName + "CatePage";
                public const string DetailPage = TabName + "DetailPage";
                public const string ApiUpdateRewriteProductUrl = TabName + "ApiUpdateRewriteProductUrl";

            }

        }
        public class StaticPath
        {

            //public class PartialView
            //{

            //    public const string _PartialUrl = "/Views/MisaProductCategory/_Partial/";
            //    public const string _PartialModel = "_PartialModal.cshtml";
            //    public const string _PartialTable = "_PartialTable.cshtml";
            //    public const string _PartialModalConfig = "_PartialModalConfig.cshtml";
            //    public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            //}
            public static class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/ProductCategory/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/ProductCategory/Edit";
                public const string Edit = "/Views/ProductCategory/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";


                #endregion



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
                public static string Image = SystemInfo.PathFileManager + "/ProductCategory/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/ProductCategory/img/";
            }

        }
    }
}
