using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Admin.Models;
using System.Dynamic;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Admin
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private string UrlUserImages = ConstantStrings.UrlUserImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private string RoleRoot = ConstantStrings.RoleRoot;
        private string RoleAdmin = ConstantStrings.RoleAdmin;
        public UsersRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
        }
        public string GetData()
        {
            try
            {
                var tempCode = _httpContextAccessor.HttpContext.Session.GetString("UserCode");

                //int page = 1;
                //int limit = 25;
                var data = _dbContext.Users.Where(p => p.Deleted == false && p.GroupUser.View == true && p.Code != tempCode).ToList();
                //PagedList<GroupUser> pagingData = new PagedList<GroupUser>(data, page, limit);
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var group = _dbContext.GroupUsers.Where(p => p.Code == item.GroupUserCode && p.Enabled == true && p.Deleted == false).FirstOrDefault();
                    dynamic child = new ExpandoObject();
                    item.Avatar = string.IsNullOrEmpty(item.Avatar) ? "default-avatar.png" : item.Avatar;
                    child.Avatar = UrlUserImages + Thumb + item.Avatar;
                    child.Code = item.Code;
                    child.Pid = item.Pid;
                    child.FullName = item.FullName;// item.FirstName + " " + item.LastName;
                    child.Email = item.Email;
                    child.LastLogin = item.LastLogin;
                    child.Enabled = item.Enabled;
                    child.Role = group.Role;
                    child.View = group.View;
                    child.Group = group.Name;
                    child.GroupUserCode = item.GroupUserCode;
                    listData.Add(child);
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(listData.Where(p => p.View == true));

                return result;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Enable(string code, bool active)
        {
            try
            {
                var model = _dbContext.Users.Where(p => p.Code == code && !p.Deleted).FirstOrDefault();
                model.Enabled = active;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Delete(string code)
        {
            try
            {
                var model = _dbContext.Users.Where(p => p.Code == code && !p.Deleted).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public string Edit(int Pid)
        {
            try
            {
                var role = _httpContextAccessor.HttpContext.Session.GetString("Role");
                dynamic item;
                if (role == RoleRoot)
                {
                    item = _dbContext.Users.Where(p => p.Pid == Pid && p.Deleted == false).FirstOrDefault();

                }
                else
                {
                    item = _dbContext.Users.Where(p => p.Pid == Pid && p.GroupUser.View == true && p.Deleted == false).FirstOrDefault();
                }

                dynamic child = new ExpandoObject();
                child.Pid = item.Pid;
                child.Code = item.Code;
                child.FileName = item.Avatar;
                child.Avatar = UrlUserImages + Thumb + item.Avatar;

                child.FullName = item.FullName;
                child.Email = item.Email;
                child.Enabled = item.Enabled;

                child.Phone = item.Phone;
                child.GroupUserCode = item.GroupUserCode;
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
        public bool Insert(User data)
        {
            try
            {
                data.Password = _common.GetHashSha256(data.Password);
                _dbContext.Users.Add(data);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool Update(User data, IFormFile Images, string newPassWord, int type)
        {
            //type = 0: user edit ,type =1: user info
            try
            {
                var model = _dbContext.Users.Where(p => p.Pid == data.Pid && !p.Deleted).FirstOrDefault();
                if (newPassWord != "" && newPassWord != null)
                {
                    model.Password = _common.GetHashSha256(newPassWord);
                }
                if (Images != null)
                {
                    if (model.Avatar != "default-avatar.png")
                    {
                        _fileServices.DeleteFile(UrlUserImages, model.Avatar);
                        _fileServices.DeleteFile(UrlUserImages, "thumb_" + model.Avatar);
                    }

                    dynamic kt = _fileServices.SaveFile(Images, UrlUserImages, model.Code);
                    if (!kt.isError)
                    {
                        _fileServices.ResizeThumbImage(Images, UrlUserImages, kt.fileName);
                        model.Avatar = kt.fileName;
                    }
                    else
                    {
                        model.Avatar = "default-avatar.png";
                    }
                }

                var Role = _httpContextAccessor.HttpContext.Session.GetString("Role") != null ? _httpContextAccessor.HttpContext.Session.GetString("Role") : "";
                if (Role.Trim().ToLower() == RoleAdmin.Trim().ToLower() || Role.Trim().ToLower() == RoleRoot.Trim().ToLower())
                {
                    model.Email = data.Email;

                    model.FullName = data.FullName;
                    if (Role.Trim().ToLower() != RoleRoot.Trim().ToLower() || type == 0)
                    {
                        model.GroupUserCode = data.GroupUserCode;
                    }
                }
                //model.Password = data.Password;
                ////model.LastName = data.LastName;
                ////model.Email = data.Email;
                model.UpdateDate = DateTime.Now;
                model.UpdateUser = _httpContextAccessor.HttpContext.Session.GetString("UserCode");
                //model.Role = data.Role;
                _dbContext.SaveChanges();
                return true;
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
                var data = _dbContext.Users.Where(p => ((p.Enabled.Equals(searchData.Enable) || searchData.Enable.Equals(null))) &&

                                                    p.Deleted == false && p.GroupUser.View == true).ToList()
                                                    .FilterSearch(new string[] { "FirstName", "LastName", "Email" }, searchData.Key).ToList();
                //PagedList<GroupUser> pagingData = new PagedList<GroupUser>(data, page, limit);
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var group = _dbContext.GroupUsers.Where(p => p.Code == item.GroupUserCode && p.Enabled == true && p.Deleted == false).FirstOrDefault();
                    dynamic child = new ExpandoObject();
                    child.Avatar = UrlUserImages + item.Avatar;
                    child.Code = item.Code;
                    child.Pid = item.Pid;
                    child.FullName = item.FullName;
                    child.Email = item.Email;
                    child.LastLogin = item.LastLogin;
                    child.Enabled = item.Enabled;
                    child.Role = group.Role;
                    child.Group = group.Name;
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
        public string GetUserInfo()
        {

            try
            {
                User item = new User();
                var code = _httpContextAccessor.HttpContext.Session.GetString("UserCode");
                item = _dbContext.Users.Where(p => p.Code == code && !p.Deleted).FirstOrDefault();
                dynamic child = new ExpandoObject();
                child.Pid = item.Pid;
                child.Code = item.Code;
                child.FileName = item.Avatar;
                child.Avatar = UrlUserImages + Thumb + item.Avatar;

                child.FullName = item.FullName;
                child.GroupUserCode = item.GroupUserCode;
                child.Email = item.Email;
                child.Enabled = item.Enabled;
                child.Phone = item.Phone;
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(child);

                return result;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Validation(User data)
        {
            try
            {
                var groupUser = _dbContext.GroupUsers.Where(p => p.Code == data.GroupUserCode).FirstOrDefault();
                if (groupUser.Role == RoleRoot)
                {
                    return new { error = true, errorMess = "Không được thao tác vai trò này!" };
                }
                var user = _dbContext.Users.Where(p => p.Code == data.Code && p.Deleted == false).FirstOrDefault();
                if (user != null)
                {
                    return new { error = true, errorMess = "User đã tồn tại" };
                }
                return new { error = false, errorMess = "" };

            }
            catch (Exception ex)
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
        }
    }
}
