using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.SEO.Models;
using Admin.ProductManagement.Api.Models;
namespace Admin.ProductManagement.Services
{
    public interface IApiProductsService
    {
        public Response<dynamic> GetProductBySlug(Api.Models.Request.GetProductBySlug input);
        public ResponseList<dynamic> GetListProductsByCateSlug(Api.Models.Request.GetListProductsByCateSlug input);
        public ResponseList<dynamic> GetListNewProductsByCateSlug(Api.Models.Request.GetListNewProductsByCateSlug input);
        public ResponseList<dynamic> GetListRelateProductsByProductSlug(Api.Models.Request.GetListRelateProductsByProductSlug input);
        public ResponseList<List<Api.Models.Response.GetListProductsByType>> GetListProductsByType(Api.Models.Request.GetListProductsByType input);
        public ResponseList<dynamic> GetListCateByCateSlug(Api.Models.Request.GetListCateByCateSlug input);
        public ResponseList<List<Api.Models.Response.GetListColorProduct>> GetListColorProduct();
        public ResponseList<List<Api.Models.Response.GetListProductSpecificaties>> GetListProductSpecificaties(string group);
        public Response<List<Api.Models.Response.GetListProductTypes>> GetListProductTypes();
        public Response<Api.Models.Response.GetCateDetail> GetCateDetail(Api.Models.Request.GetCateDetail input);
        public Response<dynamic> GetListProductDetail();
    }
}