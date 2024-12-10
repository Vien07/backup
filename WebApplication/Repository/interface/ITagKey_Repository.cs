using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.News;
using DTO.Product;

namespace CMS.Repository
{
    public interface ITagKey_Repository
    {    
        Task<List<NewsDto>> GetListNews(string lang, string key);
        Task<List<ProductDto>> GetListProduct(string lang, string key);
    }
}