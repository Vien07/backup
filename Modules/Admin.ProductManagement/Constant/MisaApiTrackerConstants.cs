using Steam.Core.Base.Constant;

namespace Admin.ProductManagement.Constants
{
    public static class MisaApiTrackerConstants
    {
        public static string PageSize = "PageSize";


        public static string AppID = "AppID";
        public static string SecretKey = "SecretKey";
        public static string Domain = "Domain";
        public static string BaseUrl = "BaseUrl";


        public static string SignatureInfo = "SignatureInfo";
        public static string LoginTime = "LoginTime";
        public static string AccessToken = "AccessToken";
        public static string CompanyCode = "CompanyCode";
        public static string Environment = "Environment";

        public static class ModuleInfo
        {
            public static string ModuleName = "MisaProduct";
            public static string PathCommand = SystemInfo.VirtualFolder+"/_content/Admin.ProductManagement/";
        }

        public static class ConfigPartial
        {
            #region index
            public const string ViewPath_Index = "/Views/MisaApiTracker/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/MisaApiTracker/Edit";
            public const string Edit = "/Views/MisaApiTracker/Edit.cshtml";

            #endregion
        }

        public static class ConfigRoute
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/MisaApiTracker/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/MisaApiTracker/Edit";
        }
    }
}
