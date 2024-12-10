using CMS.Areas.About.Models;
using CMS.Areas.Shared.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.About
{
    public class AboutRepository : IAboutRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;

        private string messErr = "";
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlAboutImages = ConstantStrings.UrlAboutImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private int AboutId = ConstantStrings.AboutId;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        public AboutRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor,
                                    IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                var listAboutPid = _dbContext.MultiLang_AboutDetails.Select(p => new { p.AboutDetailPid, p.Title, p.Content, p.Description })
                    .Distinct().ToList().FilterSearch(new string[] { "Title", "Description", "Content" }, search.Key).Select(x => x.AboutDetailPid).Distinct().ToList();
                List<AboutDetail> data = new List<AboutDetail>();

                foreach (var item in listAboutPid)
                {
                    var temp = _dbContext.AboutDetails.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item).FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }

                }

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_AboutDetails.Where(p => p.LangKey == DefaultLang && p.AboutDetailPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Slug = temp.Slug;
                    child.CounterView = item.CounterView;
                    child.PublishDate = item.PublishDate;
                    child.PicThumb = UrlAboutImages + Thumb + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.ShowFooter = item.ShowFooter;
                    child.ShowTopMenu = item.ShowTopMenu;
                    child.Enabled = item.Enabled;
                    child.Default = item.Default;
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
                        var model = _dbContext.AboutDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                _dbContext.SaveChanges();
                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_AboutDetails.Where(p => p.LangKey == DefaultLang && p.AboutDetailPid == model.Pid).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = AboutId;
                _common.SaveLog(1, "delete", logObj);
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
                        var model = _dbContext.AboutDetails.Where(p => p.Pid == item).FirstOrDefault();
                        model.Deleted = true;
                        model.UpdateDate = DateTime.Now;

                        _dbContext.SaveChanges();
                        dynamic logObj = new ExpandoObject();
                        logObj.Title = _dbContext.MultiLang_AboutDetails.Where(p => p.LangKey == DefaultLang && p.AboutDetailPid == model.Pid).FirstOrDefault().Title;
                        logObj.Pid = model.Pid;
                        logObj.Cate = AboutId;
                        _common.SaveLog(1, "delete", logObj);
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

                var data = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == Pid).ToList();
                var detail = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var About = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (About.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlAboutImages + Thumb + About.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Content = item.Content;
                        child.Pid = item.Pid;
                        child.AboutDetailPid = item.AboutDetailPid;
                        listData.Add(child);
                    }

                }
                return new
                {
                    detail = new
                    {
                        detail.TagKey,
                        detail.Enabled,
                        detail.Pid,
                        detail.PublishDate,
                        PicThumb = UrlAboutImages + Thumb + detail.PicThumb
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
        public dynamic Insert(AboutDetail aboutDetail, List<MultiLang_AboutDetail> multiLangAboutDetail)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    var defaultData = multiLangAboutDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangAboutDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    int maxOrder = (_dbContext.AboutDetails.Max(x => (int?)x.Order) ?? 1) + 1;

                    aboutDetail.Order = maxOrder;
                    aboutDetail.AboutCatePid = 1;
                    _dbContext.AboutDetails.Add(aboutDetail);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLangAboutDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.AboutDetailPid = aboutDetail.Pid;
                        if (item.LangKey != DefaultLang)
                        {
                            item.Title = title;
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                        }

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.AboutDetails
                                         join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                                         where (b.LangKey == item.LangKey && b.Slug == newSlug)
                                         select a).FirstOrDefault();

                        if (existSlug != null)
                        {
                            transaction.Rollback();
                            return new { status = false, mess = "Tiêu đề đã tồn tại" };
                        }
                        else
                        {
                            item.Slug = newSlug;
                        }
                        #endregion

                        _dbContext.MultiLang_AboutDetails.Add(item);
                        _dbContext.SaveChanges();
                    }

                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangAboutDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = aboutDetail.Pid;
                    logObj.Cate = AboutId;
                    _common.SaveLog(1, "new", logObj);
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
        public dynamic Update(AboutDetail aboutDetail, List<MultiLang_AboutDetail> multiLangAboutDetail)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = _dbContext.AboutDetails.Where(p => p.Pid == aboutDetail.Pid).FirstOrDefault();
                    string nameImages = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == aboutDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                    model.PublishDate = aboutDetail.PublishDate;
                    model.Enabled = aboutDetail.Enabled;
                    model.TagKey = aboutDetail.TagKey;
                    model.UpdateDate = DateTime.Now;
                    foreach (var item in multiLangAboutDetail)
                    {
                        var multiModel = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == aboutDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.AboutDetails
                                             join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                                             where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                             select a).FirstOrDefault();

                            if (existSlug != null)
                            {
                                transaction.Rollback();
                                return new { status = false, mess = "Tiêu đề đã tồn tại" };
                            }
                            else
                            {
                                multiModel.Slug = newSlug;
                            }
                            #endregion
                            multiModel.Title = item.Title;
                            multiModel.Content = item.Content;
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == aboutDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.AboutDetailPid = aboutDetail.Pid;
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.AboutDetails
                                             join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                                             where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                             select a).FirstOrDefault();

                            if (existSlug != null)
                            {
                                transaction.Rollback();
                                return new { status = false, mess = "Tiêu đề đã tồn tại" };
                            }
                            else
                            {
                                item.Slug = newSlug;
                            }
                            #endregion

                            _dbContext.MultiLang_AboutDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangAboutDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = aboutDetail.Pid;
                    logObj.Cate = AboutId;
                    _common.SaveLog(1, "update", logObj);
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
        public bool Coppy(long[] Pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in Pid)
                    {
                        try
                        {
                            var model = _dbContext.AboutDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.AboutDetails.Select(p => p.Order).Max();

                            AboutDetail addAbout = new AboutDetail();
                            addAbout.Order = maxOrder + 1;
                            addAbout.PicThumb = "";
                            addAbout.Enabled = false;
                            addAbout.PublishDate = DateTime.Now;
                            addAbout.TagKey = model.TagKey;
                            _dbContext.AboutDetails.Add(addAbout);
                            _dbContext.SaveChanges();
                            List<MultiLang_AboutDetail> detailModel = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == model.Pid).ToList();
                            foreach (MultiLang_AboutDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_AboutDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;

                                MultiLang_AboutDetail temp = new MultiLang_AboutDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.AboutDetailPid = addAbout.Pid;
                                _dbContext.MultiLang_AboutDetails.Add(temp);
                                _dbContext.SaveChanges();
                            }
                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_AboutDetails.Where(p => p.LangKey == DefaultLang && p.AboutDetailPid == addAbout.Pid).FirstOrDefault().Title;
                            logObj.Pid = addAbout.Pid;
                            logObj.Cate = AboutId;
                            _common.SaveLog(1, "new", logObj);
                            //_dbContext.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            //transaction.Rollback();
                        }

                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
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
                var data = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.AboutDetails.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.AboutDetails.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.AboutDetails.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.AboutDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.AboutDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.AboutDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.AboutDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.AboutDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
        public bool Preview(string obj, string objDetail, IFormFile PicThumb)
        {
            try
            {
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == AboutId.ToString()).FirstOrDefault();
                if (model != null)
                {
                    model.Obj = obj;
                    model.ObjDetail = objDetail;
                    model.IsEditPicThumb = false;
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "book" + "-" + model.Pid);

                        if (!saveFileStatus.isError)
                        {
                            model.IsEditPicThumb = true;
                            _fileServices.ResizeThumbImage(PicThumb, UrlPreviewImages, saveFileStatus.fileName);
                            model.PicThumb = saveFileStatus.fileName;
                            _dbContext.SaveChanges();
                        }
                    }
                    _dbContext.SaveChanges();
                }
                else
                {
                    ModulePreview modulePreview = new ModulePreview();
                    modulePreview.Obj = obj;
                    modulePreview.ObjDetail = objDetail;
                    modulePreview.ModuleId = AboutId.ToString();
                    modulePreview.IsEditPicThumb = true;
                    _dbContext.ModulePreviews.Add(modulePreview);
                    _dbContext.SaveChanges();
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "book" + "-" + modulePreview.Pid);

                        if (!saveFileStatus.isError)
                        {
                            _fileServices.ResizeThumbImage(PicThumb, UrlPreviewImages, saveFileStatus.fileName);
                            modulePreview.PicThumb = saveFileStatus.fileName;
                            _dbContext.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SaveStatus(long pid, bool value, string type)
        {
            try
            {
                var data = _dbContext.AboutDetails.Where(p => p.Pid == pid).FirstOrDefault();
                if (data != null)
                {
                    if (type == "menu")
                    {
                        data.ShowTopMenu = value;
                    }
                    else if (type == "default")
                    {
                        var checkDefault = _dbContext.AboutDetails.Where(p => p.Default == true && p.Pid != pid).ToList();
                        if (checkDefault.Any())
                        {
                            foreach (var item in checkDefault)
                            {
                                item.Default = false;
                            }
                        }
                        data.Default = value;

                    }
                    else
                    {
                        data.ShowFooter = value;
                    }
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
