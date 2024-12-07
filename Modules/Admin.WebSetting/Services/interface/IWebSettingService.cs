using Admin.WebSetting.Models;
using Steam.Core.Base.Models;

namespace Admin.WebSetting.Services
{
    public interface IWebSettingService
    {
        Response Save(WebSettingViewModelModelEdit model);
        Response<Dictionary<string, string>> GetWebSetting();
         Response<Dictionary<string, string>> GetWebSiteConfig();

    }
}