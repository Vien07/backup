using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Admin.Models;
using X.PagedList;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Admin
{
    public class LogRepository : ILogRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        public LogRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
        }
        public dynamic GetList(SearchDto search)
        {
            try
            {
                string cate = search.Cate != null ? search.Cate : "";
                var data = (from a in _dbContext.Logs
                            join b in _dbContext.Users on a.AdminUser equals b.Code
                            select new LogDto
                            {
                                Description = a.Description,
                                AdminUser = a.AdminUser,
                                LoginTime = a.LoginTime,
                                IP = a.IP,
                                PidCate = a.PidCate,
                                PidDetail = a.PidDetail,
                                Status = a.Status,
                                Type = a.Type,
                                PidUser = b.Pid
                            }).OrderByDescending(p => p.LoginTime).ToList().FilterSearch(new string[] { "AdminUser" }, search.Key);
                if (cate == "login")
                {
                    data = data.Where(x => x.Type == "login").ToList();
                }
                else if (cate == "")
                {

                }
                else
                {
                    data = data.Where(x => !(x.Type == "login")).ToList();
                }

                //Không còn link từ log nữa nên tắt
                //foreach (var item in data)
                //{
                //    if (item.PidCate == ConstantStrings.NewsId)
                //    {
                //        item.Url = "/b-admin/News?editPid=" + item.PidDetail;
                //    }
                //    else if (item.PidCate == ConstantStrings.ProductId)
                //    {
                //        item.Url = "/b-admin/Product?editPid=" + item.PidDetail;
                //    }
                //    else if (item.PidCate == ConstantStrings.CustomerId)
                //    {
                //        item.Url = "/b-admin/Customer/CreateOrUpdate?pid=" + item.PidDetail;
                //    }
                //    else if (item.PidCate == ConstantStrings.PromotionId)
                //    {
                //        item.Url = "/b-admin/Promotion/CreateOrUpdate?pid=" + item.PidDetail;
                //    }
                //}

                PagedList<dynamic> dataPaging = new PagedList<dynamic>(data, search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);
                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { jsData = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic GetListError(SearchDto search)
        {
            try
            {

                dynamic data = (from a in _dbContext.LogErrors
                                select new
                                {
                                    a.Trace,
                                    a.Message,
                                    a.CreateDate
                                }).OrderByDescending(p => p.CreateDate).ToList().FilterSearch(new string[] { "Trace", "Message" }, search.Key);

                PagedList<dynamic> dataPaging = new PagedList<dynamic>(data, search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);
                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { jsData = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
    }
}
