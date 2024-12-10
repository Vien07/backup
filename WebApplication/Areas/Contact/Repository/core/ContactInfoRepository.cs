using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Contact.Models;
using System.Dynamic;

namespace CMS.Areas.Contact
{
    public class ContactInfoRepository: IContactInfoRepository
    {
        private readonly DBContext _dbContext; 
        public ContactInfoRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public dynamic Update(IFormCollection data, string langKey)
        {
            try
            {
                foreach (var item in data)
                {
                    try
                    {
                        var contactInfo = _dbContext.ContactInfos.Where(p => p.Code == item.Key).FirstOrDefault();
                        if (contactInfo != null && contactInfo.isMultiLang == true)
                        {
                            var model = _dbContext.MultiLang_ContactInfos.Where(p => p.ContactInfoID == contactInfo.Pid && p.LangKey==langKey).FirstOrDefault();
                            if (model != null)
                            {
                                model.Value = item.Value;
                                _dbContext.SaveChanges();

                            }
                            else
                            {
                                MultiLang_ContactInfo temp = new MultiLang_ContactInfo();
                                temp.ContactInfoID = contactInfo.Pid;
                                temp.Value = item.Value;
                                temp.LangKey = langKey;
                                _dbContext.MultiLang_ContactInfos.Add(temp);
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            var model = _dbContext.ContactInfos.Where(p => p.Code == item.Key).FirstOrDefault();
                            if (model != null)
                            {
                                model.Value = item.Value;
                                _dbContext.SaveChanges();

                            }
                            else
                            {
                                ContactInfo temp = new ContactInfo();
                                temp.Code = item.Key;
                                temp.Value = item.Value;
                                _dbContext.ContactInfos.Add(temp);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public string GetData(string langKey)
        {
            try
            {
                List<dynamic> listData = new List<dynamic>();
                var model = _dbContext.MultiLang_ContactInfos.Where(p => p.LangKey == langKey).ToList();
                var modelContactInfo = _dbContext.ContactInfos.Where(p => p.isMultiLang == false).ToList();
                if (modelContactInfo != null && modelContactInfo.Count > 0)
                {
                    foreach (var item in modelContactInfo)
                    {
                        dynamic child = new ExpandoObject();

                        child.Value = item.Value;
                        child.Key = item.Code;
                        child.Type = item.Type;
                        listData.Add(child);
                    }
                }

                if (model != null && model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        dynamic child = new ExpandoObject();
                        var temp = _dbContext.ContactInfos.Where(p => p.Pid == item.ContactInfoID).FirstOrDefault();

                        child.Value = item.Value;
                        child.Key = temp.Code;
                        child.Type = temp.Type;
                        listData.Add(child);
                    }
                }
                else
                {
                    listData = new List<dynamic>();
                    var tempModel = _dbContext.ContactInfos.ToList();
                    foreach (var item in tempModel)
                    {
                        dynamic child = new ExpandoObject();
                        child.Value = item.Value;
                        child.Key = item.Code;
                        child.Type = item.Type;
                        listData.Add(child);
                    }
                }
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return data;

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
    }
}
