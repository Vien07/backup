//using Admin.LayoutPage.Models;
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
    public class MenuStyleEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định dạng menu", "Thông tin", "fas fa-layer-group", "/MenuStyle");

        public Database.MenuStyle Detail = new Database.MenuStyle();
        public Database.MenuItemStyle DetailMenuItemStyle = new Database.MenuItemStyle();

        public List<Database.MenuItemStyle> List = new List<Database.MenuItemStyle>();



    }
    public partial class MenuStyleController
    {
        MenuStyleEditModel _editModel = new MenuStyleEditModel();

        [HttpGet("MenuStyle/Edit/{id?}")]
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
        public ActionResult Save(MenuStyleModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString( HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res });
        }
        public JsonResult EditChild(int idChild)
        {
            var modalHtml = "";

            if (idChild == 0)
            {
                modalHtml = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Edit_ModalMenuItemEdit, new Database.MenuItemStyle());

                return new JsonResult(new { response = modalHtml });
            }

            try
            {

                var data = _srv.GetChildById(idChild).Data;
                if (data != null)
                {

                    modalHtml = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Edit_ModalMenuItemEdit, _srv.GetChildById(idChild).Data);

                    return new JsonResult(new { response = modalHtml });
                }
                return new JsonResult(new { response = modalHtml });

            }
            catch (Exception ex)
            {

                return new JsonResult(new { response = "" });
            }




        }
        public ActionResult SaveChild(MenuItemStyleModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;


            var res = _srv.SaveChild(data);

            var listData = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(data.MenuStylePid).Data);

            return new JsonResult(new { response = res, listData = listData});
        }
        public JsonResult DeleteChild(List<int> ids, long MenuStylePid)
        {
            var res = _srv.DeleteChild(ids);
            var listData = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(MenuStylePid).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        
    }
}
