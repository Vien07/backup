using Admin.PostsManagement.Services;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductController : Controller
    {
        public IProductService _service;
        public IPostsManagementService _postService;
        public IProductPolicyService _repPolicy;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public ProductController(
            IProductService service,
            IProductPolicyService repPolicy,
            IViewRendererHelper viewRender,
            ILoggerHelper logger,
            IPostsManagementService postService)
        {
            _viewRender = viewRender;
            _service = service;
            _postService = postService;
            _repPolicy = repPolicy;
            _logger = logger;
            _pageModel.Configs = _service.GetAllConfigs().Data.Items;
            _editModel.ProductTypes = _pageModel.Configs.Where(p => p.Key == ProductConstants.ConfigAdmin.ProductTypes).FirstOrDefault()?.Value;
        }
    }
}
