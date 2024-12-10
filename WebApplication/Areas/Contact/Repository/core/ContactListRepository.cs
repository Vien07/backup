using CMS.Areas.Contact.Models;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Contact
{
    public class ContactListRepository : IContactListRepository
    {
        private readonly DBContext _dbContext;
        public ContactListRepository(DBContext dbContext)
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
                        if (contactInfo.isMultiLang == true)
                        {
                            var model = _dbContext.MultiLang_ContactInfos.Where(p => p.Pid == contactInfo.Pid && p.LangKey == langKey).FirstOrDefault();
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
        public dynamic ReadContact(long Pid)
        {
            try
            {
                var model = _dbContext.ContactLists.FirstOrDefault(p => p.Pid == Pid);
                if (model.isRead == false)
                {
                    model.isRead = true;
                    model.ReadDate = DateTime.Now;
                    _dbContext.SaveChanges();
                }
                return model;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic GetData(SearchDto search)
        {
            try
            {
                var data = _dbContext.ContactLists.Where(p => (p.isRead.Equals(search.Enable) || search.Enable.Equals(null)) &&
                                                    p.isDeleted != true).OrderByDescending(x => x.RecivedDate).ToList().FilterSearch(new string[] { "Email", "FullName", "Subject", "Content" }, search.Key);
                PagedList<ContactList> dataPaging = new PagedList<ContactList>(data, search.Page, search.PageNumber);
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
        public dynamic Delete(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var modal = _dbContext.ContactLists.FirstOrDefault(p => p.Pid == item);
                        modal.isDeleted = true;
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
        public dynamic Seen(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var modal = _dbContext.ContactLists.FirstOrDefault(p => p.Pid == item);
                        if (modal.isRead == false)
                        {
                            modal.isRead = true;
                            modal.ReadDate = DateTime.Now;
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
                        var modal = _dbContext.ContactLists.FirstOrDefault(p => p.Pid == item);
                        modal.isRead = false;
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
