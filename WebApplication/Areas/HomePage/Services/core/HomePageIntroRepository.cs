using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.HomePage.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.HomePage
{
    public class HomePageIntroRepository : IHomePageIntroRepository
    {
        private readonly ICommonServices _common;
        private string messErr = "";
        private string ImageMaxWidth = "0";
        private string BannerHomePageWidth = "0";

        private string UrlHomePageImages = ConstantStrings.UrlHomePageImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private int NewsId = ConstantStrings.NewsId;
        private int RootNewsCatePid = ConstantStrings.RootNewsCatePid;
        private string KeyImageMaxWidth = ConstantStrings.KeyImageMaxWidth;
        private string KeyBannerSlideWidth = ConstantStrings.KeyBannerSlideWidth;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        public HomePageIntroRepository(DBContext dbContext, IFileServices fileServices, ICommonServices common)
        {
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
            ImageMaxWidth = _common.GetConfigValue(KeyImageMaxWidth);
            BannerHomePageWidth = _common.GetConfigValue(KeyBannerSlideWidth);
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                //var data = _dbContext.ProjectDetails.Where(p => p.Deleted != true).ToList();
                var listProjectPid = _dbContext.MultiLang_HomePages.Select(p => new { p.HomePagePid, p.Title, p.Description })
                    .Distinct().ToList().FilterSearch(new string[] { "Description", "Title" }, search.Key).Select(x => x.HomePagePid).Distinct().ToList();
                List<Models.HomePage> data = new List<Models.HomePage>();

                foreach (var item in listProjectPid)
                {
                    var temp = _dbContext.HomePages.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item && p.Type == "intro").FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_HomePages.Where(p => p.LangKey == DefaultLang && p.HomePagePid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Images = UrlHomePageImages + Fullmages + item.Images;
                    child.ImagesName = Fullmages + item.Images;
                    //child.Link = item.Link;
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
                        var model = _dbContext.HomePages.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();
                _fileServices.DeleteFile(UrlHomePageImages, Fullmages + model.Images);
                var multiLang = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == Pid).ToList();
                _dbContext.MultiLang_HomePages.RemoveRange(multiLang);
                _dbContext.HomePages.Remove(model);
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
                        var model = _dbContext.HomePages.Where(p => p.Pid == item).FirstOrDefault();
                        _fileServices.DeleteFile(UrlHomePageImages, Fullmages + model.Images);
                        var multiLang = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == item).ToList();
                        _dbContext.MultiLang_HomePages.RemoveRange(multiLang);
                        _dbContext.HomePages.Remove(model);
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

                var data = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == Pid).ToList();
                var detail = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var homepage = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (homepage.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.IntroLink = item.IntroLink;
                        child.Content = item.Content;
                        child.Pid = item.Pid;
                        child.HomePagePid = item.HomePagePid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        picThumb = UrlHomePageImages + Fullmages + detail.Images,
                        backgroundImage = UrlHomePageImages + Fullmages + detail.BackgroundImage,
                        ImagesName = Fullmages + detail.Images,
                        type = detail.Type,
                        detail.Enabled,
                        detail.Pid,
                        detail.Position
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
        public dynamic Insert(Models.HomePage homepage, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images, IFormFile backgroundImage)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    var defaultData = multiLang_HomePage.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    //int maxOrder = _dbContext.HomePages.Select(p => p.Order).DefaultIfEmpty(0).Max();
                    int maxOrder = _dbContext.HomePages.Where(x => x.Type == "intro").Max(x => (int?)x.Order) ?? 1;

                    homepage.Order = maxOrder + 1;
                    homepage.Type = "intro";

                    if (images != null)
                    {
                        dynamic kt = _fileServices.SaveFileNotResizeWidthNoBackground(images, UrlHomePageImages, defaultData.Title, Convert.ToInt32(ImageMaxWidth));
                        if (!kt.isError)
                        {
                            homepage.Images = kt.fileName;
                        }
                    }

                    if (backgroundImage != null)
                    {
                        dynamic kt = _fileServices.SaveFileNotResizeWidth(backgroundImage, UrlHomePageImages, "bg_" + defaultData.Title, Convert.ToInt32(BannerHomePageWidth));
                        if (!kt.isError)
                        {
                            homepage.BackgroundImage = kt.fileName;
                        }
                    }

                    _dbContext.HomePages.Add(homepage);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_HomePage)
                    {
                        // string title = string.IsNullOrEmpty(item.Tite) ? defaultData.Tite : item.Name;
                        item.HomePagePid = homepage.Pid;
                        _dbContext.MultiLang_HomePages.Add(item);
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
        public dynamic Update(Models.HomePage homepage, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images, IFormFile backgroundImage)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var defaultData = multiLang_HomePage.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    var model = _dbContext.HomePages.Where(p => p.Pid == homepage.Pid).FirstOrDefault();
                    if (images != null)
                    {
                        _fileServices.DeleteFile(UrlHomePageImages, Fullmages + model.Images);
                        dynamic kt = _fileServices.SaveFileNotResizeWidthNoBackground(images, UrlHomePageImages, defaultData.Title, Convert.ToInt32(ImageMaxWidth));

                        if (!kt.isError)
                        {
                            model.Images = kt.fileName;
                        }
                    }
                    if (backgroundImage != null)
                    {
                        _fileServices.DeleteFile(UrlHomePageImages, Fullmages + model.BackgroundImage);
                        dynamic kt = _fileServices.SaveFileNotResizeWidth(backgroundImage, UrlHomePageImages, "bg_" + defaultData.Title, Convert.ToInt32(BannerHomePageWidth));

                        if (!kt.isError)
                        {
                            model.BackgroundImage = kt.fileName;
                        }
                    }
                    model.Position = homepage.Position;
                    model.Enabled = homepage.Enabled;

                    _dbContext.SaveChanges();
                    #region multi lang
                    foreach (var item in multiLang_HomePage)
                    {
                        var multiModel = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == homepage.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Title = item.Title;
                            multiModel.Description = item.Description;
                            multiModel.Content = item.Content;
                            multiModel.IntroLink = item.IntroLink;
                        }
                        else
                        {
                            var tempdefaultData = _dbContext.MultiLang_HomePages.Where(p => p.HomePagePid == homepage.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? tempdefaultData.Title : item.Title;
                            item.Title = title;
                            item.HomePagePid = homepage.Pid;
                            _dbContext.MultiLang_HomePages.Add(item);
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
                var data = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.HomePages.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.HomePages.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.HomePages.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.HomePages.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.HomePages.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.HomePages.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.HomePages.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.HomePages.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
