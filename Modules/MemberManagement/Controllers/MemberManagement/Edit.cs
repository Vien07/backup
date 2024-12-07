using Admin.MemberManagement;
using Admin.MemberManagement.Models;
using ComponentUILibrary.Models;
using MemberManagementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using X.PagedList;
using PartialViews = Admin.MemberManagement.Constants.StaticPath.PartialView;

namespace Admin.MemberManagement.Controllers
{
    #region Define
    public class EditModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("Thành viên", "Chi tiết", "", "/Sample");

        public MemberManagementDetail MemberManagementModel = new MemberManagementDetail();
        public ParamSearch search;
        public IPagedList<Database.MemberManagement> List { get; set; }

        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion
    public partial class MemberManagementController : Controller
    {
        EditModel _editModel =new EditModel();
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.MemberManagementModel = data;
                }
            }

            return View(_editModel);
        }
        [HttpPost]
        public ActionResult Save(Database.MemberManagement data, List<IFormFile> files, string ListFiles, string CheckBox)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            _editModel.search = new ParamSearch();

            var res = _srv.Save(data, files, ListFiles);

            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_editModel.search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult ResetPassword(int pid)
        {
            var res = _srv.ResetPassword(pid);
            return Json(res);
        }
    }
}