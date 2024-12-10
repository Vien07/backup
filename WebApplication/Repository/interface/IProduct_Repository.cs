using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Areas.Product.Models;
using DTO.Product;

namespace CMS.Repository
{
    public interface IProduct_Repository
    {
        Task<List<ProductDto>> GetList(string lang, int page, string sortby);
        Task<List<ProductDto>> GetHotList(string lang, int limit);
        Task<List<ProductDto>> GetListBySlug(string lang, int page, string cate, string sortby);
        Task<ProductDto> GetProduct(string slug, string lang);
        Task<List<ProductDto>> GetRelateList(string slug, string lang, int limit);
        Task<ProductCateDto> GetCate(string slug, string lang);
        Task<List<ProductCateDto>> GetCateList(string lang);
        Task<List<ProductCateDto>> GetCateParentList(string lang);
        ProductDto GetProductPreview();
        Task<string> ChangePrice(int optionId, long productId);
        Task<bool> InsertComment(int productId, int customerId, string message, int? parentId);
        List<ProductCommentDto> LoadComment(long productId, int page, string filter, string sort);
    }
}