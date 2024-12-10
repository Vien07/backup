using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Dynamic;
using CMS.Services.EmailServices;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;
using System.Collections.Generic;

namespace CMS.Areas.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DBContext _dbContext = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;
        private readonly ICommonServices _common;
        public AdminRepository(DBContext context,
                               IConfiguration configuration,
                               IEmailServices emailService,
                               IHttpContextAccessor httpContextAccessor, ICommonServices common)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _emailService = emailService;
            _common = common;
        }
        public bool Login(string code, string password, string ip, bool rememberMe = false)
        {

            try
            {
                //LogServices logSrv = new LogServices();
                var user = _dbContext.Users.Where(p => p.Code == code && !p.Deleted).FirstOrDefault();
                var checkLogin = false;  //Biến kiểm tra login được hay không
                if (user != null)
                {

                    var pass = _common.GetHashSha256(password);
                    if (pass == user.Password)
                    {
                        if (!string.IsNullOrEmpty(ip))
                        {
                            user.IP = ip;
                        }
                        checkLogin = true;
                    }
                }

                if (checkLogin)
                {
                    #region Create Session
                    user.Avatar = string.IsNullOrEmpty(user.Avatar) ? "default-avatar.png" : user.Avatar;
                    _httpContextAccessor.HttpContext.Session.SetString("UserCode", user.Code);
                    var group = _dbContext.GroupUsers.Where(p => p.Code == user.GroupUserCode).FirstOrDefault();
                    _httpContextAccessor.HttpContext.Session.SetString("Role", group.Role);
                    _httpContextAccessor.HttpContext.Session.SetInt32("GroupUserCode", user.GroupUserCode);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", user.FullName);
                    _httpContextAccessor.HttpContext.Session.SetString("UserAvatar", ConstantStrings.UrlUserImages + ConstantStrings.Thumb + user.Avatar);

                    var userObj = new CookieDto { account = code, password = password, role = group.Role };

                    if (rememberMe) //có ghi nhớ đăng nhập hay không (có thì set cookie)
                    {
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddMinutes(30);
                        option.IsEssential = true;
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("BizMaC", _common.BizmacEncrytion(JsonConvert.SerializeObject(userObj)), option);
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("BizMaC", _common.BizmacEncrytion(JsonConvert.SerializeObject(userObj)));
                    }

                    #endregion

                    _common.SaveLogAdmin(code, 1);
                    user.LastLogin = DateTime.Now;
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {

                    _common.SaveLogAdmin(code, 0);
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool RecoveryPassword(string email)
        {
            try
            {
                var data = _dbContext.Users.Where(p => p.Email == email && !p.Deleted).FirstOrDefault();
                if (data != null)
                {
                    Guid g = Guid.NewGuid();
                    string rec = g.ToString().Replace("-", string.Empty);
                    data.RecoveryTime = DateTime.Now;
                    var temp = _dbContext.Users.Where(p => p.RecoveryString == rec).FirstOrDefault();
                    while (temp != null)
                    {
                        g = Guid.NewGuid();
                        rec = g.ToString().Replace("-", string.Empty);
                        temp = _dbContext.Users.Where(p => p.RecoveryString == rec).FirstOrDefault();
                    }
                    data.RecoveryString = rec;
                    _dbContext.SaveChanges();
                    dynamic obj = new ExpandoObject();
                    obj.RecoveryString = data.RecoveryString;
                    obj.Email = data.Email;
                    obj.FullName = data.FullName;
                    _emailService.SendRecoveryPassword(obj);
                }
                else
                {
                    return false;

                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool ValidateRecoveryPassword(string key)
        {
            try
            {
                var data = _dbContext.Users.Where(p => p.RecoveryString == key && !p.Deleted ).FirstOrDefault();
                if (data.RecoveryTime.Value.AddMinutes(3) > DateTime.Now)
                {
                    Guid g = Guid.NewGuid();
                    string newPass = g.ToString().Replace("-", string.Empty).Substring(0, 6);


                    data.Password = _common.GetHashSha256(newPass);
                    data.RecoveryTime = data.RecoveryTime.Value.AddMinutes(3);
                    _dbContext.SaveChanges();
                    dynamic obj = new ExpandoObject();
                    obj.Password = newPass;
                    obj.Email = data.Email;
                    obj.FullName = data.FullName;
                    _emailService.SendNewPassword(obj);
                }
                else
                {
                    return false;

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<string> GetPermissionForUser()
        {
            try
            {
                var getUserCookie = _httpContextAccessor.HttpContext.Request.Cookies["BizMaC"];
                var userData = new CookieDto();
                if (!string.IsNullOrEmpty(getUserCookie))
                {
                    userData = JsonConvert.DeserializeObject<CookieDto>(_common.BizmacDecrytion(getUserCookie));
                }
                else
                {
                    var getUserSession = _httpContextAccessor.HttpContext.Session.GetString("BizMaC");
                    userData = JsonConvert.DeserializeObject<CookieDto>(_common.BizmacDecrytion(getUserSession));
                }

                var role = string.Empty;
                var code = string.Empty;

                if (userData != null)
                {
                    role = userData.role;
                    code = userData.account;
                }
                else
                {
                    return new List<string>();
                }


                if (role == ConstantStrings.RoleRoot)
                {
                    var allPermission = _dbContext.Modules
                        .Where(x => x.Enabled && !x.Deleted)
                        .Select(x => x.Code)
                        .ToList();
                    return allPermission;
                }

                var getUser = _dbContext.Users.Where(x => x.Code == code && !x.Deleted).FirstOrDefault();

                var permissonGroup = _dbContext.GroupPermissons
                    .Where(x => x.GroupUserCode == getUser.GroupUserCode && x.PermissonCode == "VIEW")
                    .Select(x => x.ModuleCode).ToList();

                return permissonGroup;
                //var permissonUser = _dbContext.UserPermissions
                //    .Where(x => x.UserCode == getUser.Pid && x.PermissonCode == "VIEW")
                //    .Select(x => x.ModuleCode)
                //    .ToList();

                //List<string> result = new();
                //result = permissonGroup.Concat(permissonUser).Distinct().ToList();

                //return Json(result);
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
