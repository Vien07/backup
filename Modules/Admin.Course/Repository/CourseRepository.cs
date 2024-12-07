using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Admin.Course.Database;
using Admin.Course.Models;
using System.Reflection;
using Common.Helper;
using static Common.StaticMethod;
using X.PagedList;
using Admin.Common.Models;
using FluentValidation.Results;
using Admin.Course.Const;
using ComponentUILibrary.Models;
using Admin.Category.Database;

namespace Admin.Course
{
    public class CourseRepository : ICourseRepository
    {
        ILoggerHelper _logger;
        IFileHelper _fileHelper;
        ICommonHelper _common;
        CourseContext _db;
        CategoryContext _dbCạtegory;
        public CourseRepository(CourseContext db, IFileHelper fileHelper, ILoggerHelper logger, ICommonHelper common, CategoryContext dbCạtegory)
        {
            _db = db;
            _dbCạtegory = dbCạtegory;
            _logger = logger;
            _fileHelper = fileHelper;
            _common = common;
        }
        public Response<IPagedList<Database.Course>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.Course>> rs = new Response<IPagedList<Database.Course>>();
            try
            {
                var config = _db.CourseConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                var key = "";
                if (!string.IsNullOrEmpty(search.Title))
                {
                    key = search.Title.ToLower();
                }
                rs.Data = _db.Courses
                    .Where(p => !p.Deleted)
                    .Where(p => (p.UnsignedTitle.Contains(key) || p.Title.Contains(key) || string.IsNullOrEmpty(key)) && (p.Enabled == search.Enabled || search.Enabled == null))
                    .OrderByDescending(p => p.Order)
                    .ThenBy(p => p.UpdateDate)
                    .ToList().ToPagedList(search.PageIndex, Convert.ToInt32(config[StaticStringCourse.PageSizeAdmin]));

                foreach (var item in rs.Data)
                {
                    item.Images = StaticStringCourse.ImagePath + item.Images;
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
        public Response<Database.Course> GetById(int id)
        {
            Response<Database.Course> rs = new Response<Database.Course>();

            try
            {
                var model = _db.Courses.Where(p => p.Pid == id).FirstOrDefault();
                if (model != null)
                {
                    model.Images = StaticStringCourse.ImagePath + model.Images;
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
        public Response<string> GetCateById(int id)
        {
            Response<string> rs = new Response<string>();

            try
            {
                var model = _db.CourseCategories.Where(p => p.CoursePid == id).Select(p => p.CategoryPid).ToList();
                rs.Data = string.Join(",", model);
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
        public Response<Database.Course> Save(Database.Course data, List<IFormFile> files, string categories)
        {
            var validator = new CourseValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Course> rs = new Response<Database.Course>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var config = _db.CourseConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

                    if (data.Pid == 0)
                    {
                        double maxOrder = _db.Courses.Max(x => (double?)x.Order) ?? 0.9;
                        data.Order = maxOrder + 1;
                        data.Slug = _common.StringToSlug(data.Title);
                        data.UnsignedTitle = _common.RemoveSign4VietnameseString(data.Title).ToLower();
                        data.Images = "";
                        _db.Courses.Add(data);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.Courses.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.Price = data.Price;
                            model.Hot = data.Hot;
                            model.New = data.New;
                            model.SkillLevel = data.SkillLevel;
                            model.Duration = data.Duration;
                            model.NumberOfLessons = data.NumberOfLessons;
                            model.PriceDiscount = data.PriceDiscount;
                            data.UnsignedTitle = _common.RemoveSign4VietnameseString(data.Title).ToLower();
                            model.Description = data.Description;
                            model.Content = data.Content;
                            model.Slug = _common.StringToSlug(data.Title);
                            model.UpdateDate = DateTime.Now;
                            _db.SaveChanges();
                        }
                    }
                    //handle image
                    if (files.Count > 0)
                    {
                        var img = _fileHelper.UploadImageModule(files[0], StaticStringCourse.ImagePath, Convert.ToInt32(config[StaticStringCourse.MinWidth]), Convert.ToInt32(config[StaticStringCourse.MaxWidth]));
                        var model = _db.Courses.Where(p => p.Pid == data.Pid).FirstOrDefault();
                        if (model != null)
                        {
                            model.Images = img;
                            _db.SaveChanges();
                        }
                    }

                    //handle catetory
                    if (!string.IsNullOrEmpty(categories))
                    {
                        var cates = categories.Split(",").Select(p => Convert.ToInt32(p)).ToList();

                        if (cates != null)
                        {
                            //remove old cates 
                            var oldCates = _db.CourseCategories.Where(p => p.CoursePid == data.Pid).ToList();
                            _db.CourseCategories.RemoveRange(oldCates);
                            _db.SaveChanges();

                            foreach (var item in cates)
                            {
                                var newCate = new CourseCategory();
                                newCate.CoursePid = data.Pid;
                                newCate.CategoryPid = item;
                                _db.CourseCategories.Add(newCate);
                            }

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
                    var model = _db.Courses.Where(p => p.Pid == id).FirstOrDefault();
                    if (model != null)
                    {
                        model.Deleted = true;
                        _db.SaveChanges();

                        _fileHelper.DeleteFile(StaticStringCourse.ImagePath, model.Images);
                        _fileHelper.DeleteFile(StaticStringCourse.ImagePath, "full_" + model.Images);
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
                    var model = _db.Courses.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.Courses.OrderBy(p => p.Order).ToList();
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
                var model = _db.Courses.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.Courses.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Courses.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.Courses.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.Courses.OrderBy(p => p.Order).ToList();
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
        public Response<List<SelectControlData>> GetDataCategories()
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            try
            {
                rs.Data = _dbCạtegory.Categories.Where(x => !x.Deleted && x.Enabled).OrderByDescending(x => x.Order)
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
        public Response<List<CourseConfig>> GetConfig()
        {

            Response<List<CourseConfig>> rs = new Response<List<CourseConfig>>();

            try
            {
                var model = _db.CourseConfigs.ToList();
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
        public Response SaveConfig(CourseConfigDto config)
        {

            Response rs = new Response();

            try
            {
                foreach (PropertyInfo propertyInfo in config.GetType().GetProperties())
                {
                    var model = _db.CourseConfigs.Where(x => x.Key == propertyInfo.Name).FirstOrDefault();
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
        public Response<List<SelectControlData>> GetLectures()
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();

            try
            {

                rs.Data = _db.Lectures.Where(x => !x.Deleted && x.Enabled).OrderByDescending(x => x.Order)
                    .Select(x => new SelectControlData { Value = x.Pid.ToString(), Name = x.Title + " - " + x.CreateDate.ToString("dd/MM/yyyy") }).ToList();
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
        public Response<Root> GetLecture(int id)
        {

            Response<Root> rs = new Response<Root>();
            try
            {
                var lectures = _db.Lectures.Where(x => !x.Deleted && x.Enabled).ToList();
                var courseLectures = _db.Course_Lectures.ToList();

                Root data = new Root();
                data.id = id;
                data.chapters = (from chapter in _db.CourseChapters
                                 where chapter.CoursePid == id
                                 select new CourseChapterDto
                                 {
                                     id = chapter.Pid,
                                     order = chapter.Order,
                                     title = chapter.Title,
                                 }).ToList();

                foreach (var item in data.chapters)
                {

                    item.lectures = (from l in lectures
                                     join cl in courseLectures on l.Pid equals cl.LecturePid
                                     where cl.CourseChapterPid == item.id
                                     select new CourseLectureDto
                                     {
                                         title = cl.Title,
                                         lectureName = l.Title + " - " + l.CreateDate.ToString("dd/MM/yyyy"),
                                         id = cl.LecturePid,
                                         preview = cl.Preview,
                                         order = cl.Order
                                     }).ToList();
                }
                rs.Data = data;
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());
            }
            return rs;
        }
        public Response SaveLecture(Root data)
        {
            Response rs = new Response();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var model = _db.CourseChapters.Where(x => x.CoursePid == data.id).ToList();
                    _db.CourseChapters.RemoveRange(model);

                    foreach (var item in data.chapters)
                    {
                        CourseChapter chapter = new CourseChapter();
                        chapter.CoursePid = data.id;
                        chapter.Order = item.order;
                        chapter.Title = item.title;
                        _db.CourseChapters.Add(chapter);
                        _db.SaveChanges();


                        foreach (var ele in item.lectures)
                        {
                            Course_Lecture lecture = new Course_Lecture();
                            lecture.LecturePid = ele.id;
                            lecture.Order = ele.order;
                            lecture.Preview = ele.preview;
                            lecture.Title = ele.title;
                            lecture.CourseChapterPid = chapter.Pid;
                            _db.Course_Lectures.Add(lecture);
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
    }
}
