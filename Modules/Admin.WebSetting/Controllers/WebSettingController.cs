using Admin.WebSetting.Constants;
using Admin.WebSetting.Models;
using Admin.WebSetting.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Configuration;
using System.Dynamic;

namespace Admin.WebSetting.Controllers
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Cấu hình Website", "Danh sách", "fas fa-layer-group", "/WebSetting");
        public Dictionary<string, string> EditModel;
        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion

    public partial class WebSettingController : Controller
    {
        public IWebSettingService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        string AppKey = "";

        private readonly IRepository<Database.WebsiteConfiguration> _repWebsiteConfiguration;


        public PageModel _pageModel = new PageModel();
        public WebSettingController(
            IWebSettingService srv,
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _srv = srv;


        }
        public ActionResult Save(WebSettingViewModelModelEdit model)
        {

            var res = _srv.Save(model);
            if(!res.IsError)
            {
                UpdateWebConfig();
            }    
            return new JsonResult(new { response = res });
        }
        public async void UpdateWebConfig()
        {
            try
            {
                var conf = _srv.GetWebSetting().Data;
                Dictionary<string,string> requestModel = new Dictionary<string, string>();
                Response<bool> responseObj = new Response<bool>();
                requestModel = _srv.GetWebSiteConfig().Data;

                var client = new RestClient(conf[WebSettingConstants.ConfigName.ApiUpdateWebconfigValue]);
                var request = new RestRequest();
                request.Timeout = 2000;

                request.AddHeader("AppKey", SystemInfo.AppKey);
                request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
                var response = await client.ExecutePostAsync(request);
                //if (response.IsSuccessful)
                //{
                //    responseObj =  JsonConvert.DeserializeObject<Response<bool>>(response.Content);

                //}
                //else
                //{
                //    responseObj.Message = response.ErrorMessage.ToString();
                //}
            }
            catch (Exception)
            {

            }
        }
    }
}
