//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Constants;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HeaderPage
{
    public class HeaderEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("HeaderPage", "Thông tin", "fas fa-layer-group", "/HeaderPage");

        public Database.HeaderPage Detail = new Database.HeaderPage();




    }
    public partial class HeaderPageController
    {
        HeaderEditModel _editModel = new HeaderEditModel();

        [HttpGet("HeaderPage/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _rep.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(HeaderPageModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _rep.Save(data);

            var listData = _viewRender.RenderPartialViewToString( HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
