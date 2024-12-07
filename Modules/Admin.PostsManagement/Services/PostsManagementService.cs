
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Database;
using Admin.PostsManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.PostsManagement.Constants;

using Steam.Core.Base.Constant;

using Admin.PostsCategory.Database;
using Steam.Core.Base;

using ComponentUILibrary.Models;
using Admin.SEO;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;


namespace Admin.PostsManagement.Services
{
    public class PostsManagementService : IPostsManagementService
    {

        private ILoggerHelper _logger;

        private readonly IRepository<Database.PostsManagement> _repoPostsManagement;
        private readonly IRepositoryConfig<Database.PostsManagementConfig> _repConfig;
        private readonly IRepository<Database.PostsManagement_Files> _repoPostsManagementFile;
        private readonly IRepository<Admin.SEO.Database.SEO> _repSEO;
        private readonly IRepository<Admin.PostsCategory.Database.PostsCategory> _repPostsCategory;
        private ISEOService _srvSEO;

        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        PostsCategoryContext _dbCate;
        IHttpContextAccessor _httpContext;
        readonly IIdentityService _identitySrv;
        readonly UserModel CurrentUser;
        string VirtualFolder = "";
        public PostsManagementService(
           IRepository<Admin.PostsCategory.Database.PostsCategory> repPostsCategory,
            IRepository<Admin.SEO.Database.SEO> repSEO,
             ISEOService srvSEO,
            IFileHelper fileHelper,
            ILoggerHelper logger,
            IHttpContextAccessor httpContext,
            IRepositoryConfig<Database.PostsManagementConfig> repConfig,
            IRepository<Database.PostsManagement> repoPostsManagement,
            IRepository<Database.PostsManagement_Files> repoPostsManagementFile,
              IIdentityService identitySrv)
        {
            #region templand
            //var random = new Random();
            //var test = random.Next(0, 2);
            ////var xyz = httpContext.HttpContext.Request?.Headers["LangKey"];
            //var newConnectionString = "Server=.;Database=cms_db;Integrated Security=True;MultipleActiveResultSets=True";
            //var newConnectionStringa = "Server=w3bdemo.com;Database=ofd.vn;Integrated Security=False;User ID=cms_dev;Password=Ca@Tha@#2023;;MultipleActiveResultSets=True;TrustServerCertificate=Yes";
            //if (test == 0)
            //{
            //    _db = db.SetConnectionString(newConnectionString);

            //}
            //else
            //{
            //    _db = db.SetConnectionString(newConnectionStringa);

            //}
            #endregion
            _repSEO = repSEO;
            _srvSEO = srvSEO;
            _repPostsCategory = repPostsCategory;
            _logger = logger;
            _fileHelper = fileHelper;
            _repConfig = repConfig;
            _repoPostsManagement = repoPostsManagement;
            _repoPostsManagementFile = repoPostsManagementFile;
            _config = _repConfig.GetAllConfigs();
            _identitySrv = identitySrv;
            CurrentUser = _identitySrv.GetUser();
        }

        public Response<PostsManagement_List> GetList(PostsManagementModel.ParamSearch search)
        {
            search = search.Init();
            Response<PostsManagement_List> rs = new Response<PostsManagement_List>();
            var list = new PostsManagement_List();
            try
            {
                var listSeos = _srvSEO.GetSEOsByModuleCode(PostsManagementConstants.ModuleInfo.ModuleCode);
                var listPost = _repoPostsManagement.Query().Where(P => String.IsNullOrEmpty(search.Group) || P.Group == search.Group)
                    .Where(p => p.Deleted == false)
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                     .ToPagedList(search.PageIndex, Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.PageSize]));
                var pageCount = listPost.PageCount;
                var listCate = _repPostsCategory.Query().ToList();
                var listSeo = listSeos;
                var posts = (
                  from a in listPost
                  join b in listSeo on a.Pid equals b.PostPid into seoGroup
                  from b in seoGroup.DefaultIfEmpty() //  LEFT OUTER JOIN
                  join c in listCate on a.CateID equals c.Pid into cateGroup
                  from c in cateGroup.DefaultIfEmpty() //LEFT OUTER JOIN
                  select new PostsManagement_Item
                  {
                      Pid = a.Pid,
                      Title = a.Title,
                      Slug = b != null ? b.PostSlug : null, // Handle NULL values from outer join
                      Description = a.Description,
                      Images = a.Images,
                      Enabled = a.Enabled,
                      Order = a.Order,
                      CateID = a.CateID,
                      Cate = c != null ? c.Title : null, // Handle NULL values from outer join
                      CateSlug = c != null ? c.Slug : "", // Handle NULL values from outer join
                      FilePath = a.FilePath,
                      ImagePath = SystemInfo.VirtualFolder + (a.FilePath + a.Images).CheckExistsImage()
                  })
                  .Where(p => (search.isEnable == null || p.Enabled == search.isEnable))
                  .Where(p => (String.IsNullOrEmpty(search.KeySearch) == true
                      || p.Title.Contains(search.KeySearch)
                      || p.Slug.Contains(search.KeySearch)))
                  .Where(p => (search.Cate == "0" || p.CateID == Convert.ToInt64(search.Cate)))
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
        public Response<PostsManagementModel.PostsManagementDetail> GetById(int id)
        {
            Response<PostsManagementModel.PostsManagementDetail> rs = new Response<PostsManagementModel.PostsManagementDetail>();
            PostsManagementModel.PostsManagementDetail detail = new PostsManagementModel.PostsManagementDetail();
            try
            {

                detail.Detail = _repoPostsManagement.Query().Where(p => p.Pid == id).FirstOrDefault();
                var file = _repoPostsManagementFile.Query().Where(p => p.PostsManagementId == id).ToList();
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
        public Response<Database.PostsManagement> Save(SaveModel data)
        {

            #region Validate
            var validator = new PostsManagementValidator();

            ValidationResult results = validator.Validate(data);
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.PostsManagement> rs = new Response<Database.PostsManagement>();
            Database.PostsManagement saveData = new Database.PostsManagement();
            if (!success)
            {
                string mess = string.Join(";", results.Errors);

                rs.Message = mess;
                rs.IsError = true;
                return rs;
            }
            #endregion
            using (var transaction = _repoPostsManagement.BeginTransaction())
            {
                try
                {
                    saveData = data.GetDatabaseModel();

                    if (saveData.Pid == 0)
                    {
                        saveData.Order = 0.9;
                        saveData.CreateUser = CurrentUser.UserName;
                        saveData.UpdateUser = CurrentUser.UserName;
                        //modelResponse.FilePath = filePath;

                        _repoPostsManagement.Add(saveData);

                        _repoPostsManagement.SaveChanges();


                    }
                    else
                    {
                        var editData = data.GetDatabaseModel();
                        saveData = _repoPostsManagement.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (saveData != null)
                        {


                            saveData.Title = editData.Title;
                            saveData.Description = editData.Description;
                            saveData.Content = editData.Content;
                            saveData.PublishDate = editData.PublishDate;
                            saveData.TableOfContent = editData.TableOfContent;
                            saveData.isNew = editData.isNew;
                            saveData.SeeMore = editData.SeeMore;
                            saveData.Author = editData.Author;
                            saveData.LinkAuthor = editData.LinkAuthor;
                            saveData.TypePost = editData.TypePost;
                            saveData.Enabled = editData.Enabled;

                            saveData.CateID = editData.CateID;
                            saveData.SubCate = editData.SubCate;
                            saveData.Group = editData.Group;

                            saveData.UpdateUser = CurrentUser.UserName;
                            saveData.UpdateDate = DateTime.Now;


                            #region images 
                            saveData.Images_Description = editData.Images_Description;
                            saveData.Images_Alt = editData.Images_Alt;
                            saveData.Images_Caption = editData.Images_Caption;
                            #endregion
                            _repoPostsManagement.SaveChanges();

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
            rs.Data = saveData;
            return rs;

        }
        public bool SaveImage(SaveModel data)
        {

            try
            {
                var editData = _repoPostsManagement.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                var tempSaveImage = data.SaveImages(_config, editData);

                editData.Images = tempSaveImage.Images;
                editData.FilePath = tempSaveImage.FilePath;
                if (!String.IsNullOrEmpty(data.ListFiles))
                {
                    List<Database.PostsManagement_Files> files = new List<Database.PostsManagement_Files>();

                    if (data.ListImages != null)
                    {
                        foreach (var item in data.ListImages)
                        {
                            if (item.Status == "new")
                            {
                                var addFile = new Database.PostsManagement_Files
                                {
                                    FilePath = item.FilePath,
                                    PostsManagementId = editData.Pid,
                                    Caption = item.FileInfo.caption,
                                    Description = item.FileInfo.description,
                                    UploadFileName = item.FileName,
                                    Order = item.FileInfo.order,
                                    Alt = item.FileInfo.alt,
                                    CreateUser = CurrentUser.UserName,
                                    UpdateUser = CurrentUser.UserName
                                };
                                _repoPostsManagementFile.Add(addFile);
                            }
                            else if (item.Status == "edit")
                            {
                                var imageInfo = _repoPostsManagementFile.Query().Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                                imageInfo.Caption = item.FileInfo.caption;
                                imageInfo.Description = item.FileInfo.description;
                                imageInfo.Alt = item.FileInfo.alt;
                                imageInfo.Order = item.FileInfo.order;
                                imageInfo.UpdateDate = DateTime.Now;
                                imageInfo.UpdateUser = CurrentUser.UserName;

                            }
                            else if (item.Status == "delete")
                            {
                                var imageInfo = _repoPostsManagementFile.Query().Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                                if (imageInfo != null)
                                {
                                    _repoPostsManagementFile.Remove(imageInfo);

                                }
                            }
                        }


                    }
                    //SaveListFile(data, data.ListFiles);
                }
                _repoPostsManagement.SaveChanges();
                _repoPostsManagementFile.SaveChanges();


            }
            catch (Exception ex)
            {

                throw;
            }
            return true;
        }
        #region old
        //public void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
        //{
        //    try
        //    {

        //        if (filestatus == "remove" && file == null)
        //        {
        //            img = "";
        //            return;
        //        }

        //        if (filestatus != "existed")
        //        {

        //            if (!String.IsNullOrEmpty(filePath))
        //            {
        //                var arrFile = filePath.Split('/');
        //                img = arrFile[arrFile.Length - 1];
        //                imgFilePath = SystemInfo.PathFileManager + "/" + filePath.Replace(img, "");
        //            }
        //            else
        //            {
        //                if (file != null)
        //                {
        //                    img = _fileHelper.UploadImageModule(
        //                          new UploadImageInfo
        //                          {
        //                              FileName = title.ToSlug(),
        //                              Height = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxHeight].ToString()),
        //                              Width = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxWidth].ToString()),
        //                              Path = PostsManagementConstants.StaticPath.Asset.Image,
        //                              PathThumb = PostsManagementConstants.StaticPath.Asset.ImageThumb,
        //                              File = file
        //                          }
        //                          ).FileName;
        //                    //   img = _fileHelper.UploadImageToServer(
        //                    //new UploadImageInfo
        //                    //{
        //                    //    FileName = title,
        //                    //    Height = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxHeight].ToString()),
        //                    //    Width = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxWidth].ToString()),
        //                    //    Path = PostsManagementConstants.StaticPath.Asset.Image,
        //                    //    PathThumb = PostsManagementConstants.StaticPath.Asset.ImageThumb,
        //                    //    File = file
        //                    //}).Result;
        //                    imgFilePath = PostsManagementConstants.StaticPath.Asset.Image;
        //                }

        //            }

        //            //-------end-save-file-pond----------

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //public void SaveListFile(Database.PostsManagement data, string listFIlesString)
        //{
        //    List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

        //    try
        //    {

        //        var absolutepath = Directory.GetCurrentDirectory();

        //        List<Database.PostsManagement_Files> files = new List<Database.PostsManagement_Files>();
        //        foreach (var item in listFIles)
        //        {
        //            if (item.status == "new")
        //            {
        //                var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
        //                {
        //                    Base64 = item.dataUrl,
        //                    Height = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxHeight].ToString()),
        //                    Width = Convert.ToInt32(_config[PostsManagementConstants.Config.Admin.MaxWidth].ToString())
        //                     ,
        //                    FileName = data.Title.ToSlug(),
        //                    Path = PostsManagementConstants.StaticPath.Asset.Image
        //                });
        //                if (!saveFile.isError)
        //                {
        //                    files.Add(new Database.PostsManagement_Files
        //                    {
        //                        FilePath = PostsManagementConstants.StaticPath.Asset.Image,
        //                        PostsManagementId = data.Pid,
        //                        Caption = item.caption,
        //                        Description = item.description,
        //                        UploadFileName = saveFile.FileName,
        //                        Order = item.order,
        //                        CreateUser = "admin",
        //                        UpdateUser = "admin"
        //                    });

        //                }


        //            }
        //            else if (item.status == "delete")
        //            {
        //                //_fileHelper.DeleteFile(PostsManagementConstants.StaticPath.Asset.Image, item.name);
        //                var imageInfo = _db.PostsManagement_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
        //                if (imageInfo != null)
        //                {
        //                    _db.PostsManagement_Files.Remove(imageInfo);

        //                }


        //            }
        //            else if (item.status == "edit")
        //            {
        //                var imageInfo = _db.PostsManagement_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
        //                imageInfo.Caption = item.caption;
        //                imageInfo.Description = item.description;
        //                imageInfo.Alt = item.alt;
        //                imageInfo.Order = item.order;
        //                imageInfo.UpdateDate = DateTime.Now;
        //                imageInfo.UpdateUser = CurrentUser.UserName;

        //                _db.SaveChanges();
        //            }
        //            if (item.status == "server")
        //            {
        //                var arrFile = item.dataUrl.Split('/');
        //                var img = arrFile[arrFile.Length - 1];
        //                var filePath = item.dataUrl.Replace(img, "");
        //                files.Add(new Database.PostsManagement_Files
        //                {
        //                    PostsManagementId = data.Pid,
        //                    FilePath = filePath,
        //                    Caption = item.caption,
        //                    Description = item.description,
        //                    Alt = item.alt,
        //                    UploadFileName = img,
        //                    Order = item.order,
        //                    CreateUser = CurrentUser.UserName,
        //                    UpdateUser = CurrentUser.UserName,
        //                    UpdateDate = DateTime.Now,
        //                    CreateDate = DateTime.Now
        //                });


        //            }


        //        }
        //        _db.PostsManagement_Files.AddRange(files);

        //        _db.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();
            using (var transaction = _repoPostsManagement.BeginTransaction())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        _srvSEO.Delete(id, PostsManagementConstants.ModuleInfo.ModuleCode);

                        var model = _repoPostsManagement.Query().Where(p => p.Pid == id).FirstOrDefault();
                        _repoPostsManagement.Remove(model);


                        _repoPostsManagement.SaveChanges();
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
        public List<SelectControlData> GetPostsCategoryParent()
        {
            List<SelectControlData> listParent = new List<SelectControlData>();
            try
            {
                listParent = _repPostsCategory.Query().Where(p => p.Enabled == true && p.ParentID == 0).Select(
                 row => new SelectControlData
                 {
                     Value = row.Pid.ToString(),
                     Name = row.Title
                 }
             ).ToList<SelectControlData>();




            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return listParent;
        }

    }

}
