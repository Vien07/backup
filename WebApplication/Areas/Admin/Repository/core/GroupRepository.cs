using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Admin.Models;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using CMS.Services.TranslateServices;

namespace CMS.Areas.Admin
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ITranslateServices _translate;

        public GroupRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common, ITranslateServices translate)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
            _translate = translate;
        }
        public string GetData()
        {
            try
            {
                //int page = 1;
                //int limit = 25;
                var data = _dbContext.GroupUsers.Where(p => p.Deleted == false && p.View == true).ToList();
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
        public bool Enable(int code, bool active)
        {
            try
            {
                var model = _dbContext.GroupUsers.Where(p => p.Code == code).FirstOrDefault();
                model.Enabled = active;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Delete(int code)
        {
            try
            {
                var model = _dbContext.GroupUsers.Where(p => p.Code == code).FirstOrDefault();
                model.Deleted = true;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public string Edit(int code)
        {
            try
            {

                var item = _dbContext.GroupUsers.Where(p => p.Code == code && p.Deleted == false).FirstOrDefault();

                dynamic child = new ExpandoObject();
                child.Code = item.Code;
                child.Role = item.Role;
                child.Name = item.Name;
                //child.Enabled = item.Enabled;
                //child.CountUser = _dbContext.Users.Where(p => p.GroupUserCode == item.Code && p.Deleted == false).Count();

                var result = Newtonsoft.Json.JsonConvert.SerializeObject(child);

                return result;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Insert(GroupUser data)
        {
            try
            {
                _dbContext.GroupUsers.Add(data);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Update(GroupUser data)
        {
            try
            {
                var model = _dbContext.GroupUsers.Where(p => p.Code == data.Code).FirstOrDefault();
                model.Name = data.Name;
                model.Role = data.Role;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Count(int code)
        {
            try
            {
                #region check count
                var count = _dbContext.Users.Where(p => p.GroupUserCode == code && !p.Deleted).Count();
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
                var data = _dbContext.GroupUsers.Where(p => (p.Enabled.Equals(searchData.Enable) || searchData.Enable.Equals(null)) &&
                    p.Deleted == false && p.View == true).ToList().FilterSearch(new string[] { "Name", "Role" }, searchData.Key).Distinct().ToList();
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
        public dynamic Validation(GroupUser data)
        {
            try
            {
                #region check count
                var count = _dbContext.GroupUsers.Where(p => p.Name == data.Name && p.Code != data.Code).Count();
                #endregion
                if (data.Role == _translate.GetStringAdmin("alert.need-permission"))
                {
                    return new { error = true, messError = _translate.GetStringAdmin("alert.duplicate-group") };
                }
                if (count > 0)
                {
                    return new { error = true, messError = _translate.GetStringAdmin("alert.duplicate-group") };
                }
                else
                {
                    return new { error = false, messError = "" };
                }
             ;
            }
            catch (Exception ex)
            {

                return new { error = true, errorMess = _translate.GetStringAdmin("alert.unknown-error") };
            }
        }
    }
}
