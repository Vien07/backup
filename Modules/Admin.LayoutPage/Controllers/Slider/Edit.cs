//using Admin.Slider.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
using Admin.LayoutPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{
    public class SliderEditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Slider", "Thông tin", "fas fa-layer-group", "/Slider");

        public Database.Slider Detail = new Database.Slider();
        public Database.Slider DetailSlider = new Database.Slider();

        public List<Database.SliderItem> List = new List<Database.SliderItem>();



    }
    public partial class SliderController
    {
        SliderEditModel _editModel = new SliderEditModel();

        [HttpGet("Slider/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data;
                    long pid = data.Pid;
                    _editModel.List = _srv.GetChildList(pid).Data;

                }
            }

            return View(_editModel);
        
        }

        [HttpPost]
        public ActionResult Save(SliderModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
             _pageModel.Search = new ParamSearch();

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString( SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res });
        }
        public JsonResult EditChild(int idChild)
        {
            var modalHtml = "";

            if (idChild == 0)
            {
                modalHtml = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Edit_ModalSliderItemEdit, new Database.SliderItem());

                return new JsonResult(new { response = modalHtml });
            }

            try
            {

                var data = _srv.GetChildById(idChild).Data;
                if (data != null)
                {

                    modalHtml = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Edit_ModalSliderItemEdit, _srv.GetChildById(idChild).Data);

                    return new JsonResult(new { response = modalHtml });
                }
                return new JsonResult(new { response = modalHtml });

            }
            catch (Exception ex)
            {

                return new JsonResult(new { response = "" });
            }




        }
        public ActionResult SaveChild(SliderItemModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;


            var res = _srv.SaveChild(data);

            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(res.Data.SliderPid).Data);

            return new JsonResult(new { response = res, listData = listData});
        }
        public JsonResult DeleteChild(List<int> ids, long sliderPid)
        {
            var res = _srv.DeleteChild(ids);
            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(sliderPid).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult MoveChild(int fromId, int toId, long sliderPid)
        {
            _pageModel.Search = new ParamSearch();

            var res = _srv.MoveChild(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Edit_Table, _srv.GetChildList(sliderPid).Data);
            return new JsonResult(new { response = res, listData = listData });

        }

    }
}
