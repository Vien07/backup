using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using System.Dynamic;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductCategoryController : Controller
    {
        public readonly IProductCategoryService _service;
        public readonly ILoggerHelper _logger;
        public readonly IViewRendererHelper _viewRender;
        private readonly IConfiguration configuration;
        private readonly IRepositoryConfig<Database.ProductCategoryConfig> _repConfig;
        private readonly IRepository<Database.ProductCategory> _repProductCategory;

        public string CreateUser = "admin";
        Dictionary<string, string> _config;
        string AppKey = "";
        string RestartWebsiteApi = "";

        public ProductCategoryController(
            IProductCategoryService service,
            IViewRendererHelper viewRender,
            IRepository<Database.ProductCategory> repProductCategory,
            IRepositoryConfig<Database.ProductCategoryConfig> repConfig,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;

            _viewRender = viewRender;
            _service = service;
            _logger = logger;
            _repProductCategory = repProductCategory;
            _pageModel.Configs = _repConfig.GetAllConfigs();
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            _config = _repConfig.GetAllConfigs();
            AppKey = configuration["Website:AppKey"].ToString();
            RestartWebsiteApi = configuration["Website:RestartWebsiteApi"].ToString();

        }
        public JsonResult GenerateXMLRewriteUrl()
        {
            var res = _service.GenerateXMLRewriteUrl();
            return new JsonResult(new { res });
        }
        public JsonResult UpdateRewriteProductUrl()
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();

            requestModel.RewriteProductUrlXML = "";

            var res = _service.GenerateXMLRewriteUrl();
            if (!res.IsError)
            {
                try
                {
                    var apiUpdateHeader = _config[ProductCategoryConstants.Config.Website.ApiUpdateRewriteProductUrl].ToString();
                    requestModel.RewriteProductUrlXML = res.Data;
                    var client = new RestClient(apiUpdateHeader);
                    var request = new RestRequest();
                    request.AddHeader(nameof(AppKey), AppKey);
                    request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
                    var response = client.ExecutePost(request);
                    if (response.IsSuccessful)
                    {
                        responseObj = JsonConvert.DeserializeObject<Response<bool>>(response.Content);
                        var client1 = new RestClient(RestartWebsiteApi);
                        var request1 = new RestRequest();
                        request1.AddHeader(nameof(AppKey), AppKey);
                        _ = client1.ExecutePost(request1);
                    }
                    else
                    {
                        responseObj.Message = response.ErrorMessage.ToString();
                    }
                }
                catch (Exception ex)
                {
                    responseObj.Message = "Exception: " + ex.Message.ToString();
                    responseObj.IsError = true;
                }
            }
            else
            {
                responseObj.IsError = true;
            }
            return new JsonResult(new { res, responseObj });
        }
        public JsonResult RestartWebsite()
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            try
            {
                var client = new RestClient(RestartWebsiteApi);
                var request = new RestRequest();
                request.AddHeader(nameof(AppKey), AppKey);
                var response = client.ExecutePost(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<bool>>(response.Content);
                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                responseObj.Message = "Exception: " + ex.Message.ToString();
                responseObj.IsError = true;
            }
            return new JsonResult(new { responseObj });
        }
    }
}
