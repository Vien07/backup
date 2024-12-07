//using Admin.EmailManagement.Models;
using Admin.EmailManagement;
using Admin.EmailManagement.Constants;
using Admin.EmailManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.EmailManagement.Controllers
{
    public class EmailAdminModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Thông tin cấu hình email", "Thông tin", "fas fa-layer-group", "/EmailManagement");

        public Database.EmailAdmin Detail = new Database.EmailAdmin();
    }
    public partial class EmailAdminController
    {
        EmailAdminModel _editModel = new EmailAdminModel();

        [HttpGet("EmailAdmin/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data;
                    //_editModel.ListFiles = data.ListFiles;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(Admin.EmailManagement.Database.EmailAdmin data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            var listData = _viewRender.RenderPartialViewToString( EmailAdminConstans.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
