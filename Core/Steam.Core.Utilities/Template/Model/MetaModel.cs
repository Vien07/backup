using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public class MetaModel
    {
        public string PageTitle { get; set; } /*= WebsiteName;*/
        public string Description { get; set; } /*= MetaDescription;*/
        public string Keywords { get; set; } /*= MetaKeywords;*/
        public string PageUrl { get; set; }
        public string OgType { get; set; }
        public string ImageUrl { get; set; }
        public string OgImage { get; set; }
        public string HomepageUrl { get; set; } /*= RootDomain;*/
        public string SiteName { get; set; }/* = WebsiteName;*/
        public string SiteTitle { get; set; } /*= WebsiteName;*/
        public string ExtraMeta { get; set; } /*= WebsiteName;*/

        public MetaModel SetMeta(string PageTitle, string Description,
                                  string Keywords, string PageUrl,
                                  string ImageUrl, string HomepageUrl, string SiteName)
        {
            MetaModel meta = new MetaModel();
            meta.PageTitle = PageTitle;
            meta.Description = Description;
            meta.Keywords = Keywords;
            meta.PageUrl = PageUrl;
            meta.HomepageUrl = HomepageUrl;
            meta.ImageUrl = ImageUrl;
            meta.SiteName = SiteName;
            return meta;
        }
    }
}
