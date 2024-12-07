//using Admin.Sample.Models;
using Admin.PostsCategory.Models;
using Admin.PostsCategory.Database;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Admin.PostsCategory.Constants;

namespace Admin.PostsCategory.Controllers
{


    public partial class PostsCategoryController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Danh mục bài viết", "Danh sách", "fas fa-layer-group", "/PostsCategory");



            private string CreatePostsCategory = "admin";

         //   public List<Database.PostsCategory> List;
           public List<PostsCategory_Item> List = new List<PostsCategory_Item>();
            //public PostsCategory_List List = new PostsCategory_List();
            public string ListHTML = "";
            public Dictionary<string, string> Configs;
            public List<SelectControlData> Filter_ParrentPostsCategory = new List<SelectControlData>();
            public Database.PostsCategory ModelPostsCategory;
            public ParamSearch search;
            public PaginationModel Pagination = new PaginationModel();


        }
        #endregion
        public PageModel _pageModel = new PageModel();


        public IActionResult Index(ParamSearch search)
        {



            _pageModel.Configs = _repPostsCategoryConfig.GetAllConfigs();

            //search = new ParramSearch();
            _pageModel.ModelPostsCategory = new Database.PostsCategory();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Filter_ParrentPostsCategory = _srv.GetPostsCategoryParent(0).Data;
            _pageModel.List = _srv.GetList(search).Data;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.Count;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            return new JsonResult(_pageModel.ListHTML);

        }
        public string InsertRow(List<PostsCategory_Item> list, int level)
        {
            string html = "";
            var dem = 1;
            foreach (var item in list)
            {
                dem++;
                PostsCategoryViewModel PostsCategoryViewModel = new PostsCategoryViewModel();
                List<PostsCategory_Item> tempList = new List<PostsCategory_Item>();
                tempList.Add(item);
                PostsCategoryViewModel.Level = level;
                PostsCategoryViewModel.Data = tempList;
                var listChild = _pageModel.List.Where(p => p.ParentID == item.Pid).ToList();
                html += _viewRender.RenderPartialViewToString(PostsCategoryConstants.StaticPath.PartialView.Index_Table, PostsCategoryViewModel);

                if (listChild.Count() > 0)
                {
                    level++;

                    html += InsertRow(listChild, level);
                    level = level - 1;

                }
                else
                {
                    var checkItemCount = _pageModel.List.Where(p => p.ParentID == item.ParentID).Count();
                    if(dem <= checkItemCount)
                    {
                        //level = level - 1;

                    }
                    else
                    {
                        level = 0;


                    }
                }


            }
            return html;
        }
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);

            //foreach (var item in List.Where(p => p.Level == 0).ToList())
            //{
            //    var listChild = List.Where(p => p.Data.ParentID == item.Data.Pid).ToList();
            //    list += InsertRow(List);
            //}
            //string list = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.Count;
            //string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _PageModel);
            return new JsonResult(new { list = list, paging = "" });
        }
        public JsonResult Delete(int id)
        {
            _pageModel.search = new ParamSearch();
            var res = _srv.Delete(id);
            _pageModel.List = _srv.GetList(_pageModel.search).Data;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = _pageModel.ListHTML });
        }
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.search = new ParamSearch();
            var res = _repPostsCategory.Enable(ids, isEnable,CreateUser);
            _pageModel.List = _srv.GetList(_pageModel.search).Data;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            return new JsonResult(new { response = res, listData = list });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.search = new ParamSearch();

            var res = _repPostsCategory.Move(fromId, toId);
            string list = "";
            _pageModel.List = _srv.GetList(_pageModel.search).Data;

            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = list });

        }
        public JsonResult  EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repPostsCategory.EnableUpdateOrder();
            string list = "";
            _pageModel.List = _srv.GetList(search).Data;
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = list });

        }
        public JsonResult  UpdateOrder(int id, double order)
        {
            _pageModel.search = new ParamSearch();

            var res = _repPostsCategory.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult  SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());

            var res = _repPostsCategoryConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }

    }

}
