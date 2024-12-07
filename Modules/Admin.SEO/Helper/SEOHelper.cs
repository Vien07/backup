
namespace Admin.SEO
{
    public class SEOHelper
    {
        public SEOHelper()
        {
        }
        //public Response<string> GenerateMetaTag(MetaModel data)
        //{
        //    Response<string> rs = new Response<string>();

        //    try
        //    {
        //        string _websiteName = "";
        //        string _rootDomain = "";
        //        string _des = "";
        //        string _keyw ="";
        //        if (data.Description == "" || data.Description == null)
        //        {
        //            data.Description = _des;
        //        }
        //        if (data.Keywords == "" || data.Keywords == null)
        //        {
        //            data.Keywords = _keyw;

        //        }
        //        data.SiteName = _websiteName; // WebsiteName;
        //        data.SiteTitle = _websiteName; // WebsiteName;
        //        string _host = _rootDomain;// RootDomain; // _httpContextAccessor.HttpContext.Request.Host.Host.ToString();
        //        data.HomepageUrl = _host;
        //        var absolutepath = Directory.GetCurrentDirectory();
        //        var metaTag = "";
        //        var keywords = "";
        //        if (data.Keywords != null && data.Keywords != "")
        //        {
        //            keywords = "\n<meta name=\"keywords\" content=\"" + data.Keywords.Trim() + "\" />";
        //        }
        //        if (data.Keywords != null && data.Keywords != "")
        //        {
        //            var tagkeyArr = data.Keywords.Split(',');


        //            foreach (var item in tagkeyArr)
        //            {
        //                metaTag += "\n<meta property=\"article:tag\" content=\"" + item.Trim() + "\"/>";
        //            }
        //        }
        //        var template = Path.Combine(absolutepath + SEOConstants.StaticPath.Meta.MetaFile);
        //        if (File.Exists(template))
        //        {
        //            StreamReader sr = new StreamReader(template);
        //            string content = sr.ReadToEnd(); sr.Close();
        //            content = content.Replace("{{pageTitle}}", data.PageTitle);
        //            content = content.Replace("{{description}}", data.Description);
        //            //content = content.Replace("{{keywords}}", data.Keywords);
        //            if (String.IsNullOrEmpty(data.PageUrl))
        //            {
        //                content = content.Replace("{{pageUrl}}", _host);

        //            }
        //            else
        //            {
        //                content = content.Replace("{{pageUrl}}", /*_host+*/ data.PageUrl);

        //            }
        //            if (String.IsNullOrEmpty(data.ImageUrl))
        //            {
        //                content = content.Replace("{{imageUrl}}", _host + "UrlConfigurationImages" + "DefaultOgImage");
        //            }
        //            else
        //            {
        //                content = content.Replace("{{imageUrl}}", _host + data.ImageUrl);
        //            }
        //            content = content.Replace("{{homepageUrl}}", data.HomepageUrl);
        //            content = content.Replace("{{siteName}}", data.SiteName);
        //            content = content.Replace("{{siteTitle}}", data.SiteTitle);
        //            content = content.Replace("{{meta-tag}}", metaTag);
        //            content = content.Replace("{{meta-keywords}}", keywords);
        //            content = content.Replace("{{ogType}}", data.OgType);
        //            var bodyBuilder = new BodyBuilder();
        //            bodyBuilder.HtmlBody = content;
        //            //content = content.Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");

        //            rs.Data = content;
        //        }
        //        return rs;

        //    }
        //    catch (Exception ex)
        //    {
        //        rs.Message = ex.Message;
        //        rs.isError = true;
        //        return rs;
        //    }
        //}


        //public class MetaModel
        //{
        //    public string PageTitle { get; set; } /*= WebsiteName;*/
        //    public string Description { get; set; } /*= MetaDescription;*/
        //    public string Keywords { get; set; } /*= MetaKeywords;*/
        //    public string PageUrl { get; set; }
        //    public string OgType { get; set; }
        //    public string ImageUrl { get; set; }
        //    public string HomepageUrl { get; set; } /*= RootDomain;*/
        //    public string SiteName { get; set; }/* = WebsiteName;*/
        //    public string SiteTitle { get; set; } /*= WebsiteName;*/
        //    public string ExtraMeta { get; set; } /*= WebsiteName;*/

        //    public MetaModel SetMeta(string PageTitle, string Description,
        //                              string Keywords, string PageUrl,
        //                              string ImageUrl, string HomepageUrl, string SiteName)
        //    {
        //        MetaModel meta = new MetaModel();
        //        meta.PageTitle = PageTitle;
        //        meta.Description = Description;
        //        meta.Keywords = Keywords;
        //        meta.PageUrl = PageUrl;
        //        meta.HomepageUrl = HomepageUrl;
        //        meta.ImageUrl = ImageUrl;
        //        meta.SiteName = SiteName;
        //        return meta;
        //    }
        //}
        //void SaveFileHeader(string content)
        //{
        //    string fileName = "footer.html"; // Replace with the name of the file you want to update

        //    var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
        //    var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

        //    if (File.Exists(filePath))
        //    {

        //        File.WriteAllText(filePath, content);

        //    }

        //}

    }
}
