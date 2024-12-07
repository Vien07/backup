//using Steam.Core.LocalizeCulture.Models;
using Steam.Core.LocalizeManagement;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.LocalizeManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Controllers
{
    public partial class LocalizeCultureController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Quản lý Culture", "Danh sách", "fas fa-layer-group", "/LocalizeCulture/Index");
            public IPagedList<Database.LocalizeCulture> List;
            public ParamSearch Search;
            public PaginationModel Pagination = new PaginationModel();
            public Dictionary<string,string> Configs;
            public LocalizeCultureDetail EditModel;


        }
        #endregion

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();
            _pageModel.Search = new ParamSearch();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repLocalizeCulture.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId, int pageIndex)
        {
            _pageModel.Search = new ParamSearch();
            _pageModel.Search.PageIndex = pageIndex;
            var res = _repLocalizeCulture.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repLocalizeCulture.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repLocalizeCulture.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }
        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }
        public JsonResult EditModal(int id)
        {
            Response<LocalizeCultureDetail> res = new Response<LocalizeCultureDetail>();
            var search = new ParamSearch();
            if (id == 0)
            {

                _pageModel.EditModel = new LocalizeCultureDetail();
            }
            else
            {
                res = _srv.GetById(id);
                _pageModel.EditModel = res.Data;
            }


            string modalEdit = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_ModalEdit, _pageModel.EditModel);

            return new JsonResult(new { response = res, modal = modalEdit });
        }
        public ActionResult SaveModal(LocalizeCultureModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            var search = new ParamSearch();

            var res = _srv.Save(data);
            _pageModel.Search = new ParamSearch();
            var listData = _viewRender.RenderPartialViewToString(LocalizeCultureConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
