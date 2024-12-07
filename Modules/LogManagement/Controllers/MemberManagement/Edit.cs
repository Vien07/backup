using Admin.LogManagement;
using Admin.LogManagement.Models;
using ComponentUILibrary.Models;
using Admin.LogManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using X.PagedList;

namespace Admin.LogManagement
{
    #region Define
    public class EditModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("Thành viên", "Chi tiết", "", "/Sample");

        public LogManagementDetail LogManagementModel = new LogManagementDetail();
        public ParamSearch search;
        public IPagedList<Database.LogAdminActivity> List { get; set; }

        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion
    public partial class LogManagementController : Controller
    {
        EditModel _editModel =new EditModel();
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                //var data = _rep.GetById(id).Data;
                //if (data != null)
                //{
                //    _editModel.LogManagementModel = data;
                //}
            }

            return View(_editModel);
        }
        //[HttpPost]
        //public ActionResult Save(Database.LogAdminActivity data, List<IFormFile> files, string ListFiles, string CheckBox)
        //{
        //    data.CreateUser = CreateUser;
        //    data.UpdateUser = CreateUser;
        //    _editModel.search = new ParamSearch();

        //    var res = _rep.Save(data, files, ListFiles);

        //    var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(_editModel.search).Data);

        //    return new JsonResult(new { response = res, listData = listData });
        //}
    }
}