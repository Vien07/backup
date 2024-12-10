using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using CMS;
using Microsoft.Extensions.Configuration;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using CMS.Areas.Advertisement.Models;
using DTO.Common;
using CMS.Services.FileServices;
using DTO;
using CMS.Services.CommonServices;
using CMS.Services;
namespace CMS.Areas.Advertisement
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;
        string messErr = "";

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        private string KeyPageLimitAdmin = ConstantStrings.KeyPageLimitAdmin;
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private string UrlAdvertisementImages = ConstantStrings.UrlAdvertisementImages;
        private int RootAdvertisementCatePid = ConstantStrings.RootAdvertisementCatePid;
        public AdvertisementRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _common = common;
            _fileServices = fileServices;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                //var data = _dbContext.ProjectDetails.Where(p => p.Deleted != true).ToList();
                var listProjectPid = _dbContext.MultiLang_Advertisements.Select(p => new { p.AdvertisementPid, p.Title, p.Description })
                    .Distinct().ToList().FilterSearch(new string[] { "Description", "Title" }, search.Key).Select(x => x.AdvertisementPid).Distinct().ToList();
                List<Models.Advertisement> data = new List<Models.Advertisement>();

                foreach (var item in listProjectPid)
                {
                    var temp = _dbContext.Advertisements.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item).FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_Advertisements.Where(p => p.LangKey == DefaultLang && p.AdvertisementPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Images = UrlAdvertisementImages + Fullmages + item.Images;
                    child.ImagesName = Fullmages + item.Images;
                    child.Link = item.Link;
                    child.Description = temp.Description;
                    if (temp.Description.Length > 200)
                    {
                        child.Description = temp.Description.Substring(0, 150) + "...";
                    }

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
                        var model = _dbContext.Advertisements.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
                _fileServices.DeleteFile(UrlAdvertisementImages, Fullmages + model.Images);
                var multiLang = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == Pid).ToList();
                _dbContext.MultiLang_Advertisements.RemoveRange(multiLang);
                _dbContext.Advertisements.Remove(model);
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
                        var model = _dbContext.Advertisements.Where(p => p.Pid == item).FirstOrDefault();
                        _fileServices.DeleteFile(UrlAdvertisementImages, Fullmages + model.Images);
                        var multiLang = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == item).ToList();
                        _dbContext.MultiLang_Advertisements.RemoveRange(multiLang);
                        _dbContext.Advertisements.Remove(model);
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

                var data = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == Pid).ToList();
                var detail = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
                var cates = _dbContext.Advertisement_Pages.Where(p => p.AdvertisementPid == detail.Pid).ToList();
                List<dynamic> listCates = new List<dynamic>();
                foreach (var item in cates)
                {
                    listCates.Add(item.PageId);
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var slide = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (slide.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.EmbedCode = item.EmbedCode;
                        child.Pid = item.Pid;
                        child.AdvertisementPid = item.AdvertisementPid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        picThumb = UrlAdvertisementImages + Fullmages + detail.Images,
                        ImagesName = Fullmages + detail.Images,
                        detail.Link,
                        detail.Enabled,
                        detail.Pid,
                        detail.Position,
                        detail.TargetLink,
                        detail.DisplayType,
                        detail.Type,
                        listCates
                    },
                    list = listData,
                };

                //return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(Models.Advertisement slide, List<MultiLang_Advertisement> multiLang_Advertisement, IFormFile images, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    string[] pageArray = { RootAdvertisementCatePid.ToString() };
                    if (listCates != null)
                    {
                        pageArray = listCates.Split(',');
                    }

                    var defaultData = multiLang_Advertisement.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    //int maxOrder = _dbContext.Advertisements.Select(p => p.Order).DefaultIfEmpty(0).Max();
                    int maxOrder = _dbContext.Advertisements.Max(x => (int?)x.Order) ?? 1;

                    slide.Order = maxOrder + 1;
                    if(images != null)
                    {
                        dynamic kt = _fileServices.SaveFileNotResize(images, UrlAdvertisementImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            slide.Images = kt.fileName;
                        }
                    }
                    else
                    {
                        slide.Images = "";
                    }
                    


                    _dbContext.Advertisements.Add(slide);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_Advertisement)
                    {
                        // string title = string.IsNullOrEmpty(item.Tite) ? defaultData.Tite : item.Name;
                        item.AdvertisementPid = slide.Pid;
                        _dbContext.MultiLang_Advertisements.Add(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (var item in pageArray)
                    {
                        Advertisement_Page tempMultiPage = new Advertisement_Page();
                        tempMultiPage.AdvertisementPid = slide.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Advertisement_Pages.Add(tempMultiPage);
                        _dbContext.SaveChanges();
                    }
                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";
                    return new { isError = false, mess = messErr };
                }
            }
        }
        public dynamic Update(Models.Advertisement slide, List<MultiLang_Advertisement> multiLang_Advertisement, IFormFile images, string listPage)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string[] pageArray = { RootAdvertisementCatePid.ToString() };
                    if (listPage != null)
                    {
                        pageArray = listPage.Split(',');
                    }
                    var oldPage = _dbContext.Advertisement_Pages.Where(p => p.AdvertisementPid == slide.Pid).ToList();
                    _dbContext.Advertisement_Pages.RemoveRange(oldPage);
                    _dbContext.SaveChanges();
                    foreach (var item in pageArray)
                    {
                        Advertisement_Page tempMultiPage = new Advertisement_Page();
                        tempMultiPage.AdvertisementPid = slide.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Advertisement_Pages.Add(tempMultiPage);
                        _dbContext.SaveChanges();
                    }
                    var defaultData = multiLang_Advertisement.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    var model = _dbContext.Advertisements.Where(p => p.Pid == slide.Pid).FirstOrDefault();
                    if (images != null)
                    {
                        _fileServices.DeleteFile(UrlAdvertisementImages, Fullmages + model.Images);
                        dynamic kt = _fileServices.SaveFileNotResize(images, UrlAdvertisementImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            model.Images = kt.fileName;
                        }
                    }
                    model.Link = slide.Link;
                    model.Position = slide.Position;
                    model.TargetLink = slide.TargetLink;
                    model.DisplayType = slide.DisplayType;
                    model.Enabled = slide.Enabled;
                    model.Type = slide.Type;



                    _dbContext.SaveChanges();
                    #region multi lang
                    foreach (var item in multiLang_Advertisement)
                    {
                        var multiModel = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == slide.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Title = item.Title;
                            multiModel.Description = item.Description;
                            multiModel.EmbedCode = item.EmbedCode;
                        }
                        else
                        {
                            var tempdefaultData = _dbContext.MultiLang_Advertisements.Where(p => p.AdvertisementPid == slide.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? tempdefaultData.Title : item.Title;
                            string desc = string.IsNullOrEmpty(item.Description) ? tempdefaultData.Description : item.Description;
                            string embedCode = string.IsNullOrEmpty(item.EmbedCode) ? tempdefaultData.EmbedCode : item.EmbedCode;
                            item.Title = title;
                            item.Description = desc;
                            item.EmbedCode = embedCode;
                            item.AdvertisementPid = slide.Pid;
                            _dbContext.MultiLang_Advertisements.Add(item);
                        }
                    }
                    _dbContext.SaveChanges();

                    #endregion


                    transaction.Commit();


                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";
                    return new { isError = false, mess = messErr };
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
        public string Search(SearchDto searchData)
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
                var data = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Advertisements.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Advertisements.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Advertisements.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.Advertisements.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Advertisements.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Advertisements.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Advertisements.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.Advertisements.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
