//using Admin.HomePage.Models;
using Admin.HomePage;
using Admin.HomePage.Constants;
using Admin.HomePage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HomePage
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("HomePage", "Thông tin", "fas fa-layer-group", "/HomePage");

        public Database.HomePage Detail = new Database.HomePage();




    }
    public partial class HomePageController
    {

        EditModel _editModel = new EditModel();

        [HttpGet("HomePage/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _rep.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(HomePageModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new HomePageModel.ParamSearch();

            var res = _rep.Save(data);

            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
