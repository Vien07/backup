using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Admin.PostsCategory.Services;
namespace Admin.PostsCategory.Integrate
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("PostsCategory", "Danh mục bài viết", "fas fa-layer-group", "/PostsCategory");

        public Database.PostsCategory Detail = new Database.PostsCategory();
        public List<Database.PostsCategory_Files> ListFiles = new List<Database.PostsCategory_Files>();




    }

    public class PostsCategoryIntegrateController : Controller
    {

        public IPostsCategoryService _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        EditModel _editmodel = new EditModel();
        public PostsCategoryIntegrateController(IPostsCategoryService rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }

        //public JsonResult SaveCateWithPost(Admin.PostsCategory.Database.PostsCategory data, List<IFormFile> files, string ListFiles)
        //{
        //    data.CreateUser = CreateUser;
        //    data.UpdateUser = CreateUser;

        //    var res = _rep.SaveSEO(data, files, ListFiles);

        //    //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);

        //    return new JsonResult(new { response = res });
        //}

    }
}
