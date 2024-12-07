
using Admin.MemberManagement.Database;
using Admin.MemberManagement.Models;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;

using Newtonsoft.Json;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Common.SteamString;//
using Steam.Infrastructure.Repository;

namespace Admin.MemberManagement.Services
{
    public class MemberManagementService : IMemberManagementService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepositoryConfig<Database.MemberManagementConfig> _repConfig;
        private readonly IRepository<Database.MemberManagement> _repMemberManagement;
        private readonly IRepository<Database.MemberManagement_Files> _repMemberManagement_Files;

        public MemberManagementService(
            IRepositoryConfig<Database.MemberManagementConfig> repConfig,
            IRepository<Database.MemberManagement> repMemberManagement,
            IRepository<Database.MemberManagement_Files> repMemberManagement_Files,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repMemberManagement_Files = repMemberManagement_Files;
            _repMemberManagement = repMemberManagement;
            _repConfig = repConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repConfig.GetAllConfigs();
        }
        public Response<IPagedList<Database.MemberManagement>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.MemberManagement>> rs = new Response<IPagedList<Database.MemberManagement>>();
            try
            {
                search.ToString();
                rs.Data = _repMemberManagement.Query().Where(p=>((String.IsNullOrEmpty(search.KeySearch)==true || p.Email.Contains(search.KeySearch)) && p.Deleted == false))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[Constants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<MemberManagementDetail> GetById(int id)
        {
            Response<MemberManagementDetail> rs = new Response<MemberManagementDetail>();
            MemberManagementDetail detail = new MemberManagementDetail();
            try
            {

                detail.Detail = _repMemberManagement.Query().Where(p => p.Pid == id && p.Deleted == false).FirstOrDefault();
                var file = _repMemberManagement_Files.Query().Where(p => p.MemberManagementId == id).ToList();
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
        public Response<MemberManagement.Database.MemberManagement> Save(MemberManagement.Database.MemberManagement data, List<IFormFile> files, string listFiles)
        {
            //-------save-file-pond----------
            var img = "";
            if (files != null)
            {
                //foreach (var file in files)
                //{
                //    if (file != null)
                //    {
                //        //data.Images += file.FileName;
                //        img = _fileHelper.UploadImageModule(
                //            new UploadImageInfo
                //            {
                //                FileName = _commonHelper.StringToSlug(data.Email),
                //                Height = Convert.ToInt32(_config[Constants.Config.Admin.MaxHeight].ToString()),
                //                Width = Convert.ToInt32(_config[Constants.Config.Admin.MaxWidth].ToString()),
                //                ThumbHeight = Convert.ToInt32(_config[Constants.Config.Admin.ThumbHeight].ToString()),
                //                ThumbWidth = Convert.ToInt32(_config[Constants.Config.Admin.ThumbWidth].ToString()),
                //                Path = Constants.StaticPath.Asset.Image,
                //                PathThumb = Constants.StaticPath.Asset.ImageThumb,
                //                File = file
                //            }
                //            ).FileName;
                //    }

                //}
            }
            //-------end-save-file-pond----------


            var validator = new MemberManagementValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<MemberManagement.Database.MemberManagement> rs = new Response<MemberManagement.Database.MemberManagement>();
            using (var transaction = _repMemberManagement.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;
                        data.Images = img;

                        _repMemberManagement.Add(data);

                        _repMemberManagement.SaveChanges();
                    }
                    else
                    {
                        var model = _repMemberManagement.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.FirstName = data.FirstName;
                            model.LastName = data.LastName;
                            model.Email = data.Email;
                            model.Phone = data.Phone;
                            _repMemberManagement.SaveChanges();

                        }
                    }
                    //-----save-list-file-dropzone-----
                    if (!String.IsNullOrEmpty(listFiles))
                    {
                        //SaveListFile(data, listFiles);
                    }
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
        public void SaveListFile(MemberManagement.Database.MemberManagement data, string listFIlesString)
        {
            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                List<MemberManagement.Database.MemberManagement_Files> files = new List<MemberManagement.Database.MemberManagement_Files>();
                //foreach (var item in listFIles)
                //{
                //    if (item.status == "new")
                //    {
                //        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                //        {
                //            Base64 = item.dataUrl,
                //            Height = Convert.ToInt32(_config[Constants.Config.Admin.MaxHeight].ToString()),
                //            Width = Convert.ToInt32(_config[Constants.Config.Admin.MaxWidth].ToString())
                //             ,
                //            FileName = _commonHelper.StringToSlug(data.Title),
                //            Path = Constants.StaticPath.Asset.Image
                //        }); if (!saveFile.isError)
                //        {
                //            files.Add(new MemberManagement.Database.MemberManagement_Files { MemberManagementId = data.Pid, caption = item.caption, Description = item.Description, UploadFileName = saveFile.FileName, Order = item.Order });

                //        }
                //    }
                //    else if (item.status == "delete")
                //    {
                //        _fileHelper.DeleteFile(StaticPath.Asset.Image, item.name);
                //        var imageInfo = _db.MemberManagement_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                //        _db.MemberManagement_Files.Remove(imageInfo);


                //    }
                //    else if (item.status == "edit")
                //    {
                //        var imageInfo = _db.MemberManagement_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                //        imageInfo.caption = item.caption;
                //        imageInfo.Description = item.Description;
                //        imageInfo.Order = item.Order;
                //        _db.SaveChanges();
                //    }



                //}
               _repMemberManagement_Files.AddRange(files);

                _repMemberManagement_Files.SaveChanges();

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
                    var model = _repMemberManagement.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    _repMemberManagement.SaveChanges();
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
      
     


        public Response ResetPassword(int pid)
            {
            Response rs = new Response();

            try
            {
                var model = _repMemberManagement.Query().Where(p => p.Pid == pid).FirstOrDefault();

                if (model is not null)
                {
                    model.Password = "password@123";
                    _repMemberManagement.SaveChanges();
                    rs.Message = model.Password;
                }
                else
                {
                    rs.Message = "Accout does not exist";
                }
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, pid.ToString());
            }
            return rs;

        }


    }

}
