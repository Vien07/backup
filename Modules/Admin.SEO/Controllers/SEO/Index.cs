using Admin.SEO.Constants;
using Admin.SEO.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common;
namespace Admin.SEO.Controllers
{
    public partial class SEOController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("SEO", "Danh sách", "fas fa-layer-group", "/SEO");




            //public IPagedList<Database.SEO> List;
            public SEO_List List;
            public Dictionary<string, string> Configs;
            public Database.SEO EditModel;
            public ParamSearch Search;
            public PaginationModel Pagination = new PaginationModel();
            public List<SelectControlData> Filter_ParrentPostsCategory = new List<SelectControlData>();
            public List<SelectControlData> Filter_Modules = new List<SelectControlData>();
            public void InitModulesData(string input)
            {
                try
                {
                    var listTemp = input.Split(';');
                    foreach (var item in listTemp)
                    {
                        var tempType = item.Split(":");
                        if (tempType.Length == 2)
                        {
                            this.Filter_Modules.Add(new SelectControlData() { Name = tempType[0], Value = tempType[1].Trim(), Order = 0 });

                        }
                    }
                }
                catch (Exception)
                {

                    this.Filter_Modules = new List<SelectControlData>();
                }

            }


        }
        #endregion

        public IActionResult Index()
        {
            _pageModel.Configs = _CONFIG;

            _pageModel.Search = new ParamSearch();
            _pageModel.EditModel = new Database.SEO();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            //_pageModel.InitModulesData(_pageModel.Configs.GetConfigValue(Constants.SEOConstants.Config.Admin.Modules) );
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }

        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repSEO.Enable(ids, isEnable, CreateUser);
            var listData = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repSEO.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repSEO.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repSEO.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());

            var res = _repSEOConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }

    }

}
