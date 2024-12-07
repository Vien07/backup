//using Admin.EmailManagement.Models;
using Admin.EmailManagement;
using Admin.EmailManagement.Constants;
using Admin.EmailManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using System.Net;
using X.PagedList;

namespace Admin.EmailManagement.Controllers
{
    public class EditEmailTemplateModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định nghĩa mẫu EmailManagement", "Thông tin", "fas fa-layer-group", "/EmailManagement");

        public Database.EmailTemplate Detail = new Database.EmailTemplate();
        public List<Admin.EmailManagement.Database.EmailAdmin> ListEmailAdmin;
    }
    public partial class EmailTemplateController
    {
        EditEmailTemplateModel _editModel = new EditEmailTemplateModel();

        [HttpGet("EmailTemplate/Edit/{id?}")]
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
            ParamSearch paramSearch = new ParamSearch();
            _editModel.ListEmailAdmin = _srvAdmin.GetList(paramSearch).Data.ToList();
            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(Admin.EmailManagement.Database.EmailTemplate data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            var listData = _viewRender.RenderPartialViewToString(EmailTemplateConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
