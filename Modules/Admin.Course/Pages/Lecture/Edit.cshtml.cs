using Admin.Course.Models;
using Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Course.Pages.CMS.Lecture
{
    public class EditModel : PageModel
    {
        private string CreateUser = "admin";
        IViewRendererHelper _viewRender;
        private ILectureRepository _rep;
        private ILoggerHelper _logger;
        public Database.Lecture LectureModel = new Database.Lecture();
        public ParamSearch search;

        public string _PartialUrl = "~/Pages/Lecture/_Partial/";
        public string _PartialModel = "_PartialModal.cshtml";
        public string _PartialTable = "_PartialTable.cshtml";
        public string _PartialModalConfig = "_PartialModalConfig.cshtml";
        public EditModel(ILectureRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
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
                LectureModel = data;
            }   

        }
        public ActionResult OnPostSave(Database.Lecture data, List<IFormFile> files)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            search = new ParamSearch();

            var res = _rep.Save(data, files);

            var listData = _viewRender.RenderPartialViewToString(_PartialUrl, _PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

    }
}
