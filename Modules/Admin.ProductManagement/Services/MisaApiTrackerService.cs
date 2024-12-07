using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Database;
using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Repository;
using AutoMapper;
using Admin.ProductManagement.Models.SearchModels;
using System.Linq;
using Admin.ProductManagement.Models.ViewModels.MisaApiTracker;
using Admin.ProductManagement.DataTransferObjects.MisaApiTracker;

namespace Admin.ProductManagement.Services
{
    public class MisaApiTrackerService : IMisaApiTrackerService
    {
        private readonly IProductManagementRepository<MisaApiTracker> _misaApiTrackerRepository;
        private readonly IProductManagementRepository<MisaApiConfig> _misaApiConfigRepository;

        private readonly IMisaApiService _misaApiService;

        private readonly IMapper _mapper;
        private readonly ILoggerHelper _logger;

        private Dictionary<string, string> _config;

        public MisaApiTrackerService(
            ILoggerHelper logger,
            IProductManagementRepository<MisaApiTracker> misaApiTrackerRepository,
            IProductManagementRepository<MisaApiConfig> misaApiConfigRepository,
            IMisaApiService misaApiService,
            IMapper mapper)
        {
            _misaApiTrackerRepository = misaApiTrackerRepository;
            _misaApiConfigRepository = misaApiConfigRepository;

            _misaApiService = misaApiService;

            _mapper = mapper;
            _logger = logger;

            _config = _misaApiConfigRepository.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
        }

        public Response<MisaApiTrackerPagedViewModel> GetList(MisaApiTrackerSearchModel search)
        {
            Response<MisaApiTrackerPagedViewModel> result = new();
            result.Data = new();
            try
            {
                var query = _misaApiTrackerRepository.Query()
                    .Where(p => String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch))
                    .OrderByDescending(p => p.RequestDate);

                var pagedModel = query.ToPagedList(search.PageIndex, Convert.ToInt32(_config[MisaApiTrackerConstants.PageSize]));

                var list = pagedModel.ToList();

                result.Data.Items = _mapper.Map<List<MisaApiTrackerDto>>(list);
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
        public Response<MisaApiTrackerViewModel> GetById(long id)
        {
            Response<MisaApiTrackerViewModel> result = new();
            result.Data = new();
            try
            {
                var model = _misaApiTrackerRepository.Query().FirstOrDefault(p => p.Pid == id);
                result.Data.Detail = _mapper.Map<MisaApiTrackerDto>(model);

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
        public Response<MisaApiConfigViewModel> SaveConfig(IFormCollection formData)
        {
            Response<MisaApiConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var configs = _misaApiConfigRepository.Query();
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
                        _misaApiConfigRepository.Add(config);
                    }
                    _misaApiConfigRepository.SaveChanges();
                }

                listConfig = configs.ToList();
                result.Data.Items = _mapper.Map<List<MisaApiConfigDto>>(listConfig);
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
        public Response<MisaApiConfigViewModel> GetAllConfigs()
        {
            Response<MisaApiConfigViewModel> result = new();
            result.Data = new();
            try
            {
                var listConfig = _misaApiConfigRepository.Query().ToList();
                result.Data.Items = _mapper.Map<List<MisaApiConfigDto>>(listConfig);
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
        public Response<MisaApiConfigViewModel> GenerateAccessToken()
        {
            Response<MisaApiConfigViewModel> rs = new();
            try
            {
                var data = _misaApiService.GenerateAccessToken();

                if (data is not null && data.Success == true)
                {
                    var getNewConfig = _misaApiConfigRepository.Query().Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t => t.Value);
                    rs.Data = new MisaApiConfigViewModel()
                    {
                        AccessToken = getNewConfig[MisaApiTrackerConstants.AccessToken],
                        Environment = getNewConfig[MisaApiTrackerConstants.Environment],
                        LoginTime = getNewConfig[MisaApiTrackerConstants.LoginTime],
                        CompanyCode = getNewConfig[MisaApiTrackerConstants.CompanyCode],
                        SignatureInfo = getNewConfig[MisaApiTrackerConstants.SignatureInfo],
                    };
                }
                else
                {
                    rs.IsError = true;
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
    }
}
