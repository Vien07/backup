//using Admin.QuickToolBar.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
using Admin.LayoutPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{
    public class QuickToolBarEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("QuickToolBar", "Thông tin", "fas fa-layer-group", "/QuickToolBar");

        public Database.QuickToolBar Detail = new Database.QuickToolBar();
        public Database.QuickToolBar DetailQuickToolBar = new Database.QuickToolBar();

        public List<Database.QuickToolBarItem> List = new List<Database.QuickToolBarItem>();



    }
    public partial class QuickToolBarController
    {
        QuickToolBarEditModel _editModel = new QuickToolBarEditModel();

        [HttpGet("QuickToolBar/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data;
                    long pid = data.Pid;
                    _editModel.List = _srv.GetChildList(pid).Data;

                }
            }

            return View(_editModel);
        
        }

        [HttpPost]
        public ActionResult Save(QuickToolBarModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString( QuickToolBarConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res });
        }
        public JsonResult EditChild(int idChild)
        {
            var modalHtml = "";

            if (idChild == 0)
            {
                modalHtml = _viewRender.RenderPartialViewToString(QuickToolBarConstants.StaticPath.PartialView.Edit_ModalQuickToolBarItemEdit, new Database.QuickToolBarItem());

                return new JsonResult(new { response = modalHtml });
            }

            try
            {

                var data = _srv.GetChildById(idChild).Data;
                if (data != null)
                {

                    modalHtml = _viewRender.RenderPartialViewToString(QuickToolBarConstants.StaticPath.PartialView.Edit_ModalQuickToolBarItemEdit, _srv.GetChildById(idChild).Data);

                    return new JsonResult(new { response = modalHtml });
                }
                return new JsonResult(new { response = modalHtml });

            }
            catch (Exception ex)
            {

                return new JsonResult(new { response = "" });
            }




        }
        public ActionResult SaveChild(QuickToolBarItemModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;


            var res = _srv.SaveChild(data);

            var listData = _viewRender.RenderPartialViewToString(QuickToolBarConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(res.Data.QuickToolBarPid).Data);

            return new JsonResult(new { response = res, listData = listData});
        }
        public JsonResult DeleteChild(List<int> ids, long QuickToolBarPid)
        {
            var res = _srv.DeleteChild(ids);
            var listData = _viewRender.RenderPartialViewToString(QuickToolBarConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(QuickToolBarPid).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult MoveChild(int fromId, int toId, long QuickToolBarPid)
        {
            _pageModel.Search = new ParamSearch();

            var res = _srv.MoveChild(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(QuickToolBarConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(QuickToolBarPid).Data);
            return new JsonResult(new { response = res, listData = listData });

        }

    }
}
