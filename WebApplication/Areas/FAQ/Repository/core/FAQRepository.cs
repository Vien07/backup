using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.FAQ.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using CMS.Areas.Shared.Models;

namespace CMS.Areas.FAQ
{
    public class FAQRepository : IFAQRepository
    {
        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        private string UrlFAQImages = ConstantStrings.UrlFAQImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int FAQId = ConstantStrings.FAQId;
        private int RootFAQCatePid = ConstantStrings.RootFAQCatePid;

        public FAQRepository(DBContext dbContext,
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
                var data = (from a in _dbContext.FAQDetails
                            join b in _dbContext.FAQCate_FAQDetails on a.Pid equals b.FAQDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_FAQDetails on a.Pid equals c.FAQDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.FAQCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
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
                    child.PicThumb = UrlFAQImages + item.PicThumb;
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
                        var model = _dbContext.FAQDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.FAQDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                var deleteImage = _dbContext.Images_FAQs.Where(p => p.FAQDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlFAQImages, item2.Images);
                    _fileServices.DeleteFile(UrlFAQImages, Fullmages + item2.Images);
                }
                _dbContext.Images_FAQs.RemoveRange(deleteImage);

                _fileServices.DeleteFile(UrlFAQImages, Fullmages + model.PicThumb);
                _fileServices.DeleteFile(UrlFAQImages, model.PicThumb);
                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = FAQId;
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
                        var model = _dbContext.FAQDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;

                            _fileServices.DeleteFile(UrlFAQImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlFAQImages, Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_FAQs.Where(p => p.FAQDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlFAQImages, item2.Images);
                                _fileServices.DeleteFile(UrlFAQImages, Fullmages + item2.Images);
                            }
                            _dbContext.Images_FAQs.RemoveRange(deleteImage);
                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                            logObj.Pid = model.Pid;
                            logObj.Cate = FAQId;
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
                var data = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == Pid).ToList();
                var detail = _dbContext.FAQDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_FAQs.Where(p => p.FAQDetailPid == Pid).ToList();

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();
                foreach (var item in data)
                {
                    var faq = _dbContext.FAQDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (faq.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlFAQImages + faq.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Answer = item.Answer;
                        child.Content = item.Content;
                        child.TitleSEO = item.TitleSEO;
                        child.DescriptionSEO = item.DescriptionSEO;
                        child.Pid = item.Pid;
                        child.FAQDetailPid = item.FAQDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_FAQs.Where(p => p.ImagesFAQPid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlFAQImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesFAQPid;
                        listImages_lang.Add(child2);
                    }
                }
                var getListCates = _dbContext.FAQCate_FAQDetails.Where(x => x.FAQDetailPid == Pid).Select(x => x.FAQCatePid).ToList();
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
                        PicThumb = UrlFAQImages + detail.PicThumb
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
        public dynamic Insert(FAQDetail faqDetail, List<MultiLang_FAQDetail> multiLangFAQDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesFAQ,
                            List<Temp_MultiLang_Images> listLangImagesFAQ, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangFAQDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangFAQDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    if (!string.IsNullOrEmpty(faqDetail.TagKey))
                    {
                        var arrTagKey = faqDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        faqDetail.SlugTagKey = string.Join(",", tagkeys);
                    }
                    int maxOrder = _dbContext.FAQDetails.Max(x => (int?)x.Order) ?? 1;
                    faqDetail.Order = maxOrder + 1;
                    _dbContext.FAQDetails.Add(faqDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    string[] cateArray = { RootFAQCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        FAQCate_FAQDetail faqCate_FAQDetail = new FAQCate_FAQDetail();
                        faqCate_FAQDetail.FAQCatePid = Convert.ToInt32(item);
                        faqCate_FAQDetail.FAQDetailPid = faqDetail.Pid;
                        _dbContext.FAQCate_FAQDetails.Add(faqCate_FAQDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangFAQDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.FAQDetailPid = faqDetail.Pid;
                        if (item.LangKey != DefaultLang)
                        {
                            item.Title = title;
                            item.DescriptionSEO = item.DescriptionSEO;
                            item.TitleSEO = item.TitleSEO;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Answer = string.IsNullOrEmpty(item.Answer) ? defaultData.Answer : item.Answer;
                        }

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.FAQDetails
                                         join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
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
                        _dbContext.MultiLang_FAQDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlFAQImages, nameImages + "-" + faqDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlFAQImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlFAQImages, saveFileStatus.fileName);
                                }
                                faqDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlFAQImages, nameImages + "-" + faqDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlFAQImages, saveFileStatus.fileName);
                                faqDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = new Random().Next(9000, 10000);
                    if (listImagesFAQ != null)
                    {
                        foreach (var item in listImagesFAQ)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlFAQImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlFAQImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_FAQ images_FAQs = new Images_FAQ();

                                images_FAQs.Images = temp.fileName;
                                images_FAQs.Order = item.Order;
                                images_FAQs.FAQDetailPid = faqDetail.Pid;
                                _dbContext.AddRange(images_FAQs);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion 
                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = defaultData.Title;
                    logObj.Pid = faqDetail.Pid;
                    logObj.Cate = FAQId;
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
        public dynamic Update(FAQDetail faqDetail, List<MultiLang_FAQDetail> multiLangFAQDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesFAQ, List<Temp_MultiLang_Images> listLangImagesFAQ, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.FAQDetails.Where(p => p.Pid == faqDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == faqDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    model.PublishDate = faqDetail.PublishDate;
                    model.Enabled = faqDetail.Enabled;
                    model.TagKey = faqDetail.TagKey;
                    model.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(faqDetail.TagKey))
                    {
                        var arrTagKey = faqDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        model.SlugTagKey = string.Join(",", tagkeys);
                    }
                    #region edit multi_lang
                    foreach (var item in multiLangFAQDetail)
                    {
                        var multiModel = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == faqDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.FAQDetails
                                             join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
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
                            multiModel.Answer = item.Answer;
                            multiModel.DescriptionSEO = item.DescriptionSEO;
                            multiModel.TitleSEO = item.TitleSEO;
                            multiModel.Description = _common.RemoveHtmlTag(item.Description);
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == faqDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.FAQDetailPid = faqDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Answer = string.IsNullOrEmpty(item.Answer) ? defaultData.Answer : item.Answer;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.FAQDetails
                                             join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
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
                            _dbContext.MultiLang_FAQDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlFAQImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlFAQImages, Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlFAQImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlFAQImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlFAQImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlFAQImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlFAQImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }
                    #endregion

                    #region edit Cate
                    var listCateOld = _dbContext.FAQCate_FAQDetails.Where(x => x.FAQDetailPid == faqDetail.Pid).ToList();
                    _dbContext.FAQCate_FAQDetails.RemoveRange(listCateOld);
                    _dbContext.SaveChanges();
                    string[] cateArray = { RootFAQCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        FAQCate_FAQDetail faqCate_FAQDetail = new FAQCate_FAQDetail();
                        faqCate_FAQDetail.FAQCatePid = Convert.ToInt32(item);
                        faqCate_FAQDetail.FAQDetailPid = faqDetail.Pid;
                        _dbContext.FAQCate_FAQDetails.Add(faqCate_FAQDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_FAQs.Where(p => p.FAQDetailPid == faqDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_FAQs.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlFAQImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlFAQImages, Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = new Random().Next(9000, 10000);
                    foreach (var item in listImagesFAQ)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlFAQImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlFAQImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_FAQ images_FAQs = new Images_FAQ();

                                images_FAQs.Images = temp.fileName;
                                images_FAQs.Order = item.Order;
                                images_FAQs.FAQDetailPid = faqDetail.Pid;
                                _dbContext.Images_FAQs.Add(images_FAQs);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesFAQ.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_FAQ multiLang_Images_FAQs = new MultiLang_Images_FAQ();
                                    multiLang_Images_FAQs.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_FAQs.ImagesFAQPid = images_FAQs.Pid;
                                    multiLang_Images_FAQs.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_FAQs);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesFAQ.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_FAQs.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_FAQ multiLang_Images_FAQs_edit = new MultiLang_Images_FAQ();

                                    multiLang_Images_FAQs_edit.ImagesFAQPid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_FAQs_edit.Caption = tempItem.Caption;
                                    multiLang_Images_FAQs_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_FAQs.Add(multiLang_Images_FAQs_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangFAQDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = faqDetail.Pid;
                    logObj.Cate = FAQId;
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
                            var model = _dbContext.FAQDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.FAQDetails.Max(x => (int?)x.Order) ?? 1;

                            FAQDetail addFAQ = new FAQDetail();
                            addFAQ.Order = maxOrder + 1;
                            addFAQ.PicThumb = "";
                            addFAQ.Enabled = false;
                            addFAQ.PublishDate = DateTime.Now;
                            addFAQ.TagKey = model.TagKey;
                            _dbContext.FAQDetails.Add(addFAQ);
                            _dbContext.SaveChanges();

                            //Add RootFAQCate
                            var faqCate_FAQDetail = new FAQCate_FAQDetail();
                            faqCate_FAQDetail.FAQCatePid = ConstantStrings.RootFAQCatePid;
                            faqCate_FAQDetail.FAQDetailPid = addFAQ.Pid;
                            _dbContext.FAQCate_FAQDetails.Add(faqCate_FAQDetail);
                            _dbContext.SaveChanges();

                            List<MultiLang_FAQDetail> detailModel = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == model.Pid).ToList();
                            foreach (MultiLang_FAQDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_FAQDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_FAQDetail temp = new MultiLang_FAQDetail();
                                temp.Content = itemDetail.Content;
                                temp.Answer = itemDetail.Answer;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.FAQDetailPid = addFAQ.Pid;
                                _dbContext.MultiLang_FAQDetails.Add(temp);
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
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == FAQId.ToString()).FirstOrDefault();
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
                    modulePreview.ModuleId = FAQId.ToString();
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
                var toRow = _dbContext.FAQDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.FAQDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.FAQDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.FAQDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.FAQDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                var data = _dbContext.FAQDetails.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var data = _dbContext.FAQDetails.Where(p => p.Pid == pid).FirstOrDefault();
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

    }
}
