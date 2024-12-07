using Admin.ProductManagement.Constants;
using Admin.ProductManagement.DataTransferObjects.MisaApiTracker;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.MisaApiTracker;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.STeamHelper;
using System.Xml.Linq;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public class MisaApiTrackerController : Controller
    {
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Cấu hình kết nối API", "Thông tin api misa", "fas fa-layer-group", "/MisaApi");
            public MisaApiTrackerPagedViewModel Data;
            public List<MisaApiConfigDto> Configs;
            public Database.Product EditModel;
            public MisaApiTrackerSearchModel Search;
            public PaginationModel Pagination = new PaginationModel();
        }

        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Cấu hình kết nối API", "Chi tiết api misa", "fas fa-layer-group", "/MisaApi");
            public MisaApiTrackerDto Detail = new MisaApiTrackerDto();
        }

        public readonly IMisaApiTrackerService _misaApiTrackerService;
        public readonly IViewRendererHelper _viewRender;

        public string CreateUser = "admin";

        public PageModel _pageModel = new();
        public EditModel _editModel = new();

        public MisaApiTrackerController(IViewRendererHelper viewRender, IMisaApiTrackerService rep)
        {
            _viewRender = viewRender;
            _misaApiTrackerService = rep;
        }

        public IActionResult Index()
        {
            _pageModel.Configs = _misaApiTrackerService.GetAllConfigs().Data.Items;

            _pageModel.Search = new MisaApiTrackerSearchModel();
            _pageModel.EditModel = new Database.Product();
            _pageModel.Pagination = new PaginationModel();

            MisaApiTrackerPagedViewModel data = _misaApiTrackerService.GetList(_pageModel.Search).Data;
            _pageModel.Data = data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = data.PageCount;
            return View(_pageModel);
        }

        [HttpPost]
        public JsonResult Search(MisaApiTrackerSearchModel search)
        {
            MisaApiTrackerPagedViewModel data = _misaApiTrackerService.GetList(search).Data;
            _pageModel.Data = data;
            string list = _viewRender.RenderPartialViewToString(MisaApiTrackerConstants.ConfigPartial.Index_Table, _pageModel.Data);

            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = data.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var res = _misaApiTrackerService.SaveConfig(formData);
            return new JsonResult(new { response = res });
        }

        public JsonResult GenerateAccessToken()
        {
            var res = _misaApiTrackerService.GenerateAccessToken();
            return new JsonResult(new { response = res });
        }

        [HttpGet("MisaApiTracker/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                MisaApiTrackerViewModel data = _misaApiTrackerService.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }
            return View(_editModel);
        }
    }
}
