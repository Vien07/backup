//using Admin.TemplatePage.Models;
using Admin.TemplatePage;
using Admin.TemplatePage.Constants;
using Admin.TemplatePage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.TemplatePage.Controllers
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("TemplatePage", "Thông tin", "fas fa-layer-group", "/TemplatePage");

        public Database.TemplatePage Detail = new Database.TemplatePage();




    }
    public partial class TemplatePageController
    {

        EditModel _editModel = new EditModel();

        [HttpGet("TemplatePage/Edit/{id?}")]
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
        public ActionResult Save(TemplatePageModelEdit input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(input);

            //var listData = _viewRender.RenderPartialViewToString(TemplatePageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res});
        }
    }
}
