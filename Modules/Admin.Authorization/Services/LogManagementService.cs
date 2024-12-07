
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Database;
using Admin.Authorization.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.Authorization.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using System.Globalization;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Services
{
    public class LogManagementService : ILogManagementService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<LogManagement> _repLogManagement;
        private readonly IRepositoryConfig<LogManagementConfig> _repConfig;
        public LogManagementService(
            IRepository<LogManagement> repLogManagement,
             IRepositoryConfig<LogManagementConfig> repConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repLogManagement = repLogManagement;
            _repConfig = repConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.LogManagement>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.LogManagement>> rs = new Response<IPagedList<Database.LogManagement>>();
            string format = "dd/M/yyyy";
            DateTime fromdate = DateTime.Now.AddMonths(-1);
            DateTime todate = DateTime.Now;
            if (search.FromDate != null || search.ToDate != null)
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


                // Parse the string into a DateTime object


                rs.Data = _repLogManagement.Query().Where(p => p.Deleted == false)
                    .Where
                    (
                        p => (
                                (String.IsNullOrEmpty(search.KeySearch) == true ||
                                p.ActionUrl.Contains(search.KeySearch) ||
                                p.ActionName.Contains(search.KeySearch) ||
                                p.UpdateUser.Contains(search.KeySearch))
                                && (p.CreateDate >= fromdate && p.CreateDate <= todate)

                             )

                    )
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(_config[LogManagementConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<Authorization.Database.LogManagement> Save(LogManagementModelEdit input)
        {
            Response<Authorization.Database.LogManagement> rs = new Response<Authorization.Database.LogManagement>();
            Database.LogManagement modelResponse = new Database.LogManagement();
            try
            {
                //-------save-file-pond----------

                var validator = new LogManagementValidator();

                // Execute the validator
                ValidationResult results = validator.Validate(input);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                if (!success)
                {
                    string mess = string.Join(";", results.Errors);

                    rs.Message = mess;
                    rs.IsError = true;
                    return rs;
                }

                using (var transaction = _repLogManagement.BeginTransaction())
                {
                    try
                    {
                        modelResponse = input.GetDatabaseModel();

                        if (modelResponse.Pid == 0)
                        {
                            modelResponse.Order = 0.9;

                            _repLogManagement.Add(modelResponse);

                            _repLogManagement.SaveChanges();
                        }
                        else
                        {
                            var editModel = _repLogManagement.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (modelResponse != null)
                            {

                                editModel.ActionName = modelResponse.ActionName;
                                editModel.ActionData = modelResponse.ActionData;
                                editModel.ActionUrl = modelResponse.ActionUrl;
                                editModel.Status = modelResponse.Status;
                                _repLogManagement.SaveChanges();

                            }


                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        rs.IsError = true;
                        rs.Message = ex.Message;
                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, modelResponse.ToJson());

                    }
                }
                rs.Data = modelResponse;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
            }

            return rs;

        }
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repLogManagement.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repLogManagement.Remove(model);
                    //check and remove images


                    _repLogManagement.SaveChanges();
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


    }

}
