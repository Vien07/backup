//using Admin.HomePage.Models;
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
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Cấu hình trang chủ ", "Thông tin", "fas fa-layer-group", "/HomePage");

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
                var data = _srv.GetById(id).Data;
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

            var res = _srv.Save(data);

            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
