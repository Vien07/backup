
using CMS.Areas.Recruitment.Models;
using CMS.Services;
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

namespace CMS.Areas.Recruitment
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string UrlRecruitmentImages = ConstantStrings.UrlRecruitmentImages;
        private readonly DBContext _dbContext;
        IFileServices _fileServices;
        ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        public RecruitmentRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor,
                             IFileServices fileHelper, ICommonServices common)
        {
            _dbContext = dbContext;

            _httpContextAccessor = httpContextAccessor;

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
                var data = (from a in _dbContext.RecruitmentDetails
                            join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
                            where (a.Enabled == search.Enable || search.Enable == null)
                                                    && a.Deleted == false && b.LangKey == ConstantStrings.DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                CounterView = a.CounterView,
                                PublishDate = a.PublishDate,
                                ExpireDate = a.ExpireDate,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                PicThumb = a.PicThumb,
                                Title = b.Title,
                                Slug = b.Slug,
                                Description = b.Description,
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Title = item.Title;
                    child.Slug = item.Slug;
                    child.CounterView = item.CounterView;
                    child.PublishDate = item.PublishDate;
                    child.ExpireDate = item.ExpireDate;
                    child.PicThumb = UrlRecruitmentImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
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
                        var model = _dbContext.RecruitmentDetails.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.RecruitmentDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                var deleteImage = _dbContext.Images_Recruitments.Where(p => p.RecruitmentDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlRecruitmentImages, item2.Images);

                }
                _fileServices.DeleteFile(UrlRecruitmentImages, ConstantStrings.Fullmages + model.PicThumb);

                _fileServices.DeleteFile(UrlRecruitmentImages, model.PicThumb);
                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == Pid && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = ConstantStrings.RecruitmentId;
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
                        var model = _dbContext.RecruitmentDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;

                            _fileServices.DeleteFile(UrlRecruitmentImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlRecruitmentImages, ConstantStrings.Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_Recruitments.Where(p => p.RecruitmentDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlRecruitmentImages, item2.Images);

                            }
                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == model.Pid && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault().Title;
                            logObj.Pid = model.Pid;
                            logObj.Cate = ConstantStrings.RecruitmentId;
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

                var data = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == Pid).ToList();
                var detail = _dbContext.RecruitmentDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_Recruitments.Where(p => p.RecruitmentDetailPid == Pid).ToList();

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();

                var getListCates = _dbContext.RecruitmentCate_RecruitmentDetails.Where(x => x.RecruitmentDetailPid == Pid).Select(x => x.RecruitmentCatePid).ToList();
                List<dynamic> listCates = new List<dynamic>();
                if (getListCates!.Any())
                {
                    foreach (var item in getListCates)
                    {
                        listCates.Add(item);
                    }
                }
                foreach (var item in data)
                {
                    var recruitment = _dbContext.RecruitmentDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (recruitment.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.WorkPlace = item.WorkPlace;
                        child.Rank = item.Rank;
                        child.Job = item.Job;
                        child.PicThumb = UrlRecruitmentImages + recruitment.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Location = item.Location;
                        child.Salary = item.Salary;
                        child.Type = item.Type;
                        child.Exp = item.Exp;
                        child.Content = item.Content;
                        child.Pid = item.Pid;
                        child.RecruitmentDetailPid = item.RecruitmentDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_Recruitments.Where(p => p.ImagesRecruitmentPid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlRecruitmentImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesRecruitmentPid;
                        listImages_lang.Add(child2);
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
                        detail.Amount,
                        detail.ExpireDate,
                        PicThumb = UrlRecruitmentImages + detail.PicThumb
                    },
                    list = listData,
                    images = listImages,
                    listCates = listCates,
                    images_lang = listImages_lang

                };

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }

        public dynamic Insert(RecruitmentDetail recruitmentDetail, List<MultiLang_RecruitmentDetail> multiLangRecruitmentDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesRecruitment,
                            List<Temp_MultiLang_Images> listLangImagesRecruitment, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangRecruitmentDetail.Where(p => p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    string nameImages = multiLangRecruitmentDetail.Where(p => p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault().Title;

                    int maxOrder = _dbContext.RecruitmentDetails.Max(x => (int?)x.Order) ?? 1;
                    recruitmentDetail.Order = maxOrder + 1;
                    _dbContext.RecruitmentDetails.Add(recruitmentDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    //string[] cateArray = { ConstantStrings.RootRecruitmentCatePid.ToString() };
                    //if (listCates != null)
                    //{
                    //    cateArray = listCates.Split(',');
                    //}
                    //foreach (var item in cateArray)
                    //{
                    //    RecruitmentCate_RecruitmentDetail recruitmentCate_recruitmentDetail = new RecruitmentCate_RecruitmentDetail();
                    //    recruitmentCate_recruitmentDetail.RecruitmentCatePid = Convert.ToInt32(item);
                    //    recruitmentCate_recruitmentDetail.RecruitmentDetailPid = recruitmentDetail.Pid;
                    //    _dbContext.RecruitmentCate_RecruitmentDetails.Add(recruitmentCate_recruitmentDetail);
                    //}
                    //_dbContext.SaveChanges();
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangRecruitmentDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.RecruitmentDetailPid = recruitmentDetail.Pid;
                        if (item.LangKey != ConstantStrings.DefaultLang)
                        {
                            item.Title = title;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Location = string.IsNullOrEmpty(item.Location) ? defaultData.Location : item.Location;
                            item.Salary = string.IsNullOrEmpty(item.Salary) ? defaultData.Salary : item.Salary;
                            item.Type = string.IsNullOrEmpty(item.Type) ? defaultData.Type : item.Type;
                            item.Exp = string.IsNullOrEmpty(item.Exp) ? defaultData.Exp : item.Exp;
                            item.Rank = string.IsNullOrEmpty(item.Rank) ? defaultData.Rank : item.Rank;
                            item.Job = string.IsNullOrEmpty(item.Job) ? defaultData.Job : item.Job;
                            item.WorkPlace = string.IsNullOrEmpty(item.WorkPlace) ? defaultData.WorkPlace : item.WorkPlace;
                        }

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.RecruitmentDetails
                                         join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
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
                        _dbContext.MultiLang_RecruitmentDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlRecruitmentImages, nameImages + "-" + recruitmentDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlRecruitmentImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlRecruitmentImages, saveFileStatus.fileName);
                                }
                                recruitmentDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlRecruitmentImages, nameImages + "-" + recruitmentDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlRecruitmentImages, saveFileStatus.fileName);
                                recruitmentDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = 0;
                    if (listImagesRecruitment != null)
                    {
                        foreach (var item in listImagesRecruitment)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlRecruitmentImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlRecruitmentImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Recruitment images_Recruitmentes = new Images_Recruitment();

                                images_Recruitmentes.Images = temp.fileName;
                                images_Recruitmentes.Order = item.Order;
                                images_Recruitmentes.RecruitmentDetailPid = recruitmentDetail.Pid;
                                _dbContext.AddRange(images_Recruitmentes);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion 
                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = defaultData.Title;
                    logObj.Pid = recruitmentDetail.Pid;
                    logObj.Cate = ConstantStrings.RecruitmentId;
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


        public dynamic Update(RecruitmentDetail recruitmentDetail, List<MultiLang_RecruitmentDetail> multiLangRecruitmentDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesRecruitment, List<Temp_MultiLang_Images> listLangImagesRecruitment, string listCates)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.RecruitmentDetails.Where(p => p.Pid == recruitmentDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == recruitmentDetail.Pid && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    model.PublishDate = recruitmentDetail.PublishDate;
                    model.Enabled = recruitmentDetail.Enabled;
                    model.TagKey = recruitmentDetail.TagKey;
                    model.ExpireDate = recruitmentDetail.ExpireDate;
                    model.Amount = recruitmentDetail.Amount;
                    model.UpdateDate = DateTime.Now;
                    #region edit multi_lang
                    foreach (var item in multiLangRecruitmentDetail)
                    {
                        var multiModel = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == recruitmentDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.RecruitmentDetails
                                             join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
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
                            multiModel.TitleWithoutSign = _common.RemoveSign4VietnameseString(item.Title);
                            multiModel.Content = item.Content;
                            multiModel.Location = item.Location;
                            multiModel.Salary = item.Salary;
                            multiModel.Description = _common.RemoveHtmlTag(item.Description);
                            multiModel.Type = item.Type;
                            multiModel.Exp = item.Exp;
                            multiModel.Rank = item.Rank;
                            multiModel.Job = item.Job;
                            multiModel.WorkPlace = item.WorkPlace;
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == recruitmentDetail.Pid && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.TitleWithoutSign = _common.RemoveSign4VietnameseString(title);
                            item.RecruitmentDetailPid = recruitmentDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Location = string.IsNullOrEmpty(item.Location) ? defaultData.Location : item.Location;
                            item.Salary = string.IsNullOrEmpty(item.Salary) ? defaultData.Salary : item.Salary;
                            item.Type = string.IsNullOrEmpty(item.Type) ? defaultData.Type : item.Type;
                            item.Exp = string.IsNullOrEmpty(item.Exp) ? defaultData.Exp : item.Exp;
                            item.Job = string.IsNullOrEmpty(item.Job) ? defaultData.Job : item.Job;
                            item.Rank = string.IsNullOrEmpty(item.Rank) ? defaultData.Rank : item.Rank;
                            item.WorkPlace = string.IsNullOrEmpty(item.WorkPlace) ? defaultData.WorkPlace : item.WorkPlace;
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.RecruitmentDetails
                                             join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
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
                            _dbContext.MultiLang_RecruitmentDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlRecruitmentImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlRecruitmentImages, ConstantStrings.Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlRecruitmentImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlRecruitmentImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlRecruitmentImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlRecruitmentImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlRecruitmentImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }

                    #endregion
                    #region edit Cate
                    //var listCateOld = _dbContext.RecruitmentCate_RecruitmentDetails.Where(x => x.RecruitmentCatePid == model.Pid).ToList();
                    //_dbContext.RecruitmentCate_RecruitmentDetails.RemoveRange(listCateOld);
                    //_dbContext.SaveChanges();
                    //string[] cateArray = {ConstantStrings.RootRecruitmentCatePid.ToString() };
                    //if (listCates != null)
                    //{
                    //    cateArray = listCates.Split(',');
                    //}
                    //foreach (var item in cateArray)
                    //{
                    //    RecruitmentCate_RecruitmentDetail recruitmentCate_RecruitmentDetail = new RecruitmentCate_RecruitmentDetail();
                    //    recruitmentCate_RecruitmentDetail.RecruitmentCatePid = Convert.ToInt32(item);
                    //    recruitmentCate_RecruitmentDetail.RecruitmentDetailPid = recruitmentDetail.Pid;
                    //    _dbContext.RecruitmentCate_RecruitmentDetails.Add(recruitmentCate_RecruitmentDetail);
                    //}
                    //_dbContext.SaveChanges();
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_Recruitmentes.Where(p => p.RecruitmentDetailPid == recruitmentDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_Recruitments.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlRecruitmentImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlRecruitmentImages, ConstantStrings.Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = 0;
                    foreach (var item in listImagesRecruitment)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlRecruitmentImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlRecruitmentImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Recruitment images_Recruitmentes = new Images_Recruitment();

                                images_Recruitmentes.Images = temp.fileName;
                                images_Recruitmentes.Order = item.Order;
                                images_Recruitmentes.RecruitmentDetailPid = recruitmentDetail.Pid;
                                _dbContext.Images_Recruitments.Add(images_Recruitmentes);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesRecruitment.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_Recruitment multiLang_Images_Recruitmentes = new MultiLang_Images_Recruitment();
                                    multiLang_Images_Recruitmentes.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_Recruitmentes.ImagesRecruitmentPid = images_Recruitmentes.Pid;
                                    multiLang_Images_Recruitmentes.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_Recruitmentes);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesRecruitment.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_Recruitments.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_Recruitment multiLang_Images_Recruitmentes_edit = new MultiLang_Images_Recruitment();

                                    multiLang_Images_Recruitmentes_edit.ImagesRecruitmentPid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_Recruitmentes_edit.Caption = tempItem.Caption;
                                    multiLang_Images_Recruitmentes_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_Recruitments.Add(multiLang_Images_Recruitmentes_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangRecruitmentDetail.Where(p => p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = recruitmentDetail.Pid;
                    logObj.Cate = ConstantStrings.RecruitmentId;
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
                            var model = _dbContext.RecruitmentDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.RecruitmentDetails.Max(x => (int?)x.Order) ?? 1;

                            RecruitmentDetail addRecruitment = new RecruitmentDetail();
                            addRecruitment.Order = maxOrder + 1;
                            addRecruitment.PicThumb = "";
                            addRecruitment.Enabled = false;
                            addRecruitment.PublishDate = DateTime.Now;
                            addRecruitment.TagKey = model.TagKey;
                            _dbContext.RecruitmentDetails.Add(addRecruitment);
                            _dbContext.SaveChanges();
                            List<MultiLang_RecruitmentDetail> detailModel = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == model.Pid).ToList();
                            foreach (MultiLang_RecruitmentDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_RecruitmentDetail temp = new MultiLang_RecruitmentDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.RecruitmentDetailPid = addRecruitment.Pid;
                                _dbContext.MultiLang_RecruitmentDetails.Add(temp);
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
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.RecruitmentDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.RecruitmentDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.RecruitmentDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.RecruitmentDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                var data = _dbContext.RecruitmentDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
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
