using CMS.Areas.Promotion.Models;
using CMS.Areas.Shared.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using DTO.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Promotion
{
    public class PromotionRepository : IPromotionRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        private string UrlPromotionImages = ConstantStrings.UrlPromotionImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int PromotionId = ConstantStrings.PromotionId;
        private int RootPromotionCatePid = ConstantStrings.RootPromotionCatePid;

        public PromotionRepository(DBContext dbContext,
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
                var data = (from a in _dbContext.PromotionDetails
                            join b in _dbContext.PromotionCate_PromotionDetails on a.Pid equals b.PromotionDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_PromotionDetails on a.Pid equals c.PromotionDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.PromotionCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
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
                                Description = y.Description,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_PromotionDetails.Where(p => p.LangKey == DefaultLang && p.PromotionDetailPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.Slug = temp.Slug;
                    child.CounterView = item.CounterView;
                    child.PublishDate = item.PublishDate;
                    child.PicThumb = UrlPromotionImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.IsHot = item.IsHot;
                    child.Enabled = item.Enabled;
                    child.StartDate = item.StartDate;
                    child.EndDate = item.EndDate;
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
        public dynamic LoadProducts(SearchDto search)
        {
            try
            {
                int cate = search.Cate != null ? Convert.ToInt32(search.Cate) : 0;
                List<dynamic> listData = new List<dynamic>();
                var data = (from a in _dbContext.ProductDetails
                            join b in _dbContext.ProductCate_ProductDetails on a.Pid equals b.ProductDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_ProductDetails on a.Pid equals c.ProductDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.ProductCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
                                                    && a.Deleted == false && y.LangKey == DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                PicThumb = a.PicThumb,
                                Title = y.Title,
                                Price = a.Price,
                                Description = y.Description
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_ProductDetails.Where(p => p.LangKey == DefaultLang && p.ProductDetailPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    child.PicThumb = ConstantStrings.UrlProductImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.Enabled = item.Enabled;
                    child.Price = item.Price;
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
                        var model = _dbContext.PromotionDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var promotion_Product = _dbContext.Promotion_Products.Where(x => x.PromotionPid == Pid).ToList();
                _dbContext.Promotion_Products.RemoveRange(promotion_Product);
                _dbContext.SaveChanges();

                var model = _dbContext.PromotionDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                _dbContext.PromotionDetails.Remove(model);
                var deleteImage = _dbContext.Images_Promotiones.Where(p => p.PromotionDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlPromotionImages, item2.Images);
                    _fileServices.DeleteFile(UrlPromotionImages, Fullmages + item2.Images);
                }
                _dbContext.Images_Promotiones.RemoveRange(deleteImage);

                _fileServices.DeleteFile(UrlPromotionImages, model.PicThumb);
                _fileServices.DeleteFile(UrlPromotionImages, Fullmages + model.PicThumb);

                _dbContext.SaveChanges();

                //dynamic logObj = new ExpandoObject();
                //logObj.Title = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                //logObj.Pid = model.Pid;
                //logObj.Cate = PromotionId;
                //_common.SaveLog(1, "delete", logObj);
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
                        var promotion_Product = _dbContext.Promotion_Products.Where(x => x.PromotionPid == item).ToList();
                        _dbContext.Promotion_Products.RemoveRange(promotion_Product);
                        _dbContext.SaveChanges();

                        var model = _dbContext.PromotionDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            _dbContext.PromotionDetails.Remove(model);

                            _fileServices.DeleteFile(UrlPromotionImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlPromotionImages, Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_Promotiones.Where(p => p.PromotionDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlPromotionImages, item2.Images);
                                _fileServices.DeleteFile(UrlPromotionImages, Fullmages + item2.Images);
                            }
                            _dbContext.Images_Promotiones.RemoveRange(deleteImage);

                            _dbContext.SaveChanges();

                            //dynamic logObj = new ExpandoObject();
                            //logObj.Title = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                            //logObj.Pid = model.Pid;
                            //logObj.Cate = PromotionId;
                            //_common.SaveLog(1, "delete", logObj);
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

                var data = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == Pid).ToList();
                var detail = _dbContext.PromotionDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_Promotiones.Where(p => p.PromotionDetailPid == Pid).ToList();

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();
                foreach (var item in data)
                {
                    var promotion = _dbContext.PromotionDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (promotion.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlPromotionImages + promotion.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Content = item.Content;
                        child.TitleSEO = item.TitleSEO;
                        child.DescriptionSEO = item.DescriptionSEO;
                        child.Pid = item.Pid;
                        child.PromotionDetailPid = item.PromotionDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_Promotiones.Where(p => p.ImagesPromotionPid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlPromotionImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesPromotionPid;
                        listImages_lang.Add(child2);
                    }
                }
                var getListCates = _dbContext.PromotionCate_PromotionDetails.Where(x => x.PromotionDetailPid == Pid).Select(x => x.PromotionCatePid).ToList();
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
                        detail.StartDate,
                        detail.EndDate,
                        PicThumb = UrlPromotionImages + detail.PicThumb
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
        public dynamic Insert(PromotionDetail promotionDetail, List<MultiLang_PromotionDetail> multiLangPromotionDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesPromotion,
                            List<Temp_MultiLang_Images> listLangImagesPromotion, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangPromotionDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangPromotionDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    //if (!string.IsNullOrEmpty(promotionDetail.TagKey))
                    //{
                    //    var arrTagKey = promotionDetail.TagKey.Split(",");
                    //    var tagkeys = new List<string>();
                    //    foreach (var item in arrTagKey)
                    //    {
                    //        tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                    //    }
                    //    promotionDetail.SlugTagKey = string.Join(",", tagkeys);
                    //}
                    int maxOrder = _dbContext.PromotionDetails.Max(x => (int?)x.Order) ?? 1;
                    promotionDetail.Order = maxOrder + 1;
                    _dbContext.PromotionDetails.Add(promotionDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    //string[] cateArray = { RootPromotionCatePid.ToString() };
                    //if (listCates != null)
                    //{
                    //    cateArray = listCates.Split(',');
                    //}
                    //foreach (var item in cateArray)
                    //{
                    //    PromotionCate_PromotionDetail promotionCate_PromotionDetail = new PromotionCate_PromotionDetail();
                    //    promotionCate_PromotionDetail.PromotionCatePid = Convert.ToInt32(item);
                    //    promotionCate_PromotionDetail.PromotionDetailPid = promotionDetail.Pid;
                    //    _dbContext.PromotionCate_PromotionDetails.Add(promotionCate_PromotionDetail);
                    //}
                    //_dbContext.SaveChanges();
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangPromotionDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.PromotionDetailPid = promotionDetail.Pid;
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
                        var existSlug = (from a in _dbContext.PromotionDetails
                                         join b in _dbContext.MultiLang_PromotionDetails on a.Pid equals b.PromotionDetailPid
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
                        _dbContext.MultiLang_PromotionDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlPromotionImages, nameImages + "-" + promotionDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlPromotionImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlPromotionImages, saveFileStatus.fileName);
                                }
                                promotionDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPromotionImages, nameImages + "-" + promotionDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlPromotionImages, saveFileStatus.fileName);
                                promotionDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = new Random().Next(9000, 10000);
                    if (listImagesPromotion != null)
                    {
                        foreach (var item in listImagesPromotion)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlPromotionImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlPromotionImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Promotion images_Promotiones = new Images_Promotion();

                                images_Promotiones.Images = temp.fileName;
                                images_Promotiones.Order = item.Order;
                                images_Promotiones.PromotionDetailPid = promotionDetail.Pid;
                                _dbContext.AddRange(images_Promotiones);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion 
                    #region save log
                    //dynamic logObj = new ExpandoObject();
                    //logObj.Title = defaultData.Title;
                    //logObj.Pid = promotionDetail.Pid;
                    //logObj.Cate = PromotionId;
                    //_common.SaveLog(1, "new", logObj);
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
        public dynamic Update(PromotionDetail promotionDetail, List<MultiLang_PromotionDetail> multiLangPromotionDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesPromotion, List<Temp_MultiLang_Images> listLangImagesPromotion, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.PromotionDetails.Where(p => p.Pid == promotionDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == promotionDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    model.PublishDate = promotionDetail.PublishDate;
                    model.StartDate = promotionDetail.StartDate;
                    model.EndDate = promotionDetail.EndDate;
                    model.Enabled = promotionDetail.Enabled;
                    model.TagKey = promotionDetail.TagKey;
                    model.UpdateDate = DateTime.Now;
                    //if (!string.IsNullOrEmpty(promotionDetail.TagKey))
                    //{
                    //    var arrTagKey = promotionDetail.TagKey.Split(",");
                    //    var tagkeys = new List<string>();
                    //    foreach (var item in arrTagKey)
                    //    {
                    //        tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                    //    }
                    //    model.SlugTagKey = string.Join(",", tagkeys);
                    //}
                    #region edit multi_lang
                    foreach (var item in multiLangPromotionDetail)
                    {
                        var multiModel = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == promotionDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.PromotionDetails
                                             join b in _dbContext.MultiLang_PromotionDetails on a.Pid equals b.PromotionDetailPid
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
                            var defaultData = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == promotionDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.PromotionDetailPid = promotionDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.PromotionDetails
                                             join b in _dbContext.MultiLang_PromotionDetails on a.Pid equals b.PromotionDetailPid
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
                            _dbContext.MultiLang_PromotionDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlPromotionImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlPromotionImages, Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlPromotionImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlPromotionImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlPromotionImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlPromotionImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlPromotionImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }

                    #endregion

                    #region edit Cate
                    //var listCateOld = _dbContext.PromotionCate_PromotionDetails.Where(x => x.PromotionDetailPid == promotionDetail.Pid).ToList();
                    //_dbContext.PromotionCate_PromotionDetails.RemoveRange(listCateOld);
                    //_dbContext.SaveChanges();
                    //string[] cateArray = { RootPromotionCatePid.ToString() };
                    //if (listCates != null)
                    //{
                    //    cateArray = listCates.Split(',');
                    //}
                    //foreach (var item in cateArray)
                    //{
                    //    PromotionCate_PromotionDetail promotionCate_PromotionDetail = new PromotionCate_PromotionDetail();
                    //    promotionCate_PromotionDetail.PromotionCatePid = Convert.ToInt32(item);
                    //    promotionCate_PromotionDetail.PromotionDetailPid = promotionDetail.Pid;
                    //    _dbContext.PromotionCate_PromotionDetails.Add(promotionCate_PromotionDetail);
                    //}
                    //_dbContext.SaveChanges();
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_Promotiones.Where(p => p.PromotionDetailPid == promotionDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_Promotiones.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlPromotionImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlPromotionImages, Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = new Random().Next(9000, 10000);
                    foreach (var item in listImagesPromotion)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlPromotionImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlPromotionImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Promotion images_Promotiones = new Images_Promotion();

                                images_Promotiones.Images = temp.fileName;
                                images_Promotiones.Order = item.Order;
                                images_Promotiones.PromotionDetailPid = promotionDetail.Pid;
                                _dbContext.Images_Promotiones.Add(images_Promotiones);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesPromotion.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_Promotion multiLang_Images_Promotiones = new MultiLang_Images_Promotion();
                                    multiLang_Images_Promotiones.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_Promotiones.ImagesPromotionPid = images_Promotiones.Pid;
                                    multiLang_Images_Promotiones.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_Promotiones);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesPromotion.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_Promotiones.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_Promotion multiLang_Images_Promotiones_edit = new MultiLang_Images_Promotion();

                                    multiLang_Images_Promotiones_edit.ImagesPromotionPid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_Promotiones_edit.Caption = tempItem.Caption;
                                    multiLang_Images_Promotiones_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_Promotiones.Add(multiLang_Images_Promotiones_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();

                    #region save log
                    //dynamic logObj = new ExpandoObject();
                    //logObj.Title = multiLangPromotionDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    //logObj.Pid = promotionDetail.Pid;
                    //logObj.Cate = PromotionId;
                    //_common.SaveLog(1, "update", logObj);
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
                            var model = _dbContext.PromotionDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.PromotionDetails.Max(x => (int?)x.Order) ?? 1;

                            PromotionDetail addPromotion = new PromotionDetail();
                            addPromotion.Order = maxOrder + 1;
                            addPromotion.PicThumb = "";
                            addPromotion.Enabled = false;
                            addPromotion.PublishDate = DateTime.Now;
                            addPromotion.TagKey = model.TagKey;

                            _dbContext.PromotionDetails.Add(addPromotion);
                            _dbContext.SaveChanges();
                            List<MultiLang_PromotionDetail> detailModel = _dbContext.MultiLang_PromotionDetails.Where(p => p.PromotionDetailPid == model.Pid).ToList();
                            foreach (MultiLang_PromotionDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_PromotionDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_PromotionDetail temp = new MultiLang_PromotionDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.PromotionDetailPid = addPromotion.Pid;
                                _dbContext.MultiLang_PromotionDetails.Add(temp);
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
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == PromotionId.ToString()).FirstOrDefault();
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
                    modulePreview.ModuleId = PromotionId.ToString();
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
                var toRow = _dbContext.PromotionDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.PromotionDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.PromotionDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.PromotionDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.PromotionDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                var data = _dbContext.PromotionDetails.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var data = _dbContext.PromotionDetails.Where(p => p.Pid == pid).FirstOrDefault();
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

        public List<ProductDto> GetAllProduct()
        {
            try
            {
                var data = (from a in _dbContext.ProductDetails
                            join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                            where (a.Enabled && !a.Deleted) && (b.LangKey == "vi")
                            select new ProductDto()
                            {
                                Pid = a.Pid,
                                Title = b.Title,
                                PicThumb = ConstantStrings.UrlProductImages + a.PicThumb,
                                Price = a.Price
                            }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string CheckValid(List<long> listPid, long pid)
        {
            try
            {
                var promotionProducts = _dbContext.Promotion_Products.Where(x => x.PromotionPid != pid).Select(x => x.ProductPid).ToList();

                var listPidContained = listPid.Where(x => promotionProducts.Contains(x)).ToList();

                if (listPidContained.Any())
                {
                    return JsonConvert.SerializeObject(new { message = string.Join(",", listPidContained), isError = true });
                }
                return JsonConvert.SerializeObject(new { message = "", isError = false });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { message = "", isError = true });
            }
        }
        public List<PromoProduct> GetAllPromoProduct(List<long> listPid)
        {
            try
            {
                var promotions = _dbContext.Promotion_Products.ToList();



                var model = (from a in _dbContext.ProductDetails
                             join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                             where (!a.Deleted && a.Enabled) && (b.LangKey == DefaultLang) && (listPid.Contains(a.Pid))
                             orderby a.Order descending
                             select new
                             {
                                 Pid = a.Pid,
                                 Title = b.Title,
                                 PicThumb = ConstantStrings.UrlProductImages + Thumb + a.PicThumb,
                                 Price = a.Price
                                 //Options = (from child_a in _dbContext.ProductOptions
                                 //           join child_b in _dbContext.ProductOption_ProductDetails on child_a.Pid equals child_b.ProductOptionPid
                                 //           where !child_a.Deleted && child_a.Enabled && child_b.Status && child_b.ProductDetailPid == a.Pid
                                 //           select new { child_a, child_b }).ToList()
                             }).ToList();

                var data = new List<PromoProduct>();


                foreach (var item in model)
                {
                    PromoProduct p = new PromoProduct();
                    p.PicThumb = item.PicThumb;
                    p.Pid = item.Pid;
                    p.Title = item.Title;
                    p.PriceOrigin = item.Price;
                    var existsPrice = promotions.Where(x => x.ProductPid == item.Pid).FirstOrDefault();
                    if (existsPrice != null)
                    {
                        p.Price = existsPrice.Price;
                    }
                    else
                    {
                        p.Price = item.Price;
                    }

                    //var child = new List<PromoProductOption>();
                    //foreach (var ele in item.Options)
                    //{
                    //    PromoProductOption p2 = new PromoProductOption();
                    //    p2.OptionPid = ele.child_a.Pid;
                    //    p2.OptionCode = ele.child_a.Code;
                    //    p2.PriceOrigin = ele.child_b.Price;
                    //    var existsPrice = promotions.Where(x => x.ProductPid == item.Pid && x.OptionPid == ele.child_a.Pid).FirstOrDefault();
                    //    if (existsPrice != null)
                    //    {
                    //        p2.Price = existsPrice.Price;

                    //    }
                    //    else
                    //    {
                    //        p2.Price = 0;
                    //    }
                    //    child.Add(p2);
                    //}
                    //p.Options = child;
                    data.Add(p);
                }

                return data;
            }
            catch (Exception ex)
            {
                return new List<PromoProduct>();
            }
        }

        public List<PromoProduct> GetPromo(long pid)
        {
            try
            {
                var promotions = _dbContext.Promotion_Products.ToList();

                var currentPromotion = promotions.Where(x => x.PromotionPid == pid).Select(x => x.ProductPid).ToList();

                var model = (from a in _dbContext.ProductDetails
                             join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                             where (!a.Deleted && a.Enabled) && (b.LangKey == DefaultLang) && (currentPromotion.Contains(a.Pid))
                             orderby a.Order descending
                             select new
                             {
                                 Pid = a.Pid,
                                 Title = b.Title,
                                 PicThumb = ConstantStrings.UrlProductImages + Thumb + a.PicThumb,
                                 //Options = (from child_a in _dbContext.ProductOptions
                                 //           join child_b in _dbContext.ProductOption_ProductDetails on child_a.Pid equals child_b.ProductOptionPid
                                 //           where !child_a.Deleted && child_a.Enabled && child_b.Status && child_b.ProductDetailPid == a.Pid
                                 //           select new { child_a, child_b }).ToList()
                                 Price = a.Price
                             }).ToList();

                var data = new List<PromoProduct>();


                foreach (var item in model)
                {
                    PromoProduct p = new PromoProduct();
                    p.PicThumb = item.PicThumb;
                    p.Pid = item.Pid;
                    p.Title = item.Title;

                    p.PriceOrigin = item.Price;
                    var existsPrice = promotions.Where(x => x.ProductPid == item.Pid).FirstOrDefault();
                    if (existsPrice != null)
                    {
                        p.Price = existsPrice.Price;
                    }
                    else
                    {
                        p.Price = item.Price;
                    }

                    //var child = new List<PromoProductOption>();
                    //foreach (var ele in item.Options)
                    //{
                    //    PromoProductOption p2 = new PromoProductOption();
                    //    p2.OptionPid = ele.child_a.Pid;
                    //    p2.OptionCode = ele.child_a.Code;
                    //    p2.PriceOrigin = ele.child_b.Price;
                    //    var existsPrice = promotions.Where(x => x.ProductPid == item.Pid && x.OptionPid == ele.child_a.Pid).FirstOrDefault();
                    //    if (existsPrice != null)
                    //    {
                    //        p2.Price = existsPrice.Price;

                    //    }
                    //    else
                    //    {
                    //        p2.Price = 0;
                    //    }
                    //    child.Add(p2);
                    //}
                    //p.Options = child;

                    data.Add(p);
                }

                return data;
            }
            catch (Exception ex)
            {
                return new List<PromoProduct>();
            }
        }
        public bool UpdatePromotionProduct(List<PromoProduct> data, long pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var list = _dbContext.Promotion_Products.Where(x => x.PromotionPid == pid).ToList();
                    _dbContext.Promotion_Products.RemoveRange(list);
                    _dbContext.SaveChanges();

                    foreach (var item in data)
                    {
                        //foreach (var el in item.Options)
                        //{
                        //    Promotion_Product p = new Promotion_Product();
                        //    p.ProductPid = item.Pid;
                        //    p.PromotionPid = pid;
                        //    p.OptionPid = el.OptionPid;
                        //    p.Price = el.Price;
                        //    _dbContext.Promotion_Products.Add(p);
                        //    _dbContext.SaveChanges();
                        //}

                        Promotion_Product p = new Promotion_Product();
                        p.ProductPid = item.Pid;
                        p.PromotionPid = pid;
                        p.OptionPid = ConstantStrings.RootProductCatePid;
                        p.Price = item.Price;
                        _dbContext.Promotion_Products.Add(p);
                        _dbContext.SaveChanges();
                    }
                    transaction.Commit();
                    return true;

                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
