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
using CMS.Areas.Popup.Models;
using DTO.Common;
using CMS.Services.FileServices;
using DTO;
using CMS.Services.CommonServices;
using CMS.Services;
namespace CMS.Areas.Popup
{
    public class PopupRepository : IPopupRepository
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
        private string UrlPopupImages = ConstantStrings.UrlPopupImages;
        private int RootPopupCatePid = ConstantStrings.RootPopupCatePid;
        public PopupRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
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
                var listProjectPid = _dbContext.MultiLang_Popups.Select(p => new { p.PopupPid, p.Title, p.Description })
                    .Distinct().ToList().FilterSearch(new string[] { "Description", "Title" }, search.Key).Select(x => x.PopupPid).Distinct().ToList();
                List<Models.Popup> data = new List<Models.Popup>();

                foreach (var item in listProjectPid)
                {
                    var temp = _dbContext.Popups.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item).FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_Popups.Where(p => p.LangKey == DefaultLang && p.PopupPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Images = UrlPopupImages + Fullmages + item.Images;
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
                        var model = _dbContext.Popups.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
                _fileServices.DeleteFile(UrlPopupImages, Fullmages + model.Images);
                var multiLang = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == Pid).ToList();
                _dbContext.MultiLang_Popups.RemoveRange(multiLang);
                _dbContext.Popups.Remove(model);
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
                        var model = _dbContext.Popups.Where(p => p.Pid == item).FirstOrDefault();
                        _fileServices.DeleteFile(UrlPopupImages, Fullmages + model.Images);
                        var multiLang = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == item).ToList();
                        _dbContext.MultiLang_Popups.RemoveRange(multiLang);
                        _dbContext.Popups.Remove(model);
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

                var data = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == Pid).ToList();
                var detail = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
                var cates = _dbContext.Popup_Pages.Where(p => p.PopupPid == detail.Pid).ToList();
                List<dynamic> listCates = new List<dynamic>();
                foreach (var item in cates)
                {
                    listCates.Add(item.PageId);
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var slide = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (slide.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.EmbedCode = item.EmbedCode;
                        child.Pid = item.Pid;
                        child.PopupPid = item.PopupPid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        picThumb = UrlPopupImages + Fullmages + detail.Images,
                        ImagesName = Fullmages + detail.Images,
                        detail.Link,
                        detail.DelayTime,
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
        public dynamic Insert(Models.Popup slide, List<MultiLang_Popup> multiLang_Popup, IFormFile images, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    string[] pageArray = { RootPopupCatePid.ToString() };
                    if (listCates != null)
                    {
                        pageArray = listCates.Split(',');
                    }

                    var defaultData = multiLang_Popup.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    //int maxOrder = _dbContext.Popups.Select(p => p.Order).DefaultIfEmpty(0).Max();
                    int maxOrder = _dbContext.Popups.Max(x => (int?)x.Order) ?? 1;

                    slide.Order = maxOrder + 1;
                    if(images != null)
                    {
                        dynamic kt = _fileServices.SaveFileNotResize(images, UrlPopupImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            slide.Images = kt.fileName;
                        }
                    }
                    else
                    {
                        slide.Images = "";
                    }
                    


                    _dbContext.Popups.Add(slide);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_Popup)
                    {
                        // string title = string.IsNullOrEmpty(item.Tite) ? defaultData.Tite : item.Name;
                        item.PopupPid = slide.Pid;
                        _dbContext.MultiLang_Popups.Add(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (var item in pageArray)
                    {
                        Popup_Page tempMultiPage = new Popup_Page();
                        tempMultiPage.PopupPid = slide.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Popup_Pages.Add(tempMultiPage);
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
        public dynamic Update(Models.Popup slide, List<MultiLang_Popup> multiLang_Popup, IFormFile images, string listPage)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string[] pageArray = { RootPopupCatePid.ToString() };
                    if (listPage != null)
                    {
                        pageArray = listPage.Split(',');
                    }
                    var oldPage = _dbContext.Popup_Pages.Where(p => p.PopupPid == slide.Pid).ToList();
                    _dbContext.Popup_Pages.RemoveRange(oldPage);
                    _dbContext.SaveChanges();
                    foreach (var item in pageArray)
                    {
                        Popup_Page tempMultiPage = new Popup_Page();
                        tempMultiPage.PopupPid = slide.Pid;
                        tempMultiPage.PageId = Convert.ToInt32(item);
                        _dbContext.Popup_Pages.Add(tempMultiPage);
                        _dbContext.SaveChanges();
                    }
                    var defaultData = multiLang_Popup.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    var model = _dbContext.Popups.Where(p => p.Pid == slide.Pid).FirstOrDefault();
                    if (images != null)
                    {
                        _fileServices.DeleteFile(UrlPopupImages, Fullmages + model.Images);
                        dynamic kt = _fileServices.SaveFileNotResize(images, UrlPopupImages, defaultData.Title);

                        if (!kt.isError)
                        {
                            model.Images = kt.fileName;
                        }
                    }
                    model.Link = slide.Link;
                    model.DelayTime = slide.DelayTime;
                    model.Position = slide.Position;
                    model.TargetLink = slide.TargetLink;
                    model.DisplayType = slide.DisplayType;
                    model.Enabled = slide.Enabled;
                    model.Type = slide.Type;



                    _dbContext.SaveChanges();
                    #region multi lang
                    foreach (var item in multiLang_Popup)
                    {
                        var multiModel = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == slide.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Title = item.Title;
                            multiModel.Description = item.Description;
                            multiModel.EmbedCode = item.EmbedCode;
                        }
                        else
                        {
                            var tempdefaultData = _dbContext.MultiLang_Popups.Where(p => p.PopupPid == slide.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? tempdefaultData.Title : item.Title;
                            string desc = string.IsNullOrEmpty(item.Description) ? tempdefaultData.Description : item.Description;
                            string embedCode = string.IsNullOrEmpty(item.EmbedCode) ? tempdefaultData.EmbedCode : item.EmbedCode;
                            item.Title = title;
                            item.Description = desc;
                            item.EmbedCode = embedCode;
                            item.PopupPid = slide.Pid;
                            _dbContext.MultiLang_Popups.Add(item);
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
                var data = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Popups.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Popups.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Popups.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.Popups.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Popups.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Popups.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Popups.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.Popups.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
