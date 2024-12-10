using Microsoft.AspNetCore.Mvc;
using CMS.Services.WebsiteServices;
using DTO.Website;
using CMS.Services.PageServices;
using DTO;
using CMS.Services.CommonServices;

namespace CMS.Controllers
{
    public class ErrorController : Controller
    {
        private string WebsiteName = "";
        private readonly IWebsiteServices _website;
        private readonly IPageServices _page;
        private readonly ICommonServices _common;
        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        public ErrorController(IWebsiteServices website, ICommonServices common, IPageServices page)
        {
            _common = common;
            _website = website;
            _page = page;
        }
        public IActionResult Index(string code)
        {
            WebsiteName = _common.GetConfigValue(KeyWebsiteName);
            MetaDto _meta = new MetaDto();
            _meta.PageTitle = "Page Not Found | " + WebsiteName;
            _meta.Is404Page = true;
            ViewBag.Meta = _page.GetMeta(_meta);
            return View();
        }
    }
}