using Admin.ProductManagement.DTOs;
using Admin.ProductManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Api.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiMisaProductController : Controller
    {
        private readonly IMisaApiService _rep;
        public ApiMisaProductController(IMisaApiService rep)
        {
            _rep = rep;
        }

        #region Misa
        [HttpGet("GetAccessToken")]
        public IActionResult GetAccessToken()
        {
            var model = _rep.GenerateAccessToken();
            if (model is null)
            {
                return NoContent();
            }

            return Ok(model);
        }
        [HttpGet("GetCategoryList")]
        public IActionResult GetCategoryList()
        {
            var model = _rep.GetCategoryList();
            if (model is null)
            {
                return NoContent();
            }

            return Ok(model);
        }

        [HttpGet("GetProductList")]
        public IActionResult GetProductList(int pageIndex, int pageSize)
        {
            var model = _rep.GetProductList(new DateTime(1970, 1, 1), pageIndex, pageSize);
            if (model is null)
            {
                return NoContent();
            }

            return Ok(model);
        }

        [HttpGet("GetBranchList")]
        public IActionResult GetBranchList()
        {
            var model = _rep.GetBranchList();
            if (model is null)
            {
                return NoContent();
            }

            return Ok(model);
        }

        #endregion

        [HttpGet("GetMisaProductBySlug")]
        public async Task<Response<Models.Response.GetMisaProductBySlug>> GetMisaProductBySlug(string slug)
        {
            var response = await _rep.GetPostBySlug(slug);
            return response;
        }
    }
}
