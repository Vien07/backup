using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using X.PagedList;
using System.Globalization;
using CMS.Areas.Recruitment.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Recruitment;
using static CMS.Services.ExtensionServices;
using DTO.News;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repository
{
    public class Recruitment_Repository : IRecruitment_Repository
    {
        public string DateFormat = "";
        public string PageLimit = "";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;
        private readonly string UrlRecruitmentImages = ConstantStrings.UrlRecruitmentImages;
        private readonly string UrlRecruitmentCV = ConstantStrings.UrlRecruitmentCV;
        private string DefaultLang = ConstantStrings.DefaultLang;


        private readonly DBContext _dbContext;

        public Recruitment_Repository(DBContext dbContext, IHttpContextAccessor httpContextAccessor,
                                    ICommonServices common, IFileServices fileServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _common = common;
            _fileServices = fileServices;

            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
            PageLimit = _common.GetConfigValue(ConstantStrings.KeyPageLimit);
        }
        public dynamic GetList(string lang, int page, string key)
        {
            try
            {

                if (page < 1)
                {
                    page = 1;
                }
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);
                CultureInfo vi = new CultureInfo(lang);

                var model = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false && p.Enabled == true).OrderByDescending(p => p.Order).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in model)
                {
                    try
                    {
                        var detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.LangKey == lang && p.RecruitmentDetailPid == item.Pid && (p.Title.Contains(key) || string.IsNullOrEmpty(key))).FirstOrDefault();
                        //if (detail == null)
                        //{
                        //    detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.LangKey == DefaultLang && p.RecruitmentDetailPid == item.Pid && (p.Title.Contains(key) || string.IsNullOrEmpty(key))).FirstOrDefault();
                        //}
                        if (detail != null)
                        {
                            dynamic d = new ExpandoObject();
                            d.Pid = item.Pid;
                            d.Title = detail.Title;
                            d.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                            d.Content = detail.Content;
                            d.Description = detail.Description;
                            d.Amount = item.Amount;
                            d.Slug = detail.Slug;
                            d.PublishDate = item.PublishDate.ToString("d/M/yyyy");
                            d.PicThumb = UrlRecruitmentImages + ConstantStrings.Thumb + item.PicThumb;
                            d.ExpireDate = item.ExpireDate?.ToString("d/M/yyyy");
                            d.Salary = detail.Salary;
                            d.Type = detail.Type;
                            d.Exp = detail.Exp;
                            listData.Add(d);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData, page, string.IsNullOrEmpty(key) ? Convert.ToInt32(PageLimit) : Int16.MaxValue - 1);

                dynamic rs = new { list = dataPaging.ToList(), paging = new { currentPage = page, pageTotal = dataPaging.PageCount } };
                return rs;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }
        public dynamic GetListBySlug(string lang, int page, string cate)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);
                var catePid = GetPidCateBySlug(cate);
                //var listPidRecruitment = (from cateRecruitment in _dbContext.RecruitmentCate_RecruitmentDetails 
                //                   join Recruitment in _dbContext.RecruitmentDetails on cateRecruitment.RecruitmentCatePid equals Recruitment.Pid 
                //                   where cateRecruitment.Pid == catePid select Recruitment).ToList();
                //var listPidRecruitment = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentCatePid == catePid).ToList();
                var listPidRecruitment = (from cate_Recruitment in _dbContext.RecruitmentCate_RecruitmentDetails
                                          join Recruitment in _dbContext.MultiLang_RecruitmentDetails on cate_Recruitment.RecruitmentDetailPid equals Recruitment.RecruitmentDetailPid
                                          where cate_Recruitment.RecruitmentCatePid == catePid && Recruitment.LangKey == lang
                                          select Recruitment).DistinctBy(x => x.Pid).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in listPidRecruitment)
                {
                    try
                    {
                        var model = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false && p.Enabled == true && p.Pid == item.RecruitmentDetailPid).FirstOrDefault();
                        var detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.LangKey == lang && p.RecruitmentDetailPid == model.Pid).FirstOrDefault();
                        //if (detail == null)
                        //{
                        //    detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.LangKey == DefaultLang && p.RecruitmentDetailPid == model.Pid).FirstOrDefault();
                        //}
                        var images = _dbContext.Images_Recruitments.Where(p => p.RecruitmentDetailPid == item.RecruitmentDetailPid).FirstOrDefault();

                        dynamic d = new ExpandoObject();

                        d.Title = detail.Title;
                        d.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                        d.Order = model.Order;
                        d.Content = detail.Content;
                        d.Description = detail.Description;
                        d.Amount = model.Amount;
                        d.Slug = detail.Slug;
                        d.PicThumb = UrlRecruitmentImages + ConstantStrings.Thumb + model?.PicThumb;
                        d.Salary = item.Salary;
                        d.PublishDate = model.PublishDate.ToString("dd/MM/yyyy");
                        listData.Add(d);


                    }
                    catch (Exception ex)
                    {
                    }

                }
                listData = listData.OrderByDescending(p => p.Order).ToList();
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData, page, Convert.ToInt32(PageLimit));

                dynamic rs = new { list = dataPaging.ToList(), paging = new { currentPage = page, pageTotal = dataPaging.PageCount } };
                return rs;
            }
            catch (Exception ex)
            {

                return new List<dynamic>();
            }
        }
        public string RemoveHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public dynamic GetRecruitmentPreview(string detailJson, string listJson, string base64, string lang)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                CultureInfo vi = new CultureInfo(lang);
                RecruitmentDetail common = JsonConvert.DeserializeObject<RecruitmentDetail>(detailJson);
                List<MultiLang_RecruitmentDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentDetail>>(listJson);
                var detail = objList.Where(p => p.LangKey == lang).FirstOrDefault();
                dynamic Recruitment = new ExpandoObject();
                Recruitment.Title = detail.Title;
                Recruitment.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                Recruitment.Enabled = false;
                Recruitment.Content = detail.Content;
                Recruitment.Description = detail.Description;
                Recruitment.Pid = detail.Pid;
                if (base64 != "")
                {
                    Recruitment.PicThumb = ConstantStrings.formatBase64 + _fileServices.WatermarkImage(base64);

                }
                else
                {
                    string tempImages = common.PicThumb;
                    var tempArrImages = tempImages.Split("/");
                    var nameImages = tempArrImages[tempArrImages.Length - 1];
                    Recruitment.PicThumb = ConstantStrings.formatBase64 + _fileServices.WatermarkImage(UrlRecruitmentImages, ConstantStrings.Fullmages + nameImages);

                }

                Recruitment.Images = UrlRecruitmentImages + ConstantStrings.Fullmages + common.PicThumb;
                Recruitment.PublishDate = common.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi);
                Recruitment.Slug = detail.Slug;
                Recruitment.TagKey = common.TagKey;
                return Recruitment;
            }
            catch (Exception ex)
            {

                return new ExpandoObject();
            }
        }
        public dynamic GetRecruitment(string slug, string lang)
        {
            dynamic Recruitment = new ExpandoObject();

            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                CultureInfo vi = new CultureInfo(lang);

                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);
                var detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.Slug == slug).FirstOrDefault();
                var common = _dbContext.RecruitmentDetails.Where(p => p.Pid == detail.RecruitmentDetailPid && p.Enabled == true && p.Deleted == false).FirstOrDefault();

                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                Recruitment.Title = detail.Title;
                Recruitment.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                Recruitment.Enabled = common.Enabled;
                Recruitment.Content = detail.Content;
                Recruitment.Description = detail.Description;
                Recruitment.Amount = common.Amount;
                Recruitment.Pid = common.Pid;
                //if (WatermarkActive == "on")
                //{
                //    Recruitment.PicThumb = _fileServices.CreateWatermarkImage(UrlRecruitmentImages, UrlRecruitmentImagesWatermark, common.PicThumb);
                //}
                //else
                //{
                //    Recruitment.PicThumb = UrlRecruitmentImages + Fullmages + common.PicThumb;
                //}
                Recruitment.Images = UrlRecruitmentImages + ConstantStrings.Fullmages + common.PicThumb;
                //Recruitment.PublishDate = common.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm", vi);
                Recruitment.PublishDate = common.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi);
                Recruitment.Slug = detail.Slug;
                Recruitment.TagKey = common.TagKey;
                Recruitment.Location = detail.Location;
                Recruitment.ExpireDate = common.ExpireDate.Value.ToString("dd/MM/yyyy");
                return Recruitment;
            }
            catch (Exception ex)
            {

                return Recruitment;
            }
        }
        public List<dynamic> GetRelateList(string slug, string lang)
        {
            try
            {

                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false && p.Enabled == true && p.PublishDate <= DateTime.Now)
                                                  .OrderByDescending(p => p.PublishDate).Take(4).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in model)
                {
                    var detail = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.LangKey == lang && p.RecruitmentDetailPid == item.Pid && p.Slug != slug).FirstOrDefault();
                    if (detail != null)
                    {
                        dynamic d = new ExpandoObject();
                        d.Title = detail.Title;
                        d.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                        d.Content = detail.Content;
                        d.Description = detail.Description;
                        d.Amount = item.Amount;
                        d.Slug = detail.Slug;
                        d.PublishDate = item.PublishDate.ToShortDateString();
                        d.ExpireDate = item.ExpireDate?.ToString("MMMM d, yyyy");
                        d.Type = detail.Type;
                        d.PicThumb = UrlRecruitmentImages + ConstantStrings.Thumb + item.PicThumb;
                        listData.Add(d);
                    }
                }
                return listData;
            }
            catch (Exception ex)
            {
                return new List<dynamic>();
            }
        }
        public dynamic GetCate(string slug, string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = _dbContext.RecruitmentCates.Where(p => p.Deleted == false && p.Enabled == true).OrderByDescending(p => p.Order);
                //var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
                //if (role != null)
                //{
                //    model = _dbContext.RecruitmentCates.Where(p => p.Deleted == false).OrderByDescending(p => p.Order);
                //}
                var detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == lang && p.Slug == slug).FirstOrDefault();
                //if (detail == null)
                //{
                //    detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == DefaultLang && p.Slug == slug).FirstOrDefault();
                //}

                dynamic d = new ExpandoObject();
                d.Title = detail.Name;
                d.TitleAlt = detail.Name.Replace("\"", "").Replace("'", "");
                d.Slug = detail.Slug;
                d.Description = detail.Description;

                return d;
            }
            catch (Exception ex)
            {

                return new List<dynamic>();
            }
        }
        public dynamic GetCate(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = _dbContext.RecruitmentCates.Where(p => p.Deleted == false && p.Enabled == true && p.isLocked == false).OrderByDescending(p => p.Order).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in model)
                {
                    try
                    {
                        var detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == lang && p.RecruitmentCatePid == item.Pid).FirstOrDefault();
                        //if (detail == null)
                        //{
                        //    detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == DefaultLang && p.RecruitmentCatePid == item.Pid).FirstOrDefault();
                        //}

                        dynamic d = new ExpandoObject();
                        d.Title = detail.Name;
                        d.TitleAlt = detail.Name.Replace("\"", "").Replace("'", "");
                        d.Pid = item.Pid;
                        d.Slug = detail.Slug;
                        d.Count = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentCatePid == item.Pid).Count();
                        listData.Add(d);
                    }
                    catch (Exception ex)
                    {
                    }

                }
                return listData;
            }
            catch (Exception ex)
            {

                return new List<dynamic>();
            }
        }
        public List<NewsCateDto> GetCateNews(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                var model = (from a in _dbContext.NewsCates
                             join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                             where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                             orderby a.Order descending
                             select new NewsCateDto
                             {
                                 Title = b.Name,
                                 TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                 Slug = b.Slug,
                                 Pid = a.Pid,
                                 Description = b.Description,
                             }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsCateDto>();
            }
        }
        public long GetPidCateBySlug(string slug)
        {
            try
            {


                var detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.Slug == slug).FirstOrDefault();
                //if (detail == null)
                //{
                //    detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == DefaultLang && p.Slug == slug).FirstOrDefault();
                //}
                return detail.RecruitmentCatePid; ;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        public dynamic GetCateByRecruitmentPid(long pid, string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);
                var temp = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == pid).FirstOrDefault();
                var model = _dbContext.RecruitmentCates.Where(p => p.Pid == temp.RecruitmentCatePid && p.isLocked == false).FirstOrDefault();
                var detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == lang && p.RecruitmentCatePid == temp.RecruitmentCatePid).FirstOrDefault();
                //if (detail == null)
                //{
                //    detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == DefaultLang && p.RecruitmentCatePid == temp.RecruitmentCatePid).FirstOrDefault();
                //}
                dynamic d = new ExpandoObject();
                d.Title = detail.Name;
                d.TitleAlt = detail.Name.Replace("\"", "").Replace("'", "");
                d.Slug = detail.Slug;
                dynamic rs = d;
                return rs;
            }
            catch (Exception ex)
            {

                return new List<dynamic>();
            }
        }
        public dynamic GetCateByPid(long pid, string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = _dbContext.RecruitmentCates.Where(p => p.Pid == pid).FirstOrDefault();
                var detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == lang && p.RecruitmentCatePid == pid).FirstOrDefault();
                //if (detail == null)
                //{
                //    detail = _dbContext.MultiLang_RecruitmentCates.Where(p => p.LangKey == DefaultLang && p.RecruitmentCatePid == pid).FirstOrDefault();
                //}
                dynamic d = new ExpandoObject();
                d.Title = detail.Name;
                d.TitleAlt = detail.Name.Replace("\"", "").Replace("'", "");
                d.Slug = detail.Slug;
                dynamic rs = d;
                return rs;
            }
            catch (Exception ex)
            {

                return new List<dynamic>();
            }
        }
        public bool SendCV(CVDto model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Candidate candidate = new Candidate();
                    candidate.FullName = model.FullName;
                    candidate.Email = model.Email;
                    candidate.PhoneNumber = model.PhoneNumber;
                    candidate.Title = model.Title;
                    candidate.Content = model.Content;
                    candidate.CreateDate = DateTime.Now;
                    candidate.RecruitmentDetailPid = model.RecruitmentDetailPid;
                    if (model.File != null)
                    {
                        var absolutepath = Directory.GetCurrentDirectory();
                        var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        var fileName = id + "_" + model.File.FileName;
                        var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + UrlRecruitmentCV, fileName);
                        using (var srv = new FileStream(filePath, FileMode.Create))
                        {
                            model.File.CopyTo(srv);
                        }
                        candidate.CV = fileName;
                    }

                    _dbContext.Candidates.Add(candidate);
                    _dbContext.SaveChanges();
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


        public async Task<RecruitmentDto> GetRecruit(string slug, string lang)
        {

            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                CultureInfo vi = new CultureInfo(lang);

                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                var model = await (from a in _dbContext.RecruitmentDetails
                                   join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug)
                                   select new RecruitmentDto
                                   {
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Enabled = a.Enabled,
                                       Slug = b.Slug,
                                       Amount = a.Amount,
                                       Pid = a.Pid,
                                       PicThumb = UrlRecruitmentImages + ConstantStrings.Thumb + a.PicThumb,
                                       PicFull = UrlRecruitmentImages + ConstantStrings.Fullmages + a.PicThumb,
                                       PublicDate = a.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi),
                                       TagKey = a.TagKey,
                                       Location = b.Location,
                                       ExpiredDate = a.ExpireDate.Value.ToString(_common.ConvertFormatToCultureDateTime(DateFormat)),
                                       Type = b.Type,
                                       Exp = b.Exp,
                                       Salary = b.Salary
                                   }).FirstOrDefaultAsync();


                var common = await _dbContext.RecruitmentDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                return model;

            }
            catch (Exception ex)
            {
                return null;
            }
        }





        public List<NewsDto> GetListHotAndNew(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                CultureInfo vi = new CultureInfo(lang);

                var model = (from a in _dbContext.NewsDetails
                             join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                             where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                             orderby a.Order descending
                             select new NewsDto
                             {
                                 Title = b.Title,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Content = b.Content,
                                 Description = b.Description,
                                 Slug = b.Slug,
                                 PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                 PicThumb = ConstantStrings.UrlNewsImages + ConstantStrings.Thumb + a.PicThumb,
                                 PicFull = ConstantStrings.UrlNewsImages + ConstantStrings.Fullmages + a.PicThumb
                             }).Take(3).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }

        public Dictionary<string, RecruitmentDto> GetPreNextRecruit(string slug, string lang)
        {
            try
            {

                if (lang == null)
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);


                var order = (from a in _dbContext.RecruitmentDetails
                             join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
                             where b.Slug == slug
                             select a.Order).FirstOrDefault();

                var listNews = new Dictionary<string, RecruitmentDto>();
                var preNews = (from a in _dbContext.RecruitmentDetails
                               join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
                               orderby a.Order descending
                               where (!a.Deleted && a.Enabled) && (b.LangKey == lang) && a.Order < order
                               orderby a.Order descending
                               select new RecruitmentDto
                               {
                                   Order = a.Order,
                                   Slug = b.Slug,
                                   Title = b.Title,
                                   TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                   Description = b.Description

                               }).FirstOrDefault();
                if (preNews != null)
                {
                    listNews.Add("pre", preNews);
                }
                else
                {
                    listNews.Add("pre", null);
                }
                var nextNews = (from a in _dbContext.RecruitmentDetails
                                join b in _dbContext.MultiLang_RecruitmentDetails on a.Pid equals b.RecruitmentDetailPid
                                where (!a.Deleted && a.Enabled) && (b.LangKey == lang) && a.Order > order
                                orderby a.Order ascending
                                select new RecruitmentDto
                                {
                                    Order = a.Order,
                                    Slug = b.Slug,
                                    Title = b.Title,
                                    TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                    Description = b.Description
                                }).FirstOrDefault();
                if (nextNews != null)
                {
                    listNews.Add("next", nextNews);
                }
                else
                {
                    listNews.Add("next", null);
                }
                return listNews;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool checkRecruitIsExist(string lang)
        {
            try
            {
                var model = _dbContext.RecruitmentDetails.FirstOrDefault();
                if (model != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
