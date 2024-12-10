using CMS.Areas.Contact.Models;
using DTO;
using DTO.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Contact
{
    public class EnquireListRepository : IEnquireListRepository
    {
        private readonly DBContext _dbContext;
        public EnquireListRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public dynamic ReadContact(long Pid)
        {
            try
            {
                string lang = ConstantStrings.DefaultLangAdmin;

                var model = _dbContext.EnquireLists.FirstOrDefault(p => p.Pid == Pid);
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
                List<dynamic> listData = new List<dynamic>();
                var model = _dbContext.EnquireLists.ToList();
                var data = (from a in _dbContext.EnquireLists
                            join b in _dbContext.FeatureDetails on a.ServiceDetailPid equals b.Pid
                            join c in _dbContext.MultiLang_FeatureDetails on b.Pid equals c.FeatureDetailPid
                            where (!a.isDeleted && !b.Deleted && b.Enabled && (a.isRead == Convert.ToBoolean(Convert.ToInt32(search.Enable)) || search.Enable.Equals(null)) && c.LangKey == ConstantStrings.DefaultLangAdmin)
                            orderby a.RecivedDate descending
                            select new
                            {
                                Pid = a.Pid,
                                FullName = a.FullNameEnquire,
                                PhoneNumber = a.PhoneEnquire,
                                Email = a.EmailEnquire,
                                Content = a.ContentEnquire,
                                FeatureName = c.Title,
                                RecivedDate = a.RecivedDate,
                                IsRead = a.isRead,
                                DateEnquire = a.DateEnquire
                            }).ToList().FilterSearch(new string[] { "FullNameEnquire", "PhoneEnquire", "EmailEnquire", "FeatureName" }, search.Key);
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Pid = item.Pid;
                    child.FullName = item.FullName;
                    child.PhoneNumber = item.PhoneNumber;
                    child.Email = item.Email;
                    child.Content = item.Content;
                    child.FeatureName = item.FeatureName;
                    child.RecivedDate = item.RecivedDate;
                    child.IsRead = item.IsRead;
                    child.DateEnquire = item.DateEnquire;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.RecivedDate).ToList(), search.Page, search.PageNumber);
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
                        var modal = _dbContext.EnquireLists.FirstOrDefault(p => p.Pid == item);
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
                        var modal = _dbContext.EnquireLists.FirstOrDefault(p => p.Pid == item);
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
                        var modal = _dbContext.EnquireLists.FirstOrDefault(p => p.Pid == item);
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
