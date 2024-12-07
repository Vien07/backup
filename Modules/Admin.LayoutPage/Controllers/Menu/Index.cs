//using Admin.Sample.Models;
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Database;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using static Admin.LayoutPage.Constants.MenuConstants;
using Admin.LayoutPage.Constants;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Controllers
{
    #region Define
    public class MenuStyleModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định dạng Menu", "Danh sách", "fas fa-layer-group", "/MenuStyle");




        public IPagedList<Database.MenuStyle> List;
        public Dictionary<string,string> Configs;
        public Database.MenuStyle Detail;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion
    #region Define
    public class MenuPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Menu", "Danh sách", "fas fa-layer-group", "/Menu");



        private string CreateMenu = "admin";

        public List<Database.Menu> List;
        public string ListHTML = "";
        public Dictionary<string, string> Configs;
        public List<SelectControlData> Filter_ParrentMenu = new List<SelectControlData>();
        public Database.Menu MenuModel;
        public ParamSearch search;
        public PaginationModel Pagination = new PaginationModel();
        public List<SelectControlData> ListMenuStyles = new List<SelectControlData>();
        public List<SelectControlData> ListParentId = new List<SelectControlData>();
        public MenuStyleModel _pageMenuStyleModel = new MenuStyleModel();


    }
    #endregion

    public partial class MenuController
    {
        public MenuPageModel _pageModel = new MenuPageModel();
        public MenuStyleModel _pageMenuStyleModel = new MenuStyleModel();


        public IActionResult Index(ParamSearch search)
        {

            _pageModel.Configs = _repConfig.GetAllConfigs();

            //search = new ParramSearch();
            _pageModel.MenuModel = new Database.Menu();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Filter_ParrentMenu = _srv.GetMenuParent(0).Data;
             var listMenu = _srv.GetList(search).Data;
            if(listMenu != null )
            {
                foreach(var item in listMenu) { 
                if(item.ParentID==0 && item.MenuStylePid==0)
                    {
                        item.Title =item.Title + " *";
                    }    
                }
            }
            _pageModel.List = listMenu;
            //string list = "";
            //list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            //_pageModel.ListHTML = list;
            //_pageModel.Pagination.PageIndex = search.PageIndex;
            //_pageModel.Pagination.PageCount = _pageModel.List.Count;

            _pageMenuStyleModel.Configs = _repMenuStyleConfig.GetAllConfigs();
            _pageMenuStyleModel.Search = new ParamSearch();
            _pageMenuStyleModel.Detail = new Database.MenuStyle();
            _pageMenuStyleModel.Pagination = new PaginationModel();
            _pageMenuStyleModel.List = _srvMenuStyle.GetList(_pageMenuStyleModel.Search).Data;
            _pageMenuStyleModel.Pagination.PageIndex = _pageMenuStyleModel.Search.PageIndex;
            _pageMenuStyleModel.Pagination.PageCount = _pageMenuStyleModel.List.PageCount;
            _pageModel._pageMenuStyleModel = _pageMenuStyleModel;
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
        public string InsertRow(List<Database.Menu> list, int level)
        {
            string html = "";
            var dem = 1;
            foreach (var item in list)
            {
                dem++;
                MenuViewModel menuViewModel = new MenuViewModel();
                List<Database.Menu> tempList = new List<Database.Menu>();
                tempList.Add(item);
                menuViewModel.Level = level;
                menuViewModel.Data = tempList;
                var listChild = _pageModel.List.Where(p => p.ParentID == item.Pid).ToList();
                html += _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_Table, menuViewModel);

                if (listChild.Count() > 0)
                {
                    level++;

                    html += InsertRow(listChild, level);
                    level = level - 1;

                }
                else
                {
                    var checkItemCount = _pageModel.List.Where(p => p.ParentID == item.ParentID).Count();
                    if (dem <= checkItemCount)
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

            var listData = _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_TableTree, _pageModel);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            var search = new ParamSearch();

            var res = _repMenu.Enable(ids, isEnable,CreateUser);
            _pageModel.List = _srv.GetList(search).Data;

            var listData = _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_TableTree, _pageModel);

            return new JsonResult(new { response = res, listData = listData });
        }    
        public JsonResult UpdateParent(int id, int parentId)
        {
            var search = new ParamSearch();

            var res = _srv.UpdateParent(id, parentId);
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_TableTree, _pageModel);

            return new JsonResult(new { response = res, listData = list });
        }    
        public JsonResult EditModal(int id)
        {
            Response<LayoutPage.Database.Menu> res = new Response<LayoutPage.Database.Menu>();
            var search = new ParamSearch();
            if (id == 0)
            {

                _pageModel.MenuModel = new Database.Menu() ;
            }
            else
            {
                 res = _srv.GetById(id);
                _pageModel.MenuModel = res.Data;
            }
            _pageModel.ListMenuStyles = _srv.GetListMenuStyle().Data;
            _pageModel.ListParentId = _srv.GetAllMenuParent().Data;

            string modalEdit = _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_ModelEdit, _pageModel);

            return new JsonResult(new { response = res, modal = modalEdit });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.search = new ParamSearch();

            var res = _repMenu.Move(fromId, toId);
            string list = "";
            //_pageModel.List = _rep.GetList(_pageModel.search).Data;

            //list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = list });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repMenu.EnableUpdateOrder();
            string list = "";
            _pageModel.List = _srv.GetList(search).Data;
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = list });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.search = new ParamSearch();

            var res = _repMenu.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab);
            return new JsonResult(new { response = res });

        }
        public ActionResult SaveModal(MenuModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            var search = new ParamSearch();

            var res = _srv.Save(data);
            _pageModel.List = _srv.GetList(search).Data;

            var listData = _viewRender.RenderPartialViewToString(StaticPath.PartialView.Index_TableTree, _pageModel);

            return new JsonResult(new { response = res ,listData = listData });
        }
    }

}
