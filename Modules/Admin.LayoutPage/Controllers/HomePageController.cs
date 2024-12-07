using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
using Admin.LayoutPage.Database;
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{


    public partial class HomePageController : Controller
    {
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepositoryConfig<HomePageConfig> _repConfig;
        IHomePageService _srv;
        private readonly IRepository<Database.HomePage> _repoHomePage;
        public PageModel _pageModel = new PageModel();
        private readonly IConfiguration configuration;
        string AppKey = "";
        Dictionary<string, string> _config;

        public HomePageController(
            IHomePageService srv,
            IRepositoryConfig<HomePageConfig> repConfig,
            IRepository<Database.HomePage> repoHomePage,
        IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repoHomePage = repoHomePage;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _pageModel.Configs = _repConfig.GetAllConfigs();
            _config = _repConfig.GetAllConfigs();

            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
        public JsonResult GenerateHomePageHtml(long pid)
        {

            var res = _srv.GenerateHomePageHtml(pid);

            return new JsonResult(new { res });

        }
        public JsonResult UpdateHomePageHtml()
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            //requestModel.HeaderHtml = "";

            var res = _srv.GenerateHomePageHtml(0);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHomepage = _config[HomePageConstants.Config.Website.ApiUpdateHomePage].ToString();

                    requestModel.HomePageHtml = res.Data;
                    var client = new RestClient(ApiUpdateHomepage);
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
                        responseObj.IsError = true;

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
        public JsonResult UpdatePreviewHomePageHtml()
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            //requestModel.HeaderHtml = "";

            var res = _srv.GenerateHomePageHtml(0);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHomepage = _config[HomePageConstants.Config.Website.ApiUpdatePreviewHomePage].ToString();

                    requestModel.HomePageHtml = res.Data;
                    var client = new RestClient(ApiUpdateHomepage);
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
                        responseObj.IsError = true;

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
        public JsonResult RevertHomePageHtml()
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
