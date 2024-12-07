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
namespace Admin.Course
{
    public class LectureRepository : ILectureRepository
    {
        ILoggerHelper _logger;
        IFileHelper _fileHelper;
        ICommonHelper _common;
        CourseContext _db;
        public LectureRepository(CourseContext db, IFileHelper fileHelper, ILoggerHelper logger, ICommonHelper common)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _common = common;
        }
        public Response<IPagedList<Database.Lecture>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.Lecture>> rs = new Response<IPagedList<Database.Lecture>>();
            try
            {
                var config = _db.LectureConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                var key = "";
                if (!string.IsNullOrEmpty(search.Title))
                {
                    key = search.Title.ToLower();
                }

                rs.Data = _db.Lectures
                    .Where(p => !p.Deleted).ToList()
                    .Where(p => (p.UnsignedTitle.Contains(key) || p.Title.Contains(key) || string.IsNullOrEmpty(key)) && (p.Enabled == search.Enabled || search.Enabled == null))
                    .OrderByDescending(p => p.Order)
                    .ThenBy(p => p.UpdateDate)
                    .ToList().ToPagedList(search.PageIndex, Convert.ToInt32(config[StaticStringLecture.PageSizeAdmin]));

                foreach (var item in rs.Data)
                {
                    item.Images = StaticStringLecture.ImagePath + item.Images;
                }
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<Database.Lecture> GetById(int id)
        {
            Response<Database.Lecture> rs = new Response<Database.Lecture>();

            try
            {
                var model = _db.Lectures.Where(p => p.Pid == id).FirstOrDefault();
                if(model != null)
                {
                    model.Images = StaticStringLecture.ImagePath + model.Images;
                }
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
        public Response<Database.Lecture> Save(Database.Lecture data, List<IFormFile> files)
        {
            var validator = new LectureValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Lecture> rs = new Response<Database.Lecture>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var config = _db.LectureConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                    if (data.Pid == 0)
                    {
                        double maxOrder = _db.Lectures.Max(x => (double?)x.Order) ?? 0.9;
                        data.Order = maxOrder + 1;
                        data.Slug = _common.StringToSlug(data.Title);
                        data.UnsignedTitle = _common.RemoveSign4VietnameseString(data.Title).ToLower();
                        data.Images = "";
                        _db.Lectures.Add(data);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.Lectures.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.Title = data.Title;
                            data.UnsignedTitle = _common.RemoveSign4VietnameseString(data.Title).ToLower();
                            model.Description = data.Description;
                            model.Content = data.Content;
                            model.Video = data.Video;
                            model.TimeOfVideo = data.TimeOfVideo;
                            model.NumberOfQuestion = data.NumberOfQuestion;
                            model.Type = data.Type;
                            model.Slug = _common.StringToSlug(data.Title);
                            model.UpdateDate = DateTime.Now;
                            _db.SaveChanges();
                        }
                    }

                    //handle image
                    if (files.Count > 0)
                    {
                        var img = _fileHelper.UploadImageModule(files[0], StaticStringLecture.ImagePath, Convert.ToInt32(config[StaticStringLecture.MinWidth]), Convert.ToInt32(config[StaticStringLecture.MaxWidth]));
                        var model = _db.Lectures.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.Images = img;
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
                    var model = _db.Lectures.Where(p => p.Pid == id).FirstOrDefault();

                    if (model != null)
                    {
                        model.Deleted = true;
                        _db.SaveChanges();

                        _fileHelper.DeleteFile(StaticStringLecture.ImagePath, model.Images);
                        _fileHelper.DeleteFile(StaticStringLecture.ImagePath, "full_" + model.Images);
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
                    var model = _db.Lectures.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.Lectures.OrderBy(p => p.Order).ToList();
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
                var model = _db.Lectures.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.Lectures.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Lectures.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.Lectures.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.Lectures.OrderBy(p => p.Order).ToList();
                //var stt = 1;
                //foreach (var item in list)
                //{
                //    item.Order = stt;
                //    stt = stt + 1;
                //    _db.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<List<LectureConfig>> GetConfig()
        {

            Response<List<LectureConfig>> rs = new Response<List<LectureConfig>>();

            try
            {
                var model = _db.LectureConfigs.ToList();
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
        public Response SaveConfig(LectureConfigDto config)
        {

            Response rs = new Response();

            try
            {
                foreach (PropertyInfo propertyInfo in config.GetType().GetProperties())
                {
                    var model = _db.LectureConfigs.Where(x => x.Key == propertyInfo.Name).FirstOrDefault();
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
    }
}
