//using Admin.PostsCategory.Models;
using Admin.PostsCategory;
using Admin.PostsCategory.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.PostsCategory.Controllers
{

    public partial class PostsCategoryController
    {
        public class EditModel
        {

            public PageTitleModel PageTitle = new PageTitleModel("Danh mục bài viết", "Chi tiết", "", "/PostsCategory");

            public PostsCategoryDetail PostsCategoryModel = new PostsCategoryDetail();
            public Database.PostsCategory Detail = new Database.PostsCategory();
            public List<SelectControlData> Parrents = new List<SelectControlData>();
            public List<SelectControlData> ListPageCate = new List<SelectControlData>();
            public List<SelectControlData> ListPageDetail = new List<SelectControlData>();
            public ParamSearch search;





        }
        EditModel _editModel = new EditModel();

        [HttpGet("PostsCategory/Edit/{id?}")]

        public IActionResult Edit(int id)
        {
            _editModel.Parrents = _srv.GetPostsCategoryParent(id).Data;
            _editModel.ListPageCate = _srv.GetListTemplatePage("cate").Data;
            _editModel.ListPageDetail = _srv.GetListTemplatePage("detail").Data;
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.PostsCategoryModel = data;
                }
            }
            return  View(_editModel);

;
        }
        public ActionResult Save(PostsCategoryModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            data.UpdateDate = DateTime.Now;
            _editModel.search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(search).Data);

            return new JsonResult(new { response = res });
        }

    }
}
