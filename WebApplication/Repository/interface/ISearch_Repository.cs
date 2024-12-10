using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.News;
using DTO.Product;
using DTO.SearchResult;

namespace CMS.Repository
{
    public interface ISearch_Repository
    {
        Task<List<NewsDto>> GetNewsList(string lang, string key);
        Task<List<ProductDto>> GetProductList(string lang, string key);
        Task<List<SearchResultDto>> GetSearchList(string lang, string key);
        Task<List<SearchResultDto>> GetTagkeyList(string lang, string key);
    }
}