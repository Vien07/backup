//using Admin.PostsManagement.Models;
using Admin.MemberManagement;
using Admin.MemberManagement.Constants;
using Admin.MemberManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.MemberManagement.Controllers
{

    #region Define
    public class FeedbackPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Đánh giá khách hàng", "Phản hồi", "fas fa-layer-group", "/MemberManagement");




        public Feedback_List List;
        public Dictionary<string,string> Configs;
        public Database.Feedback EditModel;
        public FeedbackModel.ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
        public List<SelectControlData> Filter_ParrentPostsCategory = new List<SelectControlData>();



    }
    #endregion
    public partial class FeedbackController
    {
    

        public IActionResult Index()
        {
            //_httpContext.HttpContext.Request.Headers.Add("LangKey", "vi");

            _pageModel.Configs = _repConfig.GetAllConfigs();

            _pageModel.Search = new FeedbackModel.ParamSearch();
            _pageModel.EditModel = new Database.Feedback();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(FeedbackModel.ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(FeedbackModel.ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(FeedbackConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new FeedbackModel.ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(FeedbackConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new FeedbackModel.ParamSearch();
            var res = _repFeedback.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(FeedbackConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

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
