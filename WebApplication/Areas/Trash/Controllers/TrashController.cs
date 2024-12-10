using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Trash.Controllers
{
    [Area("Trash")]
    public class TrashController : Controller
    {

        private string DateFormat = "";
        private readonly ITrashRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;
        private readonly DBContext _dBContext;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private int NewsId = ConstantStrings.NewsId;
        private int AboutId = ConstantStrings.AboutId;

        public TrashController(DBContext dBContext, ITrashRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _dBContext = dBContext;
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }
        [CustomAuthorization("Trash", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.DateFormat = DateFormat;
            //LoadData();
            return View();
        }
        [CustomAuthorization("Trash", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {
            var data = _rep.LoadData(search);
            return Json(data);
        }      
        [CustomAuthorization("Trash", "Delete")]
        public JsonResult Delete(int Pid,int CateId )
        {

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; 
            return Json(new { Error = _rep.Delete(Pid, CateId), jsData = _rep.LoadData(search) });
        }   
        [CustomAuthorization("Trash", "Edit")]
        public JsonResult Undo(int Pid, int CateId)
        {

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; 
            return Json(new { Error = _rep.Undo(Pid, CateId), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Trash", "Delete")]
        public JsonResult DeleteAll()
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;
            return Json(new { Error = _rep.DeleteAll(), jsData = _rep.LoadData(search) });
        }
        public ActionResult LinkPost(string pidCate, string pidDetail)
        {
            //Không còn link từ trash nữa nên tắt

            //if (pidCate == NewsId.ToString())
            //{
            //    return Redirect("/b-admin/News?editPid=" + pidDetail);
            //}
            //else if (pidCate == BookId.ToString())
            //{
            //    return Redirect("/b-admin/Book?editPid=" + pidDetail);
            //}       
            //else if (pidCate == AboutId.ToString())
            //{
            //    return Redirect("/b-admin/About?editPid=" + pidDetail);
            //}

            return View();
        }
    }
}