using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.PostsCategory.Database;
using Admin.PostsCategory.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Policy;
using Admin.PostsCategory.Constants;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Drawing;
using ComponentUILibrary.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Steam.Core.Utilities.SteamModels;
using Admin.TemplatePage.Database;
using Org.BouncyCastle.Utilities;
using Admin.SEO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;

namespace Admin.PostsCategory.Services
{
    public class PostsCategoryService : IPostsCategoryService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _CONFIG;
        private readonly IRepository<Database.PostsCategory> _repPostsCategory;
        private readonly IRepository<Database.PostsCategory_Files> _repPostsCategory_Files;
        private readonly IRepositoryConfig<Database.PostsCategoryConfig> _repPostsCategoryConfig;
        private readonly IRepository<Admin.TemplatePage.Database.TemplatePage> _repTemplatePage;

        ISEOService _srvSEO;
        string langCode = "vi";
        public PostsCategoryService(
            IRepository<Database.PostsCategory_Files> repPostsCategory_Files,
            IRepository<Admin.TemplatePage.Database.TemplatePage> repTemplatePage, 
            ISEOService  srvSEO,
            IFileHelper fileHelper,
            IRepository<Database.PostsCategory> repPostsCategory,
            IRepositoryConfig<Database.PostsCategoryConfig> repPostsCategoryConfig,
            ILoggerHelper logger)
        {
            _repPostsCategory_Files = repPostsCategory_Files;
            _repPostsCategoryConfig = repPostsCategoryConfig;
            _repTemplatePage = repTemplatePage;
            _repPostsCategory = repPostsCategory;
            _logger = logger;
            _fileHelper = fileHelper;
            _srvSEO = srvSEO;
            _CONFIG = _repPostsCategoryConfig.GetAllConfigs();
        }
        public Response<List<PostsCategory_Item>> GetList(ParamSearch search)
        {

            Response<List<PostsCategory_Item>> rs = new Response<List<PostsCategory_Item>>();
            List<PostsCategory_Item> items = new List<PostsCategory_Item>();
            try
            {
                string rootSlug = _CONFIG[PostsCategoryConstants.Config.Website.PreSlug] ?? "";

                var tempListCate = _repPostsCategory.Query().Where(p => (search.Cate == "0" || p.ParentID == Convert.ToInt32(search.Cate)))
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
                var listCateParrent = _repPostsCategory.Query().Where(p => p.ParentID==0 && p.Enabled==true).ToList();
                if (search.Cate != "0")
                {
                    var parrentPostsCategory = _repPostsCategory.Query().Where(p => p.Pid == Convert.ToInt32(search.Cate)).FirstOrDefault();
                    tempListCate.Add(parrentPostsCategory);
                }
                var tempitems = (from item in tempListCate
                                 join b in listCateParrent on item.RootParentID equals b.Pid into tempJoin
                                 from c in tempJoin.DefaultIfEmpty()
                                 select new PostsCategory_Item
                                 {
                                     Banner = item.Banner,
                                     FullPathBanner = SystemInfo.VirtualFolder+ (item.BannerFilePath+ item.Banner).CheckExistsImage(),
                                     FullThumbnail = SystemInfo.VirtualFolder + (item.FilePath+item.Images).CheckExistsImage(),
                                     BannerFilePath = item.BannerFilePath,
                                     ImagePath = item.Images,
                                     FilePath = item.FilePath,
                                     Title = item.Title,
                                     Slug = item.Slug,
                                     Pid = item.Pid,
                                     ShowLevel = item.ShowLevel,
                                     ParentID = item.ParentID,
                                     Enabled = item.Enabled,
                                     WebsiteSlug ="/" +rootSlug+"/" + (item.ParentID == 0 ? item.Slug : (c != null ? c.Slug +"/"+ item.Slug : item.Slug)) +"/",
                                     WebsiteCatePage = item.WebsiteCatePage,
                                     WebsiteDetailPage = item.WebsiteDetailPage,
                                 }).ToList();
                if (tempitems != null)
                {
                    items.AddRange(tempitems);
                }
                rs.Data = items;

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
      
        public Response<PostsCategoryDetail> GetById(int id)
        {
            Response<PostsCategoryDetail> rs = new Response<PostsCategoryDetail>();
            PostsCategoryDetail detail = new PostsCategoryDetail();
            try
            {

                detail.Detail = _repPostsCategory.Query().Where(p => p.Pid == id).FirstOrDefault();
                //detail.Detail.GetLocalize(id, langCode);
                rs.IsError = false;
                rs.StatusCode = 200;
                rs.Data = detail;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());

            }
            return rs;
        }
        public Response<Database.PostsCategory> Save(PostsCategoryModelEdit input)
        {
            Database.PostsCategory data = input.GetDatabaseModel();

            //-------save-file-pond----------
            //-------save-file-pond----------
            var img_thumbnail = "";
            var filePath_thumbnail = "";
            var img_banner = "";
            var filePath_banner = "";
            SaveImage(ref img_thumbnail, ref filePath_thumbnail,
                input.FileStatus_Thumbnail,
                input.file_Thumbnail,
                input.FilePath_Thumbnail, input.Title);
            SaveImage(ref img_banner, ref filePath_banner,
                  input.FileStatus_Banner,
                  input.file_Banner,
                  input.FilePath_Banner, input.Title);
            var validator = new PostsCategoryValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.PostsCategory> rs = new Response<Database.PostsCategory>();
            using (var transaction = _repPostsCategory.BeginTransaction())
            {
                string path = "";
                try
                {

                    if (img_thumbnail != "")
                    {
                        data.Images = img_thumbnail;

                    }
                    if (filePath_thumbnail != "")
                    {
                        data.FilePath = filePath_thumbnail;

                    }

                    if (img_banner != "")
                    {
                        data.Banner = img_banner;

                    }
                    if (filePath_banner != "")
                    {
                        data.BannerFilePath = filePath_banner;

                    }

                    if (data.Pid == 0)
                    {
                        var tempParrent = _repPostsCategory.Query().Where(p => p.Pid == input.ParentID).FirstOrDefault();
                        data.Order = 0.9;

                        if (tempParrent != null)
                        {
                            data.ShowLevel = tempParrent.ShowLevel + 1;
                            path += tempParrent.Path + "-" + tempParrent.Pid;
                        }
                        data.Path = path;
                        var rootParent = _repPostsCategory.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                        var tempCheckRootParrent = rootParent.ParentID;
                        while (tempCheckRootParrent != 0)
                        {
                            rootParent = _repPostsCategory.Query().Where(p => p.Pid == tempCheckRootParrent).FirstOrDefault();
                            tempCheckRootParrent = rootParent.ParentID;

                        }
                        data.RootParentID = Convert.ToInt32(rootParent.Pid.ToString());
                        _repPostsCategory.Add(data);
                        _repPostsCategory.SaveChanges();

                    }
                    else
                    {
                        var model = _repPostsCategory.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            if (img_thumbnail != "")
                            {
                                model.Images = img_thumbnail;

                            }
                            if (filePath_thumbnail != "")
                            {
                                model.FilePath = filePath_thumbnail;

                            }
                            if (img_banner != "")
                            {
                                model.Banner = img_banner;

                            }
                            if (filePath_banner != "")
                            {
                                model.BannerFilePath = filePath_banner;

                            }
                            model.Title = data.Title;
                            model.Description = data.Description;
                            model.Slug = data.Slug;
                            model.WebsiteCatePage = data.WebsiteCatePage;
                            model.WebsiteDetailPage = data.WebsiteDetailPage;
                            model.UpdateDate = DateTime.Now;

                            if (model.ParentID != data.ParentID)
                            {

                                if (data.ParentID == 0)
                                {
                                    model.ShowLevel = 0;
                                    model.RootParentID = 0;
                                    path += data.Pid.ToString() + "-";
                                    //SetChildLevel(model.Pid, model.ShowLevel);
                                }
                                else
                                {
                                    var rootParent =_repPostsCategory.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                                    var tempCheckRootParrent = rootParent.ParentID;

                                    var modelParent = _repPostsCategory.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                                    model.ShowLevel = modelParent.ShowLevel + 1;
                                    path += modelParent.Path + "-" + modelParent.Pid;
                                    while (tempCheckRootParrent != 0)
                                    {
                                        rootParent = _repPostsCategory.Query().Where(p => p.Pid == tempCheckRootParrent).FirstOrDefault();
                                         tempCheckRootParrent = rootParent.ParentID;

                                    }
                                    model.RootParentID =Convert.ToInt32(rootParent.Pid.ToString()) ;
                                }
                                model.ParentID = data.ParentID;
                                model.Path = path;
                                _repPostsCategory.SaveChanges();

                                SetChildLevel(model.Pid, model.ShowLevel, model.Enabled, model.Path);

                            }
                            _repPostsCategory.SaveChanges();

                        }


                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

                }
            }
            rs.Data = data;
            return rs;

        }
        public void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
        {
            try
            {

                if (filestatus != "existed")
                {

                    if (!String.IsNullOrEmpty(filePath))
                    {
                        var arrFile = filePath.Split('/');
                        img = arrFile[arrFile.Length - 1];
                        imgFilePath = SystemInfo.PathFileManager + "/" + filePath.Replace(img, "");
                    }
                    else
                    {
                        if (file != null)
                        {
                            img = _fileHelper.UploadImageModule(
                                 new UploadImageInfo
                                 {
                                     FileName = title.ToSlug(),
                                     Height = Convert.ToInt32(_CONFIG[PostsCategoryConstants.Config.Admin.MaxHeight].ToString()),
                                     Width = Convert.ToInt32(_CONFIG[PostsCategoryConstants.Config.Admin.MaxWidth].ToString()),
                                     Path = PostsCategoryConstants.StaticPath.Asset.Image,
                                     PathThumb = PostsCategoryConstants.StaticPath.Asset.ImageThumb,
                                     File = file
                                 }
                                 ).FileName;
                            imgFilePath = PostsCategoryConstants.StaticPath.Asset.Image;
                        }

                    }

                    //-------end-save-file-pond----------

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SaveListFile(Database.PostsCategory data, string listFIlesString)
        {
            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

            try
            {


            }
            catch (Exception ex)
            {

            }
        }
        public Response Delete(int id)
        {

            Response rs = new Response();

            try
            {
                _srvSEO.Delete(id, PostsCategoryConstants.ModuleInfo.ModuleCode);

                DeletePostsCategory(id);




            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }
        void DeletePostsCategory(long id)
        {
            var model = _repPostsCategory.Query().Where(p => p.Pid == id).FirstOrDefault();
            var listChild = _repPostsCategory.Query().Where(p => p.ParentID == id).ToList();
            if (listChild.Count() > 0)
            {
                foreach (var item in listChild)
                {
                    DeletePostsCategory(item.Pid);

                }
            }
            _repPostsCategory.Remove(model);
            _repPostsCategory.SaveChanges(); ;

        }




        public Response<List<SelectControlData>> GetPostsCategoryParent(int id)
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            List<SelectControlData> listParent = new List<SelectControlData>();
            try
            {
                var e = _repPostsCategory.Query().Where(p => p.Pid == id).FirstOrDefault();
                if (e != null)
                {
                    listParent = _repPostsCategory.Query().Where(p => p.Enabled == true && p.Pid != id && p.ShowLevel <= e.ShowLevel).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title
                     }
                    ).ToList<SelectControlData>();
                }
                else
                {
                    listParent = _repPostsCategory.Query().Where(p => p.Enabled == true && p.Pid != id).Select(
                       row => new SelectControlData
                       {
                           Value = row.Pid.ToString(),
                           Name = row.Title
                       }
                   ).ToList<SelectControlData>();
                }



                rs.Data = listParent;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<List<SelectControlData>> GetPostsCategoryTreeChildrenByParentId(long ParentId)
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            List<SelectControlData> items = new List<SelectControlData>();
            try
            {
                var parent = _repPostsCategory.Query().Where(p => p.Pid == ParentId && p.ParentID == 0 && p.Enabled == true && p.Deleted == false).FirstOrDefault();
                if (parent != null)
                {
                    var listChild = GetChildrenPostCategory(parent.Pid);
                    if (listChild != null && listChild.Count() > 0)
                    {
                        items = listChild.Select(
                             row => new SelectControlData
                             {
                                 Value = row.Pid.ToString(),
                                 Name = row.Title
                             }
                            ).ToList<SelectControlData>();
                    }

                }
                rs.Data = items;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<List<SelectControlData>> GetListTemplatePage(string type)
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            try
            {
                var list = _repTemplatePage.Query().Where(p => (type == "" || p.PageType == type) && p.Enabled == true).Select(
                     row => new SelectControlData
                     {
                         Value = row.Url,
                         Name = row.Url
                     }
                    ).ToList<SelectControlData>();




                rs.Data = list;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public void SetChildLevel(long Pid, int Showlevel, bool isEnabled, string path)
        {
            var modelChild = _repPostsCategory.Query().Where(p => p.ParentID == Pid).ToList();
            if (modelChild.Count > 0)
            {
                foreach (var child in modelChild)
                {

                    child.ShowLevel = Showlevel + 1;
                    child.Enabled = isEnabled;
                    child.Path = path + "-" + child.ParentID;
                    _repPostsCategory.SaveChanges();
                    SetChildLevel(child.Pid, child.ShowLevel, child.Enabled, child.Path);

                }
            }
        }
        public List<Admin.PostsCategory.Database.PostsCategory> GetChildrenPostCategory(long parentId)
        {
            var result = new List<Admin.PostsCategory.Database.PostsCategory>();
            var childrens = _repPostsCategory.Query().Where(s => s.ParentID == parentId && s.Enabled == true && s.Deleted == false);

            if (childrens != null && childrens.Count() > 0)
            {
                result.AddRange(childrens);
                foreach (var child in childrens)
                {
                    var childItems = GetChildrenPostCategory(child.Pid);
                    result.AddRange(childItems);
                }
            }
            return result;
        }


        public Response<string> GenerateXMLRewriteUrl()
        {
            Response<string> rs = new Response<string>();
            try
            {
                var listPageTemplate = _repTemplatePage.Query().ToList();
                PostsCategoryHelper helper = new PostsCategoryHelper();
                var listCate = _repPostsCategory.Query().Where(p => p.ParentID == 0).ToList();
                rs = helper.GenerateXMLRewriteUrl(_CONFIG[PostsCategoryConstants.Config.Website.PreSlug].ToString(), listCate, listPageTemplate);
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = ex.ToString();
                rs.Data = "Lỗi không xác định";

                return rs;
            }
        }
        public List<Database.PostsCategory> GetCategories(string Slug)
        {
            List<Database.PostsCategory> rs = new List<Database.PostsCategory>();
            try
            {
                rs = _repPostsCategory.Query().Where(p => p.Slug == Slug && p.Enabled==true ).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, Slug);

            }
            return rs;
        }    
  
        public List<Database.PostsCategory> GetCategories(int pid)
        {
            List<Database.PostsCategory> rs = new List<Database.PostsCategory>();
            try
            {
                rs = _repPostsCategory.Query().Where(p => p.Pid == pid && p.Enabled == true).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, pid.ToString());

            }
            return rs;
        }
        public List<Database.PostsCategory> GetRootCategories(string Slug)
        {
            List<Database.PostsCategory> rs = new List<Database.PostsCategory>();
            try
            {
               rs = _repPostsCategory.Query().Where(p => p.Enabled == true).Where(p => p.Slug == Slug && p.ParentID == 0).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, Slug);

            }
            return rs;
        }
    }

}
