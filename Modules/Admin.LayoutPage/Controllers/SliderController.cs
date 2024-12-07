//using Admin.Slider.Models;
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


    public partial class SliderController : Controller
    {
        ISliderService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        Dictionary<string, string> _config;
        private readonly IConfiguration configuration;
        private readonly IRepositoryConfig<Database.SliderConfig> _repConfig;
        private readonly IRepository<Database.Slider> _repoSlider;
        string AppKey = "";

        public SliderModel _pageModel = new SliderModel();
        public SliderController(
            ISliderService srv,
            IRepositoryConfig<Database.SliderConfig> repConfig,
            IRepository<Database.Slider> repoSlider,
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repoSlider = repoSlider;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _pageModel.Configs = _repConfig.GetAllConfigs();

            _config = _repConfig.GetAllConfigs();
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
        public JsonResult GenerateSliderHtml(long sliderPid)
        {

            var res = _srv.GenerateSliderHtml(sliderPid);

            return new JsonResult(new { res });

        }
        public JsonResult UpdateSliderHtml(long sliderPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateSliderHtml(sliderPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[SliderConstants.Config.Website.ApiUpdateSlider].ToString();

                    requestModel.SliderHtml = res.Data;
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
        public JsonResult UpdatePreviewSliderHtml(long sliderPid)
        {
            dynamic requestModel = new ExpandoObject();
            Response<bool> responseObj = new Response<bool>();
            requestModel.HeaderHtml = "";

            var res = _srv.GenerateSliderHtml(sliderPid);
            if (!res.IsError)
            {
                try
                {
                    var ApiUpdateHeader = _config[SliderConstants.Config.Website.ApiUpdatePreviewSlider].ToString();

                    requestModel.SliderHtml = res.Data;
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
        public JsonResult RevertSliderHtml()
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
