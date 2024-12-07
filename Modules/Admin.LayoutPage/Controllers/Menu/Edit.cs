//using Admin.LayoutPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{
    public class MenuEditModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("LayoutPage", "Chi tiết", "", "/Menu");

        public Database.Menu MenuModel = new Database.Menu();
        public List<SelectControlData> Parrents = new List<SelectControlData>();
        public ParamSearch search;





    }
    public partial class MenuController
    {
        MenuEditModel _editModel = new MenuEditModel();

        [HttpGet("Menu/Edit/{id?}")]

        public IActionResult Edit(int id)
        {
            _editModel.Parrents = _srv.GetMenuParent(id).Data;
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.MenuModel = data;
                }
            }
            return  View(_editModel);

;
        }
        public ActionResult Save(MenuModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            _editModel.search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res });
        }

    }
}
