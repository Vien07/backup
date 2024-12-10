using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.News.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using CMS.Areas.Shared.Models;

namespace CMS.Areas.News
{
    public class NewsRepository : INewsRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int NewsId = ConstantStrings.NewsId;
        private int RootNewsCatePid = ConstantStrings.RootNewsCatePid;

        public NewsRepository(DBContext dbContext,
                             IFileServices fileHelper, ICommonServices common)
        {
            _dbContext = dbContext;
            _fileServices = fileHelper;
            _common = common;
            WatermarkActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkActive);
            WatermarkPicThumbActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkPicThumbActive);
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                int cate = search.Cate != null ? Convert.ToInt32(search.Cate) : 0;
                List<dynamic> listData = new List<dynamic>();
                var data = (from a in _dbContext.NewsDetails
                            join b in _dbContext.NewsCate_NewsDetails on a.Pid equals b.NewsDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_NewsDetails on a.Pid equals c.NewsDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.NewsCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
                                                    && a.Deleted == false && y.LangKey == DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                CounterView = a.CounterView,
                                PublishDate = a.PublishDate,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                PicThumb = a.PicThumb,
                                IsHot = a.IsHot,
                                Title = y.Title,
                                Slug = y.Slug,
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Title = item.Title;
                    child.Slug = item.Slug;
                    child.CounterView = item.CounterView;
                    child.PublishDate = item.PublishDate;
                    child.PicThumb = UrlNewsImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.IsHot = item.IsHot;
                    child.Enabled = item.Enabled;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.Order).ToList(), search.Page, search.PageNumber);
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
                        var model = _dbContext.NewsDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.NewsDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                var deleteImage = _dbContext.Images_Newses.Where(p => p.NewsDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlNewsImages, item2.Images);
                    _fileServices.DeleteFile(UrlNewsImages, Fullmages + item2.Images);
                }
                _dbContext.Images_Newses.RemoveRange(deleteImage);

                _fileServices.DeleteFile(UrlNewsImages, model.PicThumb);
                _fileServices.DeleteFile(UrlNewsImages, Fullmages + model.PicThumb);

                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = NewsId;
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
                        var model = _dbContext.NewsDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;

                            _fileServices.DeleteFile(UrlNewsImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlNewsImages, Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_Newses.Where(p => p.NewsDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlNewsImages, item2.Images);
                                _fileServices.DeleteFile(UrlNewsImages, Fullmages + item2.Images);
                            }
                            _dbContext.Images_Newses.RemoveRange(deleteImage);
                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                            logObj.Pid = model.Pid;
                            logObj.Cate = NewsId;
                            _common.SaveLog(1, "delete", logObj);
                        }
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
                var data = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == Pid).ToList();
                var detail = _dbContext.NewsDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_Newses.Where(p => p.NewsDetailPid == Pid).ToList();

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();
                foreach (var item in data)
                {
                    var news = _dbContext.NewsDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (news.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlNewsImages + news.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Content = item.Content;
                        child.TitleSEO = item.TitleSEO;
                        child.DescriptionSEO = item.DescriptionSEO;
                        child.Pid = item.Pid;
                        child.NewsDetailPid = item.NewsDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_Newses.Where(p => p.ImagesNewsPid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlNewsImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesNewsPid;
                        listImages_lang.Add(child2);
                    }
                }
                var getListCates = _dbContext.NewsCate_NewsDetails.Where(x => x.NewsDetailPid == Pid).Select(x => x.NewsCatePid).ToList();
                List<dynamic> listCates = new List<dynamic>();
                if (getListCates!.Any())
                {
                    foreach (var item in getListCates)
                    {
                        listCates.Add(item);
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
                        PicThumb = UrlNewsImages + detail.PicThumb
                    },
                    list = listData,
                    listCates = listCates,
                    images = listImages,
                    images_lang = listImages_lang

                };

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(NewsDetail newsDetail, List<MultiLang_NewsDetail> multiLangNewsDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesNews,
                            List<Temp_MultiLang_Images> listLangImagesNews, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangNewsDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangNewsDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    if (!string.IsNullOrEmpty(newsDetail.TagKey))
                    {
                        var arrTagKey = newsDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        newsDetail.SlugTagKey = string.Join(",", tagkeys);
                    }
                    int maxOrder = _dbContext.NewsDetails.Max(x => (int?)x.Order) ?? 1;
                    newsDetail.Order = maxOrder + 1;
                    _dbContext.NewsDetails.Add(newsDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    string[] cateArray = { RootNewsCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        NewsCate_NewsDetail newsCate_NewsDetail = new NewsCate_NewsDetail();
                        newsCate_NewsDetail.NewsCatePid = Convert.ToInt32(item);
                        newsCate_NewsDetail.NewsDetailPid = newsDetail.Pid;
                        _dbContext.NewsCate_NewsDetails.Add(newsCate_NewsDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangNewsDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.NewsDetailPid = newsDetail.Pid;
                        if (item.LangKey != DefaultLang)
                        {
                            item.Title = title;
                            item.DescriptionSEO = item.DescriptionSEO;
                            item.TitleSEO = item.TitleSEO;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                        }

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.NewsDetails
                                         join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
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

                        item.TitleWithoutSign = _common.RemoveSign4VietnameseString(title);
                        _dbContext.MultiLang_NewsDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlNewsImages, nameImages + "-" + newsDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlNewsImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlNewsImages, saveFileStatus.fileName);
                                }
                                newsDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlNewsImages, nameImages + "-" + newsDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlNewsImages, saveFileStatus.fileName);
                                newsDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = new Random().Next(9000, 10000);
                    if (listImagesNews != null)
                    {
                        foreach (var item in listImagesNews)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlNewsImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlNewsImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_News images_Newses = new Images_News();

                                images_Newses.Images = temp.fileName;
                                images_Newses.Order = item.Order;
                                images_Newses.NewsDetailPid = newsDetail.Pid;
                                _dbContext.AddRange(images_Newses);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion 
                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = defaultData.Title;
                    logObj.Pid = newsDetail.Pid;
                    logObj.Cate = NewsId;
                    _common.SaveLog(1, "new", logObj);
                    #endregion
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
        public dynamic Update(NewsDetail newsDetail, List<MultiLang_NewsDetail> multiLangNewsDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesNews, List<Temp_MultiLang_Images> listLangImagesNews, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.NewsDetails.Where(p => p.Pid == newsDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == newsDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    model.PublishDate = newsDetail.PublishDate;
                    model.Enabled = newsDetail.Enabled;
                    model.TagKey = newsDetail.TagKey;
                    model.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(newsDetail.TagKey))
                    {
                        var arrTagKey = newsDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        model.SlugTagKey = string.Join(",", tagkeys);
                    }
                    #region edit multi_lang
                    foreach (var item in multiLangNewsDetail)
                    {
                        var multiModel = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == newsDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.NewsDetails
                                             join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
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

                            multiModel.TitleWithoutSign = _common.RemoveSign4VietnameseString(item.Title);
                            multiModel.Title = item.Title;
                            multiModel.Content = item.Content;
                            multiModel.DescriptionSEO = item.DescriptionSEO;
                            multiModel.TitleSEO = item.TitleSEO;
                            multiModel.Description = _common.RemoveHtmlTag(item.Description);
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == newsDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;

                            item.Title = title;
                            item.NewsDetailPid = newsDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.NewsDetails
                                             join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
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

                            item.DescriptionSEO = item.DescriptionSEO;
                            item.TitleSEO = item.TitleSEO;
                            item.TitleWithoutSign = _common.RemoveSign4VietnameseString(title);
                            _dbContext.MultiLang_NewsDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlNewsImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlNewsImages, Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlNewsImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlNewsImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlNewsImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlNewsImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlNewsImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }

                    #endregion

                    #region edit Cate
                    var listCateOld = _dbContext.NewsCate_NewsDetails.Where(x => x.NewsDetailPid == newsDetail.Pid).ToList();
                    _dbContext.NewsCate_NewsDetails.RemoveRange(listCateOld);
                    _dbContext.SaveChanges();
                    string[] cateArray = { RootNewsCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        NewsCate_NewsDetail newsCate_NewsDetail = new NewsCate_NewsDetail();
                        newsCate_NewsDetail.NewsCatePid = Convert.ToInt32(item);
                        newsCate_NewsDetail.NewsDetailPid = newsDetail.Pid;
                        _dbContext.NewsCate_NewsDetails.Add(newsCate_NewsDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_Newses.Where(p => p.NewsDetailPid == newsDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_Newses.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlNewsImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlNewsImages, Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = new Random().Next(9000, 10000);
                    foreach (var item in listImagesNews)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlNewsImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlNewsImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_News images_Newses = new Images_News();

                                images_Newses.Images = temp.fileName;
                                images_Newses.Order = item.Order;
                                images_Newses.NewsDetailPid = newsDetail.Pid;
                                _dbContext.Images_Newses.Add(images_Newses);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesNews.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_News multiLang_Images_Newses = new MultiLang_Images_News();
                                    multiLang_Images_Newses.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_Newses.ImagesNewsPid = images_Newses.Pid;
                                    multiLang_Images_Newses.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_Newses);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesNews.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_Newses.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_News multiLang_Images_Newses_edit = new MultiLang_Images_News();

                                    multiLang_Images_Newses_edit.ImagesNewsPid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_Newses_edit.Caption = tempItem.Caption;
                                    multiLang_Images_Newses_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_Newses.Add(multiLang_Images_Newses_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangNewsDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = newsDetail.Pid;
                    logObj.Cate = NewsId;
                    _common.SaveLog(1, "update", logObj);
                    #endregion
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
                            var model = _dbContext.NewsDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.NewsDetails.Max(x => (int?)x.Order) ?? 1;

                            NewsDetail addNews = new NewsDetail();
                            addNews.Order = maxOrder + 1;
                            addNews.PicThumb = "";
                            addNews.Enabled = false;
                            addNews.PublishDate = DateTime.Now;
                            addNews.TagKey = model.TagKey;
                            _dbContext.NewsDetails.Add(addNews);
                            _dbContext.SaveChanges();

                            //Add RootNewsCate
                            var newsCate_NewsDetail = new NewsCate_NewsDetail();
                            newsCate_NewsDetail.NewsCatePid = ConstantStrings.RootNewsCatePid;
                            newsCate_NewsDetail.NewsDetailPid = addNews.Pid;
                            _dbContext.NewsCate_NewsDetails.Add(newsCate_NewsDetail);
                            _dbContext.SaveChanges();

                            List<MultiLang_NewsDetail> detailModel = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == model.Pid).ToList();
                            foreach (MultiLang_NewsDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_NewsDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_NewsDetail temp = new MultiLang_NewsDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.NewsDetailPid = addNews.Pid;
                                _dbContext.MultiLang_NewsDetails.Add(temp);
                                _dbContext.SaveChanges();
                            }

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
        public bool Preview(string obj, string objDetail, IFormFile PicThumb)
        {
            try
            {
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == NewsId.ToString()).FirstOrDefault();
                if (model != null)
                {
                    model.Obj = obj;
                    model.ObjDetail = objDetail;
                    model.IsEditPicThumb = false;
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "new" + "-" + model.Pid);

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
                    modulePreview.ModuleId = NewsId.ToString();
                    modulePreview.IsEditPicThumb = false;
                    _dbContext.ModulePreviews.Add(modulePreview);
                    _dbContext.SaveChanges();
                    if (PicThumb != null)
                    {
                        modulePreview.IsEditPicThumb = true;
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "new" + "-" + modulePreview.Pid);
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
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.NewsDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.NewsDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.NewsDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.NewsDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.NewsDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
        public bool UpdateOrder(long Pid, int order)
        {
            try
            {
                var data = _dbContext.NewsDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool SaveStatus(long pid, bool value, string type)
        {
            try
            {
                var data = _dbContext.NewsDetails.Where(p => p.Pid == pid).FirstOrDefault();
                if (data != null)
                {
                    if (type == "hot")
                    {
                        data.IsHot = value;
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
        private bool CheckExist(string slug, string lang)
        {
            var model = _dbContext.MultiLang_NewsDetails.Where(x => x.LangKey == lang && x.Slug == slug).FirstOrDefault();
            if (model == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
