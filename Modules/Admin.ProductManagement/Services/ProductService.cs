using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Models.ViewModels.Product;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Admin.ProductManagement.Constants;
using Steam.Core.Base.Constant;
using Admin.ProductManagement.DTOs;
using Admin.ProductManagement.Enums;
using Microsoft.EntityFrameworkCore;
using Admin.SEO;
using Newtonsoft.Json;
using AutoMapper;
using Admin.ProductManagement.Repository;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.UpdateModels;
using Admin.ProductManagement.DataTransferObjects.MisaReponse;
using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.SEO.Services;
namespace Admin.ProductManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductManagementRepository<ProductDetail> _productDetailRepository;
        private readonly IProductManagementRepository<ProductConfig> _productConfigRepository;
        private readonly IProductManagementRepository<Product_Files> _productFileRepository;
        private readonly IProductManagementRepository<ProductDetail_PostDetail> _productDetail_PostDetailRepository;

        private readonly ISEOService _srvSEO;
        private readonly IMisaApiService _misaApiService;

        private readonly ILoggerHelper _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        private Dictionary<string, string> _config;

        public ProductService(
            IProductRepository productRepository,
            IProductManagementRepository<ProductDetail> productDetailRepository,
            IProductManagementRepository<ProductConfig> productConfigRepository,
            IProductManagementRepository<Product_Files> productFileRepository,
            IProductManagementRepository<ProductDetail_PostDetail> productDetail_PostDetailRepository,
            ISEOService srvSEOository,
            IMisaApiService misaApiService,
            IFileHelper fileHelper,
            ILoggerHelper logger,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productDetailRepository = productDetailRepository;
            _productConfigRepository = productConfigRepository;
            _productFileRepository = productFileRepository;
            _productDetail_PostDetailRepository = productDetail_PostDetailRepository;

            _misaApiService = misaApiService;
            _srvSEO = srvSEOository;

            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;

            _config = _productConfigRepository.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
        }

        public Response<ProductPagedViewModel> GetList(ProductSearchModel search)
        {
            Response<ProductPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var keySearch = search.KeySearch;
                var query = _productRepository
                    .Query()
                    .Where(p => (string.IsNullOrEmpty(keySearch) || p.Title.Contains(keySearch)) && !p.Deleted)
                    .OrderBy(p => p.Order)
                    .ThenBy(p => p.UpdateDate);
                var pagedModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[ProductConstants.ConfigAdmin.PageSize]));
                var list = pagedModel.ToList();

                result.Data.Items = _mapper.Map<List<ProductDto>>(list);
                result.Data.PageSize = pagedModel.PageSize;
                result.Data.PageNumber = pagedModel.PageNumber;
                result.Data.PageCount = pagedModel.PageCount;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());
            }
            return result;
        }
        public Response<ProductDetailPagedViewModel> GetProductDetailList(long parentId)
        {
            Response<ProductDetailPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var query = _productRepository.GetProductDetail(parentId);
                result.Data.Items = query.ToList();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return result;
        }
        public Response<ProductViewModel> GetById(long id)
        {
            Response<ProductViewModel> result = new();
            result.Data = new();
            try
            {
                var files = _productFileRepository.Query().Where(p => p.Pid == id).ToList();
                result.Data.Files = _mapper.Map<List<ProductFileDto>>(files);

                var model = _productRepository.Query().Where(p => !p.Deleted && p.Pid == id).FirstOrDefault();
                result.Data.Detail = _mapper.Map<ProductDto>(model);

                var postIds = _productDetail_PostDetailRepository.Query().Where(p => p.ProductID == id).Select(p=>p.PostID).ToList();
                result.Data.Detail.PostIds = postIds;

                var productChildren = _productRepository.GetProductDetail(id).ToList();
                result.Data.ProductChildren = productChildren;

                result.IsError = false;
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());
            }
            return result;
        }

        public Response<ProductViewModel> Save(ProductSaveModel data)
        {
            //-------save-file-pond----------
            //-------save-file-pond----------
            var img_front = "";
            var filePath_front = "";
            var img_back = "";
            var filePath_back = "";

            SaveImage(ref img_front, ref filePath_front,
                data.FileStatus,
                data.Files,
                data.FilePath, data.Title);

            SaveImage(ref img_back, ref filePath_back,
                  data.BackFileStatus,
                  data.BackFiles,
                  data.BackFilePath, data.Title);


            var validator = new ProductSaveModelValidator();

            // Execute the validator
            ValidationResult validationResults = validator.Validate(data);

            // Inspect any validation failures.
            bool success = validationResults.IsValid;
            List<ValidationFailure> failures = validationResults.Errors;
            Response<ProductViewModel> result = new();
            result.Data = new();
            Product modelResponse = new();
            using (var transaction = _productRepository.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        var newProduct = _mapper.Map<Product>(data);
                        newProduct.Images = img_front;
                        newProduct.BackImages = img_back;
                        newProduct.FilePath = filePath_front;
                        newProduct.BackFilePath = filePath_back;
                        newProduct.Order = 0.9;
                        _productRepository.Add(newProduct);
                        _productRepository.SaveChanges();

                        modelResponse = newProduct;
                    }
                    else
                    {
                        var editModel = _mapper.Map<ProductUpdateModel>(data);
                        var updateModel = _productRepository.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (updateModel != null)
                        {
                            _mapper.Map(editModel, updateModel);

                            if (!string.IsNullOrEmpty(img_front))
                            {
                                updateModel.Images = img_front;
                            }

                            if (!string.IsNullOrEmpty(img_back))
                            {
                                updateModel.BackImages = img_back;
                            }

                            if (!string.IsNullOrEmpty(filePath_front))
                            {
                                updateModel.FilePath = filePath_front;
                            }

                            if (!string.IsNullOrEmpty(filePath_back))
                            {
                                updateModel.BackFilePath = filePath_back;
                            }

                            if (data.BackFileStatus == "remove" && data.BackFiles == null)
                            {
                                updateModel.BackImages = "";
                                updateModel.BackFilePath = "";
                            }

                            if (data.FileStatus == "remove" && data.Images == null)
                            {
                                updateModel.Images = "";
                                updateModel.FilePath = "";
                            }
                            _productRepository.SaveChanges();
                            modelResponse = updateModel;
                        }
                    }
                    //-----save-list-file-dropzone-----
                    if (!String.IsNullOrEmpty(data.ListFiles))
                    {
                        SaveListFile(data, data.ListFiles);
                    }
                    //---------end save list lisst file--------
                    transaction.Commit();
                    result.Data.Detail = _mapper.Map<ProductDto>(modelResponse);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.Data.Detail = null;
                    result.IsError = true;
                    result.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());
                }
            }
            return result;
        }
        private void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
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
                                     Height = Convert.ToInt32(_config[ProductConstants.ConfigAdmin.MaxHeight].ToString()),
                                     Width = Convert.ToInt32(_config[ProductConstants.ConfigAdmin.MaxWidth].ToString()),
                                     Path = ProductConstants.ConfigAsset.Image,
                                     PathThumb = ProductConstants.ConfigAsset.ImageThumb,
                                     File = file
                                 }
                                 ).FileName;
                            imgFilePath = ProductConstants.ConfigAsset.Image;
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
        private void SaveListFile(ProductSaveModel data, string listFileStrings)
        {
            List<FileInfoModel> listFiles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFileStrings);
            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                List<Database.Product_Files> files = new List<Database.Product_Files>();
                foreach (var item in listFiles)
                {
                    if (item.status == "new")
                    {
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = Convert.ToInt32(_config[ProductConstants.ConfigAdmin.MaxHeight].ToString()),
                            Width = Convert.ToInt32(_config[ProductConstants.ConfigAdmin.MaxWidth].ToString())
                             ,
                            FileName = data.Title.ToSlug(),
                            Path = ProductConstants.ConfigAsset.Image
                        }); if (!saveFile.isError)
                        {
                            files.Add(new Database.Product_Files
                            {
                                MisaProductId = data.Pid,
                                Alt = item.alt,
                                Caption = item.caption,
                                Description = item.description,
                                UploadFileName = saveFile.FileName,
                                Order = item.order,
                                CreateUser = "admin",
                                UpdateUser = "admin"
                            });

                        }
                    }
                    else if (item.status == "delete")
                    {
                        //_fileHelper.DeleteFile(MisaProductConstants.StaticPath.Asset.Image, item.name);
                        var imageInfo = _productFileRepository.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        _productFileRepository.Remove(imageInfo);


                    }
                    else if (item.status == "edit")
                    {
                        var imageInfo = _productFileRepository.Query().Where(p => p.Pid == Convert.ToInt32(item.id)).FirstOrDefault();
                        imageInfo.Caption = item.caption;
                        imageInfo.Description = item.description;
                        imageInfo.Order = item.order;
                        imageInfo.Alt = item.alt;
                        _productFileRepository.SaveChanges();
                    }
                    if (item.status == "server")
                    {
                        var arrFile = item.name.Split('/');
                        var img = arrFile[arrFile.Length - 1];
                        var filePath = item.name.Replace(img, "");
                        files.Add(new Database.Product_Files
                        {
                            MisaProductId = data.Pid,
                            FilePath = filePath,
                            Caption = item.caption,
                            Description = item.description,
                            Alt = item.alt,
                            UploadFileName = img,
                            Order = item.order,
                            CreateUser = "admin",
                            UpdateUser = "admin"
                        });
                    }


                }
                _productFileRepository.AddRange(files);
                _productFileRepository.SaveChanges();
            }
            catch { }
        }

        public Response Delete(List<long> ids)
        {
            Response result = new();
            using (var transaction = _productRepository.BeginTransaction())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var product = _productRepository.Query().Where(x => x.Pid == id).FirstOrDefault();
                        if (product != null)
                        {
                            _productRepository.Remove(product);
                            _productRepository.SaveChanges();
                            _srvSEO.Delete((int)id, ProductConstants.ModuleInfo.ModuleCode);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    result.IsError = true;
                    result.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
                }
            }
            return result;
        }
        public Response DeleteProductDetail(List<long> ids)
        {
            Response result = new();
            try
            {
                var listProductDetails = _productDetailRepository.Query().Where(x => ids.Contains(x.Pid));
                _productDetailRepository.RemoveRange(listProductDetails);
                _productDetailRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
            }
            return result;
        }

        public Response Enable(List<long> ids, bool isEnable)
        {
            Response result = new();
            try
            {
                var listProducts = _productRepository.Query().Where(x => ids.Contains(x.Pid)).ToList();
                foreach (var item in listProducts)
                {
                    item.Enabled = isEnable;
                }
                _productRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
            }
            return result;
        }
        public Response EnableUpdateOrder()
        {
            Response result = new();
            try
            {
                var list = _productRepository.Query().OrderBy(p => p.Order).ToList();
                var order = 1;
                foreach (var item in list)
                {
                    item.Order = order;
                    order = order + 1;
                    _productRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return result;
        }
        public Response UpdateOrder(int id, double order)
        {
            Response result = new();
            try
            {
                var model = _productRepository.Query().Where(p => p.Pid == id).FirstOrDefault();
                model.Order = order;
                _productRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "id:" + id.ToString());
            }
            return result;
        }
        public Response Move(int fromId, int toId)
        {
            Response result = new();
            try
            {
                var fromModel = _productRepository.Query().Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _productRepository.Query().Where(p => p.Pid == toId).FirstOrDefault();

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

                    _productRepository.SaveChanges();
                    var list = _productRepository.Query().OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                    }
                    _productRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return result;
        }

        public Response<ProductConfigViewModel> SaveConfig(IFormCollection formData)
        {
            Response<ProductConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var configs = _productConfigRepository.Query();
                var listConfig = configs.ToList();
                foreach (var item in formData)
                {
                    var key = item.Key;
                    var value = item.Value;
                    var config = listConfig.Where(p => p.Key == key).FirstOrDefault();
                    if (config != null)
                    {
                        config.Value = value;
                        config.UpdateDate = DateTime.Now;
                        config.UpdateUser = string.Empty;
                    }
                    else
                    {
                        config = new();
                        config.Key = key;
                        config.Value = value;
                        config.CreateDate = DateTime.Now;
                        config.CreateUser = string.Empty;
                        config.UpdateDate = DateTime.Now;
                        config.UpdateUser = string.Empty;
                        _productConfigRepository.Add(config);
                    }
                    _productConfigRepository.SaveChanges();
                }

                listConfig = configs.ToList();
                result.Data.Items = _mapper.Map<List<ProductConfigDto>>(listConfig);
                result.StatusCode = 200;
                return result;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.StatusCode = 500;
                result.Message = "Lỗi không xác định";
                return result;
            }
        }
        public Response<ProductConfigViewModel> GetAllConfigs()
        {
            Response<ProductConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var listConfig = _productConfigRepository.Query().ToList();
                result.Data.Items = _mapper.Map<List<ProductConfigDto>>(listConfig);
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.StatusCode = 500;
                result.Message = "Lỗi không xác định";
            }
            return result;
        }
        public Response<ProductConfigViewModel> GetConfigByKey(string key)
        {
            Response<ProductConfigViewModel> result = new();
            try
            {
                var config = _productConfigRepository.Query().Where(p => p.Key == key).FirstOrDefault();
                result.Data.Item = _mapper.Map<ProductConfigDto>(config);
                result.StatusCode = 200;
            }
            catch
            {
                result.IsError = true;
                result.StatusCode = 500;
                result.Message = "Lỗi không xác định";
            }
            return result;
        }

        #region SYNC MISA
        public Response<bool> SyncMisa(SyncMisaProductSearchModel param)
        {
            Response<bool> result = new();
            try
            {
                var pageIndex = 1;
                var pageSize = 100;

                while (pageIndex > 0)
                {
                    MisaResponseModel<List<MisaResponseProductDto>> response = _misaApiService.GetProductList(param.SyncDate, pageIndex, pageSize);

                    bool checkContinue = response.Success && response.Data.Any();
                    if (checkContinue)
                    {
                        CreateOrUpdateProduct(response.Data, param);
                        pageIndex++;
                    }
                    else { pageIndex = 0; }
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        private void CreateOrUpdateProduct(List<MisaResponseProductDto> data, SyncMisaProductSearchModel param)
        {
            try
            {
                //đồng bộ sản phẩm cũ
                if (param.SyncOldProduct)
                {
                    //kiểm tra có thuộc tính cần đồng bộ ko
                    if (!string.IsNullOrEmpty(param.SelectMultiSyncMisa))
                    {
                        // parse các thuộc tính cần đồng bộ
                        var arrProperties = param.SelectMultiSyncMisa.Split(",");
                        foreach (var item in data)
                        {
                            UpdateProduct(item, arrProperties);
                        }
                    }
                }

                //kéo sản phẩm mới
                if (param.SyncNewProduct)
                {
                    //lấy tất cả sản phẩm mới để kiểm tra trùng trong hệ thống
                    var getCurrentMisaProductIDList = _productRepository.Query().Select(x => x.MisaProductID).ToList();

                    foreach (var item in data)
                    {
                        //kiểm tra sản phẩm đã có trong hệ thống
                        if (!getCurrentMisaProductIDList.Contains(item.Id))
                        {
                            CreateProduct(item);
                        }
                    }
                }
            }
            catch { }
        }
        private void CreateProduct(MisaResponseProductDto model)
        {
            try
            {
                var product = _mapper.Map<Product>(model);
                product.UpdateUser = "admin";
                product.CreateUser = "admin";
                _productRepository.Add(product);
                _productRepository.SaveChanges();

                if (model.ListDetail != null && model.ListDetail.Any())
                {
                    var productDetails = _mapper.Map<List<ProductDetail>>(model.ListDetail);
                    foreach (var item in productDetails)
                    {
                        item.UpdateUser = "admin";
                        item.CreateUser = "admin";
                        item.ParentPid = Convert.ToInt32(product.Pid);
                    }
                    _productDetailRepository.AddRange(productDetails);
                }
            }
            catch { }
        }
        private void UpdateProduct(MisaResponseProductDto model, string[] arrProperties)
        {
            try
            {
                var product = _productRepository.Query().FirstOrDefault(x => x.MisaProductID.Equals(model.Id));
                //đồng bộ thuộc tính sản phẩm cha
                if (product != null)
                {
                    product.UpdateDate = DateTime.Now;
                    product.UnitID = model.UnitId;
                    foreach (var prop in arrProperties)
                    {
                        switch (prop)
                        {
                            case nameof(EnumSyncProperties.Sku):
                                product.Sku = model.Code;
                                break;
                            case nameof(EnumSyncProperties.Title):
                                product.Title = model.Name;
                                break;
                            case nameof(EnumSyncProperties.SellingPrice):
                                product.SellingPrice = model.SellingPrice;
                                break;
                            case nameof(EnumSyncProperties.Description):
                                product.Description = model.Description;
                                break;
                        }
                    }
                    _productRepository.SaveChanges();

                    if (model.ListDetail != null && model.ListDetail.Any())
                    {
                        foreach (var ele in model.ListDetail)
                        {
                            CreateOrUpdateProductDetail(ele, arrProperties, model.Id);
                        }
                    }
                }
            }
            catch { }
        }
        private void CreateOrUpdateProductDetail(MisaResponseProductDetailDto model, string[] arrProperties, Guid parentId)
        {
            try
            {
                var product = _productRepository.Query().FirstOrDefault(x => x.MisaProductID.Equals(parentId));
                var productDetail = _productDetailRepository.Query().FirstOrDefault(x => x.MisaProductID.Equals(model.Id));
                //đồng bộ thuộc tính sản phẩm con
                if (productDetail != null)
                {
                    foreach (var prop in arrProperties)
                    {
                        switch (prop)
                        {
                            case nameof(EnumSyncProperties.Sku):
                                productDetail.Sku = model.Code;
                                break;
                            case nameof(EnumSyncProperties.Title):
                                productDetail.Title = model.Name;
                                break;
                            case nameof(EnumSyncProperties.SellingPrice):
                                productDetail.SellingPrice = model.SellingPrice;
                                break;
                            case nameof(EnumSyncProperties.Size):
                                productDetail.Size = model.Size;
                                break;
                            case nameof(EnumSyncProperties.Color):
                                productDetail.Color = model.Color;
                                break;
                            default:
                                break;
                        };
                    }
                    productDetail.UpdateDate = DateTime.Now;
                    _productDetailRepository.SaveChanges();
                }
                //tạo sản phẩm con mới
                else if (arrProperties.Contains(nameof(EnumSyncProperties.Color)))
                {
                    if (product != null)
                    {
                        var newProductDetail = _mapper.Map<ProductDetail>(model);
                        newProductDetail.ParentPid = Convert.ToInt32(product.Pid);
                        newProductDetail.UpdateUser = "admin";
                        newProductDetail.CreateUser = "admin";
                        _productDetailRepository.Add(newProductDetail);
                        _productDetailRepository.SaveChanges();
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
