using Steam.Core.Base.Constant;

namespace Admin.ProductManagement.Constants
{
    public static class ProductSpecificatyConstants
    {
        public static string PageSize = "PageSize";

        public static class ModuleInfo
        {
            public static string ModuleName = "MisaProduct";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.ProductManagement/";
        }

        public static class ConfigPartial
        {
            #region index
            public const string ViewPath_Index = "/Views/ProductSpecificaty/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/ProductSpecificaty/Edit";
            public const string Edit = "/Views/ProductSpecificaty/Edit.cshtml";

            #endregion

        }

        public static class ConfigRoute
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/ProductSpecificaty/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/ProductSpecificaty/Edit";
        }
    }
}
