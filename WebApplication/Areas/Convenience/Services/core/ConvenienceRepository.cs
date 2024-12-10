using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using CMS;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Convenience.Models;
using DTO.Common;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using CMS.Services;
using DTO;

namespace CMS.Areas.Convenience
{
    public class ConvenienceRepository : IConvenienceRepository
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _core;
        string messErr = "";

        private readonly DBContext _dbContext;
        IFileServices _fileServices;

        public ConvenienceRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices core)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _core = core;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                //var data = _dbContext.ProjectDetails.Where(p => p.Deleted != true).ToList();
                var listProjectPid = _dbContext.MultiLang_Conveniences.Select(p => new { p.ConveniencePid, p.Title })
                    .Distinct().ToList().FilterSearch(new string[] { "Title" }, search.Key).Select(x => x.ConveniencePid).Distinct().ToList();
                List<Models.Convenience> data = new List<Models.Convenience>();

                foreach (var item in listProjectPid)
                {
                    var temp = _dbContext.Conveniences.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item).FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_Conveniences.Where(p => p.LangKey == ConstantStrings.DefaultLangAdmin && p.ConveniencePid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_Conveniences.Where(p => p.ConveniencePid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Images = ConstantStrings.UrlConvenienceImages + item.Url;
                    child.ImagesName = item.Url;
                    child.Link = item.Link;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.Enabled = item.Enabled;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.Order), search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);
                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { Data = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Enable(long[] Pid, bool active)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Conveniences.Where(p => p.Pid == item).FirstOrDefault();
                        model.Enabled = active;
                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Delete(int Pid)
        {
            try
            {
                var model = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();
                _fileServices.DeleteFile(ConstantStrings.UrlConvenienceImages, model.Url);
                var multiLang = _dbContext.MultiLang_Conveniences.Where(p => p.ConveniencePid == Pid).ToList();
                _dbContext.MultiLang_Conveniences.RemoveRange(multiLang);
                _dbContext.Conveniences.Remove(model);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public dynamic Delete(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Conveniences.Where(p => p.Pid == item).FirstOrDefault();
                        _fileServices.DeleteFile(ConstantStrings.UrlConvenienceImages, model.Url);
                        var multiLang = _dbContext.MultiLang_Conveniences.Where(p => p.ConveniencePid == item).ToList();
                        _dbContext.MultiLang_Conveniences.RemoveRange(multiLang);
                        _dbContext.Conveniences.Remove(model);
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public dynamic Edit(long Pid)
        {
            try
            {

                var data = _dbContext.MultiLang_Conveniences.Where(p => p.ConveniencePid == Pid).ToList();
                var detail = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var Project = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Project.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        child.Pid = item.Pid;
                        child.ConveniencePid = item.ConveniencePid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        picThumb = ConstantStrings.UrlConvenienceImages + detail.Url,
                        ImagesName = detail.Url,
                        detail.Link,
                        detail.Enabled,
                        detail.Pid
                    },
                    list = listData
                };

                //return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(Models.Convenience convenience, List<MultiLang_Convenience> multiLang_Convenience, IFormFile images)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    var defaultData = multiLang_Convenience.Where(p => p.LangKey == ConstantStrings.DefaultLangAdmin).FirstOrDefault();
                    int maxOrder = _dbContext.Conveniences.Max(x => (int?)x.Order) ?? 1;
                    convenience.Order = maxOrder + 1;

                    if (images != null)
                    {
                        dynamic kt = _fileServices.SaveFileOriginal(images, ConstantStrings.UrlConvenienceImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            convenience.Url = kt.fileName;
                        }
                    } 
                   
                    _dbContext.Conveniences.Add(convenience);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_Convenience)
                    {
                        // string title = string.IsNullOrEmpty(item.Tite) ? defaultData.Tite : item.Name;
                        item.ConveniencePid = convenience.Pid;
                        _dbContext.MultiLang_Conveniences.Add(item);
                        _dbContext.SaveChanges();
                    }


                    transaction.Commit();


                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    messErr = "Something Wrong!";
                    return new { status = false, mess = messErr };

                }
            }
        }
        public dynamic Update(Models.Convenience Convenience, List<MultiLang_Convenience> multiLang_Convenience, IFormFile images)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var defaultData = multiLang_Convenience.Where(p => p.LangKey == ConstantStrings.DefaultLangAdmin).FirstOrDefault();
                    var model = _dbContext.Conveniences.Where(p => p.Pid == Convenience.Pid).FirstOrDefault();
                    if (images != null)
                    {
                        _fileServices.DeleteFile(ConstantStrings.UrlConvenienceImages, model.Url);
                        dynamic kt = _fileServices.SaveFileOriginal(images, ConstantStrings.UrlConvenienceImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            model.Url = kt.fileName;
                        }
                    }
                    model.Link = Convenience.Link;
                    model.Enabled = Convenience.Enabled;
                    var list = _dbContext.MultiLang_Conveniences.Where(p => p.ConveniencePid == Convenience.Pid).ToList();
                    foreach (var item in multiLang_Convenience)
                    {
                        var tempModel = _dbContext.MultiLang_Conveniences.Where(p => p.Pid == item.Pid).FirstOrDefault();
                        if (tempModel != null)
                        {
                            tempModel.Title = item.Title;
                        }
                        else
                        {
                            item.ConveniencePid = Convenience.Pid;
                            _dbContext.MultiLang_Conveniences.Add(item);
                        }
                    }


                    _dbContext.SaveChanges();



                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    messErr = "Something Wrong!";
                    return new { status = false, mess = messErr };
                }
            }
        }
        public bool Count(int code)
        {
            try
            {
                #region check count
                var count = _dbContext.Users.Where(p => p.GroupUserCode == code).Count();
                #endregion
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
             ;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public string SearchDto(SearchDto searchData)
        {
            try
            {
                //int page = 1;
                //int limit = 25;
                var data = _dbContext.GroupUsers.Where(p => ((
                                                           ((p.Name.Contains(searchData.Key) ||
                                                           p.Role.Contains(searchData.Key)) || String.IsNullOrEmpty(searchData.Key))
                                                           ) && (p.Enabled.Equals(searchData.Enable) || searchData.Enable.Equals(null))) &&

                                                    p.Deleted == false).ToList();
                //PagedList<GroupUser> pagingData = new PagedList<GroupUser>(data, page, limit);
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Role = item.Role;
                    child.Code = item.Code;
                    child.Name = item.Name;
                    child.Enabled = item.Enabled;
                    child.CountUser = _dbContext.Users.Where(p => p.GroupUserCode == item.Code && p.Deleted == false).Count();
                    listData.Add(child);
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(listData);

                return result;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Validation(dynamic data)
        {
            try
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
            catch (Exception ex)
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
        }
        public bool Up(long Pid)
        {
            try
            {
                int temp = 0;
                var data = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Conveniences.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
                if (changeData != null)
                {
                    data.Order = changeData.Order;
                    changeData.Order = temp;
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool Down(long Pid)
        {
            try
            {
                int temp = 0;
                var data = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Conveniences.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
                data.Order = changeData.Order;
                changeData.Order = temp;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool UpdateOrder(long Pid, int order)
        {
            try
            {
                var data = _dbContext.Conveniences.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.Conveniences.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Conveniences.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Conveniences.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Conveniences.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                        foreach (var item in listData)
                        {
                            if (item.Order > 1)
                            {
                                item.Order = item.Order - 1;

                            }
                        }
                        fromRow.Order = orderTo;
                        toRow.Order = orderTo - 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }
                }
                else
                {
                    var listData = _dbContext.Conveniences.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                    if (orderTo != 1)
                    {
                        foreach (var item in listData)
                        {
                            if (item.Order < max)
                            {
                                item.Order = item.Order + 1;
                            }
                        }
                        fromRow.Order = orderTo;

                        toRow.Order = orderTo + 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }

                }


                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
