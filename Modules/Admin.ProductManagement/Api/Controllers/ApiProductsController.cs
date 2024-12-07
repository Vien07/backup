
using Admin.ProductManagement.Api.Models;
using Admin.ProductManagement.DataTransferObjects.MisaReponse;
using Admin.ProductManagement.DTOs;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Api.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public partial class ApiProductsController : Controller
    {

        private readonly ILogger _logger;
        private readonly IApiProductsService _rep;
        private readonly IMisaApiService _misaApi;

        public ApiProductsController(IApiProductsService rep, IMisaApiService misaApi)
        {
            _rep = rep;
            _misaApi = misaApi;
        }
        //[HttpGet("GetList")]
        //public dynamic GetList()
        //{
        //  var Search = new ProductsManagementModel.ParamSearch();
        //    var a = _rep.GetList(Search);
        //    return a;
        //}
        [HttpPost("GetProductBySlug")]
        public dynamic GetProductBySlug(Models.Request.GetProductBySlug input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetProductBySlug(input);
            return rs;
        }

        [HttpGet("GetListProductDetail")]
        public dynamic GetListProductDetail()
        {
            var rs = _rep.GetListProductDetail();
            return rs;
        }

        [HttpPost("GetListProductsByCateSlug")]
        public dynamic GetListProductsByCateSlug(Models.Request.GetListProductsByCateSlug input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetListProductsByCateSlug(input);
            return rs;
        }
        [HttpPost("GetListNewProductsByCateSlug")]
        public dynamic GetListNewProductsByCateSlug(Models.Request.GetListNewProductsByCateSlug input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetListNewProductsByCateSlug(input);
            return rs;
        }
        [HttpPost("GetListRelateProductsByProductSlug")]
        public dynamic GetListRelateProductsByProductSlug(Models.Request.GetListRelateProductsByProductSlug input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetListRelateProductsByProductSlug(input);
            return rs;
        }

        [HttpPost("GetListCateByCateSlug")]
        public dynamic GetListCateByCateSlug(Models.Request.GetListCateByCateSlug input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetListCateByCateSlug(input);
            return rs;
        }
        [HttpGet("GetListProductTypes")]
        public dynamic GetListProductTypes()
        {
            var rs = _rep.GetListProductTypes();
            return rs;
        }
        [HttpGet("GetListProductSpecificaties")]
        public dynamic GetListProductSpecificaties(string? group)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetListProductSpecificaties(group);
            return rs;
        } 
        [HttpPost("GetCateDetail")]
        public dynamic GetCateDetail(Models.Request.GetCateDetail input)
        {
            //var Search = new ParamSearch();
            var rs = _rep.GetCateDetail(input);
            return rs;
        }

        [HttpGet("GetMisaAddress")]
        public Response<List<MisaResponseAddressDto>> GetMisaAddress(string kind, string parentId)
        {
            Response<List<MisaResponseAddressDto>> rs = new Response<List<MisaResponseAddressDto>>();
            var response = _misaApi.GetMisaAddress(kind, parentId);
            if (response != null)
            {

                rs.IsError = false;
                rs.StatusCode = 200;
                rs.Data = response.Data;
            }
            else
            {
                rs.IsError = true;
                rs.StatusCode = 404;
                rs.Message = "No resources found";
            }
            return rs;
        }

        [HttpPost("CreateMisaOrder")]
        public async Task<Response<bool>> CreateMisaOrder(Models.Request.CreateMisaOrder request)
        {
            var response = await _misaApi.CreateMisaOrder(request);
            return response;
        }
    }

}
