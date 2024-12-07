using Admin.Course;
using Admin.Course.Models;
using Common.Helper;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Admin.Course.Pages.CMS.Course
{
    public class LectureModel : PageModel
    {
        IViewRendererHelper _viewRender;
        private ICourseRepository _rep;
        private ILoggerHelper _logger;

        public List<SelectControlData> Lectures = new List<SelectControlData>();
        public int ID { get; set; }
        public LectureModel(ICourseRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
        public void OnGet(int id)
        {
            ID = id;

            var lectures = _rep.GetLectures().Data;
            if (lectures != null)
            {
                Lectures = lectures;
            }
        }
        public ActionResult OnPostSave([FromBody] Root data)
        {
            return new JsonResult(new { response = _rep.SaveLecture(data) });
        }
        public ActionResult OnGetLecture(int id)
        {
            return new JsonResult(new { response = _rep.GetLecture(id) });
        }
    }
}
