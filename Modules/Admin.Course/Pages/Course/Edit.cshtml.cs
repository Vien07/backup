using Admin.Course;
using Admin.Course.Models;
using Common.Helper;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Course.Pages.CMS.Course
{
    public class EditModel : PageModel
    {
        private string CreateUser = "admin";
        IViewRendererHelper _viewRender;
        private ICourseRepository _rep;
        private ILoggerHelper _logger;
        public Database.Course CourseModel = new Database.Course();
        public List<SelectControlData> CategoryData = new List<SelectControlData>();
        public string CourseCategories = "";
        public ParamSearch search;

        public string _PartialUrl = "~/Pages/Course/_Partial/";
        public string _PartialModel = "_PartialModal.cshtml";
        public string _PartialTable = "_PartialTable.cshtml";
        public string _PartialModalConfig = "_PartialModalConfig.cshtml";
        public EditModel(ICourseRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
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
                CourseModel = data;
            }

            //get all data category
            var categories = _rep.GetDataCategories().Data;
            if (categories != null)
            {
                CategoryData = categories;
            }


            //get course category
            var courseCates = _rep.GetCateById(id).Data;
            if (courseCates != null)
            {
                CourseCategories = courseCates;
            }
        }
        public ActionResult OnPostSave(Database.Course data, List<IFormFile> files, string categories)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            search = new ParamSearch();

            var res = _rep.Save(data, files, categories);

            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

    }
}
