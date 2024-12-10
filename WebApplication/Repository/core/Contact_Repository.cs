using CMS.Areas.Contact.Models;
using CMS.Services.CommonServices;
using DTO;
using DTO.Branch;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Repository
{
    public class Contact_Repository : IContact_Repository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;
        public Contact_Repository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, ICommonServices common)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
            _common = common;
        }
        public dynamic GetContactInfo(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = _dbContext.MultiLang_ContactInfos.Where(p => p.LangKey == lang).ToList();
                var model2 = _dbContext.ContactInfos.Where(p => p.isMultiLang == false).ToList();
                string json = "{";
                foreach (var item in model)
                {
                    var contactInfo = _dbContext.ContactInfos.Where(p => p.Pid == item.ContactInfoID).FirstOrDefault().Code;
                    json += "'" + contactInfo + "':'" + item.Value + "',";
                }
                foreach (var item in model2)
                {
                    json += "'" + item.Code + "':'" + item.Value + "',";
                }
                json = json + "}";
                dynamic data = JValue.Parse(json);
                return data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<BranchDto> GetBranchList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);

                var model = (from a in _dbContext.Branchs
                             join b in _dbContext.MultiLang_Branchs on a.Pid equals b.BranchPid
                             where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                             orderby a.Order descending
                             select new BranchDto
                             {
                                 Pid = a.Pid,
                                 Title = b.Title,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Address = b.Address,
                                 PhoneNumber = a.PhoneNumber,
                                 Map = a.Link,
                             }).ToList();

                return model;
            }
            catch (Exception)
            {
                return new List<BranchDto>();
            }
        }
        public bool SaveContact(ContactList data)
        {
            try
            {
                data.Content = _common.RemoveHtmlTag(data.Content);
                data.FullName = _common.RemoveHtmlTag(data.FullName);
                data.Email = _common.RemoveHtmlTag(data.Email);
                data.Subject = _common.RemoveHtmlTag(data.Subject);
                data.Phone = _common.RemoveHtmlTag(data.Phone);
                data.Address = _common.RemoveHtmlTag(data.Address);
                data.RecivedDate = DateTime.Now;
                _dbContext.ContactLists.Add(data);
                _dbContext.SaveChanges();
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public bool SaveEnquire(EnquireList data)
        {
            try
            {
                data.ContentEnquire = _common.RemoveHtmlTag(data.ContentEnquire);
                data.FullNameEnquire = _common.RemoveHtmlTag(data.FullNameEnquire);
                data.EmailEnquire = _common.RemoveHtmlTag(data.EmailEnquire);
                data.ServiceName = _common.RemoveHtmlTag(data.ServiceName);
                data.PhoneEnquire = _common.RemoveHtmlTag(data.PhoneEnquire);
                data.ServiceDetailPid = Convert.ToInt64(_common.RemoveHtmlTag(data.ServiceDetailPid.ToString()));
                data.DateEnquire = Convert.ToDateTime(_common.RemoveHtmlTag(data.DateEnquire.ToString()));
                data.RecivedDate = DateTime.Now;
                _dbContext.EnquireLists.Add(data);
                _dbContext.SaveChanges();
                return false;
            }
            catch (Exception ex)
            {

                return true;
            }
        }
    }
}
