namespace ComponentUILibrary.Models
{
    public class PageTitleModel
    {
        public string ModuleName { get; set; }
        public string View { get; set; }
        public string ICon { get; set; } 
        public string BackLink { get; set; }
        public PageTitleModel(string moduleName,string view ,string icon, string backlink)
        {
            ModuleName = moduleName;
            View = view;
            ICon = icon;
            BackLink = backlink;

        }
    }
}