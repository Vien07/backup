using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Product.Controllers
{
    [Area("Product")]

    public class ProductCommentController : Controller
    {
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        private readonly IProductCommentRepository _rep;
        private readonly ICommonServices _common;

        private readonly DBContext _dBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductCommentController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, IProductCommentRepository rep, ICommonServices common)
        {
            _common = common;

            _rep = rep;
            _dBContext = dBContext;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            _httpContextAccessor = httpContextAccessor;
            DateFormat = common.GetConfigValue(ConstantStrings.KeyDateFormat);

        }
        [CustomAuthorization("Product", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        public IActionResult Modal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        #region action Detail  
        [CustomAuthorization("Product", "VIEW")]
        public JsonResult LoadData(SearchDto SearchDto)
        {
            var data = _rep.LoadData(SearchDto);
            return Json(data);
        }
        [CustomAuthorization("Product", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(SearchDto) });
        }
        [CustomAuthorization("Product", "DELETE")]
        public JsonResult DeleteAll(int Pid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.DeleteAll(Pid), jsData = _rep.LoadData(SearchDto) });
        }
        [CustomAuthorization("BookComment", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(SearchDto) });

        }
        [CustomAuthorization("Product", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(SearchDto) });

        }
        #endregion

    }
}
