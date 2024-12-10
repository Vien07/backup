using DTO.Gallery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface IGallery_Repository
    {
        Task<List<GalleryDto>> GetHighViewList(string lang, int limit);
        Task<List<GalleryDto>> GetHotList(string lang, int limit);
        Task<List<GalleryDto>> GetList(string lang, int page);
        Task<Dictionary<GalleryCateDto, List<GalleryDto>>> GetListDict(string lang, int limitCate, int limitList);
        Task<List<GalleryDto>> GetListBySlug(string lang, int page, string cate);
        Task<GalleryDto> GetGallery(string slug, string lang);
        Task<List<GalleryDto>> GetRelateList(string slug, string lang, int limit);
        Task<GalleryCateDto> GetCate(string slug, string lang);
        Task<List<GalleryCateDto>> GetCateList(string lang);
        GalleryDto GetGalleryPreview();
    }
}