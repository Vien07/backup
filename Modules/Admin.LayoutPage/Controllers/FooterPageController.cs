//using Admin.FooterPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
using Admin.LayoutPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using System.Dynamic;
using X.PagedList;
using static Admin.LayoutPage.Constants.FooterPageConstants;
using Admin.LayoutPage.Services;
using Steam.Infrastructure.Repository;
namespace Admin.LayoutPage.Controllers
{


    public partial class FooterPageController : Controller
    {
        public IFooterPageService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.FooterPageConfig> _repFooterPageConfig;
        private readonly IRepository<Database.FooterPage> _repFooterPage;

        public FooterPageModel _pageModel = new FooterPageModel();
        Dictionary<string, string> _config;
        private readonly IConfiguration configuration;
        string AppKey = "";
        public FooterPageController(
            IFooterPageService srv,
            IViewRendererHelper viewRender,
            IRepositoryConfig<Database.FooterPageConfig> repFooterPageConfig,
            ILoggerHelper logger)
        {
            _repFooterPageConfig = repFooterPageConfig;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _config = _repFooterPageConfig.GetAllConfigs();
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
        public JsonResult GenerateFooterHtml(long footerPid)
        {

            var res = _srv.GenerateFooterHtml(footerPid);

            return new JsonResult(new {  res });

        }
        public JsonResult UpdateFooterHtml(long footerPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();

            requestModel.HeaderHtml = "";

            var res = _srv.GenerateFooterHtml(footerPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[Config.Website.ApiUpdateFooterPage].ToString();

                    requestModel.FooterHtml = res.Data;
                    var client = new RestClient(ApiUpdateHeader);
                    var request = new RestRequest();
                    request.AddHeader(nameof(AppKey), AppKey);
                    request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
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
            }
            else
            {
                responseObj.IsError = true;
            }
            return new JsonResult(new { res, responseObj });


        }
        public JsonResult UpdatePreviewFooterHtml(long footerPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();

            requestModel.HeaderHtml = "";

            var res = _srv.GenerateFooterHtml(footerPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[Config.Website.ApiUpdatePreviewFooterPage].ToString();

                    requestModel.FooterHtml = res.Data;
                    var client = new RestClient(ApiUpdateHeader);
                    var request = new RestRequest();
                    request.AddHeader(nameof(AppKey), AppKey);
                    request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
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
            }
            else
            {
                responseObj.IsError = true;
            }
            return new JsonResult(new { res, responseObj });


        }
        public JsonResult RevertFooterHtml()
        {
            dynamic responseObj = new ExpandoObject();

            try
            {

                var ApiRevertFooter = _config[Config.Website.ApiRevertFooterPage].ToString();
                var client = new RestClient(ApiRevertFooter);
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
