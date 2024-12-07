
using Microsoft.AspNetCore.Http;
using Admin.WebsiteKeys.Database;
using Admin.WebsiteKeys.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.WebsiteKeys.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;

namespace Admin.WebsiteKeys.Service
{
    public class WebsiteKeysService : IWebsiteKeysService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.WebsiteKeys> _repWebsiteKeys;
        private readonly IRepositoryConfig<Database.WebsiteKeysConfig> _repWebsiteKeysConfig;
        public WebsiteKeysService(
            IRepository<Database.WebsiteKeys> repWebsiteKeys,
            IRepositoryConfig<Database.WebsiteKeysConfig> repWebsiteKeysConfig,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repWebsiteKeys = repWebsiteKeys;
            _repWebsiteKeysConfig = repWebsiteKeysConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repWebsiteKeysConfig.GetAllConfigs();


        }
        public Response<IPagedList<Database.WebsiteKeys>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.WebsiteKeys>> rs = new Response<IPagedList<Database.WebsiteKeys>>();
            try
            {
                bool isSystemKey = false;
                if(search.View=="systemkey")
                {
                    isSystemKey = true;
                }
                search.ToString();
                rs.Data = _repWebsiteKeys.Query().Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Key.Contains(search.KeySearch)) && p.isSystemKey== isSystemKey)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[WebsiteKeysConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<WebsiteKeysDetail> GetById(int id)
        {
            Response<WebsiteKeysDetail> rs = new Response<WebsiteKeysDetail>();
            WebsiteKeysDetail detail = new WebsiteKeysDetail();
            try
            {

                detail.Detail = _repWebsiteKeys.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<WebsiteKeys.Database.WebsiteKeys> Save(WebsiteKeysModelEdit input)
        {
            Response<WebsiteKeys.Database.WebsiteKeys> rs = new Response<WebsiteKeys.Database.WebsiteKeys>();
            Database.WebsiteKeys newModel = new Database.WebsiteKeys();
            try
            {
                //-------save-file-pond----------
                var img = "";
                var filePath = "";
                if (input.fileStatus != "existed")
                {
                    if (input.files != null)
                    {
                        if (input.MediaPath != null && input.MediaPath != "")
                        {
                            var arrFile = input.MediaPath.Split('/');
                            img = arrFile[arrFile.Length - 1];
                            filePath = SystemInfo.PathFileManager + "/" + input.MediaPath.Replace(img, "");
                        }
                        else
                        {
                            img = _fileHelper.UploadImageModule(
                             new UploadImageInfo
                             {
                                 FileName = input.Key.ToSlug(),
                                 Height = Convert.ToInt32(_config[WebsiteKeysConstants.Config.Admin.MaxHeight].ToString()),
                                 Width = Convert.ToInt32(_config[WebsiteKeysConstants.Config.Admin.MaxWidth].ToString()),
                                 Path = WebsiteKeysConstants.StaticPath.Asset.Image,
                                 PathThumb = WebsiteKeysConstants.StaticPath.Asset.ImageThumb,
                                 File = input.files
                             }
                             ).FileName;
                            filePath = WebsiteKeysConstants.StaticPath.Asset.Image;
                        }
                    }
                    //-------end-save-file-pond----------

                }
                var validator = new WebsiteKeysValidator();

                // Execute the validator
                ValidationResult results = validator.Validate(input);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                if (!success)
                {
                    string mess = string.Join(";", results.Errors);

                    rs.Message = mess;
                    rs.IsError = true;
                    return rs;
                }

                using (var transaction = _repWebsiteKeys.BeginTransaction())
                {
                    try
                    {
                        newModel = input.GetDatabaseModel();

                        if (img != "")
                        {
                            newModel.MediaPath = img;

                        }
                        if (filePath != "")
                        {
                            newModel.MediaPath = filePath;

                        }
                        if (newModel.Pid == 0)
                        {
                            newModel.Order = 0.9;
                            newModel.MediaPath = img;

                            _repWebsiteKeys.Add(newModel);

                            _repWebsiteKeys.SaveChanges();
                        }
                        else
                        {
                            var editModel = _repWebsiteKeys.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (editModel != null)
                            {
                                if (img != "")
                                {
                                    editModel.MediaPath = img;

                                }
                                if (filePath != "")
                                {
                                    editModel.MediaPath = filePath;

                                }
                                editModel.Key = newModel.Key;
                                editModel.Description = newModel.Description;
                                editModel.Value = newModel.Value;
                                editModel.Group = newModel.Group;
                                editModel.isSystemKey = newModel.isSystemKey;
                                editModel.isMedia = newModel.isMedia;
                                editModel.MediaPath = newModel.MediaPath;
                                _repWebsiteKeys.SaveChanges();

                            }


                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        rs.IsError = true;
                        rs.Message = ex.Message;
                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, newModel.ToJson());

                    }
                }
                rs.Data = newModel;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
            }

            return rs;

        }
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repWebsiteKeys.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repWebsiteKeys.Remove(model);
                    //check and remove images


                    _repWebsiteKeys.SaveChanges();
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
     

    }

}
