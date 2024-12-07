//using Admin.SEO.Models;
using Admin.SEO;
using Admin.SEO.Constants;
using Admin.SEO.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.SEO.Controllers
{

    public partial class SEOController
    {

        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("SEO", "Thông tin", "fas fa-layer-group", "/SEO");

            public Database.SEO Detail = new Database.SEO();
            public List<Database.SEO_Files> ListFiles = new List<Database.SEO_Files>();




        }
        EditModel _editModel = new EditModel();

        [HttpGet("SEO/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                    _editModel.ListFiles = data.ListFiles;
                }
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(SaveModel data )
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;

            var res = _srv.Save(data);

            //var listData = _viewRender.RenderPartialViewToString(SEOConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = "" });
        }
        [HttpPost]
        public JsonResult GenerateMetaTag(SaveModel add)
        {
            MetaModel modelMeta = new MetaModel();
            modelMeta.PageTitle = add.PostTitle;
            modelMeta.Description = add.MetaDescription;
            modelMeta.Keywords = add.TagKeys != null ? add.TagKeys : "";
            modelMeta.OgType = add.OgType;

            modelMeta.OgImage = add.OgImage ?? "";
            string meta = _metaHelper.GenerateMetaTag(modelMeta) + add.ExtraMeta ?? "";
            return new JsonResult(new { response = meta });
        }
    }
}
