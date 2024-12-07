
using Admin.LogManagement.Database;
using Admin.LogManagement.Models;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;

using Newtonsoft.Json;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Common.SteamString;

namespace Admin.LogManagement
{
    public class LogManagementRepository : ILogManagementRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        LogManagementContext _db;
        public LogManagementRepository(LogManagementContext db, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _config = _db.LogManagementConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public Response<IPagedList<Database.LogAdminActivity>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.LogAdminActivity>> rs = new Response<IPagedList<Database.LogAdminActivity>>();
            try
            {

            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<List<LogManagement.Database.LogManagementConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<LogManagement.Database.LogManagementConfig>> rs = new Response<List<LogManagement.Database.LogManagementConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    LogManagement.Database.LogManagementConfig LogManagementConfig = _db.LogManagementConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (LogManagementConfig != null)
                    {
                        LogManagementConfig.Type = tab;
                        LogManagementConfig.Value = value;
                        LogManagementConfig.UpdateDate = DateTime.Now;
                        LogManagementConfig.UpdateUser = "";

                    }
                    else
                    {
                        LogManagementConfig = new LogManagement.Database.LogManagementConfig();
                        LogManagementConfig.Type = tab;

                        LogManagementConfig.Key = key;
                        LogManagementConfig.Group = "";
                        LogManagementConfig.Value = value;
                        LogManagementConfig.CreateDate = DateTime.Now;
                        LogManagementConfig.CreateUser = "";
                        LogManagementConfig.UpdateDate = DateTime.Now;
                        LogManagementConfig.UpdateUser = "";
                        _db.LogManagementConfigs.Add(LogManagementConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.LogManagementConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<List<LogManagement.Database.LogManagementConfig>> GetAllConfigs()
        {
            Response<List<LogManagement.Database.LogManagementConfig>> rs = new Response<List<LogManagement.Database.LogManagementConfig>>();
            try
            {

                var listConfig = _db.LogManagementConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<LogManagement.Database.LogManagementConfig> GetConfigByKey(string key)
        {
            Response<LogManagement.Database.LogManagementConfig> rs = new Response<LogManagement.Database.LogManagementConfig>();
            try
            {

                var config = _db.LogManagementConfigs.Where(p => p.Key == key).FirstOrDefault();
                rs.Data = config;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }

    }

}
