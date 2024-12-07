using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductSpecificatyController : Controller
    {
        public readonly IProductSpecificatyService _service;
        public readonly ILoggerHelper _logger;
        public readonly IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public EditModel _editModel = new EditModel();

        public ProductSpecificatyController(IViewRendererHelper viewRender, ILoggerHelper logger, IProductSpecificatyService rep)
        {
            _viewRender = viewRender;
            _service = rep;
            _logger = logger;
        }
    }
}
