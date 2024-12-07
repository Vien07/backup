//using Steam.Core.LocalizeManagement.Models;
using Steam.Core.LocalizeManagement;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.LocalizeManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Steam.Core.LocalizeManagement.Controllers
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("LocalizeManagement", "Thông tin", "fas fa-layer-group", "/LocalizeManagement");

        public Database.LocalizeManagement Detail = new Database.LocalizeManagement();




    }
    public partial class LocalizeManagementController
    {

        EditModel _editModel = new EditModel();

        [HttpGet("LocalizeManagement/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(LocalizeManagementModelEdit input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(input);

            //var listData = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res});
        }
    }
}
