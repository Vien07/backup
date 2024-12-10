using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Comment.Models;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO;
using CMS.Services;
using DTO.Common;
using Devsense.PHP.Syntax;

namespace CMS.Areas.Comment
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _core;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private string messErr = "";
        private string UrlCommentImages = ConstantStrings.UrlCommentImages;
        private string DefaultLang = ConstantStrings.DefaultLangAdmin;
        private string Fullmages = ConstantStrings.Fullmages;
        private string Thumb = ConstantStrings.Thumb;

        public CommentRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices core)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _core = core;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                var model = (from a in _dbContext.Comments
                             join b in _dbContext.MultiLang_Comments on a.Pid equals b.CommentPid
                             where !a.Deleted && !a.isLocked && (a.Enabled == search.Enable || search.Enable == null) && b.LangKey == DefaultLang
                             select new
                             {
                                 Name = b.Name,
                                 PicThumb = a.PicThumb,
                                 Pid = a.Pid,
                                 Order = a.Order,
                                 Enabled = a.Enabled
                             }).ToList().FilterSearch(new string[] { "Description", "Name" }, search.Key);

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in model)
                {
                    dynamic child = new ExpandoObject();
                    child.Name = item.Name;
                    child.PicThump = UrlCommentImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.Enabled = item.Enabled;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.Order), search.Page, search.PageNumber);
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
                        var model = _dbContext.Comments.Where(p => p.Pid == item).FirstOrDefault();
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
        public dynamic Delete(int Pid)
        {
            try
            {

                var model = _dbContext.Comments.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                _dbContext.Comments.Remove(model);
                //model.Deleted = true; 
                //model.UpdateDate = DateTime.Now;
                _dbContext.SaveChanges();
                return new { value = true }; ;
            }
            catch (Exception ex)
            {

                return new { value = false, type = "error" };
            }

        }
        public dynamic DeleteAll(int Pid)
        {
            try
            {

                return new { value = true }; ;
            }
            catch (Exception ex)
            {

                return new { value = false, type = "error" };
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
                        var model = _dbContext.Comments.Where(p => p.Pid == item && p.isLocked == false).FirstOrDefault();
                        _dbContext.Comments.Remove(model);


                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        return new { value = false, type = "error" };

                    }
                }
                return new { value = true }; ;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public dynamic Edit(long Pid)
        {
            try
            {

                var data = _dbContext.MultiLang_Comments.Where(p => p.CommentPid == Pid).ToList();
                var detail = _dbContext.Comments.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var Services = _dbContext.Comments.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Services.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Name = item.Name;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Pid = item.Pid;
                        child.CommentPid = item.CommentPid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        picThumb = UrlCommentImages + Fullmages + detail.PicThumb,
                        image = UrlCommentImages + Fullmages + detail.Image,
                        detail.Enabled,
                        detail.Star,
                        detail.Pid
                    },
                    list = listData
                };

                //return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(Models.Comment Comment, List<MultiLang_Comment> multiLang_Comment, IFormFile Images, IFormFile PicThumb2)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    var defaultData = multiLang_Comment.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    int maxOrder = _dbContext.Comments.Max(x => (int?)x.Order) ?? 1;

                    //int maxOrder = _dbContext.Comments.Select(p => p.Order).DefaultIfEmpty(0).Max();
                    Comment.Pid = 0;
                    Comment.Order = maxOrder + 1;

                    if (Images != null)
                    {
                        dynamic kt = _fileServices.SaveFile(Images, UrlCommentImages, defaultData.Name);

                        if (!kt.isError)
                        {
                            _fileServices.ResizeThumbImage(Images, UrlCommentImages, kt.fileName);
                            Comment.PicThumb = kt.fileName;
                        }
                    }

                    if (PicThumb2 != null)
                    {
                        dynamic kt = _fileServices.SaveFile(PicThumb2, UrlCommentImages, defaultData.Name + "img");

                        if (!kt.isError)
                        {
                            _fileServices.ResizeThumbImage(PicThumb2, UrlCommentImages, kt.fileName);
                            Comment.Image = kt.fileName;
                        }
                    }

                    _dbContext.Comments.Add(Comment);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_Comment)
                    {
                        string title = string.IsNullOrEmpty(item.Name) ? defaultData.Name : item.Name;
                        item.CommentPid = Comment.Pid;
                        item.Slug = _core.EncodeTitle(title);
                        _dbContext.MultiLang_Comments.Add(item);
                        _dbContext.SaveChanges();
                    }


                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    messErr = "Something Wrong!";
                    return new { isError = false, mess = messErr };
                }
            }
        }
        public dynamic Update(Models.Comment Comment, List<MultiLang_Comment> multiLang_Comment, IFormFile images, IFormFile PicThumb2)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var model = _dbContext.Comments.Where(p => p.Pid == Comment.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_Comments.Where(p => p.CommentPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault();

                    model.Enabled = Comment.Enabled;
                    model.Star = Comment.Star;
                    model.UpdateDate = DateTime.Now;

                    if (images != null)
                    {
                        _fileServices.DeleteFile(UrlCommentImages, Fullmages + model.PicThumb);
                        _fileServices.DeleteFile(UrlCommentImages, Thumb + model.PicThumb);
                        model.PicThumb = "";
                    }
                    dynamic kt = _fileServices.SaveFile(images, UrlCommentImages, nameImages.Name);
                    if (!kt.isError)
                    {
                        _fileServices.ResizeThumbImage(images, UrlCommentImages, kt.fileName);
                        model.PicThumb = kt.fileName;
                    }

                    if (PicThumb2 != null)
                    {
                        _fileServices.DeleteFile(UrlCommentImages, Fullmages + model.Image);
                        _fileServices.DeleteFile(UrlCommentImages, Thumb + model.Image);
                        model.Image = "";
                    }

                    dynamic kt2 = _fileServices.SaveFile(PicThumb2, UrlCommentImages, nameImages.Name + "img");
                    if (!kt2.isError)
                    {
                        _fileServices.ResizeThumbImage(PicThumb2, UrlCommentImages, kt2.fileName);
                        model.Image = kt2.fileName;
                    }

                    var list = _dbContext.MultiLang_Comments.Where(p => p.CommentPid == Comment.Pid).ToList();
                    foreach (var item in multiLang_Comment)
                    {
                        var tempModel = _dbContext.MultiLang_Comments.Where(p => p.Pid == item.Pid).FirstOrDefault();
                        if (tempModel != null)
                        {
                            if (model.isLocked == false)
                            {
                                tempModel.Name = item.Name;
                                tempModel.Slug = _core.EncodeTitle(tempModel.Name);
                            }

                            tempModel.Description = item.Description;
                        }
                        else
                        {
                            item.CommentPid = Comment.Pid;
                            _dbContext.MultiLang_Comments.Add(item);
                        }
                    }


                    _dbContext.SaveChanges();



                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    messErr = "Something Wrong!";
                    return new { isError = false, mess = messErr };
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
        public bool Coppy(long[] Pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in Pid)
                    {
                        try
                        {
                            var model = _dbContext.Comments.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.Comments.Select(p => p.Order).DefaultIfEmpty(0).Max();

                            Models.Comment addProject = new Models.Comment();
                            addProject.Order = maxOrder + 1;
                            addProject.Enabled = false;
                            _dbContext.Comments.Add(addProject);
                            _dbContext.SaveChanges();
                            List<MultiLang_Comment> detailModel = _dbContext.MultiLang_Comments.Where(p => p.CommentPid == model.Pid).ToList();
                            foreach (MultiLang_Comment itemDetail in detailModel)
                            {

                                MultiLang_Comment temp = new MultiLang_Comment();
                                temp.Name = itemDetail.Name + " (Coppy)";
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Slug = itemDetail.Slug;
                                temp.CommentPid = addProject.Pid;
                                _dbContext.MultiLang_Comments.Add(temp);
                                _dbContext.SaveChanges();
                            }

                            //_dbContext.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            //transaction.Rollback();
                        }

                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
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
        public bool Up(long Pid)
        {
            try
            {
                int temp = 0;
                var data = _dbContext.Comments.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Comments.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
                if (changeData != null)
                {
                    data.Order = changeData.Order;
                    changeData.Order = temp;
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool Down(long Pid)
        {
            try
            {
                int temp = 0;
                var data = _dbContext.Comments.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Comments.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
                data.Order = changeData.Order;
                changeData.Order = temp;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool UpdateOrder(long Pid, int order)
        {
            try
            {
                var data = _dbContext.Comments.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.Comments.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Comments.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Comments.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Comments.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                        foreach (var item in listData)
                        {
                            if (item.Order > 1)
                            {
                                item.Order = item.Order - 1;

                            }
                        }
                        fromRow.Order = orderTo;
                        toRow.Order = orderTo - 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }
                }
                else
                {
                    var listData = _dbContext.Comments.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                    if (orderTo != 1)
                    {
                        foreach (var item in listData)
                        {
                            if (item.Order < max)
                            {
                                item.Order = item.Order + 1;
                            }
                        }
                        fromRow.Order = orderTo;

                        toRow.Order = orderTo + 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }

                }


                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
