using Admin.ProductManagement.Constants;
using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductSpecificatyController : Controller
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Thuộc tính sản phẩm", "Chi tiết bảng màu", "fas fa-layer-group", "/MisaApi");
            public ProductSpecificatyDto Detail = new();
            public string Group { get; set; } = "";
        }

        [HttpGet("ProductSpecificaty/Edit/{Group}/{id?}")]
        public IActionResult Edit(string  Group, int id)
        {
            _editModel.Group = Group;
            if (id != 0)
            {
                var data = _service.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                }
            }
            return View(Group,_editModel);
        }

        [HttpPost]
        public ActionResult Save(ProductSpecificatySaveModel data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            _pageModel.Search = new ProductSpecificatySearchModel();
            var res = _service.Save(data);

            var listData = _viewRender.RenderPartialViewToString(ProductSpecificatyConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

    }
}
