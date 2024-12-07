
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Database;
using Steam.Core.LocalizeManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;

namespace Steam.Core.LocalizeManagement.Services
{
    public class LocalizeManagementService : ILocalizeManagementService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.LocalizeManagement> _repLocalizeManagement;
        private readonly IRepositoryConfig<Database.LocalizeManagementConfig> _repLocalizeManagementConfig;
        public LocalizeManagementService(
            IRepository<Database.LocalizeManagement> repLocalizeManagement,
            IRepositoryConfig<Database.LocalizeManagementConfig> repLocalizeManagementConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repLocalizeManagement = repLocalizeManagement;
            _repLocalizeManagementConfig = repLocalizeManagementConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repLocalizeManagementConfig.GetAllConfigs();
        }
        public Response<IPagedList<Database.LocalizeManagement>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.LocalizeManagement>> rs = new Response<IPagedList<Database.LocalizeManagement>>();
            try
            {
                bool isSystemKey = false;
                if(search.View=="systemkey")
                {
                    isSystemKey = true;
                }
                search.ToString();
                rs.Data = _repLocalizeManagement.Query().Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Key.Contains(search.KeySearch)) && p.isSystemKey== isSystemKey)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[LocalizeManagementConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<LocalizeManagementDetail> GetById(int id)
        {
            Response<LocalizeManagementDetail> rs = new Response<LocalizeManagementDetail>();
            LocalizeManagementDetail detail = new LocalizeManagementDetail();
            try
            {

                detail.Detail = _repLocalizeManagement.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<LocalizeManagement.Database.LocalizeManagement> Save(LocalizeManagementModelEdit input)
        {
            Response<LocalizeManagement.Database.LocalizeManagement> rs = new Response<LocalizeManagement.Database.LocalizeManagement>();
            Database.LocalizeManagement newModel = new Database.LocalizeManagement();
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
                                 Height = Convert.ToInt32(_config[LocalizeManagementConstants.Config.Admin.MaxHeight].ToString()),
                                 Width = Convert.ToInt32(_config[LocalizeManagementConstants.Config.Admin.MaxWidth].ToString()),
                                 Path = LocalizeManagementConstants.StaticPath.Asset.Image,
                                 PathThumb = LocalizeManagementConstants.StaticPath.Asset.ImageThumb,
                                 File = input.files
                             }
                             ).FileName;
                            filePath = LocalizeManagementConstants.StaticPath.Asset.Image;
                        }
                    }
                    //-------end-save-file-pond----------

                }
                var validator = new LocalizeManagementValidator();

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

                using (var transaction = _repLocalizeManagement.BeginTransaction())
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

                            _repLocalizeManagement.Add(newModel);

                            _repLocalizeManagement.SaveChanges();
                        }
                        else
                        {
                            var editModel = _repLocalizeManagement.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

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
                                _repLocalizeManagement.SaveChanges();

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
                    var model = _repLocalizeManagement.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repLocalizeManagement.Remove(model);
                    //check and remove images


                    _repLocalizeManagement.SaveChanges();
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
