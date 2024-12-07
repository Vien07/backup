//using Admin.FooterPage.Models;
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
    public class FooterPageEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Footer", "Thông tin", "fas fa-layer-group", "/FooterPage");

        public Database.FooterPage Detail = new Database.FooterPage();
        public Database.FooterPage DetailFooterPage = new Database.FooterPage();

        public List<Database.FooterItem> List = new List<Database.FooterItem>();



    }
    public partial class FooterPageController
    {
        FooterPageEditModel _editModel = new FooterPageEditModel();

        [HttpGet("FooterPage/Edit/{id?}")]
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
        public ActionResult Save(FooterPageModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString( FooterPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res });
        }
        public JsonResult EditChild(int idChild)
        {
            var modalHtml = "";

            if (idChild == 0)
            {
                modalHtml = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Edit_ModalFooterItemEdit, new Database.FooterItem());

                return new JsonResult(new { response = modalHtml });
            }

            try
            {

                var data = _srv.GetChildById(idChild).Data;
                if (data != null)
                {

                    modalHtml = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Edit_ModalFooterItemEdit, _srv.GetChildById(idChild).Data);

                    return new JsonResult(new { response = modalHtml });
                }
                return new JsonResult(new { response = modalHtml });

            }
            catch (Exception ex)
            {

                return new JsonResult(new { response = "" });
            }




        }
        public ActionResult SaveChild(FooterItemModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;


            var res = _srv.SaveChild(data);

            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(res.Data.FooterPagePid).Data);

            return new JsonResult(new { response = res, listData = listData});
        }
        public JsonResult DeleteChild(List<int> ids, long FooterPagePid)
        {
            var res = _srv.DeleteChild(ids);
            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(FooterPagePid).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        
    }
}
