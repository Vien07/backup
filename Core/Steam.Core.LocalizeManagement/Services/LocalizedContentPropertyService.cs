
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
    public class LocalizedContentPropertyService : ILocalizedContentPropertyService
    {
        private ILoggerHelper _logger;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.LocalizedContentProperty> _repLocalizedContentProperty;
        private readonly IRepositoryConfig<Database.LocalizedContentPropertyConfig> _repLocalizedContentPropertyConfig;
        LocalizeManagementContext _db;
        public LocalizedContentPropertyService(
            IRepository<Database.LocalizedContentProperty> repLocalizedContentProperty,
            IRepositoryConfig<Database.LocalizedContentPropertyConfig> repLocalizedContentPropertyConfig,
            ILoggerHelper logger)
        {
            _repLocalizedContentProperty = repLocalizedContentProperty;
            _repLocalizedContentPropertyConfig = repLocalizedContentPropertyConfig;
            _logger = logger;
            _config = _repLocalizedContentPropertyConfig.GetAllConfigs();

        }
        public Response<IPagedList<LocalizeManagement.Database.LocalizedContentProperty>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.LocalizedContentProperty>> rs = new Response<IPagedList<Database.LocalizedContentProperty>>();
            try
            {
                bool isSystemKey = false;
                if(search.View=="systemkey")
                {
                    isSystemKey = true;
                }
                search.ToString();
                rs.Data = _repLocalizedContentProperty.Query().Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true ))
                    .ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[LocalizedContentPropertyConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public string Translate(long entityID, string properyName,string entityType,string cultureID,string value)
        {
            //localizeProperties.FirstOrDefault(x => x.ProperyName == nameof(product.Name))?.Value
            try
            {
              string transResult= _repLocalizedContentProperty.Query().Where(p => p.CultureID == cultureID
                && p.EntityID == entityID
                && p.ProperyName == properyName
                && p.EntityType == entityType).FirstOrDefault()?.Value;

                if (transResult != null)
                {
                    return transResult;

                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
  
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message,"");

            }
            return value;
        }
        public bool SetLocallize(long entityID, string properyName, string entityType, string cultureID, string value)
        {
            try
            {
                var temp = _repLocalizedContentProperty.Query().Where(p => p.CultureID == cultureID
                  && p.EntityID == entityID
                  && p.ProperyName == properyName
                  && p.EntityType == entityType).FirstOrDefault();
                if (temp != null)
                {
                    temp.Value = value;
                    _repLocalizedContentProperty.SaveChanges();
                    return true;

                }
                else
                {
                    LocalizedContentProperty data = new LocalizedContentProperty();
                    data.EntityID = entityID;   
                    data.ProperyName = properyName;   
                    data.EntityType = entityType;   
                    data.CultureID = cultureID;   
                    data.Value = value;
                    _repLocalizedContentProperty.Add(data);
                    _repLocalizedContentProperty.SaveChanges();
                    return true;
                }


            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return false;

            }
            return true;
        }
        public Response<LocalizeManagement.Database.LocalizedContentProperty> Update(LocalizedContentPropertyModelEdit input)
        {
            Response<LocalizeManagement.Database.LocalizedContentProperty> rs = new Response<LocalizeManagement.Database.LocalizedContentProperty>();
            LocalizeManagement.Database.LocalizedContentProperty newModel = new LocalizeManagement.Database.LocalizedContentProperty();
            try
            {
                //-------save-file-pond----------

                var validator = new LocalizedContentPropertyValidator();

                // Execute the validator
                //ValidationResult results = validator.Validate(input);

                //// Inspect any validation failures.
                //bool success = results.IsValid;
                //List<ValidationFailure> failures = results.Errors;
                bool success = true;
                if (!success)
                {
                    //string mess = string.Join(";", results.Errors);

                    //rs.Message = mess;
                    //rs.IsError = true;
                    return rs;
                }

                using (var transaction = _repLocalizedContentProperty.BeginTransaction())
                {
                    try
                    {
                        newModel = input.GetDatabaseModel();


                        if (newModel.Pid == 0)
                        {

                            _repLocalizedContentProperty.Add(newModel);

                            _repLocalizedContentProperty.SaveChanges();
                        }
                        else
                        {
                            var editModel = _repLocalizedContentProperty.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (editModel != null)
                            {



                                _repLocalizedContentProperty.SaveChanges();

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
                    var model = _repLocalizedContentProperty.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repLocalizedContentProperty.Remove(model);
                    //check and remove images


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

   


    }

}
