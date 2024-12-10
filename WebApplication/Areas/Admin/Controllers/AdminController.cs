using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;
using CMS.Services.TranslateServices;
using System;
using System.Collections.Generic;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IHttpContextAccessor _httpContextAccessor;

        private readonly IAdminRepository _rep;
        private string RootDomain = "";
        private readonly ICommonServices _common;

        public AdminController(IAdminRepository adminRepository
            , IHttpContextAccessor httpContextAccessor, ICommonServices common)
        {
            _rep = adminRepository;
            _common = common;
            _httpContextAccessor = httpContextAccessor;
            RootDomain = _common.GetConfigValue(ConstantStrings.KeyRootDomain);
        }
        public IActionResult Index(string redirect_to = "", bool? messPer = null)
        {
            try
            {

                ViewBag.RootDomain = RootDomain;
                if (!string.IsNullOrEmpty(redirect_to))
                {
                    _httpContextAccessor.HttpContext.Session.Clear();
                    foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }

                    if (messPer == true)
                    {
                        ViewBag.messPer = true;
                    }

                    if (!redirect_to.Contains("/b-admin/"))
                    {
                        ViewBag.redirectTo = "/b-admin/" + redirect_to;
                    }
                    else
                    {
                        ViewBag.redirectTo = redirect_to;
                    }

                    return View();
                }
                else
                {
                    var getUserCookie = _httpContextAccessor.HttpContext.Request.Cookies["BizMaC"];
                    var getUserSession = _httpContextAccessor.HttpContext.Session.GetString("BizMaC");
                    if (!string.IsNullOrEmpty(getUserCookie))
                    {
                        var decryptCookie = _common.BizmacDecrytion(getUserCookie);
                        if (!string.IsNullOrEmpty(decryptCookie))
                        {
                            var cookieData = JsonConvert.DeserializeObject<CookieDto>(decryptCookie);
                            var code = cookieData.account;
                            var password = cookieData.password;
                            Login(code, password);
                            return Redirect("/b-admin/Dashboard/");
                        }
                    }
                    else if (!string.IsNullOrEmpty(getUserSession))
                    {
                        var decryptSession = _common.BizmacDecrytion(getUserSession);
                        if (!string.IsNullOrEmpty(decryptSession))
                        {
                            var sessionData = JsonConvert.DeserializeObject<CookieDto>(decryptSession);
                            var code = sessionData.account;
                            var password = sessionData.password;
                            Login(code, password);
                            return Redirect("/b-admin/Dashboard/");
                        }
                    }
                    return View();
                }
            }
            catch
            {
                _httpContextAccessor.HttpContext.Session.Clear();
                foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                return View();
            }
        }
        public JsonResult Login(string Code, string Password, string RedirectTo = "", string RememberMe = "")
        {
            var rememberMe = false;

            if (!string.IsNullOrEmpty(RememberMe))
            {
                rememberMe = true;
            }
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var loginData = _rep.Login(Code, Password.Trim(), ipAddress, rememberMe);
            if (loginData)
            {
                var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
                if (!string.IsNullOrEmpty(RedirectTo))
                {
                    try
                    {
                        RedirectTo = RedirectTo.Replace("/b-admin/", "");
                        var url = RedirectTo.Split("/");
                        return Json(new { jsData = $"/b-admin/{url[0]}/{url[1]}", error = true, role = role });
                    }
                    catch
                    {
                        return Json(new { jsData = "/b-admin/Dashboard/Index", error = true, role = role });
                    }
                }
                return Json(new { jsData = "/b-admin/Dashboard/Index", error = true, role = role });
            }
            return Json(new { jsData = "", error = false });
        }
        public IActionResult LogOut()
        {
            ViewBag.RootDomain = RootDomain;

            _httpContextAccessor.HttpContext.Session.Clear();
            foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View("Index");
        }
        public IActionResult Forget()
        {
            ViewBag.RootDomain = RootDomain;

            return View();
        }
        public JsonResult SendMailValidate(string email)
        {

            return Json(new { error = _rep.RecoveryPassword(email) });
        }
        public ActionResult ValidateRecoveryPassword(string key)
        {
            if (_rep.ValidateRecoveryPassword(key))
            {
                return Redirect("/b-admin/");
            }
            else
            {
                return Json(new { error = "TIMEOUT" });
            }
        }
        public JsonResult GetPermissionForUser()
        {
            try
            {                
                return Json(_rep.GetPermissionForUser());
            }
            catch (Exception ex)
            {
                return Json(new List<string>());
            }
        }
    }
}