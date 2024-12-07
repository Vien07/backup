
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaskShedulerManagement.Constants
{
    public static class TaskScheduleConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "TaskShedulerManagement";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.TaskShedulerManagement/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/TaskSchedule";
            public static string PageEdit = SystemInfo.VirtualFolder + "/TaskSchedule/Edit";
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
                #region Index
                public const string ViewPath_Index = "/Views/TaskSchedule/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/TaskSchedule/Edit";
                public const string Edit = "/Views/TaskSchedule/Edit.cshtml";
                public const string Edit_TabSEO = $"{ViewPath_Edit}/_TabSEO.cshtml";
                public const string Edit_TabCategory = $"{ViewPath_Edit}/_TabCategory.cshtml";
                public const string Edit_TabSetting = $"{ViewPath_Edit}/_TabSetting.cshtml";
                public const string Edit_TabImages = $"{ViewPath_Edit}/_TabImages.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                public const string Edit_TabContent_Policy = $"{ViewPath_Edit}/_TabContent_Policy.cshtml";
                #endregion
            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/TaskShedulerManagement/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/TaskShedulerManagement/img/";
            }

        }
    }
}
