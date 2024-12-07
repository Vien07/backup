using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductCategory;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductCategoryController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Danh mục sản phẩm", "Danh sách", "fas fa-layer-group", "/MisaProductCategory");
            public List<ProductCategoryDto> List;
            public ProductCategoryPagedViewModel Data;
            public string ListHTML = "";
            public Dictionary<string,string> Configs;
            public List<SelectControlData> Filter_ParrentMisaProductCategory = new List<SelectControlData>();
            public ProductCategoryDto ModelMisaProductCategory;
            public ProductCategorySearchModel search;
            public PaginationModel Pagination = new PaginationModel();
        }
        #endregion

        public PageModel _pageModel = new PageModel();
        public IActionResult Index(ProductCategorySearchModel search)
        {

            _pageModel.Configs = _repConfig.GetAllConfigs();
            _pageModel.ModelMisaProductCategory = new ProductCategoryDto();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Filter_ParrentMisaProductCategory = _service.GetProductCategoryParent(0).Data;
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.Count;
            return View(_pageModel);
        }
        public JsonResult GetList(ProductCategorySearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            return new JsonResult(_pageModel.ListHTML);
        }
        public string InsertRow(List<ProductCategoryDto> list, int level)
        {
            string html = "";
            var dem = 1;
            foreach (var item in list)
            {
                dem++;
                ListProductCategoryViewModel listProductCategoryViewModel = new();
                List<ProductCategoryDto> tempList = new();
                tempList.Add(item);
                listProductCategoryViewModel.Level = level;
                listProductCategoryViewModel.Data = tempList;
                var listChild = _pageModel.List.Where(p => p.ParentID == item.Pid).ToList();
                html += _viewRender.RenderPartialViewToString(ProductCategoryConstants.StaticPath.PartialView.Index_Table, listProductCategoryViewModel);

                if (listChild.Count() > 0)
                {
                    level++;
                    html += InsertRow(listChild, level);
                    level = level - 1;
                }
                else
                {
                    var checkItemCount = _pageModel.List.Where(p => p.ParentID == item.ParentID).Count();
                    if (dem <= checkItemCount)
                    {
                        //level = level - 1;
                    }
                    else
                    {
                        level = 0;
                    }
                }
            }
            return html;
        }
        public JsonResult Search(ProductCategorySearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.Count;
            return new JsonResult(new { list = list, paging = "" });
        }
        public JsonResult Delete(int id)
        {
            _pageModel.search = new ProductCategorySearchModel();
            var res = _service.Delete(id);
            _pageModel.Data = _service.GetList(_pageModel.search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            _pageModel.ListHTML = list;
            return new JsonResult(new { response = res, listData = _pageModel.ListHTML });
        }
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.search = new ProductCategorySearchModel();
            var res = _repProductCategory.Enable(ids, isEnable,CreateUser);
            _pageModel.Data = _service.GetList(_pageModel.search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = "";
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            return new JsonResult(new { response = res, listData = list });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.search = new ProductCategorySearchModel();

            var res = _repProductCategory.Move(fromId, toId);
            string list = "";
            _pageModel.Data = _service.GetList(_pageModel.search).Data;
            _pageModel.List = _pageModel.Data.Items;

            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            return new JsonResult(new { response = res, listData = list });
        }
        public JsonResult EnableUpdateOrder(ProductCategorySearchModel search)
        {
            var res = _repProductCategory.EnableUpdateOrder();
            string list = "";
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            list += InsertRow(_pageModel.List.Where(p => p.ParentID == 0).ToList(), 0);
            return new JsonResult(new { response = res, listData = list });
        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.search = new ProductCategorySearchModel();
            var res = _repProductCategory.UpdateOrder(id, order);
            return new JsonResult(new { response = res });
        }
        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab); 
            return new JsonResult(new { response = res });
        }
    }
}
