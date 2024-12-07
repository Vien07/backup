
using Microsoft.AspNetCore.Http;
using Admin.Collection.Database;
using Admin.Collection.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Admin.Collection.Constants;
using Admin.Collection.Models;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Text.Json;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Admin.SEO.Api.Models;
using Admin.PostsCategory.Database;
using Admin.ProductManagement.Services;
using Admin.ProductManagement.Database;
using Newtonsoft.Json;
using ComponentUILibrary.Models;
using Admin.PostsCategory.Constants;
using Admin.SEO.Services;
namespace Admin.Collection
{
    public class CollectionRepository : ICollectionRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        CollectionContext _db;
        PostsCategoryContext _dbCate;
        ISEOService _srvSEO;
        IProductCollectionService _repMSProduct;
        public CollectionRepository(PostsCategoryContext dbCate, CollectionContext db,
            ISEOService srvSEO, IProductCollectionService repMSProduct
            , IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _srvSEO = srvSEO;
            _dbCate = dbCate;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.CollectionConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
            _repMSProduct = repMSProduct;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //double limitRamGB = 0.11;
            //long UsedMemory = System.Diagnostics.Process.GetCurrentProcess().PagedMemorySize64;
            //double limitRamBytes = limitRamGB * 1024 * 1024 * 1024;
            //if (UsedMemory > limitRamBytes)
            //{
            //    GC.Collect(); // Collect all generations
            //      GC.Collect(2,GCCollectionMode.Forced);
            //}
        }
        public Response<Collection_List> GetList(CollectionModel.ParamSearch search)
        {
            Response<Collection_List> rs = new Response<Collection_List>();
            var list = new Collection_List();
            try
            {
                search.ToString();
                var listPost = _db.Collections.Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Title.Contains(search.KeySearch)))
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                     .ToPagedList(search.PageIndex, Convert.ToInt32(_config[CollectionConstants.Config.Admin.PageSize]));
                var pageCount = listPost.PageCount;
                var listCate = _dbCate.PostsCategories.ToList();
                //var listSeo = _dbSEO.SEOs.ToList();
                var posts = (
                       from a in listPost
                           //join b in listSeo on a.Pid equals b.PostPid
                       select new Collection_Item
                       {
                           Pid = a.Pid,
                           Title = a.Title,
                           //Slug = b.PostSlug,
                           Description = a.Description,
                           Images = a.Images,
                           Enabled = a.Enabled,
                           Order = a.Order,
                           CateID = a.CateID,
                           FilePath = a.FilePath,
                           ImagePath = SystemInfo.MedidaFileServer+ a.FilePath + a.Images,

                       }).ToList();
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
        public Response<CollectionModel.CollectionDetail> GetById(int id)
        {
            Response<CollectionModel.CollectionDetail> rs = new Response<CollectionModel.CollectionDetail>();
            CollectionModel.CollectionDetail detail = new CollectionModel.CollectionDetail();
            try
            {

                detail.Detail = _db.Collections.Where(p => p.Pid == id).FirstOrDefault();
                var file = _db.Collection_Files.Where(p => p.CollectionId == id).ToList();
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
        public Response<Database.Collection> Save(CollectionModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";
            var filePath = "";

            SaveImage(ref img, ref filePath,
           data.FileStatus,
           data.files,
           data.FilePath, data.Title);
            //-------end-save-file-pond----------


            var validator = new CollectionValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Collection> rs = new Response<Database.Collection>();
            Database.Collection modelResponse = new Database.Collection();
            if (!success)
            {
                string mess = string.Join(";", results.Errors);

                rs.Message = mess;
                rs.IsError = true;
                return rs;
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    modelResponse = data.GetDatabaseModel();
                    if (img != "")
                    {
                        modelResponse.Images = img;

                    }
                    if (filePath != "")
                    {
                        modelResponse.FilePath = filePath;

                    }
                    if (modelResponse.Pid == 0)
                    {
                        modelResponse.Order = 0.9;

                        //modelResponse.FilePath = filePath;

                        _db.Collections.Add(modelResponse);

                        _db.SaveChanges();
                    }
                    else
                    {
                        modelResponse = _db.Collections.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (modelResponse != null)
                        {
                            if (img != "")
                            {
                                modelResponse.Images = img;

                            }
                            if (filePath != "")
                            {
                                modelResponse.FilePath = filePath;

                            }
                            if (data.FileStatus == "remove" && data.files == null)
                            {
                                modelResponse.Images = "";
                                modelResponse.FilePath = "";

                            }
                            modelResponse.Title = data.Title;
                            modelResponse.Description = data.Description;
                            modelResponse.Content = data.Content;
                            modelResponse.PublishDate = data.PublishDate;

                            modelResponse.CateID = data.CateID;
                            modelResponse.ProductIDs = data.ProductIDs;
                            modelResponse.SubCate = data.SubCate;
                            //if (data.files != null && data.FilePath =="")
                            //{
                            //    _fileHelper.DeleteFile(CollectionConstants.StaticPath.Asset.Image, modelResponse.Images);
                            //    _fileHelper.DeleteFile(CollectionConstants.StaticPath.Asset.ImageThumb, modelResponse.Images);
                            //}
                            //modelResponse.Images = img;

                            #region images 
                            modelResponse.Images_Description = data.Images_Description;
                            modelResponse.Images_Alt = data.Images_Alt;
                            modelResponse.Images_Caption = data.Images_Caption;
                            #endregion
                            _db.SaveChanges();

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
                                      Height = Convert.ToInt32(_config[PostsCategoryConstants.Config.Admin.MaxHeight].ToString()),
                                      Width = Convert.ToInt32(_config[PostsCategoryConstants.Config.Admin.MaxWidth].ToString()),
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

        public void SaveListFile(Database.Collection data, string listFIlesString)
        {
            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                List<Database.Collection_Files> files = new List<Database.Collection_Files>();
                foreach (var item in listFIles)
                {
                    if (item.status == "new")
                    {
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = Convert.ToInt32(_config[CollectionConstants.Config.Admin.MaxHeight].ToString()),
                            Width = Convert.ToInt32(_config[CollectionConstants.Config.Admin.MaxWidth].ToString())
                             ,
                            FileName = data.Title.ToSlug(),
                            Path = CollectionConstants.StaticPath.Asset.Image
                        }); if (!saveFile.isError)
                        {
                            files.Add(new Database.Collection_Files { CollectionId = data.Pid, Caption = item.caption, Description = item.description, UploadFileName = saveFile.FileName, Order = item.order, CreateUser = "admin", UpdateUser = "admin" });

                        }
                    }
                    else if (item.status == "delete")
                    {
                        //_fileHelper.DeleteFile(CollectionConstants.StaticPath.Asset.Image, item.name);
                        var imageInfo = _db.Collection_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        _db.Collection_Files.Remove(imageInfo);


                    }
                    else if (item.status == "edit")
                    {
                        var imageInfo = _db.Collection_Files.Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        imageInfo.Caption = item.caption;
                        imageInfo.Description = item.description;
                        imageInfo.Order = item.order;
                        _db.SaveChanges();
                    }
                    if (item.status == "server")
                    {
                        var arrFile = item.name.Split('/');
                        var img = arrFile[arrFile.Length - 1];
                        var filePath = item.name.Replace(img, "");
                        files.Add(new Database.Collection_Files
                        {
                            CollectionId = data.Pid,
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
                _db.Collection_Files.AddRange(files);

                _db.SaveChanges();

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
                    _srvSEO.Delete(id, CollectionConstants.ModuleInfo.ModuleCode);

                    var model = _db.Collections.Where(p => p.Pid == id).FirstOrDefault();
                    _db.Collections.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    //var files = _db.Collection_Files.Where(p => p.CollectionId == model.Pid).ToList();
                    //if(files!= null)
                    //{
                    //    foreach (var file in files)
                    //    {
                    //        _fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, file.UploadFileName);

                    //    }
                    //    _db.Collection_Files.RemoveRange(files);

                    //}


                    //

                    _db.SaveChanges();
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
        public Response Enable(List<int> ids, bool isEnable)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.Collections.Where(p => p.Pid == id).FirstOrDefault();
                    model.Enabled = isEnable;
                    _db.SaveChanges();
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
        public Response EnableUpdateOrder()
        {

            Response rs = new Response();

            try
            {
                var list = _db.Collections.OrderBy(p => p.Order).ToList();
                var order = 1;
                foreach (var item in list)
                {
                    item.Order = order;
                    order = order + 1;
                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;

        }
        public Response UpdateOrder(int id, double order)
        {

            Response rs = new Response();

            try
            {
                var model = _db.Collections.Where(p => p.Pid == id).FirstOrDefault();
                model.Order = order;
                _db.SaveChanges();


            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "id:" + id.ToString());

            }
            return rs;

        }
        public Response Move(int fromId, int toId)
        {

            Response rs = new Response();

            try
            {
                var fromModel = _db.Collections.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Collections.Where(p => p.Pid == toId).FirstOrDefault();

                if (fromModel != null && fromModel != null)
                {
                    var fromOrder = fromModel.Order;
                    var toOrder = toModel.Order;
                    if (fromOrder > toOrder)
                    {
                        fromModel.Order = toModel.Order - 0.00001;

                    }
                    else if (fromOrder < toOrder)
                    {
                        fromModel.Order = toModel.Order + 0.00001;
                    }

                    _db.SaveChanges();
                    var list = _db.Collections.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.Collections.OrderBy(p => p.Order).ToList();
                //var stt = 1;
                //foreach (var item in list)
                //{
                //    item.Order = stt;
                //    stt = stt + 1;
                //    _db.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<List<Database.CollectionConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<Database.CollectionConfig>> rs = new Response<List<Database.CollectionConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    Database.CollectionConfig CollectionConfig = _db.CollectionConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (CollectionConfig != null)
                    {
                        CollectionConfig.Type = tab;
                        CollectionConfig.Value = value;
                        CollectionConfig.UpdateDate = DateTime.Now;
                        CollectionConfig.UpdateUser = "";

                    }
                    else
                    {
                        CollectionConfig = new Database.CollectionConfig();
                        CollectionConfig.Type = tab;

                        CollectionConfig.Key = key;
                        CollectionConfig.Group = "";
                        CollectionConfig.Value = value;
                        CollectionConfig.CreateDate = DateTime.Now;
                        CollectionConfig.CreateUser = "";
                        CollectionConfig.UpdateDate = DateTime.Now;
                        CollectionConfig.UpdateUser = "";
                        _db.CollectionConfigs.Add(CollectionConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.CollectionConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<List<Database.CollectionConfig>> GetAllConfigs()
        {
            Response<List<Database.CollectionConfig>> rs = new Response<List<Database.CollectionConfig>>();
            try
            {

                var listConfig = _db.CollectionConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<Database.CollectionConfig> GetConfigByKey(string key)
        {
            Response<Database.CollectionConfig> rs = new Response<Database.CollectionConfig>();
            try
            {

                var config = _db.CollectionConfigs.Where(p => p.Key == key).FirstOrDefault();
                rs.Data = config;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public List<Admin.PostsCategory.Database.PostsCategory> GetChildrenPostCategory(long parentId)
        {
            var result = new List<Admin.PostsCategory.Database.PostsCategory>();
            var childrens = _dbCate.PostsCategories.Where(s => s.ParentID == parentId && s.Enabled == true && s.Deleted == false);

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
        public List<SelectControlData> GetCatesForCollection()
        {
            List<SelectControlData> result = new List<SelectControlData>();
            try
            {
                result = _repMSProduct.GetProductCates();
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public List<Collection_Product_Item> GetProductOfCollection(string sku)
        {
            List<Collection_Product_Item> result = new List<Collection_Product_Item>();
            try
            {
                var rs = _repMSProduct.GetListProductByListSKU(sku);
                string jsonString = JsonConvert.SerializeObject(rs);

                result = JsonConvert.DeserializeObject<List<Collection_Product_Item>>(jsonString);
                return result;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public IPagedList<Collection_Product_Item> GetListProductOfCollection(long pid)
        {
            List<Collection_Product_Item> result = new List<Collection_Product_Item>();
            try
            {
                var listSKU = _db.Collections.Where(s => s.Pid == pid).FirstOrDefault().ProductIDs;
                var rs = _repMSProduct.GetListProductByListSKU(listSKU);
                string jsonString = JsonConvert.SerializeObject(rs);

                result = JsonConvert.DeserializeObject<List<Collection_Product_Item>>(jsonString);
                return result.ToPagedList(1, 10);
            }
            catch (Exception)
            {

                return null;
            }


        }
        public Collection_Product_List GetListProducts(Collection_Product_List.ParamSearch input)
        {
            Collection_Product_List result = new Collection_Product_List();
            try
            {
                var rs = _repMSProduct.GetListProduct(input);
                string jsonString = JsonConvert.SerializeObject(rs);

                result = JsonConvert.DeserializeObject<Collection_Product_List>(jsonString);
                
            }
            catch (Exception ex)
            {

            }

            return result;

        }

    }

}
