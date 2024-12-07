//using Admin.WebsiteKeys.Models;
using Admin.WebsiteKeys;
using Admin.WebsiteKeys.Constants;
using Admin.WebsiteKeys.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.WebsiteKeys.Controllers
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("WebsiteKeys", "Thông tin", "fas fa-layer-group", "/WebsiteKeys");

        public Database.WebsiteKeys Detail = new Database.WebsiteKeys();




    }
    public partial class WebsiteKeysController
    {

        EditModel _editModel = new EditModel();

        [HttpGet("WebsiteKeys/Edit/{id?}")]
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
        public ActionResult Save(WebsiteKeysModelEdit input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(input);

            //var listData = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res});
        }
    }
}
