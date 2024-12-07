using Admin.Course.Database;
using Admin.Course.Models;
using Common.Helper;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Admin.Course.Pages.CMS.Member
{
    public class CourseManagementModel : PageModel
    {
        private string CreateUser = "admin";
        IViewRendererHelper _viewRender;
        private IMemberRepository _rep;
        private ILoggerHelper _logger;
        public Database.Member MemberModel = new Database.Member();
        public ParamSearch search;

        public List<SelectControlData> Courses = new List<SelectControlData>();
        public long Pid;

        public string _PartialUrl = "~/Pages/Member/_Partial/";
        public string _PartialModel = "_PartialModal.cshtml";
        public string _PartialTable = "_PartialTable.cshtml";
        public string _PartialModalConfig = "_PartialModalConfig.cshtml";
        public CourseManagementModel(IMemberRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
        public void OnGet(int id)
        {
            var data = _rep.GetById(id).Data;
            if (data != null)
            {
                MemberModel = data;
            }


            var courses = _rep.GetCourses().Data;
            if (courses != null)
            {
                Courses = courses;
            }

            Pid = id;
        }
        public ActionResult OnGetData(int id)
        {
            var data = _rep.GetListCourseMember(id).Data;
            return new JsonResult(data);
        }
        public ActionResult OnPostSave(Database.Course_Member data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;

            var res = _rep.SaveCourseMember(data);
            return new JsonResult(new { response = res });
        }
        public ActionResult OnPostEdit(int courseMemberPid)
        {
            var res = _rep.GetCourseMember(courseMemberPid);
            return new JsonResult(new { response = res });
        }
        public ActionResult OnPostDelete(int courseMemberPid)
        {
            var res = _rep.DeleteCourseMember(courseMemberPid);
            return new JsonResult(new { response = res });
        }
        public ActionResult OnPostSendMail(int courseMemberPid)
        {
            var res = _rep.SendMailCourseMember(courseMemberPid);
            return new JsonResult(new { response = res });
        }
    }
}
