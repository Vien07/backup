using Admin.SEO.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.STeamHelper;
using static Admin.SEO.SEOHelper;
using Admin.SEO.Services;

namespace Admin.SEO.Integrate
{
    public class EditModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("SEO", "Thông tin", "fas fa-layer-group", "/SEO");

        public Database.SEO Detail = new Database.SEO();
        public List<Database.SEO_Files> ListFiles = new List<Database.SEO_Files>();



    }

    public class SEOIntegrateController : Controller
    {

        public ISEOService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        IMetaHelper _metaHelper;

        public string CreateUser = "admin";
        EditModel _editmodel = new EditModel();
        public SEOIntegrateController(ISEOService srv, IMetaHelper metaHelper, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _metaHelper = metaHelper;
        }
        //[HttpPost("SEOIntegrated/GetList")]

        [HttpPost]
        public JsonResult Save(SEOIntegrateModelEdit add)
        {
            if(add.PostSlug!=null)
            {
                add.PostSlug = add.PostSlug.ToString();

            }

            if (String.IsNullOrEmpty(add.Meta))
            {
                MetaModel modelMeta = new MetaModel();
                modelMeta.PageTitle = add.PostTitle;
                modelMeta.Description = add.MetaDescription;
                modelMeta.Keywords = add.TagKeys;
                modelMeta.OgType = add.OgType;
                modelMeta.OgImage = add.OgImage;
                add.Meta = (_metaHelper.GenerateMetaTag(modelMeta) + add.ExtraMeta ?? "").ToRemoveBreakSympol() ;
            }
            add.CreateUser = CreateUser;
            add.UpdateUser = CreateUser;
            add.UpdateDate = DateTime.Now;

            add.Pid = add.SEOPid;
            var res = _srv.SaveSEO(add);

            //var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = "" });
        }

        [HttpPost]
        public JsonResult GenerateMetaTag(SEOIntegrateModelEdit add)
        {
            MetaModel modelMeta = new MetaModel();
            modelMeta.PageTitle = add.PostTitle;
            modelMeta.Description = add.MetaDescription;
            modelMeta.Keywords = add.TagKeys!=null? add.TagKeys : "";
            modelMeta.OgType = add.OgType;

            modelMeta.OgImage = add.OgImage ?? "";         
            string meta = _metaHelper.GenerateMetaTag(modelMeta) + add.ExtraMeta ?? "";
            return new JsonResult(new { response = meta});
        }
    }
}
