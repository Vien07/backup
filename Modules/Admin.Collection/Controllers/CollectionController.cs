//using Admin.Collection.Models;
using Admin.Collection;
using Admin.Collection.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Collection
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Bộ sưu tập", "Danh sách", "fas fa-layer-group", "/Collection");




        public Collection_List List;
        public List<Database.CollectionConfig> Configs;
        public Database.Collection EditModel;
        public CollectionModel.ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion

    public partial class CollectionController : Controller
    {
        public ICollectionRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public CollectionController(ICollectionRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
    }

}
