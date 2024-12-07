using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Database;
using Admin.Authorization.Models;
using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using FluentValidation.Results;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Services
{
    public class GroupRoleService : IGroupRoleService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private string CreateUser = "admin";
        private readonly IRepository<Group_ModuleRole> _repModuleRoleGroup;
        private readonly IRepository<GroupRole> _repGroupRole;
        private readonly IRepositoryConfig<AuthorizationConfig> _repConfig;
        private readonly IRepository<ModuleRole> _repModuleRole;
        private readonly IRepository<User_Groups> _repUser_Groups;
        public GroupRoleService(
           IRepositoryConfig<AuthorizationConfig> repConfig,
           IRepository<Group_ModuleRole> repModuleRoleGroup,
           IRepository<ModuleRole> repModuleRole,
           IRepository<GroupRole> repGroupRole,
           IRepository<User_Groups> repUser_Groups,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repUser_Groups = repUser_Groups;
            _repModuleRole = repModuleRole;
            _repGroupRole = repGroupRole;
            _repModuleRoleGroup = repModuleRoleGroup;
            _repConfig = repConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repConfig.GetAllConfigs();
        }
        public Response<IPagedList<Database.GroupRole>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.GroupRole>> rs = new Response<IPagedList<Database.GroupRole>>();
            try
            {
                search.ToString();
                rs.Data = _repGroupRole.Query().Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)) && p.Deleted == false)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
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

        public Response<List<long>> GetListPermission(long pidGroup)
        {
            Response<List<long>> rs = new Response<List<long>>();
            try
            {
                rs.Data = _repModuleRoleGroup.Query().Where(p => p.Id_GroupRole == pidGroup).Select(s => s.Id_ModuleRole).ToList();
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, pidGroup.ToString());

            }
            return rs;
        }

        public Response<GroupRoleDetail> GetById(int id)
        {
            Response<GroupRoleDetail> rs = new Response<GroupRoleDetail>();
            GroupRoleDetail detail = new GroupRoleDetail();
            try
            {

                detail.Detail = _repGroupRole.Query().Where(p => p.Pid == id).FirstOrDefault();
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
        public Response<Authorization.Database.GroupRole> Save(Authorization.Database.GroupRole data, long[] listRole)
        {
            var validator = new GroupRoleValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Authorization.Database.GroupRole> rs = new Response<Authorization.Database.GroupRole>();
            using (var transaction = _repGroupRole.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repGroupRole.Add(data);
                        _repGroupRole.SaveChanges();
                        // Add list ModuleRole to Group
                        if (listRole.Count() > 0)
                        {
                            AddRangeModuleRoleGroup(data.Pid, listRole);
                        }
                    }
                    else
                    {
                        var model = _repGroupRole.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;

                            // Update list ModuleRole to Group
                            if (listRole.Count() > 0)
                            {
                                RemoveGroupRoleFromModuleRoleGroupWithId(data.Pid);
                                AddRangeModuleRoleGroup(data.Pid, listRole);
                            }
                            else
                            {
                                RemoveGroupRoleFromModuleRoleGroupWithId(data.Pid);
                            }
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

        private void AddRangeModuleRoleGroup(long pidGroupRole, long[] listRole)
        {
            var ModuleRoleGroupsRange = new List<Authorization.Database.Group_ModuleRole>();
            foreach (var item in listRole)
            {
                _repModuleRoleGroup.Add(new Group_ModuleRole { Id_GroupRole = pidGroupRole, Id_ModuleRole = item, CreateUser = CreateUser, UpdateUser = CreateUser });
                _repModuleRoleGroup.SaveChanges();
                var ParentModuleRole = _repModuleRole.Query().Where(s => s.Pid == item).Select(p => p.IdParent).FirstOrDefault();
                if (ParentModuleRole != null)
                {
                    var IsExistModuleRoleGroup = _repModuleRoleGroup.Query().Where(s => s.Id_ModuleRole == ParentModuleRole && s.Id_GroupRole == pidGroupRole).FirstOrDefault();
                    if (IsExistModuleRoleGroup == null)
                    {
                        _repModuleRoleGroup.Add(new Group_ModuleRole { Id_GroupRole = pidGroupRole, Id_ModuleRole = ParentModuleRole.Value, CreateUser = CreateUser, UpdateUser = CreateUser });
                        _repModuleRoleGroup.SaveChanges();
                    }
                }
            }
        }

        private void RemoveGroupRoleFromModuleRoleGroupWithId(long pidGroupRole)
        {
            var listRemove = _repModuleRoleGroup.Query().Where(s => s.Id_GroupRole == pidGroupRole);
            _repModuleRoleGroup.RemoveRange(listRemove);
            _repModuleRoleGroup.SaveChanges();
        }

        private void RemoveGroupRoleFromUserGroupWithId(long pidGroupRole)
        {
            var listRemove = _repUser_Groups.Query().Where(s => s.Id_GroupRole == pidGroupRole);
            _repUser_Groups.RemoveRange(listRemove);
            _repUser_Groups.SaveChanges();
        }

        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repGroupRole.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    RemoveGroupRoleFromModuleRoleGroupWithId(id);
                    RemoveGroupRoleFromUserGroupWithId(id);
                    _repGroupRole.SaveChanges();
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
