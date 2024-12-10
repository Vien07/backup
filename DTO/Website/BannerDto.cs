using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Website
{
    public class BannerDto
    {
        public string Home { get; set; } /*= WebsiteName;*/
        public string HomeUrl { get; set; } /*= MetaDescription;*/
        public string Banner { get; set; } /*= MetaKeywords;*/
        public string CurrentPage { get; set; } /*= MetaKeywords;*/

        public BannerDto SetBanner(string Home, string HomeUrl,
                                  string Banner, string CurrentPage)
        {
            BannerDto banner = new BannerDto();
            banner.CurrentPage = CurrentPage;
            banner.Home = Home;
            banner.HomeUrl = HomeUrl;
            banner.Banner = Banner;
            return banner;
        }
    }
}
