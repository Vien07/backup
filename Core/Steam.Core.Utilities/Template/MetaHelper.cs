using MimeKit;
using Steam.Core.Base.Constant;
using Steam.Core.Utilities.STeamHelper;

namespace Steam.Core.Utilities.STeamHelper
{
    public class MetaHelper : IMetaHelper
    {
        ILoggerHelper _logger;
        public MetaHelper(ILoggerHelper logger)
        {
            _logger = logger;
        }
        public string GenerateMetaTag(MetaModel data)
        {

            try
            {
                var absolutepath = Directory.GetCurrentDirectory();

                var template = Path.Combine(absolutepath + SystemInfo.MetaFile);

                string _websiteName = "";
                string _rootDomain =SystemInfo.MedidaFileServer;
                string _des = "";
                string _keyw = "";
                if (data.Description == "" || data.Description == null)
                {
                    data.Description = _des;
                }
                if (data.Keywords == "" || data.Keywords == null)
                {
                    data.Keywords = _keyw;

                }
                data.SiteName = _websiteName; // WebsiteName;
                data.SiteTitle = _websiteName; // WebsiteName;
                string _host = _rootDomain;// RootDomain; // _httpContextAccessor.HttpContext.Request.Host.Host.ToString();
                var metaTag = "";
                var keywords = "";
                if (data.Keywords != null && data.Keywords != "")
                {
                    keywords = "\n<meta name=\"keywords\" content=\"" + data.Keywords.Trim() + "\" />";
                }
                if (data.Keywords != null && data.Keywords != "")
                {
                    var tagkeyArr = data.Keywords.Split(',');


                    foreach (var item in tagkeyArr)
                    {
                        metaTag += "\n<meta property=\"article:tag\" content=\"" + item.Trim() + "\"/>";
                    }
                }
                if (File.Exists(template))
                {
                    StreamReader sr = new StreamReader(template);
                    string content = sr.ReadToEnd(); sr.Close();
                    content = content.Replace("{{pageTitle}}", data.PageTitle);
                    content = content.Replace("{{description}}", data.Description);
                    //content = content.Replace("{{keywords}}", data.Keywords);
                    content = content.Replace("{{pageUrl}}", /*_host+*/ data.PageUrl);

                    if (String.IsNullOrEmpty(data.ImageUrl))
                    {
                        content = content.Replace("{{imageUrl}}", _host + data.ImageUrl);
                    }
                    else
                    {
                        content = content.Replace("{{imageUrl}}", _host + data.ImageUrl);
                    }
                    content = content.Replace("{{homepageUrl}}", data.HomepageUrl);
                    content = content.Replace("{{siteName}}", data.SiteName);
                    content = content.Replace("{{siteTitle}}", data.SiteTitle);
                    content = content.Replace("{{meta-tag}}", metaTag);
                    content = content.Replace("{{meta-keywords}}", keywords);
                    content = content.Replace("{{ogType}}", data.OgType);
                    content = content.Replace("{{ogimage}}", _host + data.OgImage);
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = content;
                    //content = content.Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");
                    return content;

                }
                return "";

            }
            catch (Exception ex)
            {
        
                return "";
            }
        }



    }
}
