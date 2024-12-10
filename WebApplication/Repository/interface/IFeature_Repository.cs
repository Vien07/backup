using DTO.Feature;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface IFeature_Repository
    {
        Task<List<FeatureDto>> GetHighViewList(string lang, int limit);
        Task<List<FeatureDto>> GetHotList(string lang, int limit);
        Task<List<FeatureDto>> GetList(string lang, int page);
        Task<Dictionary<FeatureCateDto, List<FeatureDto>>> GetListDict(string lang, int limitCate, int limitList);
        Task<List<FeatureDto>> GetListBySlug(string lang, int page, string cate);
        Task<FeatureDto> GetFeature(string slug, string lang);
        Task<List<FeatureDto>> GetRelateList(string slug, string lang, int limit);
        Task<FeatureCateDto> GetCate(string slug, string lang);
        Task<List<FeatureCateDto>> GetCateList(string lang);
        FeatureDto GetFeaturePreview();
        Task<List<FeatureDto>> GetFeatureListForForm(string lang);
        Task<string> GetSlugDefault(string lang);
    }
}