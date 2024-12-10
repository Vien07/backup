using CMS.Areas.Shared.Models;
using CMS.Areas.DiscountCode.Models;
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

namespace CMS.Areas.DiscountCode
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        private string UrlDiscountCodeImages = ConstantStrings.UrlDiscountCodeImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int DiscountCodeId = ConstantStrings.DiscountCodeId;
        private int RootDiscountCodeCatePid = ConstantStrings.RootDiscountCodeCatePid;

        public DiscountCodeRepository(DBContext dbContext,
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
                var data = (from a in _dbContext.DiscountCodeDetails
                            join b in _dbContext.DiscountCodeCate_DiscountCodeDetails on a.Pid equals b.DiscountCodeDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_DiscountCodeDetails on a.Pid equals c.DiscountCodeDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.DiscountCodeCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
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
                                Code = a.Code,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                MaxQuantity = a.MaxQuantity,
                                UsedQuantity = a.UsedQuantity,
                                DiscountCodeType = a.DiscountCodeType,
                                DiscountCodeValue = a.DiscountCodeValue,
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Title = item.Title;
                    child.Slug = item.Slug;
                    child.CounterView = item.CounterView;
                    child.PublishDate = item.PublishDate;
                    child.PicThumb = UrlDiscountCodeImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.IsHot = item.IsHot;
                    child.Enabled = item.Enabled;
                    child.Code = item.Code;
                    child.StartDate = item.StartDate;
                    child.EndDate = item.EndDate;
                    child.MaxQuantity = item.MaxQuantity;
                    child.UsedQuantity = item.UsedQuantity;
                    child.DiscountCodeType = item.DiscountCodeType;
                    child.DiscountCodeValue = item.DiscountCodeValue;
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
                        var model = _dbContext.DiscountCodeDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.DiscountCodeDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                var deleteImage = _dbContext.Images_DiscountCodes.Where(p => p.DiscountCodeDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlDiscountCodeImages, item2.Images);
                    _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + item2.Images);
                }
                _dbContext.Images_DiscountCodes.RemoveRange(deleteImage);

                _fileServices.DeleteFile(UrlDiscountCodeImages, model.PicThumb);
                _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + model.PicThumb);

                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = DiscountCodeId;
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
                        var model = _dbContext.DiscountCodeDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;

                            _fileServices.DeleteFile(UrlDiscountCodeImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_DiscountCodes.Where(p => p.DiscountCodeDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlDiscountCodeImages, item2.Images);
                                _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + item2.Images);
                            }
                            _dbContext.Images_DiscountCodes.RemoveRange(deleteImage);

                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                            logObj.Pid = model.Pid;
                            logObj.Cate = DiscountCodeId;
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
                var data = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == Pid).ToList();
                var detail = _dbContext.DiscountCodeDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_DiscountCodes.Where(p => p.DiscountCodeDetailPid == Pid).ToList();

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();
                foreach (var item in data)
                {
                    var discountCode = _dbContext.DiscountCodeDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (discountCode.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlDiscountCodeImages + discountCode.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Content = item.Content;
                        child.TitleSEO = item.TitleSEO;
                        child.DescriptionSEO = item.DescriptionSEO;
                        child.Pid = item.Pid;
                        child.DiscountCodeDetailPid = item.DiscountCodeDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_DiscountCodes.Where(p => p.ImagesDiscountCodePid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlDiscountCodeImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesDiscountCodePid;
                        listImages_lang.Add(child2);
                    }
                }
                var getListCates = _dbContext.DiscountCodeCate_DiscountCodeDetails.Where(x => x.DiscountCodeDetailPid == Pid).Select(x => x.DiscountCodeCatePid).ToList();
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
                        detail.Code,
                        detail.MaxQuantity,
                        detail.MinTotal,
                        detail.DiscountCodeValue,
                        detail.DiscountCodeType,
                        PicThumb = UrlDiscountCodeImages + detail.PicThumb
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
        public dynamic Insert(DiscountCodeDetail discountCodeDetail, List<MultiLang_DiscountCodeDetail> multiLangDiscountCodeDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesDiscountCode,
                            List<Temp_MultiLang_Images> listLangImagesDiscountCode, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var checkCode = _dbContext.DiscountCodeDetails.Where(x => x.Code == discountCodeDetail.Code).Count();
                    if (checkCode > 0)
                    {
                        return new { status = false, mess = "Mã này đã tồn tại" };
                    }
                    var defaultData = multiLangDiscountCodeDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangDiscountCodeDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    if (!string.IsNullOrEmpty(discountCodeDetail.TagKey))
                    {
                        var arrTagKey = discountCodeDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        discountCodeDetail.SlugTagKey = string.Join(",", tagkeys);
                    }
                    int maxOrder = _dbContext.DiscountCodeDetails.Max(x => (int?)x.Order) ?? 1;
                    discountCodeDetail.Order = maxOrder + 1;

                    _dbContext.DiscountCodeDetails.Add(discountCodeDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    string[] cateArray = { RootDiscountCodeCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        DiscountCodeCate_DiscountCodeDetail discountCodeCate_DiscountCodeDetail = new DiscountCodeCate_DiscountCodeDetail();
                        discountCodeCate_DiscountCodeDetail.DiscountCodeCatePid = Convert.ToInt32(item);
                        discountCodeCate_DiscountCodeDetail.DiscountCodeDetailPid = discountCodeDetail.Pid;
                        _dbContext.DiscountCodeCate_DiscountCodeDetails.Add(discountCodeCate_DiscountCodeDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangDiscountCodeDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.DiscountCodeDetailPid = discountCodeDetail.Pid;
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
                        var existSlug = (from a in _dbContext.DiscountCodeDetails
                                         join b in _dbContext.MultiLang_DiscountCodeDetails on a.Pid equals b.DiscountCodeDetailPid
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
                        _dbContext.MultiLang_DiscountCodeDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlDiscountCodeImages, nameImages + "-" + discountCodeDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlDiscountCodeImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlDiscountCodeImages, saveFileStatus.fileName);
                                }
                                discountCodeDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlDiscountCodeImages, nameImages + "-" + discountCodeDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlDiscountCodeImages, saveFileStatus.fileName);
                                discountCodeDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = new Random().Next(9000, 10000);
                    if (listImagesDiscountCode != null)
                    {
                        foreach (var item in listImagesDiscountCode)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlDiscountCodeImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlDiscountCodeImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_DiscountCode images_DiscountCodes = new Images_DiscountCode();

                                images_DiscountCodes.Images = temp.fileName;
                                images_DiscountCodes.Order = item.Order;
                                images_DiscountCodes.DiscountCodeDetailPid = discountCodeDetail.Pid;
                                _dbContext.AddRange(images_DiscountCodes);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion 
                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = defaultData.Title;
                    logObj.Pid = discountCodeDetail.Pid;
                    logObj.Cate = DiscountCodeId;
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
        public dynamic Update(DiscountCodeDetail discountCodeDetail, List<MultiLang_DiscountCodeDetail> multiLangDiscountCodeDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesDiscountCode, List<Temp_MultiLang_Images> listLangImagesDiscountCode, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.DiscountCodeDetails.Where(p => p.Pid == discountCodeDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == discountCodeDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    var checkCode = _dbContext.DiscountCodeDetails.Where(x => x.Code == discountCodeDetail.Code && x.Code != model.Code).Count();
                    if (checkCode > 0)
                    {
                        return new { status = false, mess = "Mã này đã tồn tại" };
                    }
                    if(model.UsedQuantity > 0 && (model.DiscountCodeValue != discountCodeDetail.DiscountCodeValue || model.DiscountCodeType != discountCodeDetail.DiscountCodeType || model.Code != discountCodeDetail.Code))
                    {
                        return new { status = false, mess = "Không thể thay đổi giá trị của mã đã được sử dụng" };
                    }
                    model.PublishDate = discountCodeDetail.PublishDate;
                    model.Enabled = discountCodeDetail.Enabled;
                    model.TagKey = discountCodeDetail.TagKey;
                    model.StartDate = discountCodeDetail.StartDate;
                    model.EndDate = discountCodeDetail.EndDate;
                    model.Code = discountCodeDetail.Code;
                    model.MaxQuantity = discountCodeDetail.MaxQuantity;
                    model.MinTotal = discountCodeDetail.MinTotal;
                    model.DiscountCodeValue = discountCodeDetail.DiscountCodeValue;
                    model.DiscountCodeType = discountCodeDetail.DiscountCodeType;
                    model.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(discountCodeDetail.TagKey))
                    {
                        var arrTagKey = discountCodeDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        model.SlugTagKey = string.Join(",", tagkeys);
                    }
                    #region edit multi_lang
                    foreach (var item in multiLangDiscountCodeDetail)
                    {
                        var multiModel = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == discountCodeDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.DiscountCodeDetails
                                             join b in _dbContext.MultiLang_DiscountCodeDetails on a.Pid equals b.DiscountCodeDetailPid
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
                            var defaultData = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == discountCodeDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.DiscountCodeDetailPid = discountCodeDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.DiscountCodeDetails
                                             join b in _dbContext.MultiLang_DiscountCodeDetails on a.Pid equals b.DiscountCodeDetailPid
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
                            _dbContext.MultiLang_DiscountCodeDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlDiscountCodeImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlDiscountCodeImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlDiscountCodeImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlDiscountCodeImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlDiscountCodeImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlDiscountCodeImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }

                    #endregion

                    #region edit Cate
                    var listCateOld = _dbContext.DiscountCodeCate_DiscountCodeDetails.Where(x => x.DiscountCodeDetailPid == discountCodeDetail.Pid).ToList();
                    _dbContext.DiscountCodeCate_DiscountCodeDetails.RemoveRange(listCateOld);
                    _dbContext.SaveChanges();
                    string[] cateArray = { RootDiscountCodeCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        DiscountCodeCate_DiscountCodeDetail discountCodeCate_DiscountCodeDetail = new DiscountCodeCate_DiscountCodeDetail();
                        discountCodeCate_DiscountCodeDetail.DiscountCodeCatePid = Convert.ToInt32(item);
                        discountCodeCate_DiscountCodeDetail.DiscountCodeDetailPid = discountCodeDetail.Pid;
                        _dbContext.DiscountCodeCate_DiscountCodeDetails.Add(discountCodeCate_DiscountCodeDetail);
                    }
                    _dbContext.SaveChanges();
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_DiscountCodes.Where(p => p.DiscountCodeDetailPid == discountCodeDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_DiscountCodes.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlDiscountCodeImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlDiscountCodeImages, Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = new Random().Next(9000, 10000);
                    foreach (var item in listImagesDiscountCode)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlDiscountCodeImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlDiscountCodeImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_DiscountCode images_DiscountCodes = new Images_DiscountCode();

                                images_DiscountCodes.Images = temp.fileName;
                                images_DiscountCodes.Order = item.Order;
                                images_DiscountCodes.DiscountCodeDetailPid = discountCodeDetail.Pid;
                                _dbContext.Images_DiscountCodes.Add(images_DiscountCodes);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesDiscountCode.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_DiscountCode multiLang_Images_DiscountCodes = new MultiLang_Images_DiscountCode();
                                    multiLang_Images_DiscountCodes.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_DiscountCodes.ImagesDiscountCodePid = images_DiscountCodes.Pid;
                                    multiLang_Images_DiscountCodes.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_DiscountCodes);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesDiscountCode.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_DiscountCodes.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_DiscountCode multiLang_Images_DiscountCodes_edit = new MultiLang_Images_DiscountCode();

                                    multiLang_Images_DiscountCodes_edit.ImagesDiscountCodePid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_DiscountCodes_edit.Caption = tempItem.Caption;
                                    multiLang_Images_DiscountCodes_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_DiscountCodes.Add(multiLang_Images_DiscountCodes_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangDiscountCodeDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = discountCodeDetail.Pid;
                    logObj.Cate = DiscountCodeId;
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
                            var model = _dbContext.DiscountCodeDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.DiscountCodeDetails.Max(x => (int?)x.Order) ?? 1;

                            DiscountCodeDetail addDiscountCode = new DiscountCodeDetail();
                            addDiscountCode.Order = maxOrder + 1;
                            addDiscountCode.PicThumb = "";
                            addDiscountCode.Enabled = false;
                            addDiscountCode.PublishDate = DateTime.Now;
                            addDiscountCode.TagKey = model.TagKey;
                            addDiscountCode.Code = model.Code;
                            addDiscountCode.StartDate = model.StartDate;
                            addDiscountCode.EndDate = model.EndDate;
                            addDiscountCode.MaxQuantity = model.MaxQuantity;
                            addDiscountCode.MinTotal = model.MinTotal;
                            addDiscountCode.DiscountCodeValue = model.DiscountCodeValue;
                            addDiscountCode.DiscountCodeType = model.DiscountCodeType;
                            _dbContext.DiscountCodeDetails.Add(addDiscountCode);
                            _dbContext.SaveChanges();

                            //Add RootDiscountCodeCate
                            var discountCodeCate_DiscountCodeDetail = new DiscountCodeCate_DiscountCodeDetail();
                            discountCodeCate_DiscountCodeDetail.DiscountCodeCatePid = ConstantStrings.RootDiscountCodeCatePid;
                            discountCodeCate_DiscountCodeDetail.DiscountCodeDetailPid = addDiscountCode.Pid;
                            _dbContext.DiscountCodeCate_DiscountCodeDetails.Add(discountCodeCate_DiscountCodeDetail);
                            _dbContext.SaveChanges();

                            List<MultiLang_DiscountCodeDetail> detailModel = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.DiscountCodeDetailPid == model.Pid).ToList();
                            foreach (MultiLang_DiscountCodeDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_DiscountCodeDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_DiscountCodeDetail temp = new MultiLang_DiscountCodeDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.DiscountCodeDetailPid = addDiscountCode.Pid;
                                _dbContext.MultiLang_DiscountCodeDetails.Add(temp);
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
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == DiscountCodeId.ToString()).FirstOrDefault();
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
                    modulePreview.ModuleId = DiscountCodeId.ToString();
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
                var toRow = _dbContext.DiscountCodeDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.DiscountCodeDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.DiscountCodeDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.DiscountCodeDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.DiscountCodeDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                var data = _dbContext.DiscountCodeDetails.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var data = _dbContext.DiscountCodeDetails.Where(p => p.Pid == pid).FirstOrDefault();
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
        public string GetAutoCode()
        {
            string autoCode;

            do
            {
                autoCode = _common.GenerateRandomString(10);
            }
            while (_dbContext.DiscountCodeDetails.Any(x => x.Code == autoCode));

            return autoCode;
        }

    }
}
