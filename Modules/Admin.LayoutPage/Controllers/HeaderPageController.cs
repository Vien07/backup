//using Admin.LayoutPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
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
using System.Dynamic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{
    #region Define
    public class HeaderPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Cấu hình Header", "Danh sách", "fas fa-layer-group", "~/HeaderPage");




        public IPagedList<Database.HeaderPage> List;
        public Dictionary<string, string> Configs;
        public Database.HeaderPage EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion

    public partial class HeaderPageController : Controller
    {
        public IHeaderPageService _srv;
        private readonly IRepositoryConfig<Database.HeaderPageConfig> _repHeaderPageConfig;
        private readonly IRepository<Database.HeaderPage> _repHeaderPage;

        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        Dictionary<string, string> _config;
        private readonly IConfiguration configuration;
        string AppKey = "";


        public HeaderPageModel _pageModel = new HeaderPageModel();
        public HeaderPageController(
             IRepository<Database.HeaderPage> repHeaderPage,
            IRepositoryConfig<Database.HeaderPageConfig> repHeaderPageConfig,
            IHeaderPageService srv, 
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _repHeaderPage = repHeaderPage;
            _repHeaderPageConfig = repHeaderPageConfig;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _pageModel.Configs = _repHeaderPageConfig.GetAllConfigs();
            _config = _repHeaderPageConfig.GetAllConfigs(); ;
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
        public JsonResult GenerateHeaderHtml(long pid)
        {

            var res = _srv.GenerateHeaderHtml(pid);

            return new JsonResult(new {   res });

        }
        public JsonResult UpdatePreviewHeaderHtml(long pid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateHeaderHtml(pid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[HeaderPageConstants.Config.Website.ApiUpdatePreviewHeader].ToString();

                    requestModel.HeaderHtml = res.Data;
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
                responseObj.IsError= true;
            }
            return new JsonResult(new { res, responseObj });


        }     
        public JsonResult UpdateHeaderHtml(long pid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateHeaderHtml(pid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[HeaderPageConstants.Config.Website.ApiUpdateHeader].ToString();

                    requestModel.HeaderHtml = res.Data;
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
                responseObj.IsError= true;
            }
            return new JsonResult(new { res, responseObj });


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
