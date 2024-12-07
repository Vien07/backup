using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Database;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.ProductManagement.Repository;
using AutoMapper;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Models.ViewModels.ProductPolicy;
using Admin.ProductManagement.DataTransferObjects.ProductPolicy;

namespace Admin.ProductManagement.Services
{
    public class ProductPolicyService : IProductPolicyService
    {
        private readonly IProductManagementRepository<ProductPolicy> _productPolicyRepository;
        private readonly IProductManagementRepository<ProductPolicyConfig> _productPolicyConfigRepository;

        private readonly ILoggerHelper _logger;
        private readonly IMapper _mapper;

        private Dictionary<string, string> _config;

        public ProductPolicyService(
            IProductManagementRepository<ProductPolicy> productPolicyRepository, 
            IProductManagementRepository<ProductPolicyConfig> productPolicyConfigRepository, 
            ILoggerHelper logger, 
            IMapper mapper)
        {
            _productPolicyRepository = productPolicyRepository;
            _productPolicyConfigRepository = productPolicyConfigRepository;

            _config = _productPolicyConfigRepository.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

            _logger = logger;
            _mapper = mapper;
        }

        public Response<ProductPolicyPagedViewModel> GetList(ProductPolicySearchModel search)
        {
            Response<ProductPolicyPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var query = _productPolicyRepository
                    .Query()
                    .Where(p => p.Deleted == false)
                    .Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate);

                var pagingModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[ProductPolicyConstants.Config.Admin.PageSize]));

                var list = pagingModel.ToList();

                result.Data.Items = _mapper.Map<List<ProductPolicyDto>>(list);
                result.Data.PageSize = pagingModel.PageSize;
                result.Data.PageNumber = pagingModel.PageNumber;
                result.Data.PageCount = pagingModel.PageCount;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());
            }
            return result;
        }
        public Response<ProductPolicyViewModel> GetById(int id)
        {
            Response<ProductPolicyViewModel> result = new();
            result.Data = new();
            try
            {
                var model = _productPolicyRepository.Query().Where(p => p.Pid == id).FirstOrDefault();

                result.IsError = false;
                result.StatusCode = 200;
                result.Data.Detail = _mapper.Map<ProductPolicyDto>(model);
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
        public Response<ProductPolicyViewModel> Save(ProductPolicySaveModel saveModel)
        {
            Response<ProductPolicyViewModel> result = new();
            result.Data = new();
            try
            {
                var validator = new ProductPolicySaveModelValidator();
                // Execute the validator
                ValidationResult results = validator.Validate(saveModel);
                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;
                if (!success)
                {
                    string mess = string.Join(";", results.Errors);
                    result.Message = mess;
                    result.IsError = true;
                    return result;
                }

                ProductPolicy modelResponse = new();

                using (var transaction = _productPolicyRepository.BeginTransaction())
                {
                    if (saveModel.Pid == 0)
                    {
                        var model = _mapper.Map<ProductPolicy>(saveModel);
                        model.Order = 0.9;
                        _productPolicyRepository.Add(model);
                        _productPolicyRepository.SaveChanges();

                        modelResponse = model;
                    }
                    else
                    {
                        var editModel = _productPolicyRepository.Query().Where(p => p.Pid == saveModel.Pid).FirstOrDefault();
                        if (editModel != null)
                        {
                            _mapper.Map<ProductPolicySaveModel, ProductPolicy>(saveModel, editModel);
                            _productPolicyRepository.SaveChanges();

                            modelResponse = editModel;
                        }
                    }
                    transaction.Commit();
                }

                result.Data.Detail = _mapper.Map<ProductPolicyDto>(modelResponse);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
            }
            return result;
        }
        public Response Delete(List<long> ids)
        {
            Response result = new();
            try
            {
                var listProductPolicy = _productPolicyRepository.Query().Where(x => ids.Contains(x.Pid)).ToList();
                _productPolicyRepository.RemoveRange(listProductPolicy);
                _productPolicyRepository.SaveChanges();
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
                _productPolicyRepository.Query().Where(p => ids.Contains(p.Pid)).ToList().ForEach(e =>
                {
                    e.Enabled = isEnable;
                });

                _productPolicyRepository.SaveChanges();
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
                var list = _productPolicyRepository.Query().OrderBy(p => p.Order).ToList();
                var order = 1;
                foreach (var item in list)
                {
                    item.Order = order;
                    order = order + 1;
                }
                _productPolicyRepository.SaveChanges();
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
                var model = _productPolicyRepository.Query().Where(p => p.Pid == id).FirstOrDefault();
                if (model != null)
                {
                    model.Order = order;
                }
                _productPolicyRepository.SaveChanges();
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
                var fromModel = _productPolicyRepository.Query().Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _productPolicyRepository.Query().Where(p => p.Pid == toId).FirstOrDefault();
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

                    _productPolicyRepository.SaveChanges();
                    var list = _productPolicyRepository.Query().OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _productPolicyRepository.SaveChanges();
                    }
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
        public Response<ProductPolicyConfigViewModel> SaveConfig(IFormCollection formData)
        {
            Response<ProductPolicyConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var configs = _productPolicyConfigRepository.Query();
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
                        _productPolicyConfigRepository.Add(config);
                    }
                    _productPolicyConfigRepository.SaveChanges();
                }

                listConfig = configs.ToList();
                result.Data.Items = _mapper.Map<List<ProductPolicyConfigDto>>(listConfig);
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
        public Response<ProductPolicyConfigViewModel> GetAllConfigs()
        {
            Response<ProductPolicyConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var listConfig = _productPolicyConfigRepository.Query().ToList();
                result.Data.Items = _mapper.Map<List<ProductPolicyConfigDto>>(listConfig);
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
        public Response<ProductPolicyConfigViewModel> GetConfigByKey(string key)
        {
            Response<ProductPolicyConfigViewModel> result = new();
            try
            {
                var config = _productPolicyConfigRepository.Query().Where(p => p.Key == key).FirstOrDefault();
                result.Data.Item = _mapper.Map<ProductPolicyConfigDto>(config);
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
        public List<ProductPolicyWithGroupViewModel> GetListWithGroup()
        {
            List<ProductPolicyWithGroupViewModel> listPolicies = new List<ProductPolicyWithGroupViewModel>();
            var config = _productPolicyConfigRepository.Query().Where(p => p.Key == Constants.ProductPolicyConstants.Config.Admin.GroupPolicy).FirstOrDefault();
            try
            {
                var listTemp = config.Value.Split(';');
                foreach (var item in listTemp)
                {
                    var tempType = item.Split(":");
                    if (tempType != null)
                    {
                        if (tempType.Count() == 2)
                        {
                            listPolicies.Add(new ProductPolicyWithGroupViewModel { Title = tempType[0].Trim(), ParentCode = "", Pid = tempType[1].Trim() });
                        }
                    }
                }
                var listP = _productPolicyRepository.Query().Where(p => p.Enabled == true).ToList();
                foreach (var item in listP)
                {
                    listPolicies.Add(new ProductPolicyWithGroupViewModel { Title = item.Name, ParentCode = item.Group, Pid = item.Pid.ToString() });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return listPolicies;
        }
    }
}
