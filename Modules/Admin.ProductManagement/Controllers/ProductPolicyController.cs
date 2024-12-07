using Admin.ProductManagement.Models;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductPolicyController : Controller
    {
        public readonly IProductPolicyService _productPolicyService;
        public readonly ILoggerHelper _logger;
        public readonly IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public ProductPolicyController(IProductPolicyService rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _productPolicyService = rep;
            _logger = logger;
        }
    }
}
