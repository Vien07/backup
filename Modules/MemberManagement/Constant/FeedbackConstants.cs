using Microsoft.AspNetCore.Http;
using  Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.MemberManagement.Constants
{
    public static class FeedbackConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "MemberManagement";
            public static string ModuleCode = "MemberManagement";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.MemberManagement/commands/";

        }
        public static class Config
        {
            public static class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/Feedback";
                public static string PageEdit = SystemInfo.VirtualFolder + "/Feedback/Edit";
            }
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
            }

        }
        public static class StaticPath
        {

            public static class PartialView
            {

                public const string ViewPath = "/Views/Feedback";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table =   PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_TabSEO = PartialTabsPath +"/Edit"+"/_TabSEO.cshtml";
                public const string Edit_TabCategory = PartialTabsPath + "/Edit" + "/_TabCategory.cshtml";
                public const string Edit_TabSetting = PartialTabsPath + "/Edit" + "/_TabSetting.cshtml";
                public const string Edit_TabImages = PartialTabsPath + "/Edit" + "/_TabImages.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
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
                public static string Image = SystemInfo.PathFileManager+"/Feedback/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/Feedback/img/_thumb/";
            }

        }
    }
}
