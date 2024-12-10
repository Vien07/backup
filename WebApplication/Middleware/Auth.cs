using CMS.Areas.Customer.Models;
using CMS.Helper;
using CMS.Services.CommonServices;
using CMS.Services.TranslateServices;
using DTO;
using DTO.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using static DTO.ConstantStrings;

namespace CMS.Middleware
{
    [AttributeUsage(AttributeTargets.All)]
    public class Auth : Attribute, IAuthorizationFilter
    {
        private string _policy { get; set; }
        public Auth(string policy)
        {
            _policy = policy;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var _tokenHelper = context.HttpContext.RequestServices.GetRequiredService<TokenHelper>();
                var _dbContext = context.HttpContext.RequestServices.GetRequiredService<DBContext>();
                var _common = context.HttpContext.RequestServices.GetRequiredService<ICommonServices>();
                var _translate = context.HttpContext.RequestServices.GetRequiredService<ITranslateServices>();
                //var _memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                var session = context.HttpContext.Session.GetString(ConstantStrings.CustomerCookieName);
                var coookie = context.HttpContext.Request.Cookies[ConstantStrings.CustomerCookieName];

                var flag = false;

                //var accessToken = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                var token = "";

                if (!string.IsNullOrEmpty(session))
                {
                    token = session;
                }
                else
                {
                    token = coookie;
                }

                if (!string.IsNullOrEmpty(token))
                {
                    if (_tokenHelper.IsTokenValid(token))
                    {
                        var userId = Convert.ToInt32(_tokenHelper.GetValuePayload(token, "user_id"));


                        var customer = _common.GetProfile(userId);

                        if (customer != null)
                        {
                            context.HttpContext.Items.Add(CustomerCookieName, customer);
                            flag = true;
                        }
                    }
                }

                if (!flag)
                {
                    context.HttpContext.Items.Remove(CustomerCookieName);
                    context.Result = new RedirectResult(_translate.GetUrl("url.sign-in"));
                }
            }
            catch
            {
                context.HttpContext.Items.Remove(CustomerCookieName);
                context.Result = new RedirectResult("/error");
            }
        }
    }
}
