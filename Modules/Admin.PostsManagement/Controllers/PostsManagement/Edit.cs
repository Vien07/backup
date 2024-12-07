using Admin.PostsManagement.Constants;
using Admin.PostsManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Admin.PostsManagement.Services;

using X.PagedList;
namespace Admin.PostsManagement.Controllers
{

    public partial class PostsManagementController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Quản lý bài viết", "Thông tin", "fas fa-layer-group", "/PostsManagement");
            public string Group = "Post";

            public Database.PostsManagement Detail = new Database.PostsManagement();
            public List<Database.PostsManagement_Files> ListFiles = new List<Database.PostsManagement_Files>();
            public List<SelectControlData> ListSubCate = new List<SelectControlData>();

            public List<SelectControlData> ListPostTypes = new List<SelectControlData>();
            public string PostTypes { get; set; }

            public void InitListPostTypesData()
            {
                try
                {
                    var listTemp = this.PostTypes.Split(';');
                    foreach (var item in listTemp)
                    {
                        var tempType = item.Split(":");
                        if (tempType.Length == 2)
                        {
                            this.ListPostTypes.Add(new SelectControlData() { Name = tempType[0], Value = tempType[1], Order = 0 });

                        }
                    }
                }
                catch (Exception)
                {

                    this.ListPostTypes = new List<SelectControlData>();
                }

            }
            public void SetEditModel(IPostsManagementService _srv,int id, string group)
            {
        
            }

        }
        EditModel _editModel = new EditModel();

        [HttpGet("PostsManagement/Edit/{Group}/{id?}")]
        public IActionResult Edit(int id, string group)
        {
            _editModel.Group = group;
            var config = _CONFIG;
            _editModel.PostTypes = config.Where(p => p.Key == PostsManagementConstants.Config.Admin.PostType).FirstOrDefault().Value;
            _editModel.InitListPostTypesData();
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                    _editModel.ListFiles = data.ListFiles;
                    _editModel.ListSubCate = _srv.GetChildrenPostCategory(_editModel.Detail.CateID).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title,
                         ParrentID = row.ParentID,
                     }
                    ).ToList<SelectControlData>(); ;

                }
            }
            //_editModel.SetEditModel(_srv,id,group);
            return View(_editModel);
        }
        [HttpPost]
        public  ActionResult Save(SaveModel data)
        {

            var res = _srv.Save(data);
            if (!res.IsError)
            {
                data.Pid=res.Data.Pid; 
                _srv.SaveImage(data);

            }

            return new JsonResult(new { response = res });
        }
        public ActionResult GetListSubCate(int parentId)
        {
            var listCate = _srv.GetChildrenPostCategory(parentId);
            TreeSelectModel treeSelectModel = new TreeSelectModel();

            treeSelectModel.MapToObj(listCate, "Pid", "Title", "ParentID", "Order", "Enabled", parentId.ToString());
            var jsonSubCate = treeSelectModel.TreeSelectDataJson();
            return new JsonResult(new { jsonSubCate });
        }
    }
}
