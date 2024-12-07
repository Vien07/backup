//using Admin.PostsCategory.Models;
using Admin.PostsCategory;
using Admin.PostsCategory.Constants;
using Admin.PostsCategory.Database;
using Admin.PostsCategory.Models;
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
using static Admin.PostsCategory.Constants.PostsCategoryConstants;
using Admin.PostsCategory.Services;
namespace Admin.PostsCategory.Controllers
{

    public partial class PostsCategoryController : Controller
    {
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        private readonly IConfiguration configuration;

        public string CreateUser = "admin";
        Dictionary<string, string> _CONFIG;
        string AppKey = "";
        string RestartWebsiteApi = "";
        private readonly IRepository<Database.PostsCategory> _repPostsCategory;
        private readonly IRepository<Database.PostsCategory_Files> _repPostsCategory_Files;
        private readonly IRepositoryConfig<Database.PostsCategoryConfig> _repPostsCategoryConfig;
        public IPostsCategoryService _srv;

        public PostsCategoryController(
            IPostsCategoryService srv,
            IViewRendererHelper viewRender,
            IRepository<Database.PostsCategory> repPostsCategory,
            IRepository<Database.PostsCategory_Files> repPostsCategory_Files,
            IRepositoryConfig<Database.PostsCategoryConfig> repPostsCategoryConfig,
            ILoggerHelper logger)
        {
            _repPostsCategory = repPostsCategory;
            _repPostsCategory_Files = repPostsCategory_Files;
            _repPostsCategoryConfig = repPostsCategoryConfig;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _pageModel.Configs = _repPostsCategoryConfig.GetAllConfigs();
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());

            _CONFIG = _repPostsCategoryConfig.GetAllConfigs();
            AppKey = configuration["Website:AppKey"].ToString();
            RestartWebsiteApi = configuration["Website:RestartWebsiteApi"].ToString();
        
        }
        public JsonResult GenerateXMLRewriteUrl()
        {

            var res = _srv.GenerateXMLRewriteUrl();

            return new JsonResult(new { res });

        }
        public JsonResult UpdateRewritePostUrl()
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();

            requestModel.RewritePostUrlXML = "";

            var res = _srv.GenerateXMLRewriteUrl();
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _CONFIG[Config.Website.ApiUpdateRewritePostUrl].ToString();

                    requestModel.RewritePostUrlXML = res.Data;
                    var client = new RestClient(ApiUpdateHeader);
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
                        var response1 = client1.ExecutePost(request1);

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
