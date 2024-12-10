using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Website;
namespace CMS.Services.PageServices
{
    public interface IPageServices
    {
        Task<string> GetBanner(int pid);
        string GetMeta(MetaDto data);
        string GetBanner(BannerDto data);
        Task<List<SlideDto>> GetSlide();
        Task<List<AdvertisementDto>> GetAdvertisements(long modulePid);
        Task<PopupDto> GetPopup(long modulePid);
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}
