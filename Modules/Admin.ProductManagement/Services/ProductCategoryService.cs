using Microsoft.AspNetCore.Http;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;
using Newtonsoft.Json;
using ComponentUILibrary.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Steam.Core.Utilities.SteamModels;
using Admin.TemplatePage.Database;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Helpers;
using Admin.ProductManagement.Constants;
using Admin.SEO;
using AutoMapper;
using Admin.ProductManagement.Repository;
using Admin.ProductManagement.Models.ViewModels.ProductCategory;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.ProductManagement.Models.ViewModels.Product;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.UpdateModels;
using System.Collections.Generic;
using Admin.SEO.Services;
using Steam.Infrastructure.Repository;
namespace Admin.ProductManagement.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductManagementRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepositoryConfig<Database.ProductCategoryConfig> _repCateConfig;

        private readonly ISEOService _srvRepository;

        private readonly TemplatePageContext _dbTemplatePage;

        private readonly ILoggerHelper _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        private Dictionary<string, string> _config;

        public ProductCategoryService(
             IRepositoryConfig<Database.ProductCategoryConfig> repCateConfig,
            IProductManagementRepository<ProductCategory> productCategoryRepository,
            ISEOService srvRepository,
            TemplatePageContext dbTemplatePage,
            IFileHelper fileHelper,
            IMapper mapper,
            ILoggerHelper logger)
        {
            _productCategoryRepository = productCategoryRepository;
            _repCateConfig = repCateConfig;
            _srvRepository = srvRepository;

            _dbTemplatePage = dbTemplatePage;

            _mapper = mapper;
            _logger = logger;
            _fileHelper = fileHelper;

            _config = _repCateConfig.GetAllConfigs() ;
        }

        public Response<ProductCategoryPagedViewModel> GetList(ProductCategorySearchModel search)
        {
            Response<ProductCategoryPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var query = _productCategoryRepository
                    .Query()
                    .Where(p => (search.Cate == "0" || p.ParentID == Convert.ToInt32(search.Cate)))
                    .OrderByDescending(p => p.Order)
                    .ThenByDescending(p => p.UpdateDate);

                var pagedModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[ProductConstants.ConfigAdmin.PageSize]));
                var list = pagedModel.ToList();

                result.Data.Items = _mapper.Map<List<ProductCategoryDto>>(list);
                result.Data.PageSize = pagedModel.PageSize;
                result.Data.PageNumber = pagedModel.PageNumber;
                result.Data.PageCount = pagedModel.PageCount;

                if (search.Cate != "0")
                {
                    var parentMisaProductCategory = _productCategoryRepository.Query().Where(p => p.Pid == Convert.ToInt32(search.Cate)).FirstOrDefault();
                    result.Data.Items.Add(_mapper.Map<ProductCategoryDto>(parentMisaProductCategory));
                }
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());
            }
            return result;
        }
        public Response<ProductCategoryViewModel> GetById(long id)
        {
            Response<ProductCategoryViewModel> result = new();
            result.Data = new();
            try
            {
                var model = _productCategoryRepository.Query().Where(p => !p.Deleted && p.Pid == id).FirstOrDefault();
                result.Data.Detail = _mapper.Map<ProductCategoryDto>(model);

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

        public Response<ProductCategoryViewModel> Save(ProductCategorySaveModel data)
        {
            //-------save-file-pond----------
            //-------save-file-pond----------
            var img_thumbnail = "";
            var filePath_thumbnail = "";
            var img_banner = "";
            var filePath_banner = "";

            SaveImage(ref img_thumbnail, ref filePath_thumbnail,
                data.FileStatus_Thumbnail,
                data.file_Thumbnail,
                data.FilePath_Thumbnail, data.Title);

            SaveImage(ref img_banner, ref filePath_banner,
                  data.FileStatus_Banner,
                  data.file_Banner,
                  data.FilePath_Banner, data.Title);

            //-------end-save-file-pond----------


            var validator = new ProductCategorySaveModelValidator();

            // Execute the validator
            ValidationResult validationResults = validator.Validate(data);

            // Inspect any validation failures.
            bool success = validationResults.IsValid;
            List<ValidationFailure> failures = validationResults.Errors;

            Response<ProductCategoryViewModel> result = new();
            result.Data = new();
            ProductCategory modelResponse = new();

            using (var transaction = _productCategoryRepository.BeginTransaction())
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
                        var newProductCategory = _mapper.Map<ProductCategory>(data);
                        var tempParrent = _productCategoryRepository.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                        newProductCategory.Order = 0.9;

                        if (tempParrent != null)
                        {
                            newProductCategory.ShowLevel = tempParrent.ShowLevel + 1;
                            path += tempParrent.Path + "-" + tempParrent.Pid;
                        }

                        newProductCategory.Path = path;
                        _productCategoryRepository.Add(newProductCategory);
                        _productCategoryRepository.SaveChanges();
                        modelResponse = newProductCategory;
                    }
                    else
                    {
                        var editModel = _mapper.Map<ProductCategoryUpdateModel>(data);
                        var updateModel = _productCategoryRepository.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (updateModel != null)
                        {
                            _mapper.Map(editModel, updateModel);

                            if (img_thumbnail != "")
                            {
                                updateModel.Images = img_thumbnail;
                            }

                            if (filePath_thumbnail != "")
                            {
                                updateModel.FilePath = filePath_thumbnail;
                            }

                            if (img_banner != "")
                            {
                                updateModel.Banner = img_banner;
                            }

                            if (filePath_banner != "")
                            {
                                updateModel.BannerFilePath = filePath_banner;
                            }

                            if (updateModel.ParentID != data.ParentID)
                            {
                                if (data.ParentID == 0)
                                {
                                    updateModel.ShowLevel = 0;
                                    path += data.Pid.ToString() + "-";
                                }
                                else
                                {
                                    var modelParent = _productCategoryRepository.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                                    updateModel.ShowLevel = modelParent.ShowLevel + 1;
                                    path += modelParent.Path + "-" + modelParent.Pid;
                                }
                                updateModel.Path = path;
                                _productCategoryRepository.SaveChanges();
                                SetChildLevel(updateModel.Pid, updateModel.ShowLevel, updateModel.Enabled, updateModel.Path);
                            }
                            _productCategoryRepository.SaveChanges();
                            modelResponse = updateModel;
                        }
                    }
                    transaction.Commit();
                    result.Data.Detail = _mapper.Map<ProductCategoryDto>(modelResponse);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.IsError = true;
                    result.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());
                }
            }
            return result;
        }
        private void SaveImage(ref string img, ref string imgFilePath, string fileStatus, IFormFile file, string filePath, string title)
        {
            try
            {
                if (fileStatus != "existed")
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
                                     Height = Convert.ToInt32(_config[ProductCategoryConstants.Config.Admin.MaxHeight].ToString()),
                                     Width = Convert.ToInt32(_config[ProductCategoryConstants.Config.Admin.MaxWidth].ToString()),
                                     Path = ProductCategoryConstants.StaticPath.Asset.Image,
                                     PathThumb = ProductCategoryConstants.StaticPath.Asset.ImageThumb,
                                     File = file
                                 }
                                 ).FileName;
                            imgFilePath = ProductCategoryConstants.StaticPath.Asset.Image;
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
        private void SetChildLevel(long Pid, int Showlevel, bool isEnabled, string path)
        {
            var modelChild = _productCategoryRepository.Query().Where(p => p.ParentID == Pid).ToList();
            if (modelChild.Count > 0)
            {
                foreach (var child in modelChild)
                {
                    child.ShowLevel = Showlevel + 1;
                    child.Enabled = isEnabled;
                    child.Path = path + "-" + child.ParentID;
                    _productCategoryRepository.SaveChanges();
                    SetChildLevel(child.Pid, child.ShowLevel, child.Enabled, child.Path);
                }
            }
        }

        public Response Delete(long id)
        {
            Response rs = new Response();
            try
            {
                _srvRepository.Delete((int)id, ProductCategoryConstants.ModuleInfo.ModuleCode);
                DeleteProductCategory(id);
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());
            }
            return rs;
        }
        private void DeleteProductCategory(long id)
        {
            var model = _productCategoryRepository.Query().Where(p => p.Pid == id).FirstOrDefault();
            var listChild = _productCategoryRepository.Query().Where(p => p.ParentID == id).ToList();
            if (listChild.Any())
            {
                foreach (var item in listChild)
                {
                    DeleteProductCategory(item.Pid);
                }
            }
            _productCategoryRepository.Remove(model);
            _productCategoryRepository.SaveChanges();
        }



        public Response<List<SelectControlData>> GetProductCategoryParent(long id)
        {
            Response<List<SelectControlData>> result = new();
            List<SelectControlData> listParent = new();
            try
            {
                var e = _productCategoryRepository.Query().Where(p => p.Pid == id).FirstOrDefault();
                if (e != null)
                {
                    listParent = _productCategoryRepository.Query().Where(p => p.Enabled == true && p.Pid != id && p.ShowLevel <= e.ShowLevel).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title
                     }).ToList();
                }
                else
                {
                    listParent = _productCategoryRepository.Query().Where(p => p.Enabled == true && p.Pid != id).Select(
                       row => new SelectControlData
                       {
                           Value = row.Pid.ToString(),
                           Name = row.Title
                       }).ToList();
                }
                result.Data = listParent;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return result;
        }
        public Response<List<SelectControlData>> GetProductCategoryTreeChildrenByParentId(long parentId)
        {
            Response<List<SelectControlData>> result = new();
            List<SelectControlData> items = new();
            try
            {
                var parent = _productCategoryRepository.Query().Where(p => p.Pid == parentId && p.ParentID == 0 && p.Enabled == true && p.Deleted == false).FirstOrDefault();
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
                             }).ToList();
                    }
                }
                result.Data = items;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return result;
        }
        public Response<List<SelectControlData>> GetListTemplatePage(string type)
        {
            Response<List<SelectControlData>> result = new();
            try
            {
                var list = _dbTemplatePage.TemplatePages.Where(p => (type == "" || p.PageType == type) && p.Enabled == true).Select(
                     row => new SelectControlData
                     {
                         Value = row.Url,
                         Name = row.Url
                     }).ToList();
                result.Data = list;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return result;
        }
        public List<ProductCategoryDto> GetChildrenPostCategory(long parentId)
        {
            var result = new List<ProductCategoryDto>();
            var childrens = _productCategoryRepository.Query().Where(s => s.ParentID == parentId && s.Enabled == true && s.Deleted == false);
            var childrensDto = _mapper.Map<List<ProductCategoryDto>>(childrens);
            if (childrensDto != null && childrensDto.Count() > 0)
            {
                result.AddRange(childrensDto);
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
            Response<string> result = new();
            try
            {
                var listPageTemplate = _dbTemplatePage.TemplatePages.ToList();
                MisaProductCategoryHelper helper = new MisaProductCategoryHelper();
                var listCate = _productCategoryRepository.Query().Where(p => p.ParentID == 0).ToList();
                result = helper.GenerateXMLRewriteUrl(_config[ProductCategoryConstants.Config.Website.PreSlugCate].ToString(),
                    _config[ProductCategoryConstants.Config.Website.PreSlugDetail].ToString(),
                    _config[ProductCategoryConstants.Config.Website.CatePage].ToString(),
                    _config[ProductCategoryConstants.Config.Website.DetailPage].ToString(),
                    _config[ProductCategoryConstants.Config.Website.Parameter].ToString());
                return result;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.StatusCode = 500;
                result.Message = ex.ToString();
                result.Data = "Lỗi không xác định";
                return result;
            }
        }
    }
}
