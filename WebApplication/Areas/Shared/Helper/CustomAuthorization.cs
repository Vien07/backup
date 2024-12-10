using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Shared.Helper
{

    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomAuthorization : Attribute, IAuthorizationFilter
    {

        private string _menu = "";
        private string _permision = "";
        private string RoleRoot = ConstantStrings.RoleRoot;
        private string RoleAdmin = ConstantStrings.RoleAdmin;
        private string[] Domains = new string[] { "localhost", "greenecard.demo.bizmac.io", "www.greenecard.vn", "greenecard.vn" };
        public CustomAuthorization(string menu, string permision)
        {
            _menu = menu;
            _permision = permision;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                ICommonServices _common = filterContext.HttpContext.RequestServices.GetRequiredService<ICommonServices>();
                DBContext _dbContext = filterContext.HttpContext.RequestServices.GetRequiredService<DBContext>();

                var isRememberMe = false;
                var getUserCookie = filterContext.HttpContext.Request.Cookies["BizMaC"];
                var userData = new CookieDto();
                if (!string.IsNullOrEmpty(getUserCookie))
                {
                    userData = JsonConvert.DeserializeObject<CookieDto>(_common.BizmacDecrytion(getUserCookie));
                }
                else
                {
                    var getUserSession = filterContext.HttpContext.Session.GetString("BizMaC");
                    userData = JsonConvert.DeserializeObject<CookieDto>(_common.BizmacDecrytion(getUserSession));
                    if (userData != null)
                    {
                        isRememberMe = true;
                    }
                }

                var role = string.Empty;
                var code = string.Empty;
                var password = string.Empty;

                if (userData != null)
                {
                    role = userData.role;
                    code = userData.account;
                    password = userData.password;
                    var userObj = new CookieDto { account = code, password = password, role = role };
                    if (!isRememberMe) //có ghi nhớ đăng nhập hay không (có thì set cookie)
                    {
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddMinutes(90);
                        option.IsEssential = true;
                        filterContext.HttpContext.Response.Cookies.Append("BizMaC", _common.BizmacEncrytion(JsonConvert.SerializeObject(userObj)), option);
                    }
                    else
                    {
                        filterContext.HttpContext.Session.SetString("BizMaC", _common.BizmacEncrytion(JsonConvert.SerializeObject(userObj)));
                    }
                }
                else
                {
                    ReturnUrl(filterContext);
                }


                filterContext.HttpContext.Session.SetString("LangCompose", "");

                var controllerInfo = filterContext.ActionDescriptor as ControllerActionDescriptor;

                if (filterContext != null)
                {
                    var licensed = false;
                    string _controller = controllerInfo.ControllerName;
                    string _action = controllerInfo.ActionName;
                    var checkUser = false;

                    #region check multi login by IP address
                    var userInfo = _dbContext.Users.Where(p => p.Code == code && !p.Deleted).FirstOrDefault();
                    if (userInfo != null)
                    {
                        var pass = _common.GetHashSha256(password);
                        if (pass == userInfo.Password)
                        {
                            checkUser = true;
                        }
                    }
                    #endregion

                    if (checkUser)
                    {
                        var module = _dbContext.Modules.FirstOrDefault(x => x.Locked == true && x.Code == _menu); //module lock k cần ktra
                        var getUser = _dbContext.Users.Where(x => x.Code == code && !x.Deleted).FirstOrDefault();

                        var checkPer = false;
                        var permissonGroup = _dbContext.GroupPermissons.Where(x => x.GroupUserCode == getUser.GroupUserCode && x.ModuleCode == _menu && x.PermissonCode == _permision).FirstOrDefault();
                        var permissonUser = _dbContext.UserPermissions.Where(x => x.UserCode == getUser.Pid && x.ModuleCode == _menu && x.PermissonCode == _permision).FirstOrDefault();
                        if (permissonUser != null)
                        {
                            checkPer = permissonUser.Status.Value;
                        }
                        else
                        {
                            if (permissonGroup != null)
                            {
                                checkPer = true;
                            }
                            else
                            {
                                checkPer = false;
                            }
                        }

                        if (checkPer || module != null)
                        {
                            licensed = true;
                            filterContext.HttpContext.Session.SetString("CurrentPage", _controller + "__" + _action);
                        }
                        else
                        {
                            licensed = false;
                        }

                        //quyền supperadmin k cần ktra
                        if (role == RoleRoot)
                        {
                            licensed = true;
                        }

                        var currentDomain = filterContext.HttpContext.Request.Host.Host.ToString();
                        if (!Domains.Contains(currentDomain))
                        {
                            licensed = false;
                        }

                        if (!licensed)
                        {
                            if (_action == "Index")
                            {
                                var latestPage = filterContext.HttpContext.Session.GetString("CurrentPage");
                                if (!string.IsNullOrEmpty(latestPage))
                                {
                                    var redirect_to = latestPage.Split("_")[0] + "/" + latestPage.Split("_")[1];
                                    ReturnUrl(filterContext, redirect_to);
                                }
                                else
                                {
                                    ReturnUrl(filterContext);
                                }
                            }
                            else
                            {
                                filterContext.Result = new CustomUnauthorizedResult("You do not have permission to access this resource", statusCode: 500);
                            }
                        }
                    }
                    else
                    {
                        ReturnUrl(filterContext);
                    }
                }
            }
            catch (Exception ex)
            {

                ReturnUrl(filterContext);
            }
        }
        public void ReturnUrl(AuthorizationFilterContext filterContext, string redirect_to = "")
        {
            filterContext.HttpContext.Session.Clear();
            foreach (var cookie in filterContext.HttpContext.Request.Cookies.Keys)
            {
                filterContext.HttpContext.Response.Cookies.Delete(cookie);
            }

            if (!string.IsNullOrEmpty(redirect_to))
            {
                filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary(new { controller = "b-admin", action = "", redirect_to = redirect_to }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary(new { controller = "b-admin", action = "" }));
            }
        }
        public class CustomError
        {
            public string Error { get; }
            public CustomError(string message)
            {
                Error = message;
            }
        }
        public class CustomUnauthorizedResult : JsonResult
        {
            public CustomUnauthorizedResult(string message, int statusCode) : base(new CustomError(message))
            {
                StatusCode = statusCode;
            }
        }
    }
}


