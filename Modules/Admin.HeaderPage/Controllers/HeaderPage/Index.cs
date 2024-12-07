//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Constants;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Models;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using System.Dynamic;
using X.PagedList;

namespace Admin.HeaderPage
{
    public partial class HeaderPageController 
    {

        public IActionResult Index()
        {

            _pageModel.Search = new ParamSearch();
            _pageModel.EditModel = new Database.HeaderPage();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _rep.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _rep.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _rep.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<int> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _rep.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _rep.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _rep.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var res = _rep.SaveConfig(formData, tab);

            return new JsonResult(new { response = res });

        }     
        public JsonResult GenerateHeaderHtml()
        {

            var res = _rep.GenerateHeaderHtml();

            return new JsonResult(new { response = res });

        }    
        public JsonResult UpdateHeaderHtml()
        {
            dynamic requestModel = new ExpandoObject();
            dynamic responseObj = new ExpandoObject();
            requestModel.HeaderHtml = "";

            var res = _rep.GenerateHeaderHtml();
            try
            {
                var ApiUpdateHeader = _config[HeaderPageConstants.Config.Website.ApiUpdateHeader].ToString();

                requestModel.HeaderHtml = res.Data;
                var client = new RestClient(ApiUpdateHeader);
                var request = new RestRequest();
                request.AddHeader(nameof(AppKey), AppKey);
                request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
                var response = client.ExecutePost(request);
                if(response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                responseObj.Message = "Exception: " + ex.Message.ToString();

            }
            return new JsonResult(new { response = res, responseFromApi= responseObj });


        } 
        public JsonResult RevertHeaderHtml()
        {
            dynamic responseObj = new ExpandoObject();

            try
            {

               var ApiRevertHeader = _config[HeaderPageConstants.Config.Website.ApiRevertHeader].ToString();
                var client = new RestClient(ApiRevertHeader);
                var request = new RestRequest();
                request.AddHeader(nameof(AppKey), AppKey);
                var response = client.ExecutePost(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<dynamic>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }

            }
            catch (Exception ex)
            {
                responseObj.Message = "Exception: " + ex.Message.ToString();
            }
            return new JsonResult(new { responseFromApi = responseObj });


        }


    }

}
