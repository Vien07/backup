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
    public class CandidateRepository : ICandidateRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string UrlRecruitmentImages = ConstantStrings.UrlRecruitmentImages;
        private readonly DBContext _dbContext;
        IFileServices _fileServices;
        ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        public CandidateRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor,
                             IFileServices fileHelper, ICommonServices common)
        {
            _dbContext = dbContext;

            _httpContextAccessor = httpContextAccessor;

            _fileServices = fileHelper;
            _common = common;
            WatermarkActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkActive);
            WatermarkPicThumbActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkPicThumbActive);
        }

        public dynamic GetData(SearchDto search)
        {
            try
            {
                List<dynamic> listData = new List<dynamic>();
                var model = _dbContext.Candidates.ToList();
                var data = (from a in _dbContext.Candidates
                            join b in _dbContext.RecruitmentDetails on a.RecruitmentDetailPid equals b.Pid
                            join c in _dbContext.MultiLang_RecruitmentDetails on b.Pid equals c.RecruitmentDetailPid
                            where (!a.Deleted && !b.Deleted && b.Enabled && (a.IsRead == Convert.ToBoolean(Convert.ToInt32(search.Enable)) || search.Enable.Equals(null)) && c.LangKey == ConstantStrings.DefaultLangAdmin)
                            select new
                            {
                                Pid = a.Pid,
                                Enabled = a.Enabled,
                                FullName = a.FullName,
                                PhoneNumber = a.PhoneNumber,
                                Email = a.Email,
                                Content = a.Content,
                                Title = c.Title,
                                Title1 = a.Title,
                                CV = ConstantStrings.UrlRecruitmentCV + a.CV,
                                CreateDate = a.CreateDate,
                                IsRead = a.IsRead
                            }).ToList().FilterSearch(new string[] { "FullName", "PhoneNumber", "Email", "Title" }, search.Key);
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Pid = item.Pid;
                    child.Enabled = item.Enabled;
                    child.FullName = item.FullName;
                    child.PhoneNumber = item.PhoneNumber;
                    child.Email = item.Email;
                    child.Content = item.Content;
                    child.Title = item.Title;
                    child.Title1 = item.Title1;
                    child.CV = item.CV;
                    child.CreateDate = item.CreateDate;
                    child.IsRead = item.IsRead;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.CreateDate).ToList(), search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);

                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { jsData = rs, Paging = Paging };
            }
            catch (Exception ex)
            {
                return "[]";
            }
        }
        public dynamic ReadContact(long Pid)
        {
            try
            {
                var model = _dbContext.Candidates.FirstOrDefault(p => p.Pid == Pid);
                if (model.IsRead == false)
                {
                    model.IsRead = true;
                    _dbContext.SaveChanges();
                }
                return model;
            }
            catch (Exception ex)
            {

                return "[]";
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
                        var model = _dbContext.Candidates.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;
                            _dbContext.SaveChanges();
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
        public dynamic Seen(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var modal = _dbContext.Candidates.FirstOrDefault(p => p.Pid == item);
                        if (modal.IsRead == false)
                        {
                            modal.IsRead = true;
                            _dbContext.SaveChanges();
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
        public dynamic NotSeen(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var modal = _dbContext.Candidates.FirstOrDefault(p => p.Pid == item);
                        modal.IsRead = false;
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
    }
}
