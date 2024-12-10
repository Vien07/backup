
using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _rep;
        IFileServices _fileSrv;
        private readonly ICommonServices _common;

        IHttpContextAccessor _httpContextAccessor;
        // private readonly CoreServices _core;
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        public CandidateController(DBContext _dBContext, IHttpContextAccessor httpContextAccessor, ICandidateRepository rep
                            , IFileServices fileSrv, ICommonServices common)
        {

            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _fileSrv = fileSrv;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }

        [CustomAuthorization("Candidate", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        [CustomAuthorization("Candidate", "VIEW")]
        public JsonResult GetData(SearchDto search)
        {
            var data = _rep.GetData(search);
            return Json(data);
        }

        [CustomAuthorization("Candidate", "VIEW")]
        public JsonResult ReadContact(long Pid)
        {
            return Json(_rep.ReadContact(Pid));
        }

        [CustomAuthorization("Candidate", "DELETE")]
        public JsonResult Delete(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.Delete(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("Candidate", "VIEW")]
        public JsonResult Seen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.Seen(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("Candidate", "VIEW")]
        public JsonResult NotSeen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.NotSeen(Pid), listData = _rep.GetData(search) });

        }
    }
}
