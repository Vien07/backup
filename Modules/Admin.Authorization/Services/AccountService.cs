using Microsoft.AspNetCore.Http;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.Authorization;
using Admin.Authorization.Database;
using Admin.Authorization.Models;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Common.SteamString;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Services
{
    using BCrypt.Net;


    public class AccountService : IAccountService
    {
        private ILoggerHelper _logger;

        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<User> _repUser;
        private readonly IRepository<User_Groups> _repUser_Groups;
        private readonly IRepositoryConfig<AuthorizationConfig> _repConfig;
        private string CreateUser = "admin";
        public AccountService(
            IRepository<User> repUser,
            IRepositoryConfig<AuthorizationConfig> repConfig,
             IRepository<User_Groups> repUser_Groups,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repUser = repUser;
            _logger = logger;
            _repUser_Groups = repUser_Groups;
            _fileHelper = fileHelper;
            _config = _repConfig.GetAllConfigs();
        }
        public Response<IPagedList<Admin.Authorization.Database.User>> GetList(ParamSearch search)
        {
            search = search ?? new ParamSearch();
            Response<IPagedList<Admin.Authorization.Database.User>> rs = new Response<IPagedList<Admin.Authorization.Database.User>>();
            try
            {
                search.ToString();

                rs.Data = _repUser.Query().Where(p=>((String.IsNullOrEmpty(search.KeySearch)==true || p.UserName.Contains(search.KeySearch)) && p.Deleted == false))
                    .OrderBy(p => p.Order).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[Admin.Authorization.Constants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<List<long>> GetLisPermissionGroup(long pidUser)
        {
            Response<List<long>> rs = new Response<List<long>>();
            try
            {
                rs.Data = _repUser_Groups.Query().Where(p => p.Id_User == pidUser).Select(s => s.Id_GroupRole).ToList();
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, pidUser.ToString());

            }
            return rs;
        }

        public Response<UserDetail> GetById(int Id)
        {
            Response<UserDetail> rs = new Response<UserDetail>();
            UserDetail detail = new UserDetail();
            try
            {

                detail.Detail = _repUser.Query().Where(p => p.Pid == Id).FirstOrDefault();
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
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, Id.ToString());

            }
            return rs;
        }
        public Response<Admin.Authorization.Database.User> Save(AccountModelEdit input)
        {
            //-------save-file-pond----------
            var img = "";
            var modelReponse = new Admin.Authorization.Database.User();
            if (input.file != null)
            {
  
                        //data.Images += file.FileName;
                        img = _fileHelper.UploadImageModule(
                            new UploadImageInfo
                            {
                                FileName = input.UserName.ToSlug(),
                                Height = Convert.ToInt32(_config[Admin.Authorization.Constants.Config.Admin.MaxHeight].ToString()),
                                Width = Convert.ToInt32(_config[Admin.Authorization.Constants.Config.Admin.MaxWidth].ToString()),
                                Path = Admin.Authorization.Constants.StaticPath.Asset.Image,
                                PathThumb = Admin.Authorization.Constants.StaticPath.Asset.ImageThumb,
                                File = input.file
                            }
                            ).FileName;
            }

            var validator = new UserValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(input);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Admin.Authorization.Database.User> rs = new Response<Admin.Authorization.Database.User>();
            using (var transaction = _repUser.BeginTransaction())
            {
                try
                {
                    if (input.Pid == 0)
                    {
                        modelReponse = input.GetDatabaseModel();
                        if (input.Password is null || input.Password == "")
                        {
                            input.Password = input.UserName + "@123";
                        }
                        modelReponse.Order = 0.9;
                        modelReponse.Image = img;
                        modelReponse.Password = BCrypt.HashPassword(input.Password);
                        _repUser.Add(modelReponse);
                        _repUser.SaveChanges();
                        if (!string.IsNullOrEmpty(input.GroupRoleID))
                        {
                            AddUserToUserGroup(modelReponse.Pid, long.Parse(input.GroupRoleID));
                        }
                    }
                    else
                    {
                         modelReponse = _repUser.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                        if (modelReponse != null)
                        {
                            modelReponse.Name = input.Name;
                            modelReponse.UserName = input.UserName;
                            //if (files != null && files.Count > 0)
                            //{
                            //    _fileHelper.DeleteFile(Admin.Authorization.Constants.StaticPath.Asset.Image, model.Image);
                            //    model.Image = img;
                            //}
                            modelReponse.Image = img;
                            _repUser.SaveChanges();
                            if (!string.IsNullOrEmpty(input.GroupRoleID))
                            {
                                AddUserToUserGroup(modelReponse.Pid, long.Parse(input.GroupRoleID));
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
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, modelReponse.ToJson());

                }
            }
            rs.Data = modelReponse;
            return rs;

        }

        public Response ResetPassword(int pid)
        {
            Response rs = new Response();

            try
            {
                var model = _repUser.Query().Where(p => p.Pid == pid).FirstOrDefault();

                if (model is not null)
                {
                    model.Password = BCrypt.HashPassword(model.UserName + "@123");
                    _repUser.SaveChanges();
                    rs.Message = model.UserName + "@123";
                }
                else
                {
                    rs.Message = "Accout does not exist";
                }
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, pid.ToString());
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
                    var model = _repUser.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    RemoveUserFromUserGroupWithId(id);
                    _repUser.SaveChanges();
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

        private void AddUserToUserGroup(long pidUser, long pidGroupRole)
        {
            RemoveUserFromUserGroupWithId(pidUser);
            User_Groups newGroupRole = new User_Groups { Id_User = pidUser, Id_GroupRole = pidGroupRole, CreateUser = CreateUser, UpdateUser = CreateUser };
            _repUser_Groups.Add(newGroupRole);
            _repUser_Groups.SaveChanges();
        }

        private void RemoveUserFromUserGroupWithId(long pidUser)
        {
            var listRemove = _repUser_Groups.Query().Where(s => s.Id_User == pidUser);
            _repUser_Groups.RemoveRange(listRemove);
            _repUser_Groups.SaveChanges();
        }



    }

}
