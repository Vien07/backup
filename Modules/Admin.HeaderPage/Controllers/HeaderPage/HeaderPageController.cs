//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HeaderPage
{
    #region Define
    public class HeaderPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("HeaderPage", "Danh sách", "fas fa-layer-group", "/HeaderPage");


    

        public IPagedList<Database.HeaderPage> List;
        public List<Database.HeaderPageConfig> Configs;
        public Database.HeaderPage EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion

    public partial class HeaderPageController : Controller
    {
        public IHeaderPageRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        Dictionary<string, string> _config;
        private readonly IConfiguration configuration;
        string AppKey="";


        public HeaderPageModel _pageModel = new HeaderPageModel();
        public HeaderPageController(IHeaderPageRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
            _pageModel.Configs = _rep.GetAllConfigs().Data;
            _config = _pageModel.Configs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            AppKey = configuration["Website:AppKey"].ToString();
        }
    }

}
