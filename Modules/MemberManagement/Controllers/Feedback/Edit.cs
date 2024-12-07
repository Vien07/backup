//using Admin.PostsManagement.Models;
using Admin.MemberManagement;
using Admin.MemberManagement.Constants;
using Admin.MemberManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Admin.MemberManagement.Controllers
{
    public class FeedbackEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Feedback", "Phản hồi", "fas fa-layer-group", "/MemberManagement");

        public Database.Feedback Detail = new Database.Feedback();
        public List<Database.Feedback_Files> ListFiles = new List<Database.Feedback_Files>();
        public List<SelectControlData> ListSubCate = new List<SelectControlData>();





    }
    public partial class FeedbackController
    {
        FeedbackEditModel _editModel = new FeedbackEditModel(); 

         [HttpGet("Feedback/Edit/{id?}")]
        public IActionResult Edit(int id)
        {

            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                    _editModel.ListFiles = data.ListFiles;
                    

                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(FeedbackModelEdit data )
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            _pageModel.Search = new FeedbackModel.ParamSearch();
            var res = _srv.Save(data);

            var listData = _viewRender.RenderPartialViewToString(Constants.FeedbackConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }   
        
    }
}
