using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Course.Database;
using Admin.Course.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Common.Helper;
using static Common.StaticMethod;
using X.PagedList;
using Admin.Common.Models;
using FluentValidation.Results;
using Admin.Course.Const;
using ComponentUILibrary.Models;
using Common.Model;

namespace Admin.Course
{
    public class MemberRepository : IMemberRepository
    {
        ILoggerHelper _logger;
        IFileHelper _fileHelper;
        ICommonHelper _common;
        CourseContext _db;
        ConfigurationContext _dbConfig;
        IMailHelper _mail;
        public MemberRepository(CourseContext db, IFileHelper fileHelper, ILoggerHelper logger, ICommonHelper common, IMailHelper mail, ConfigurationContext dbConfig)
        {
            _db = db;
            _dbConfig = dbConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _common = common;
            _mail = mail;
        }
        public Response<IPagedList<Database.Member>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.Member>> rs = new Response<IPagedList<Database.Member>>();
            try
            {
                var config = _db.MemberConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                var key = "";
                if (!string.IsNullOrEmpty(search.Title))
                {
                    key = search.Title.ToLower();
                }

                rs.Data = _db.Members
                    .Where(p => !p.Deleted).ToList()
                    .Where(p => (p.Email.Contains(key) || p.PhoneNumber.Contains(key) || string.IsNullOrEmpty(key)) && (p.Enabled == search.Enabled || search.Enabled == null))
                    .OrderByDescending(p => p.Order)
                    .ThenBy(p => p.UpdateDate)
                    .ToList().ToPagedList(search.PageIndex, Convert.ToInt32(config[StaticStringMember.PageSizeAdmin]));

            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<Database.Member> GetById(int id)
        {
            Response<Database.Member> rs = new Response<Database.Member>();

            try
            {
                var model = _db.Members.Where(p => p.Pid == id).FirstOrDefault();
                rs.Data = model;
                return rs;

            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());

            }
            return rs;
        }
        public Response<Database.Member> Save(Database.Member data, List<IFormFile> files)
        {
            var validator = new MemberValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Member> rs = new Response<Database.Member>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var config = _db.MemberConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                    if (data.Pid == 0)
                    {
                        double maxOrder = _db.Members.Max(x => (double?)x.Order) ?? 0.9;
                        data.Order = maxOrder + 1;
                        _db.Members.Add(data);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.Members.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.Email = data.Email;
                            model.PhoneNumber = data.PhoneNumber;
                            model.UpdateDate = DateTime.Now;
                            _db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.isError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());
                }
            }
            return rs;
        }
        public Response Delete(List<int> ids)
        {
            Response rs = new Response();
            try
            {
                foreach (var id in ids)
                {
                    var model = _db.Members.Where(p => p.Pid == id).FirstOrDefault();

                    if (model != null)
                    {
                        model.Deleted = true;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());
            }
            return rs;
        }
        public Response Enable(List<int> ids, bool isEnable)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.Members.Where(p => p.Pid == id).FirstOrDefault();
                    model.Enabled = isEnable;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }
        public Response EnableUpdateOrder()
        {

            Response rs = new Response();

            try
            {
                var list = _db.Members.OrderBy(p => p.Order).ToList();
                var order = 1;
                foreach (var item in list)
                {
                    item.Order = order;
                    order = order + 1;
                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;

        }
        public Response UpdateOrder(int id, double order)
        {

            Response rs = new Response();

            try
            {
                var model = _db.Members.Where(p => p.Pid == id).FirstOrDefault();
                model.Order = order;
                _db.SaveChanges();


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "id:" + id.ToString());

            }
            return rs;

        }
        public Response Move(int fromId, int toId)
        {

            Response rs = new Response();

            try
            {
                var fromModel = _db.Members.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Members.Where(p => p.Pid == toId).FirstOrDefault();

                if (fromModel != null && fromModel != null)
                {
                    var fromOrder = fromModel.Order;
                    var toOrder = toModel.Order;
                    if (fromOrder > toOrder)
                    {
                        fromModel.Order = toModel.Order - 0.00001;

                    }
                    else if (fromOrder < toOrder)
                    {
                        fromModel.Order = toModel.Order + 0.00001;
                    }

                    _db.SaveChanges();
                    var list = _db.Members.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<List<MemberConfig>> GetConfig()
        {

            Response<List<MemberConfig>> rs = new Response<List<MemberConfig>>();

            try
            {
                var model = _db.MemberConfigs.ToList();
                rs.Data = model;
            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;

        }
        public Response SaveConfig(MemberConfigDto config)
        {

            Response rs = new Response();

            try
            {
                foreach (PropertyInfo propertyInfo in config.GetType().GetProperties())
                {
                    var model = _db.MemberConfigs.Where(x => x.Key == propertyInfo.Name).FirstOrDefault();
                    if (model != null)
                    {
                        model.Value = propertyInfo.GetValue(config).ToString();
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, config.ToJson());
            }
            return rs;

        }

        public Response<List<SelectControlData>> GetCourses()
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();

            try
            {

                rs.Data = _db.Courses.Where(x => !x.Deleted && x.Enabled).OrderByDescending(x => x.Order)
                    .Select(x => new SelectControlData { Value = x.Pid.ToString(), Name = x.Title }).ToList();
                return rs;

            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<List<Models.CourseMemberViewModel>> GetListCourseMember(int memberPid)
        {
            Response<List<Models.CourseMemberViewModel>> rs = new Response<List<Models.CourseMemberViewModel>>();
            try
            {
                rs.Data = (from c in _db.Courses
                           join cm in _db.Course_Members on c.Pid equals cm.CoursePid
                           where cm.MemberPid == memberPid
                           orderby cm.CreateDate descending
                           select new CourseMemberViewModel
                           {
                               CourseName = c.Title,
                               ExpireDate = cm.ExpireDate.ToString("dd/MM/yyyy HH:mm:ss"),
                               StartDate = cm.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                               Key = cm.Key,
                               Actived = cm.ExpireDate > cm.StartDate,
                               Pid = cm.Pid,
                           }).ToList();
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, memberPid.ToString());
            }
            return rs;
        }
        public Response<Database.Course_Member> SaveCourseMember(Database.Course_Member data)
        {
            Response<Database.Course_Member> rs = new Response<Database.Course_Member>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        _db.Course_Members.Add(data);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.Course_Members.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.CoursePid = data.CoursePid;
                            model.Key = data.Key;
                            model.StartDate = data.StartDate;
                            model.ExpireDate = data.ExpireDate;
                            model.UpdateDate = DateTime.Now;
                            _db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.isError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());
                }
            }
            return rs;
        }
        public Response<Database.Course_Member> GetCourseMember(int courseMemberPid)
        {
            Response<Database.Course_Member> rs = new Response<Database.Course_Member>();
            try
            {
                var model = _db.Course_Members.Where(x => x.Pid == courseMemberPid).FirstOrDefault();
                rs.Data = model;
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, courseMemberPid.ToString());
            }
            return rs;
        }
        public Response DeleteCourseMember(int courseMemberPid)
        {
            Response rs = new Response();
            try
            {
                var model = _db.Course_Members.Where(p => p.Pid == courseMemberPid).FirstOrDefault();

                if (model != null)
                {
                    _db.Remove(model);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, courseMemberPid.ToJson());
            }
            return rs;
        }
        public Response SendMailCourseMember(int courseMemberPid)
        {
            Response rs = new Response();
            try
            {
                var model = (from a in _db.Members
                             join b in _db.Course_Members on a.Pid equals b.MemberPid
                             where b.Pid == courseMemberPid
                             select new
                             {
                                 a.Email,
                                 b.Key,
                                 StartDate = b.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                                 ExpireDate = b.ExpireDate.ToString("dd/MM/yyyy HH:mm:ss"),
                             }).FirstOrDefault();

                if (model != null)
                {
                    var config = _dbConfig.Configurations.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value).ToObject<Setting>();

                    _mail.SendMail(
                       model.Email,
                       "Khóa học của bạn đã được duyệt",
                       string.Format("Key của bạn là: {0}.\nNgày bắt đầu: {1}. \nNgày kết thúc: {2}", model.Key, model.StartDate, model.ExpireDate),
                       config);
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, courseMemberPid.ToJson());
            }
            return rs;
        }
    }
}
