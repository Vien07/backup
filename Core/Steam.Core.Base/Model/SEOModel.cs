
namespace Steam.Core.Base.Models
{
    public class SEOModel
    {
        public string NavListButton { get; set; }
        public string GetCatePidFrom { get; set; }
        public string GetPostPidFrom { get; set; }
        public string GetPostTitlteFrom { get; set; }
        public string MetaDescriptionGenerateFrom { get; set; } = "";
        public string Slug { get; set; }
        public string GetOgImageFrom { get; set; } = "OgImage";
        public string OgImage { get; set; } = "";
        public string? ModuleCode { get; set; } = "";
        public string SlugGenerateFrom { get; set; }
        public string MetaDescription { get; set; }
        public string PostTitle { get; set; }
        public string TagKeys { get; set; }
        public string Meta { get; set; }
        public string ExtraMeta { get; set; }
        public string Image { get; set; }
        public long PostPid { get; set; }
        public long SeoPid { get; set; }
        public bool Display { get; set; } = true; //set false is display vertically
    }
}
