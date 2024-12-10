using DTO.FAQ;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface IFAQ_Repository
    {
        Task<List<FAQDto>> GetHighViewList(string lang, int limit);
        Task<List<FAQDto>> GetHotList(string lang, int limit);
        Task<List<FAQDto>> GetList(string lang, int page);
        Task<Dictionary<FAQCateDto, List<FAQDto>>> GetListDict(string lang, int limitCate, int limitList);
        Task<List<FAQDto>> GetListBySlug(string lang, int page, string cate);
        Task<FAQDto> GetFAQ(string slug, string lang);
        Task<List<FAQDto>> GetRelateList(string slug, string lang, int limit);
        Task<FAQCateDto> GetCate(string slug, string lang);
        Task<List<FAQCateDto>> GetCateList(string lang);
        FAQDto GetFAQPreview();
    }
}