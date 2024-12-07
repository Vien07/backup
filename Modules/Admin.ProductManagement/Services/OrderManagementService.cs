using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Database;
using System.Reflection;
using X.PagedList;
using Admin.ProductManagement.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using System.Globalization;
using AutoMapper;
using Admin.ProductManagement.Repository;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.OrderManagement;
using Admin.ProductManagement.DataTransferObjects.OrderManagement;

namespace Admin.ProductManagement.Services
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IProductManagementRepository<OrderManagement> _orderManagementRepository;
        private readonly IProductManagementRepository<OrderManagementConfig> _orderManagementConfigRepository;

        private readonly ILoggerHelper _logger;
        private readonly IMapper _mapper;

        private Dictionary<string, string> _config;
        public OrderManagementService(
            IProductManagementRepository<OrderManagement> orderManagementRepository,
            IProductManagementRepository<OrderManagementConfig> orderManagementConfigRepository,

            ILoggerHelper logger,
            IMapper mapper)
        {
            _orderManagementRepository = orderManagementRepository;
            _orderManagementConfigRepository = orderManagementConfigRepository;

            _logger = logger;
            _mapper = mapper;

            _config = _orderManagementConfigRepository.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
        }

        public Response<OrderManagementPagedViewModel> GetList(OrderManagementSearchModel search)
        {
            Response<OrderManagementPagedViewModel> result = new();
            result.Data = new();

            string format = "dd/M/yyyy";
            DateTime fromdate = DateTime.Now.AddMonths(-1);
            DateTime todate = DateTime.Now;

            if (search != null && (search.FromDate != null || search.ToDate != null))
            {
                try
                {
                    DateTime.TryParseExact(search.FromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromdate);
                    DateTime.TryParseExact(search.ToDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out todate);
                }
                catch { }
            }

            try
            {
                var keySearch = search.KeySearch;
                var query = _orderManagementRepository
                    .Query()
                    .Where(p => p.Deleted == false)
                    .Where(p => ((String.IsNullOrEmpty(keySearch) == true ||
                                p.FullName.Contains(keySearch) ||
                                p.Email.Contains(keySearch) ||
                                p.Address.Contains(keySearch) ||
                                p.OrderNo.Contains(keySearch))))
                    .OrderBy(p => p.Order)
                    .ThenBy(p => p.UpdateDate);

                var pagedModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[OrderManagementConstants.Config.Admin.PageSize]));
                var list = pagedModel.ToList();

                result.Data.Items = _mapper.Map<List<OrderManagementDto>>(list);
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

        public Response Delete(List<long> ids)
        {
            Response result = new();
            try
            {
                var listOrders = _orderManagementRepository.Query().Where(x => ids.Contains(x.Pid)).ToList();
                _orderManagementRepository.RemoveRange(listOrders);
                _orderManagementRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
            }
            return result;
        }

        public Response<OrderManagementConfigViewModel> SaveConfig(IFormCollection formData, string group)
        {
            Response<OrderManagementConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var configs = _orderManagementConfigRepository.Query();
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
                        config.Group = group;
                        _orderManagementConfigRepository.Add(config);
                    }
                    _orderManagementConfigRepository.SaveChanges();
                }

                listConfig = configs.ToList();
                result.Data.Items = _mapper.Map<List<OrderManagementConfigDto>>(listConfig);
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
        public Response<OrderManagementConfigViewModel> GetAllConfigs()
        {
            Response<OrderManagementConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var listConfig = _orderManagementConfigRepository.Query().ToList();
                result.Data.Items = _mapper.Map<List<OrderManagementConfigDto>>(listConfig);
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
        public Response<OrderManagementConfigViewModel> GetConfigByKey(string key)
        {
            Response<OrderManagementConfigViewModel> result = new();
            try
            {
                var config = _orderManagementConfigRepository.Query().Where(p => p.Key == key).FirstOrDefault();
                result.Data.Item = _mapper.Map<OrderManagementConfigDto>(config);
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
    }
}
