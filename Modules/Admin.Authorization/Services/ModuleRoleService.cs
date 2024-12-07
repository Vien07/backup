using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Database;
using Admin.Authorization.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Services
{
    public class ModuleRoleService : IModuleRoleService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<ModuleRole> _repModuleRole;
        private readonly IRepository<Group_ModuleRole> _repModuleRoleGroup;
        private readonly IRepositoryConfig<AuthorizationConfig> _repConfig;
        public ModuleRoleService(
           IRepository<ModuleRole> repModuleRole,
           IRepository<Group_ModuleRole> repModuleRoleGroup,
           IRepositoryConfig<AuthorizationConfig> repConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repModuleRoleGroup = repModuleRoleGroup;
            _repModuleRole = repModuleRole;
            _repConfig = repConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.ModuleRole>> GetList(ParamSearch search)
        {
            search = search ?? new ParamSearch();
            Response<IPagedList<Database.ModuleRole>> rs = new Response<IPagedList<Database.ModuleRole>>();
            try
            {
                search.ToString();
                rs.Data = _repModuleRole.Query().Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.Deleted == false)
                .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate)
                .ToPagedList(search.PageIndex, Convert.ToInt32(_config[Constants.Config.Admin.PageSize]));

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<List<Database.ModuleRole>> GetListNoPagelList(ParamSearch search)
        {
            search = search ?? new ParamSearch();
            Response<List<Database.ModuleRole>> rs = new Response<List<ModuleRole>>();
            try
            {
                search.ToString();
                rs.Data = _repModuleRole.Query().Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.Deleted == false)
                .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<List<Database.ModuleRole>> GetListNoPagelListNotContainAnonymousRole(ParamSearch search)
        {
            search = search ?? new ParamSearch();
            Response<List<Database.ModuleRole>> rs = new Response<List<ModuleRole>>();
            try
            {
                search.ToString();
                rs.Data = _repModuleRole.Query().Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.Deleted == false && p.AllowAnonymous == false)
                .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<IPagedList<Database.ModuleRole>> GetListParent(ParamSearch search)
        {
            search = search ?? new ParamSearch();
            Response<IPagedList<Database.ModuleRole>> rs = new Response<IPagedList<Database.ModuleRole>>();
            try
            {
                search.ToString();
                rs.Data = _repModuleRole.Query().Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.IdParent == null && p.Deleted == false)
                .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate)
                .ToPagedList(search.PageIndex, Convert.ToInt32(_config[Constants.Config.Admin.PageSize]));

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<IPagedList<Database.ModuleRole>> GetListChild(ParamSearch search, long? IdParent)
        {
            search = search ?? new ParamSearch();
            Response<IPagedList<Database.ModuleRole>> rs = new Response<IPagedList<Database.ModuleRole>>();
            try
            {
                search.ToString();
                rs.Data = _repModuleRole.Query().Where(p => ((String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.IdParent == IdParent && p.Deleted == false))
                .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate)
                .ToPagedList(search.PageIndex, Convert.ToInt32(_config[Constants.Config.Admin.PageSize]));

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<ModuleRoleDetail> GetById(int id)
        {
            Response<ModuleRoleDetail> rs = new Response<ModuleRoleDetail>();
            ModuleRoleDetail detail = new ModuleRoleDetail();
            try
            {

                detail.Detail = _repModuleRole.Query().Where(p => p.Pid == id).FirstOrDefault();
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
        public Response<Authorization.Database.ModuleRole> Save(Authorization.Database.ModuleRole data)
        {
            var validator = new ModuleRoleGroupValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Authorization.Database.ModuleRole> rs = new Response<Authorization.Database.ModuleRole>();
            using (var transaction = _repModuleRole.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repModuleRole.Add(data);

                        _repModuleRole.SaveChanges();
                    }
                    else
                    {
                        var model = _repModuleRole.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;
                            model.IdParent = data.IdParent;
                            model.RolePath = data.RolePath;
                            model.AllowAnonymous = data.AllowAnonymous;
                            model.ApiKey = data.ApiKey;
                            model.ActionName = data.ActionName;
                            model.Log = data.Log;
                            _repModuleRole.SaveChanges();

                        }


                    }
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

                }
            }
            rs.Data = data;
            return rs;

        }

        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repModuleRole.Query().Where(p => p.Pid == id).FirstOrDefault();
                    if(model != null)
                    {
                        model.Deleted = true;
                    }
                    if(model != null && model.IdParent == null)
                    {
                        var listModuleRoleChild = _repModuleRole.Query().Where(s => s.IdParent == model.Pid);
                        if(listModuleRoleChild.Count() > 0)
                        {
                            foreach (var item in listModuleRoleChild)
                            {
                                item.Deleted = true;
                            }
                            _repModuleRole.SaveChanges();
                        }
                    }
                    RemoveModuleFromeModuleRoleGroupWithId(id);
                    _repModuleRole.SaveChanges();
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

        private void RemoveModuleFromeModuleRoleGroupWithId(long pidModule)
        {
            var listRemove = _repModuleRoleGroup.Query().Where(s => s.Id_ModuleRole == pidModule);
            _repModuleRoleGroup.RemoveRange(listRemove);
            _repModuleRoleGroup.SaveChanges();
        }



    }

}
