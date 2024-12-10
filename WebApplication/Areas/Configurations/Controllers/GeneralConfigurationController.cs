using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Configurations.Models;
using System.IO;
using DTO;
using Newtonsoft.Json;
using DTO.Common;
using System.Collections.Generic;
using System;
using CMS.Services.CommonServices;
using CMS.Areas.Shared.Helper;
using CMS.Services.FileServices;

namespace CMS.Areas.Configurations.Controllers
{
    [Area("Configurations")]
    public class GeneralConfigurationController : Controller
    {
        private readonly IGeneralConfigurationRepository _rep;
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string PageLimitAdmin = "";
        public string DateFormat = "";
        private IFileServices _fileServices;
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        public GeneralConfigurationController(IGeneralConfigurationRepository rep, IHttpContextAccessor httpContextAccessor, ICommonServices common, IFileServices fileServices)
        {
            _rep = rep;
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
            _fileServices = fileServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Action
        [HttpPost]
        public JsonResult UpdateSetting(IFormCollection formData)
        {
            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            foreach (var item in formData.Files)
            {
                //var id = _common.GenerateRandomString(6);

                IFormFile file = item;
                //var filename = id + "_" + file.FileName;

                //var filenameSplit = file.FileName.Split('.');
                //string extend = "";
                //if (filenameSplit.Length > 1)
                //{
                //    extend = filenameSplit[1];
                //}
                //else
                //{
                //    extend = "png";
                //}
                //string newFileName = item.Name + "." + extend;

                //var filePath = Path.Combine(absolutepath + "\\wwwroot" + ConstantStrings.UrlConfigurationImages, newFileName);
                //using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                //{
                //    file.CopyTo(fileStream);
                //}
                //Configuration child = new Configuration();
                //child.Key = item.Name;
                //child.Value = newFileName;
                //dynamic kt = _rep.Update(child, -1);


                if (item.Name == "home-image"
                    || item.Name == "home-image-mobile"
                    || item.Name == "faq-image" 
                    || item.Name == "bct-image" 
                    || item.Name == "certificate-image"
                    || item.Name == "feature-image"
                    )
                {
                    var rs = _fileServices.SaveFileOriginal(file, ConstantStrings.UrlConfigurationImages, item.Name);

                    if (!rs.isError)
                    {
                        Configuration child = new Configuration();
                        child.Key = item.Name;
                        child.Value = rs.fileName;
                        dynamic kt = _rep.Update(child, -1);
                    }
                }
                else
                {
                    var rs = _fileServices.SaveFileNotResizeNotRename(file, ConstantStrings.UrlConfigurationImages, item.Name);

                    if (!rs.isError)
                    {
                        Configuration child = new Configuration();
                        child.Key = item.Name;
                        child.Value = rs.fileName;
                        dynamic kt = _rep.Update(child, -1);
                    }                   
                }

            }
            string keyError = "";
            foreach (var item in formData)
            {
                Configuration child = new Configuration();
                child.Key = item.Key;
                child.Value = item.Value;
                dynamic kt = _rep.Update(child, 1);
                if (!kt.Error)
                {
                    keyError += kt.Name + ";";
                }
            }
            var data = _rep.GetList();

            return Json(new { jsData = data, keyError = keyError });
        }
        [HttpGet]
        public JsonResult GetList()
        {
            var data = _rep.GetList();
            return Json(new { jsData = data });
        }
        #endregion

        #region Email template
        //public IActionResult Compose(string lang)
        //{
        //    _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
        //    return PartialView("Modal");
        //}

        [CustomAuthorization("GeneralConfiguration", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj)
        {

            Models.EmailTemplate objDetail = JsonConvert.DeserializeObject<Models.EmailTemplate>(stringObj);
            List<MultiLang_EmailTemplate> objList = JsonConvert.DeserializeObject<List<MultiLang_EmailTemplate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("GeneralConfiguration", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        public JsonResult LoadContactData(string lang)
        {

            var data = _rep.LoadContactData(lang);
            return Json(data);
        }
        [CustomAuthorization("GeneralConfiguration", "VIEW")]
        public JsonResult Edit(int Pid)
        {
            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("GeneralConfiguration", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj)
        {
            Models.EmailTemplate objDetail = JsonConvert.DeserializeObject<Models.EmailTemplate>(stringObj);
            List<Models.MultiLang_EmailTemplate> objList = JsonConvert.DeserializeObject<List<Models.MultiLang_EmailTemplate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("GeneralConfiguration", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("GeneralConfiguration", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("GeneralConfiguration", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });
        }
        public ActionResult OpenAddModal(string lang)
        {
            lang = "vi";
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
    }
    #endregion
}