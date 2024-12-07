//using Admin.Slider.Models;
using Admin.Slider;
using Admin.Slider.Constants;
using Admin.Slider.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Slider
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Slider", "Thông tin", "fas fa-layer-group", "/Slider");

        public Database.Slider Detail = new Database.Slider();




    }
    public partial class SliderController
    {

        EditModel _editModel = new EditModel();

        [HttpGet("Slider/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _rep.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(SliderModelEdit input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _rep.Save(input);

            //var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res});
        }
    }
}
