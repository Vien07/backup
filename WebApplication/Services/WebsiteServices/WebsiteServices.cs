using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using DTO;
using DTO.Website;
using CMS.Areas.Admin.Models;
using CMS.Services.CommonServices;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CMS.Services.WebsiteServices
{
    public class WebsiteServices : IWebsiteServices
    {
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DBContext _dbContext;
        private string KeyMaintenance = ConstantStrings.KeyMaintenance;

        private string maintenance = "";
        public WebsiteServices(DBContext dbContext, IHttpContextAccessor httpContextAccessor, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            _dbContext = dbContext;
            maintenance = _common.GetConfigValue(KeyMaintenance);
        }
        public void StartUp()
        {
            if (maintenance == "on")
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/maintenance.html");
            }
        }
        public void SetVisitor()
        {
            try
            {
                string domain = _common.GetConfigValue(ConstantStrings.KeyRootDomain);
                Uri myUri = new Uri(domain);
                var ipDomain = Dns.GetHostAddresses(myUri.Host)[0].ToString();

                var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                if (ipAddress != ipDomain && ipAddress != "115.79.141.71")
                {
                    string visitorId = _httpContextAccessor.HttpContext.Request.Cookies[ConstantStrings.VisitorId];
                    if (visitorId == null)
                    {
                        var newId = Guid.NewGuid().ToString();
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(ConstantStrings.VisitorId, newId,
                            new CookieOptions()
                            {

                            });

                        Visitor data = new Visitor();
                        data.Id = newId;
                        data.IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        data.DeviceName = "";
                        data.Brower = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                        data.LoginTime = DateTime.Now;
                        data.LastOnlineTime = DateTime.Now.AddMinutes(5);
                        _dbContext.Visitors.Add(data);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        var data = _dbContext.Visitors.Where(p => p.Id == visitorId).FirstOrDefault();
                        if (data != null)
                        {
                            data.LastOnlineTime = DateTime.Now.AddMinutes(5);
                            _dbContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<VisitorDto> GetVisitor()
        {
            var visitorModel = new VisitorDto();
            try
            {
                var visitors = await _dbContext.LoadStoredProc("GetVistors").ExecuteStoredProc<VisitorDto>();
                visitorModel.TotalString = _common.ConvertFormatMoney(visitors[0].Total.ToString());
                visitorModel.OnlineString = _common.ConvertFormatMoney(visitors[0].Online);
                return visitorModel;
            }
            catch (Exception ex)
            {
                return visitorModel;
            }
        }
    }
}
