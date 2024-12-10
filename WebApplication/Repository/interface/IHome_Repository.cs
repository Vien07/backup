using DTO.About;
using DTO.Comment;
using DTO.FAQ;
using DTO.Gallery;
using DTO.News;
using DTO.Convenience;
using DTO.Product;
using DTO.Feature;
using DTO.Website;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface IHome_Repository
    {
        Task<List<AboutDto>> GetAboutFooterList(string lang);
        Task<List<AboutDto>> GetAboutMenuList(string lang);
        Task<List<ProductDto>> GetProductList(string lang, int limit);
        Task<List<NewsDto>> GetNewsList(string lang, int limit);
        Task<List<FeatureDto>> GetServiceListForForm(string lang);
        Task<List<HomePageDto>> GetHomePage(string type, string lang);
        List<NewsCateDto> GetNewsCateList(string lang);
        List<GalleryCateDto> GetGalleryCateList(string lang);
        List<ProductCateDto> GetProductCateList(string lang);
        Task<List<RecruitmentCateDto>> GetRecruitmentCateList(string lang);
        Task<List<ConvenienceDto>> GetConvenienceList(string lang);
        Task<List<CommentDto>> GetCommentList(string lang);
        //CustomerCookieModel GetCustomerByCookie();
        Task<List<FAQDto>> GetFAQList(string lang, int limit);
        Task<List<GalleryDto>> GetGalleryList(string lang, int limit);
        Task<List<FeatureDto>> GetServiceList(string lang, int limit);
        
    }
}