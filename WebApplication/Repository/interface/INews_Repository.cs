using DTO.Gallery;
using DTO.News;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface INews_Repository
    {
        Task<List<NewsDto>> GetHighViewList(string lang, int limit);
        Task<List<NewsDto>> GetHotList(string lang, int limit);
        Task<List<NewsDto>> GetList(string lang, int page);
        Task<Dictionary<NewsCateDto, List<NewsDto>>> GetListDict(string lang, int limitCate, int limitList);
        Task<List<NewsDto>> GetListBySlug(string lang, int page, string cate);
        Task<NewsDto> GetNews(string slug, string lang);
        Task<List<NewsDto>> GetRelateList(string slug, string lang, int limit);
        Task<NewsCateDto> GetCate(string slug, string lang);
        Task<List<NewsCateDto>> GetCateList(string lang);
        NewsDto GetNewsPreview();
        Task<List<GalleryDto>> GetGalleryList(string lang, int limit);
    }
}