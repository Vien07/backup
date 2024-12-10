using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Banner.Models;
using DTO;
using CMS.Services.FileServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using CMS.Services.CommonServices;

namespace CMS.Areas.Banner
{
    public class BannerRepository : IBannerRepository
    {
        private readonly DBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private string BannerSlideWidth = "0";

        private string messErr = "";
        private string Fullmages = ConstantStrings.Fullmages;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlBannerImages = ConstantStrings.UrlBannerImages;
        private string KeyBannerSlideWidth = ConstantStrings.KeyBannerSlideWidth;
        public BannerRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            _dbContext = dbContext;
            _fileServices = fileServices;
            BannerSlideWidth = _common.GetConfigValue(KeyBannerSlideWidth);
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {

                int cate = search.Cate != null ? Convert.ToInt32(search.Cate) : 0;
                List<dynamic> listData = new List<dynamic>();
                var data = (from a in _dbContext.Banners
                            join b in _dbContext.Banner_Pages on a.Pid equals b.BannerPid
                            join c in _dbContext.MultiLang_Banners on a.Pid equals c.BannerPid
                            where (b.PageId == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
                                                    && a.Deleted == false && c.LangKey == DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                Title = c.Title,
                                Description = c.Description,
                                Images = a.Images
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);
                foreach (var item in data)
                {
                    dynamic a = new ExpandoObject();
                    a.Pid = item.Pid;
                    a.Order = item.Order;
                    a.Enabled = item.Enabled;
                    a.Title = item.Title;
                    a.Images = UrlBannerImages + Fullmages + item.Images;
                    listData.Add(a);
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
                        var model = _dbContext.Banners.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.Banners.Where(p => p.Pid == Pid).FirstOrDefault();
                _fileServices.DeleteFile(UrlBannerImages, Fullmages + model.Images);
                var multiLang = _dbContext.MultiLang_Banners.Where(p => p.BannerPid == Pid).ToList();
                _dbContext.MultiLang_Banners.RemoveRange(multiLang);
                _dbContext.Banners.Remove(model);
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
                        var model = _dbContext.Banners.Where(p => p.Pid == item).FirstOrDefault();
                        _fileServices.DeleteFile(UrlBannerImages, Fullmages + model.Images);
                        var multiLang = _dbContext.MultiLang_Banners.Where(p => p.BannerPid == item).ToList();
                        _dbContext.MultiLang_Banners.RemoveRange(multiLang);
                        _dbContext.Banners.Remove(model);
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
        public dynamic Edit(int Pid)
        {
            try
            {
                var data = _dbContext.Banners.Where(p => p.Deleted == false && p.Pid == Pid).FirstOrDefault();
                if (data != null)
                {
                    var mulltiLang = _dbContext.MultiLang_Banners.Where(p => p.BannerPid == Pid).ToList();
                    var cates = _dbContext.Banner_Pages.Where(p => p.BannerPid == data.Pid).ToList();
                    List<dynamic> listCates = new List<dynamic>();
                    foreach (var item in cates)
                    {
                        listCates.Add(item.PageId);
                    }
                    List<dynamic> listData = new List<dynamic>();
                    foreach (var item in mulltiLang)
                    {

                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        listData.Add(child);

                    }
                    return new
                    {
                        detail = new
                        {
                            picThumb = UrlBannerImages + Fullmages + data.Images,
                            ImagesName = Fullmages + data.Images,
                            data.Enabled,
                            data.Pid,
                            listCates
                        },
                        list = listData
                    };

                }

                return "[]";
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(Models.Banner banner, List<MultiLang_Banner> multiLang_Banner, string listCates, IFormFile PicThumb)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    string[] pageArray = { ConstantStrings.RootCourseCatePid.ToString() };
                    if (listCates != null)
                    {
                        pageArray = listCates.Split(',');
                    }
                    var defaultData = multiLang_Banner.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    int maxOrder = _dbContext.Banners.Max(x => (int?)x.Order) ?? 1;
                    banner.Order = maxOrder + 1;
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFileNotResizeWidth(PicThumb, UrlBannerImages, defaultData.Title + "-" + banner.Pid, Convert.ToInt32(BannerSlideWidth));

                        if (!saveFileStatus.isError)
                        {
                            banner.Images = saveFileStatus.fileName;
                            _dbContext.SaveChanges();

                        }
                    }
                    _dbContext.Banners.Add(banner);
                    _dbContext.SaveChanges();
                    foreach (var item in pageArray)
                    {
                        Banner_Page tempMultiPage = new Banner_Page();
                        tempMultiPage.BannerPid = banner.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Banner_Pages.Add(tempMultiPage);
                        _dbContext.SaveChanges();
                    }
                    foreach (var item in multiLang_Banner)
                    {
                        item.BannerPid = banner.Pid;
                        _dbContext.MultiLang_Banners.Add(item);
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
        public dynamic Update(Models.Banner banner, List<MultiLang_Banner> multiLang_Banner, string listPage, IFormFile images)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string[] pageArray = { ConstantStrings.RootCourseCatePid.ToString() };
                    if (listPage != null)
                    {
                        pageArray = listPage.Split(',');
                    }
                    var oldPage = _dbContext.Banner_Pages.Where(p => p.BannerPid == banner.Pid).ToList();
                    _dbContext.Banner_Pages.RemoveRange(oldPage);
                    _dbContext.SaveChanges();
                    foreach (var item in pageArray)
                    {
                        Banner_Page tempMultiPage = new Banner_Page();
                        tempMultiPage.BannerPid = banner.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Banner_Pages.Add(tempMultiPage);
                        _dbContext.SaveChanges();
                    }
                    var defaultData = multiLang_Banner.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    var model = _dbContext.Banners.Where(p => p.Pid == banner.Pid).FirstOrDefault();
                    if (images != null)
                    {
                        _fileServices.DeleteFile(UrlBannerImages, Fullmages + model.Images);
                        dynamic kt = _fileServices.SaveFileNotResizeWidth(images, UrlBannerImages, defaultData.Title, Convert.ToInt32(BannerSlideWidth));

                        if (!kt.isError)
                        {
                            model.Images = kt.fileName;
                        }
                    }
                    //model.Enabled = banner.Enabled;

                    #region multi lang
                    foreach (var item in multiLang_Banner)
                    {
                        var multiModel = _dbContext.MultiLang_Banners.Where(p => p.BannerPid == banner.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Title = item.Title;
                            //multiModel.Description = item.Description;
                        }
                        else
                        {
                            var tempdefaultData = _dbContext.MultiLang_Banners.Where(p => p.BannerPid == banner.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? tempdefaultData.Title : item.Title;
                            item.Title = title;
                            item.BannerPid = banner.Pid;
                            _dbContext.MultiLang_Banners.Add(item);
                        }
                        #endregion
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
                var data = _dbContext.Banners.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Banners.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Banners.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Banners.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Banners.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.Banners.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Banners.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Banners.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Banners.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.Banners.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
