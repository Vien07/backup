using CMS.Areas.Configurations.Models;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;

namespace CMS.Areas.Configurations
{
    public class GeneralConfigurationRepository : IGeneralConfigurationRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private string DefaultLang = ConstantStrings.DefaultLangAdmin;
        public GeneralConfigurationRepository(DBContext dbContext, IFileServices fileServices)
        {
            _dbContext = dbContext;
            _fileServices = fileServices;
        }
        public dynamic Update(Configuration data, int type)
        {
            try
            {
                //type = -1  when input type=file have value
                //type = 1 when when input type=file dont't have value
                var model = _dbContext.Configurations.Where(p => p.Key == data.Key).FirstOrDefault();
                if (model == null)
                {
                    data.Group = "Extend";
                    _dbContext.Configurations.Add(data);
                    _dbContext.SaveChanges();
                    return new { Error = true, Name = "" };
                }
                else
                {
                    if (type == -1)
                    {
                        //_fileServices.DeleteFile(ConstantStrings.UrlConfigurationImages, model.Value);
                        model.Value = data.Value;
                        _dbContext.SaveChanges();
                        return new { Error = true, Name = "" };
                    }
                    else if (type == 1)
                    {
                        if (model.Type != "file")
                        {
                            model.Value = data.Value;
                            _dbContext.SaveChanges();
                            return new { Error = true, Name = "" };
                        }
                    }
                }
                return new { Error = true, Name = "" };
            }
            catch (Exception ex)
            {
                return new { Error = false, Name = data.NameKey };
            }
        }
        public dynamic Update(Configuration data)
        {
            //type = -1  when input type=file have value
            //type = 1 when when input type=file dont't have value
            var model = _dbContext.Configurations.Where(p => p.Key == data.Key).FirstOrDefault();
            if (model == null)
            {
                data.Group = "Extend";
                _dbContext.Configurations.Add(data);
                _dbContext.SaveChanges();
                return new { Error = true, Name = "" };
            }
            else
            {
                try
                {
                    model.Value = data.Value;
                    _dbContext.SaveChanges();
                    return new { Error = true, Name = "" };
                }
                catch (Exception ex)
                {

                    return new { Error = false, Name = model.NameKey };
                }
            }
        }
        public string GetList()
        {
            try
            {
                var model = _dbContext.Configurations.ToList();
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                return data;

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }

        #region Email template
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                var arrKey = new string[] { };
                if (!string.IsNullOrEmpty(search.Key))
                {
                    arrKey = search.Key.Split(";");
                }
                int cate = search.Cate != null ? Convert.ToInt32(search.Cate) : 0;
                List<dynamic> listData = new List<dynamic>();
                var data = (from a in _dbContext.EmailTemplates
                            join b in _dbContext.MultiLang_EmailTemplates on a.Pid equals b.EmailTemplatePid
                            where a.Enabled && (arrKey.Contains(a.Group) || string.IsNullOrEmpty(search.Key))
                                                     && b.LangKey == DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                Title = a.Title,
                            }).ToList();

                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Title = item.Title;
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
                        var model = _dbContext.EmailTemplates.Where(p => p.Pid == item).FirstOrDefault();
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

                var data = _dbContext.MultiLang_EmailTemplates.Where(p => p.EmailTemplatePid == Pid).ToList();
                var detail = _dbContext.EmailTemplates.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Subject = item.Subject;
                    child.FromName = item.FromName;
                    child.LangKey = item.LangKey;
                    child.Content = item.Content;
                    child.Pid = item.Pid;
                    child.EmailTemplatePid = item.EmailTemplatePid;
                    listData.Add(child);

                }
                var emailkey = _dbContext.EmailTempateVariables.ToList();
                List<dynamic> tempEmailkey = new List<dynamic>();
                tempEmailkey.Add(new { Group = detail.Group, ListKey = emailkey.Where(p => p.Group == detail.Group).ToList() });

                return new
                {
                    detail = new
                    {
                        detail.Enabled,
                        detail.Title,
                        detail.Pid,
                    },
                    list = listData,
                    emailKey = tempEmailkey

                };

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(Models.EmailTemplate newsDetail, List<MultiLang_EmailTemplate> multiLangEmailTemplate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangEmailTemplate.Where(p => p.LangKey == DefaultLang).FirstOrDefault();

                    int maxOrder = _dbContext.EmailTemplates.Max(x => (int?)x.Order) ?? 1;
                    newsDetail.Order = maxOrder + 1;
                    newsDetail.Title = newsDetail.Title;
                    _dbContext.EmailTemplates.Add(newsDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                    #region save multi lang
                    foreach (var item in multiLangEmailTemplate)
                    {
                        item.Pid = 0;
                        item.EmailTemplatePid = newsDetail.Pid;
                        if (item.LangKey != DefaultLang)
                        {
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                        }
                        _dbContext.MultiLang_EmailTemplates.Add(item);
                        _dbContext.SaveChanges();
                    }
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
        public dynamic Update(Models.EmailTemplate newsDetail, List<MultiLang_EmailTemplate> multiLangEmailTemplate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.EmailTemplates.Where(p => p.Pid == newsDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_EmailTemplates.Where(p => p.EmailTemplatePid == newsDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    model.Enabled = newsDetail.Enabled;
                    model.Title = newsDetail.Title;
                    #region edit multi_lang
                    foreach (var item in multiLangEmailTemplate)
                    {
                        var multiModel = _dbContext.MultiLang_EmailTemplates.Where(p => p.EmailTemplatePid == newsDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Content = item.Content;
                            multiModel.Subject = item.Subject;
                            multiModel.FromName = item.FromName;
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_EmailTemplates.Where(p => p.EmailTemplatePid == newsDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            item.EmailTemplatePid = newsDetail.Pid;
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.FromName = string.IsNullOrEmpty(item.FromName) ? defaultData.FromName : item.FromName;
                            item.Subject = string.IsNullOrEmpty(item.Subject) ? defaultData.Subject : item.Subject;
                            _dbContext.MultiLang_EmailTemplates.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion




                    _dbContext.SaveChanges();


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
        public string LoadContactData(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = ConstantStrings.DefaultLang;
                }

                var model = _dbContext.MultiLang_ContactInfos.Where(p => p.LangKey == lang).ToList();
                var model2 = _dbContext.ContactInfos.Where(p => p.isMultiLang == false).ToList();
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                foreach (var item in model)
                {
                    var contactInfo = _dbContext.ContactInfos.Where(p => p.Pid == item.ContactInfoID).FirstOrDefault().Code;
                    keyValuePairs.Add(contactInfo, item.Value);
                }
                foreach (var item in model2)
                {
                    keyValuePairs.Add(item.Code, item.Value);
                }
                return JsonConvert.SerializeObject(keyValuePairs);
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        #endregion
    }
}
