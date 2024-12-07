using Admin.Collection.Constants;
using Admin.Collection.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Collection
{

    public partial class CollectionController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Bộ sưu tập", "Thông tin", "fas fa-layer-group", "/Collection");

            public Database.Collection Detail = new Database.Collection();
            public List<Database.Collection_Files> ListFiles = new List<Database.Collection_Files>();
            public List<SelectControlData> ListSubCate = new List<SelectControlData>();

            public IPagedList<Collection_Product_Item> ListCollectionProducts;
            public Collection_Product_List ListProducts;
            public List<SelectControlData> ListCatesForCollection = new List<SelectControlData>();

            public PaginationModel Pagination_TableChooseProduct = new PaginationModel();



        }
        EditModel _editModel = new EditModel();

        [HttpGet("Collection/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            Collection_Product_List.ParamSearch search = new Collection_Product_List.ParamSearch();
            if (id != 0)
            {
                var data = _rep.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                    _editModel.ListFiles = data.ListFiles;
                    _editModel.ListCollectionProducts = _rep.GetListProductOfCollection(data.Detail.Pid);
                    search.ChoosenProducts = data.Detail.ProductIDs;

                    _editModel.ListSubCate = _rep.GetChildrenPostCategory(id).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title,
                         ParrentID = row.ParentID,
                     }
                    ).ToList<SelectControlData>(); ;

                }
            }
            _editModel.ListProducts = _rep.GetListProducts(search);

            _editModel.ListCatesForCollection = _rep.GetCatesForCollection();

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(CollectionModelEdit data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            _pageModel.Search = new CollectionModel.ParamSearch();
            var res = _rep.Save(data);

            var listData = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public ActionResult GetListSubCate(int parentId)
        {
            var listCate = _rep.GetChildrenPostCategory(parentId);
            TreeSelectModel treeSelectModel = new TreeSelectModel();

            treeSelectModel.MapToObj(listCate, "Pid", "Title", "ParentID", "Order", "Enabled", parentId.ToString());
            var jsonSubCate = treeSelectModel.TreeSelectDataJson();

            //var listData = Component.InvokeAsync(nameof(ComponentUILibrary.ViewComponents.TreeSelectComponent), treeSelectModel).ToString();
            //return ViewComponent("TreeSelectComponent",treeSelectModel);

            return new JsonResult(new { jsonSubCate });
        }

        [HttpPost]
        public JsonResult SearchProductForCollection(Collection_Product_List.ParamSearch search)
        {
            _editModel.ListProducts = _rep.GetListProducts(search);
            string list = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Edit_TableChooseProduct, _editModel.ListProducts);


            _editModel.Pagination_TableChooseProduct.PageIndex = search.PageIndex;
            _editModel.Pagination_TableChooseProduct.PageCount = _editModel.ListProducts.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _editModel.Pagination_TableChooseProduct);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult AddProductToCollection(string sku)
        {
            var listProduct = _rep.GetProductOfCollection(sku);

            string list = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Edit_RowTableColleciton, listProduct);


            //_editModel.Pagination_TableChooseProduct.PageIndex = search.PageIndex;
            //_editModel.Pagination_TableChooseProduct.PageCount = _editModel.ListProducts.PageCount;
            //string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _editModel.Pagination_TableChooseProduct);
            return new JsonResult(new { list = list, paging = "" });
        }

    }
}
