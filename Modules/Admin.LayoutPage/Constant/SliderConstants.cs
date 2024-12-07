
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class SliderConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Slider";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/Slider/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/Slider/Edit";
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

                public const string ApiUpdateSlider = TabName + "ApiUpdateSlider";
                public const string ApiUpdatePreviewSlider = TabName + "ApiUpdatePreviewSlider";
                public const string ApiRevertSlider = TabName + "ApiRevertSlider";
                public const string ApiPreviewPage = TabName + "ApiPreviewPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/Slider/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                public const string Index_TableTree = $"{ViewPath_Index}/_TableTree.cshtml";
                public const string Index_ModelEdit = $"{ViewPath_Index}/_ModalEdit.cshtml";

                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/Slider/Edit";
                public const string Edit = "/Views/Slider/Edit.cshtml";
                public const string Edit_ModalSliderItemEdit = $"{ViewPath_Edit}/_ModalSliderItemEdit.cshtml"; 
                public const string Edit_Table = $"{ViewPath_Edit}/_Table.cshtml"; 
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml"; 

                #endregion

                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/Slider/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/Slider/img/";
            }

        }
    }
}

