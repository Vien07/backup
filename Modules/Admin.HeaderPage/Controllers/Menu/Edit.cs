//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HeaderPage
{
    public class MenuEditModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("HeaderPage", "Chi tiết", "", "/Menu");

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
            _editModel.Parrents = _rep.GetMenuParent(id).Data;
            if (id != 0)
            {
                var data = _rep.GetById(id).Data;
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

            var res = _rep.Save(data);

            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res });
        }

    }
}
