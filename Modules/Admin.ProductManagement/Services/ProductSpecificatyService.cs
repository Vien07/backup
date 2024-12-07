using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Database;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Repository;
using Admin.ProductManagement.Models.ViewModels.ProductSpecificaty;
using Admin.ProductManagement.Models.SearchModels;
using AutoMapper;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.UpdateModels;
using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;

namespace Admin.ProductManagement.Services
{
    public class ProductSpecificatyService : IProductSpecificatyService
    {
        private readonly IProductManagementRepository<ProductSpecificaty> _productSpecificatyRepository;
        private readonly IProductManagementRepository<ProductSpecificatyConfig> _productSpecificatyConfigRepository;

        private readonly ILoggerHelper _logger;
        private readonly IMapper _mapper;

        private Dictionary<string, string> _config;
        public ProductSpecificatyService(
            IProductManagementRepository<ProductSpecificaty> productSpecificatyRepository,
            IProductManagementRepository<ProductSpecificatyConfig> productSpecificatyConfigRepository,
            ILoggerHelper logger,
            IMapper mapper)
        {
            _productSpecificatyRepository = productSpecificatyRepository;
            _productSpecificatyConfigRepository = productSpecificatyConfigRepository;

            _logger = logger;
            _mapper = mapper;

            _config = productSpecificatyConfigRepository.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
        }

        public Response<ProductSpecificatyPagedViewModel> GetList(ProductSpecificatySearchModel search)
        {
            Response<ProductSpecificatyPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var keySearch = search.KeySearch;
                var query = _productSpecificatyRepository
                    .Query()
                    .Where(p => p.Group == search.Group)
                    .Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && !p.Deleted)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate);
                var pagedModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[ProductSpecificatyConstants.PageSize]));
                var list = pagedModel.ToList();

                result.Data.Items = _mapper.Map<List<ProductSpecificatyDto>>(list);
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
        public Response<ProductSpecificatyViewModel> GetById(long id)
        {
            Response<ProductSpecificatyViewModel> result = new();
            result.Data = new();
            try
            {
                var model = _productSpecificatyRepository.Query().Where(p => p.Pid == id).FirstOrDefault();
                result.Data.Detail = _mapper.Map<ProductSpecificatyDto>(model);
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

        public Response<ProductSpecificatyViewModel> Save(ProductSpecificatySaveModel data)
        {
            var validator = new ProductSpecificatySaveModelValidator();

            // Execute the validator
            ValidationResult validationResults = validator.Validate(data);

            // Inspect any validation failures.
            bool success = validationResults.IsValid;
            List<ValidationFailure> failures = validationResults.Errors;
            Response<ProductSpecificatyViewModel> result = new();
            result.Data = new();
            ProductSpecificaty modelResponse = new();

            using (var transaction = _productSpecificatyRepository.BeginTransaction())
            {
                try
                {
                    if (modelResponse.Pid == 0)
                    {
                        var newProductSpec = _mapper.Map<ProductSpecificaty>(data);
                        newProductSpec.Order = 0.9;
                        _productSpecificatyRepository.Add(newProductSpec);
                        _productSpecificatyRepository.SaveChanges();

                        modelResponse = newProductSpec;
                    }
                    else
                    {
                        var editModel = _mapper.Map<ProductSpecificatyUpdateModel>(data);
                        var updateModel = _productSpecificatyRepository.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (updateModel != null)
                        {
                            _mapper.Map(editModel, updateModel);
                            _productSpecificatyRepository.SaveChanges();
                            modelResponse = updateModel;
                        }
                    }
                    transaction.Commit();

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

        public Response<ProductSpecificatyConfigViewModel> SaveConfig(IFormCollection formData)
        {
            Response<ProductSpecificatyConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var configs = _productSpecificatyConfigRepository.Query();
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
                        _productSpecificatyConfigRepository.Add(config);
                    }
                    _productSpecificatyConfigRepository.SaveChanges();
                }

                listConfig = configs.ToList();
                result.Data.Items = _mapper.Map<List<ProductSpecificatyConfigDto>>(listConfig);
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
        public Response<ProductSpecificatyConfigViewModel> GetAllConfigs()
        {
            Response<ProductSpecificatyConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var listConfig = _productSpecificatyConfigRepository.Query().ToList();
                result.Data.Items = _mapper.Map<List<ProductSpecificatyConfigDto>>(listConfig);
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

        public Response Delete(List<long> ids)
        {
            Response result = new();
            try
            {
                var model = _productSpecificatyRepository.Query().Where(p => ids.Contains(p.Pid)).ToList();
                foreach (var item in model)
                {
                    item.Deleted = true;
                }
                _productSpecificatyRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
            }
            return result;
        }
    }
}
