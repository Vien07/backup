
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Database;
using Steam.Core.LocalizeManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Steam.Infrastructure.Repository;

namespace Steam.Core.LocalizeManagement.Services
{
    public class LocalizeCultureService : ILocalizeCultureService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.LocalizeCulture> _repLocalizeCulture;
        private readonly IRepositoryConfig<Database.LocalizeCultureConfig> _repLocalizeCultureConfig;
        public LocalizeCultureService(
            IRepository<Database.LocalizeCulture> repLocalizeCulture,
             IRepositoryConfig<Database.LocalizeCultureConfig> repLocalizeCultureConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repLocalizeCulture = repLocalizeCulture;
            _repLocalizeCultureConfig = repLocalizeCultureConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repLocalizeCultureConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.LocalizeCulture>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.LocalizeCulture>> rs = new Response<IPagedList<Database.LocalizeCulture>>();
            try
            {
                
                search.ToString();
                rs.Data = _repLocalizeCulture.Query().Where(p=>p.Deleted==false)
                    .Where(
                    p=>(String.IsNullOrEmpty(search.KeySearch)==true || (p.LangCode.Contains(search.KeySearch)) || (p.Name.Contains(search.KeySearch))))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[LocalizeCultureConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<LocalizeCultureDetail> GetById(int id)
        {
            Response<LocalizeCultureDetail> rs = new Response<LocalizeCultureDetail>();
            LocalizeCultureDetail detail = new LocalizeCultureDetail();
            try
            {

                detail.Detail = _repLocalizeCulture.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<LocalizeManagement.Database.LocalizeCulture> Save(LocalizeCultureModelEdit input)
        {
            Response<LocalizeManagement.Database.LocalizeCulture> rs = new Response<LocalizeManagement.Database.LocalizeCulture>();
            Database.LocalizeCulture newModel = new Database.LocalizeCulture();
            try
            {
                var validator = new LocalizeCultureValidator();

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

                using (var transaction = _repLocalizeCulture.BeginTransaction())
                {
                    try
                    {
                        newModel = input.GetDatabaseModel();

                        if (newModel.Pid == 0)
                        {
                            newModel.Order = 0.9;

                            _repLocalizeCulture.Add(newModel);

                            _repLocalizeCulture.SaveChanges();
                        }
                        else
                        {
                            var editModel = _repLocalizeCulture.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (editModel != null)
                            {

                                editModel.SlugKey = newModel.SlugKey;

                                _repLocalizeCulture.SaveChanges();

                            }


                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        rs.IsError = true;
                        rs.Message = ex.Message;
                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, newModel.ToJson());

                    }
                }
                rs.Data = newModel;
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
                    var model = _repLocalizeCulture.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repLocalizeCulture.Remove(model);
                    //check and remove images


                    _repLocalizeCulture.SaveChanges();
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
