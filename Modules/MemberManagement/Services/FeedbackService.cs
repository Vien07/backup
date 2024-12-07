
using Microsoft.AspNetCore.Http;
using Admin.MemberManagement.Database;
using Admin.MemberManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Admin.MemberManagement.Constants;

using Steam.Core.Base.Constant;

using Steam.Core.Base;

using Admin.SEO.Services;
using Newtonsoft.Json;
using Steam.Infrastructure.Repository;

namespace Admin.MemberManagement.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly MultiLangService _multiLangService;

        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepositoryConfig<Database.FeedbackConfig> _repFeedbackConfig;
        private readonly IRepository<Database.Feedback> _repFeedback;
        private readonly IRepository<Database.Feedback_Files> _repFeedback_Files;
        ISEOService _srvSEO;
        IHttpContextAccessor _httpContext;
        string VirtualFolder="";
        public FeedbackService(
            IRepository<Database.Feedback_Files> repFeedback_Files,
           IRepository<Database.Feedback> repFeedback,
           IRepositoryConfig<Database.FeedbackConfig> repFeedbackConfig,
            ISEOService srvSEO,
            IFileHelper fileHelper, 
            ILoggerHelper logger,
            IHttpContextAccessor httpContext)
        {
            _repFeedbackConfig = repFeedbackConfig;
            _repFeedback_Files = repFeedback_Files;
            _repFeedback = repFeedback;
            _srvSEO = srvSEO;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = repFeedbackConfig.GetAllConfigs() ;
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            VirtualFolder = conf["SystemConfig:VirtualFolder"].ToString();

        }

        public Response<Feedback_List> GetList(FeedbackModel.ParamSearch search)
        {
            search = search.Init();
            Response<Feedback_List> rs = new Response<Feedback_List>();
            var list = new Feedback_List();
            try
            {
                var listSeos = _srvSEO.GetSEOsByModuleCode("Feedback");
                var listPost = _repFeedback.Query().Where(p => p.Deleted == false)
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                     .ToPagedList(search.PageIndex, Convert.ToInt32(_config[FeedbackConstants.Config.Admin.PageSize]));
                var pageCount = listPost.PageCount;
                var listSeo = listSeos;
                var posts = (
                  from a in listPost
                  join b in listSeo on a.Pid equals b.PostPid into seoGroup
                  from b in seoGroup.DefaultIfEmpty() //  LEFT OUTER JOIN
                  select new Feedback_Item
                  {
                      Pid = a.Pid,
                      FullName = a.FullName,
                      Email = a.Email,
                      Rating = a.Rating,
                      SKU = a.SKU,
                      Slug = b != null ? b.PostSlug : null, // Handle NULL values from outer join
                      Content = a.Content,
                      Images = a.Images,
                      Enabled = a.Enabled,
                      Approval = a.Approval,
                      Order = a.Order,
                      FilePath = a.FilePath,
                  })
                  .Where(p => (search.isEnable==null || p.Enabled == search.isEnable))
                  .Where(p => (String.IsNullOrEmpty(search.KeySearch) == true
                      || p.FullName.Contains(search.KeySearch)
                      || p.Slug.Contains(search.KeySearch)))
                  .Where(p => (search.Cate=="0" || p.CateID == Convert.ToInt64(search.Cate)))
                  .ToList();



                list.Items = posts;
                list.PageCount = listPost.PageCount; ;
                list.PageNumber = listPost.PageNumber; ;
                list.PageSize = listPost.PageSize; ;
                rs.Data = list;

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<FeedbackModel.FeedbackDetail> GetById(int id)
        {
            Response<FeedbackModel.FeedbackDetail> rs = new Response<FeedbackModel.FeedbackDetail>();
            FeedbackModel.FeedbackDetail detail = new FeedbackModel.FeedbackDetail();
            try
            {

                detail.Detail = _repFeedback.Query().Where(p => p.Pid == id).FirstOrDefault();
                var file = _repFeedback_Files.Query().Where(p => p.FeedbackId == id).ToList();
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
        public Response<Database.Feedback> Save(FeedbackModelEdit data)
        {
            
            var validator = new FeedbackValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Feedback> rs = new Response<Database.Feedback>();
            Database.Feedback modelResponse = new Database.Feedback();
            if (!success)
            {
                string mess = string.Join(";", results.Errors);

                rs.Message = mess;
                rs.IsError = true;
                return rs;
            }

            using (var transaction = _repFeedback.BeginTransaction())
            {
                try
                {
                    modelResponse = data.GetDatabaseModel();
                   
                    if (modelResponse.Pid == 0)
                    {
                        modelResponse.Order = 0.9;

                        //modelResponse.FilePath = filePath;

                        _repFeedback.Add(modelResponse);

                        _repFeedback.SaveChanges();
                    }
                    else
                    {
                        var editData = _repFeedback.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (editData != null)
                        {
                            

                            editData.FullName = modelResponse.FullName;
                            editData.Email = modelResponse.Email;
                            editData.Rating = modelResponse.Rating;
                            editData.SKU = modelResponse.SKU;
                            editData.Content = modelResponse.Content;
                            //editData.PublishDate = modelResponse.PublishDate;
                            editData.isNew = modelResponse.isNew;


                            editData.CateID = modelResponse.CateID;
                            editData.SubCate = modelResponse.SubCate;


                            #region images 
                            editData.Images_Description = modelResponse.Images_Description;
                            editData.Images_Alt = modelResponse.Images_Alt;
                            editData.Images_Caption = modelResponse.Images_Caption;
                            #endregion
                            _repFeedback.SaveChanges();

                        }


                    }
                    //-----save-list-file-dropzone-----
                    if (!String.IsNullOrEmpty(data.ListFiles))
                    {
                        SaveListFile(data, data.ListFiles);
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
            rs.Data = modelResponse;
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
                                      Height = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.MaxHeight].ToString()),
                                      Width = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.MaxWidth].ToString()),
                                      ThumbHeight = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.ThumbHeight].ToString()),
                                      ThumbWidth = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.ThumbWidth].ToString()),
                                      Path = FeedbackConstants.StaticPath.Asset.Image,
                                      PathThumb = FeedbackConstants.StaticPath.Asset.ImageThumb,
                                      File = file
                                  }
                                  ).FileName;
                            imgFilePath = FeedbackConstants.StaticPath.Asset.Image;
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

        public void SaveListFile(Database.Feedback data, string listFIlesString)
        {

            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);
            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                List<Database.Feedback_Files> files = new List<Database.Feedback_Files>();
                foreach (var item in listFIles)
                {
                    if (item.status == "new")
                    {
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.MaxHeight].ToString()),
                            Width = Convert.ToInt32(_config[FeedbackConstants.Config.Admin.MaxWidth].ToString())
                             ,
                            FileName = data.Email.ToSlug(),
                            Path = FeedbackConstants.StaticPath.Asset.Image
                        }); if (!saveFile.isError)
                        {
                            files.Add(new Database.Feedback_Files { FeedbackId = data.Pid, Caption = item.caption, Description = item.description, UploadFileName = saveFile.FileName, Order = item.order, CreateUser = "admin", UpdateUser = "admin" });

                        }
                    }
                    else if (item.status == "delete")
                    {
                        //_fileHelper.DeleteFile(FeedbackConstants.StaticPath.Asset.Image, item.name);
                        var imageInfo = _repFeedback_Files.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        _repFeedback_Files.Remove(imageInfo);


                    }
                    else if (item.status == "edit")
                    {
                        var imageInfo = _repFeedback_Files.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        imageInfo.Caption = item.caption;
                        imageInfo.Description = item.description;
                        imageInfo.Order = item.order;
                        _repFeedback_Files.SaveChanges();
                    }
                    if (item.status == "server")
                    {
                        var arrFile = item.name.Split('/');
                        var img = arrFile[arrFile.Length - 1];
                        var filePath = item.name.Replace(img, "");
                        files.Add(new Database.Feedback_Files
                        {
                            FeedbackId = data.Pid,
                            FilePath = filePath,
                            Caption = item.caption,
                            Description = item.description,
                            UploadFileName = img,
                            Order = item.order,
                            CreateUser = "admin",
                            UpdateUser = "admin"
                        });


                    }


                }
                _repFeedback_Files.AddRange(files);

                _repFeedback_Files.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();
            using (var transaction = _repFeedback.BeginTransaction())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        _srvSEO.Delete(id, "Feedback");

                        var model = _repFeedback.Query().Where(p => p.Pid == id).FirstOrDefault();
                        _repFeedback.Remove(model);


                        _repFeedback.SaveChanges();
                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

                }
            }
            return rs;

        }
       

    }

}
