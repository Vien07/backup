
using Admin.Sample.Constants;
using Admin.Sample.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base.Models;

using PartialViews = Admin.Sample.Constants.SampleConstants.StaticPath.PartialView;
namespace Admin.Sample.Controllers
{

    public partial class SampleController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Sample", "Danh sách", "fas fa-layer-group", "/Sample");
            public PagedListDto<SampleDetailDTO> List = new PagedListDto<SampleDetailDTO>();
            public Dictionary<string, string> Configs = new Dictionary<string, string>();
            public ParamSearch Search = new ParamSearch();
            public PaginationModel Pagination = new PaginationModel();
        }
        #endregion

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();

            var aaaa = _repConfig.GetConfigByKey(SampleConstants.Config.Admin.PageSize);
            _pageModel.List = _rep.GetList(_pageModel.Search);
            return View(_pageModel);
        }
        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _rep.GetList(search);
            string list = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _rep.GetList(_pageModel.Search));
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repoSampleDetail.Enable(ids, isEnable, CreateUser);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _rep.GetList(_pageModel.Search));

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repoSampleDetail.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _rep.GetList(_pageModel.Search));
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {

            var res = _repoSampleDetail.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _rep.GetList(search));
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repoSampleDetail.UpdateOrder(id, order);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _rep.GetList(_pageModel.Search));

            return new JsonResult(new { response = res, listData = listData });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab);
            return new JsonResult(new { response = res });

        }

    }

}
