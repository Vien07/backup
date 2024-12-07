
using Admin.PostsManagement.Constants;
using Admin.PostsManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Admin.PostsManagement.Controllers
{


    public partial class PostsManagementController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Quản lý bài viết", "Danh sách", "fas fa-layer-group", "/PostsManagement");
            public string Group = "Post";




            public PostsManagement_List List;
            public Dictionary<string, string> Configs = new Dictionary<string, string>();
            public Database.PostsManagement EditModel;
            public PostsManagementModel.ParamSearch Search;
            public PaginationModel Pagination = new PaginationModel();
            public List<SelectControlData> Filter_ParrentPostsCategory = new List<SelectControlData>();



        }
        #endregion

        [HttpGet("PostsManagement/Index/{Group:regex(^([[^\\.]]+)$)}")]
        public IActionResult Index(string Group)
        {
            ViewBag.Group = Group;
            TempData["Group"] = Group;
            //_httpContext.HttpContext.Request.Headers.Add("LangKey", "vi");
            _pageModel.Group = Group;
            _pageModel.Configs = _CONFIG;
            _pageModel.Search = new PostsManagementModel.ParamSearch();
            _pageModel.Search.Group = Group;
            _pageModel.EditModel = new Database.PostsManagement();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Filter_ParrentPostsCategory = _srv.GetPostsCategoryParent();
            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(PostsManagementModel.ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(PostsManagementModel.ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new PostsManagementModel.ParamSearch();
            _pageModel.Search.Group = TempData.Peek("Group").ToString();
          var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new PostsManagementModel.ParamSearch();
            _pageModel.Search.Group = TempData.Peek("Group").ToString();
            var res = _repoPostsManagement.Enable(ids, isEnable, CreateUser);
            var listData = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new PostsManagementModel.ParamSearch();

            var res = _repoPostsManagement.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(PostsManagementModel.ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repoPostsManagement.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new PostsManagementModel.ParamSearch();

            var res = _repoPostsManagement.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());

            var res = _repConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }

    }

}
