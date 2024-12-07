//using Admin.QuickToolBar.Models;
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
using Admin.LayoutPage.Services;
using Steam.Infrastructure.Repository;
namespace Admin.LayoutPage.Controllers
{


    public partial class QuickToolBarController : Controller
    {
        public IQuickToolBarService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        Dictionary<string, string> _config;
        private readonly IConfiguration configuration;
        string AppKey = "";
        private readonly IRepositoryConfig<Database.QuickToolBarConfig> _repConfig;
        private readonly IRepository<Database.QuickToolBar> _repQuickToolBar;
        public QuickToolBarModel _pageModel = new QuickToolBarModel();
        public QuickToolBarController(
            IRepositoryConfig<Database.QuickToolBarConfig> repConfig,
            IRepository<Database.QuickToolBar> repQuickToolBar,
            IQuickToolBarService srv, 
            IViewRendererHelper viewRender, 
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repQuickToolBar = repQuickToolBar;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _pageModel.Configs = _repConfig.GetAllConfigs();

            _config = _repConfig.GetAllConfigs();
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
        public JsonResult GenerateQuickToolBarHtml(long QuickToolBarPid)
        {

            var res = _srv.GenerateQuickToolBarHtml(QuickToolBarPid);

            return new JsonResult(new { res });

        }
        public JsonResult UpdateQuickToolBarHtml(long QuickToolBarPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateQuickToolBarHtml(QuickToolBarPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[QuickToolBarConstants.Config.Website.ApiUpdateQuickToolBar].ToString();

                    requestModel.QuickToolBarHtml = res.Data;
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
        public JsonResult UpdatePreviewQuickToolBarHtml(long QuickToolBarPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateQuickToolBarHtml(QuickToolBarPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[QuickToolBarConstants.Config.Website.ApiUpdatePreviewQuickToolBar].ToString();

                    requestModel.QuickToolBarHtml = res.Data;
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
        public JsonResult RevertQuickToolBarHtml()
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
