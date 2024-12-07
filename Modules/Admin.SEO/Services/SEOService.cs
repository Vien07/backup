using Microsoft.AspNetCore.Http;
using Admin.SEO.Database;
using Admin.SEO.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Newtonsoft.Json;
using Admin.SEO.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Admin.SEO.Api.Models;
using Steam.Core.Common;
using Steam.Infrastructure.Repository;

namespace Admin.SEO.Services
{
    public class SEOService : ISEOService
    {
        private readonly IRepository<Database.SEO> _repSEO;
        private readonly IRepository<Database.SEO_Files> _repSEO_Files;
        private readonly IRepositoryConfig<Database.SEOConfig> _repSEOConfig;
        Dictionary<string, string> _CONFIG;

        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        public SEOService(
            IFileHelper fileHelper,
            IRepositoryConfig<Database.SEOConfig> repSEOConfig,
            IRepository<Database.SEO> repSEO,
            ILoggerHelper logger)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _repSEOConfig = repSEOConfig;
            _repSEO = repSEO;
            _CONFIG = _repSEOConfig.GetAllConfigs();

        }
        public List<Database.SEO> GetSEOsByModuleCode(string moduleCode)
        {
            List<Database.SEO> rs = new List<Database.SEO>();
            try
            {
                rs = _repSEO.Query().Where(p => p.ModuleCode == moduleCode).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, moduleCode);

            }
            return rs;
        }
        public Database.SEO GetSEO(long pid, string moduleCode)
        {
            Database.SEO rs = new Database.SEO();
            try
            {
                rs = _repSEO.Query().Where(p => p.ModuleCode == moduleCode && p.PostPid == pid).FirstOrDefault();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, moduleCode);

            }
            return rs;
        }
        public List<Database.SEO> GetSEO(string Slug, string moduleCode)
        {
            List<Database.SEO> rs = new List<Database.SEO>();
            try
            {
                rs = _repSEO.Query().Where(p => p.ModuleCode == moduleCode && p.PostSlug == Slug).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, moduleCode);

            }
            return rs;
        }
        public bool CountSEO(string Slug, string moduleCode)
        {
            try
            {
                var seo = _repSEO.Query().Where(p => p.ModuleCode == moduleCode && p.PostSlug == Slug).FirstOrDefault();
                seo.CountView = seo.CountView + 1;
                _repSEO.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, moduleCode);

            }
            return true;
        }
      
        public Response<SEO_List> GetList(ParamSearch search)
        {
            search = search.Init();

            Response<SEO_List> rs = new Response<SEO_List>();
            List<SEO_List.SEO_Item> listItems = new List<SEO_List.SEO_Item>();
            SEO_List data = new SEO_List();
            try
            {
                search.ToString();
                var listSEO = _repSEO.Query().Where(p => String.IsNullOrEmpty(search.Module) == true || p.ModuleCode == search.Module)
                     .Where(p => search.isEnable == null || p.Enabled == search.isEnable)
                     .Where(p => p.Deleted == false)
                     .Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.PostSlug.Contains(search.KeySearch)))
                     .OrderBy(p => p.Order).ThenBy(p => p.CreateDate).ToList()
                     .ToPagedList(search.PageIndex, Convert.ToInt32(_config[SEOConstants.Config.Admin.PageSize]));
                listItems = listSEO.DeepClone<List<SEO_List.SEO_Item>>();

                data.Items = listItems;
                data.PageCount = listSEO.PageCount;
                data.PageNumber = listSEO.PageNumber;
                data.PageSize = listSEO.PageSize;

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            rs.Data = data;
            return rs;
        }
        public Response<SEODetail> GetById(int id)
        {
            Response<SEODetail> rs = new Response<SEODetail>();
            SEODetail detail = new SEODetail();
            try
            {

                detail.Detail = _repSEO.Query().Where(p => p.Pid == id).FirstOrDefault();
                var file = _repSEO_Files.Query().Where(p => p.SEOId == id).ToList();
                if (file != null)
                {
                    detail.ListFiles = file;
                }
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

        public Response<SEO.Database.SEO> SaveSEO(SEO.Database.SEO data)
        {
            //-------save-file-pond----------
            var img = "";
            //if (files != null)
            //{
            //    foreach (var file in files)
            //    {
            //        if (file != null)
            //        {
            //            //data.Images += file.FileName;
            //            img = _fileHelper.UploadImageModule(
            //                new UploadImageInfo
            //                {
            //                    FileName = data.Title.ToSlug(),
            //                    Height = Convert.ToInt32(_config[Constants.Config.Admin.MaxHeight].ToString()),
            //                    Width = Convert.ToInt32(_config[Constants.Config.Admin.MaxWidth].ToString()),
            //                    ThumbHeight = Convert.ToInt32(_config[Constants.Config.Admin.ThumbHeight].ToString()),
            //                    ThumbWidth = Convert.ToInt32(_config[Constants.Config.Admin.ThumbWidth].ToString()),
            //                    Path = Constants.StaticPath.Asset.Image,
            //                    PathThumb = Constants.StaticPath.Asset.ImageThumb,
            //                    File = file
            //                }
            //                ).FileName;
            //        }

            //    }
            //}
            //-------end-save-file-pond----------


            var validator = new Database.SEO.SEOValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<SEO.Database.SEO> rs = new Response<SEO.Database.SEO>();
            if (data.PostPid == 0)
            {
                rs.IsError = true;
                rs.Message = "Vui lòng lưu bài viết trước khi định nghĩa SEO!";
                return rs;
            }
            using (var transaction = _repSEO.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        var checkIsExist = _repSEO.Query().Where(p => p.PostSlug == data.PostSlug && p.ModuleCode == data.ModuleCode).FirstOrDefault();
                        if (checkIsExist != null)
                        {
                            rs.IsError = true;
                            rs.Message = "Đường dẫn đã được sử dụng!";
                            return rs;
                        }
                        data.Order = 0.9;
                        data.Images = img;

                        _repSEO.Add(data);

                        _repSEO.SaveChanges();
                    }
                    else
                    {
                        var model = _repSEO.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.PostSlug = data.PostSlug;
                            model.MetaDescription = data.MetaDescription;
                            model.Meta = data.Meta;
                            model.TagKeys = data.TagKeys;
                            model.UpdateDate = DateTime.Now;
                            //if (files != null && files.Count > 0)
                            //{
                            //    _fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                            //    _fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                            //    model.Images = img;
                            //}
                            _repSEO.SaveChanges();

                        }


                    }
                    //-----save-list-file-dropzone-----
                    //if (!String.IsNullOrEmpty(listFiles))
                    //{
                    //    SaveListFile(data, listFiles);
                    //}
                    //---------end save list lisst file--------
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
        public Response<SEO.Database.SEO> Save(SEO.Database.SEO data)
        {
            //-------save-file-pond----------
            var img = "";



            var validator = new Database.SEO.SEOValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<SEO.Database.SEO> rs = new Response<SEO.Database.SEO>();


            try
            {

                var model = _repSEO.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();
                if (model != null)
                {
                    var checkIsExist = _repSEO.Query().Where(p => p.PostSlug == model.PostSlug && p.Pid != model.Pid).FirstOrDefault();
                    if (checkIsExist != null)
                    {
                        rs.IsError = true;
                        rs.Message = "Đường dẫn đã được sử dụng!";
                        return rs;
                    }

                    if (model != null)
                    {
                        model.PostSlug = data.PostSlug;
                        model.MetaDescription = data.MetaDescription;
                        model.Meta = data.Meta;
                        model.TagKeys = data.TagKeys;
                        model.UpdateDate = DateTime.Now;

                        _repSEO.SaveChanges();

                    }
                }




            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

            }

            rs.Data = data;
            return rs;

        }
        public void SaveListFile(SEO.Database.SEO data, string listFIlesString)
        {
            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                List<SEO.Database.SEO_Files> files = new List<SEO.Database.SEO_Files>();
                foreach (var item in listFIles)
                {
                    if (item.status == "new")
                    {
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = Convert.ToInt32(_config[SEOConstants.Config.Admin.MaxHeight].ToString()),
                            Width = Convert.ToInt32(_config[SEOConstants.Config.Admin.MaxWidth].ToString())
                             ,
                            FileName = data.PostTitle.ToSlug(),
                            Path = SEOConstants.StaticPath.Asset.Image
                        }); if (!saveFile.isError)
                        {
                            files.Add(new SEO.Database.SEO_Files { SEOId = data.Pid, Caption = item.caption, Description = item.description, UploadFileName = saveFile.FileName, Order = item.order });

                        }
                    }
                    else if (item.status == "delete")
                    {
                        _fileHelper.DeleteFile(SEOConstants.StaticPath.Asset.Image, item.name);
                        var imageInfo = _repSEO_Files.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        _repSEO_Files.Remove(imageInfo);


                    }
                    else if (item.status == "edit")
                    {
                        var imageInfo = _repSEO_Files.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        imageInfo.Caption = item.caption;
                        imageInfo.Description = item.description;
                        imageInfo.Order = item.order;
                        _repSEO_Files.SaveChanges();
                    }



                }
                _repSEO_Files.AddRange(files);
                _repSEO_Files.SaveChanges();
                _repSEO.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repSEO.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repSEO.Remove(model);
                    _repSEO.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }
        public Response Delete(int id, string moduleCode)
        {

            Response rs = new Response();

            try
            {
                var model = _repSEO.Query().Where(p => p.ModuleCode == moduleCode).Where(p => p.Pid == id).FirstOrDefault();
                _repSEO.Remove(model);
                _repSEO.SaveChanges();

            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }

        #region api for website 
        public Response<List<Response_GetAllListSEOActive>> GetAllListSEOActive()
        {
            Response<List<Response_GetAllListSEOActive>> rs = new Response<List<Response_GetAllListSEOActive>>();
            try
            {
                rs.Data = _repSEO.Query().Where(p => p.Deleted == false).Where(p => p.Enabled == true)
                     .Select(x => new Response_GetAllListSEOActive
                     {
                         PostSlug = x.PostSlug,
                         PostPid = x.PostPid
                     }).ToList();

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "getlissterror");

            }
            return rs;
        }
        public Response<dynamic> GetPostBySlug(string postSlug)
        {
            Response<dynamic> rs = new Response<dynamic>();
            SEODetail detail = new SEODetail();
            try
            {

                detail.Detail = _repSEO.Query().Where(p => p.PostSlug == postSlug).FirstOrDefault();

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
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        #endregion
    }

}
