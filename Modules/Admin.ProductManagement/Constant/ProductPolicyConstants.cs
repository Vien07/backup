
using Steam.Core.Base.Constant;


namespace Admin.ProductManagement.Constants
{
    public static class ProductPolicyConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "ProductPolicy";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.ProductManagement/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/ProductPolicy/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/ProductPolicy/Edit";
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
                public const string GroupPolicy = TabName + "GroupPolicy";
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
                public const string ViewPath_Index = "/Views/ProductPolicy/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/ProductPolicy/Edit";
                public const string Edit = "/Views/ProductPolicy/Edit.cshtml";

                #endregion


                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/ProductPolicy/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/ProductPolicy/img/_thumb/";
            }

        }
    }
}
