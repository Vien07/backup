using CMS.Areas.Recruitment.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;


namespace CMS.Areas.Recruitment
{
    public class RecruitmentCateRepository : IRecruitmentCateRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DBContext _dbContext;
        IFileServices _fileSrv;
        ICommonServices _common;
        public RecruitmentCateRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileSrv, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileSrv = fileSrv;
            _common = common;
        }
        public dynamic LoadData(SearchDto search)
        {

            List<dynamic> dataList = new List<dynamic>();

            List<dynamic> data = new List<dynamic>();
            GetAll(ref data, null, "");

            if (!string.IsNullOrEmpty(search.Key))
            {
                foreach (var item in data)
                {
                    if (item.Name.Contains(search.Key))
                    {
                        dataList.Add(item);
                    }
                }
            }
            else
            {
                dataList = data;
            }

            PagedList<dynamic> dataPaging = new PagedList<dynamic>(dataList.OrderByDescending(p => p.Order), search.Page, search.PageNumber);
            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);
            dynamic Paging =
            new
            {
                lastpage = dataPaging.PageCount,
                curentpage = search.Page,
            };
            return new { Data = rs, Paging = Paging };
        }
        public void GetAll(ref List<dynamic> list, long? parentId = null, string prefix = "")
        {
            var data = (from a in _dbContext.RecruitmentCates
                        join b in _dbContext.MultiLang_RecruitmentCates on a.Pid equals b.RecruitmentCatePid
                        where (!a.Deleted && !a.isLocked && a.ParentId == parentId) && (b.LangKey == ConstantStrings.DefaultLangAdmin)
                        orderby a.Order descending
                        select new
                        {
                            Name = b.Name,
                            Pid = a.Pid,
                            Order = a.Order,
                            Enabled = a.Enabled
                        }).ToList();

            foreach (var item in data)
            {
                dynamic d = new ExpandoObject();
                d.Name = prefix + item.Name;
                d.CountPost = (from a in _dbContext.RecruitmentCate_RecruitmentDetails
                               where a.RecruitmentCatePid == item.Pid
                               join b in _dbContext.RecruitmentDetails on a.RecruitmentDetailPid equals b.Pid
                               where b.Deleted == false
                               select new
                               {
                                   Pid = b.Pid
                               }).Distinct().ToList().Count();
                d.Pid = item.Pid;
                d.Order = item.Order;
                d.Enabled = item.Enabled;
                list.Add(d);
            }

            foreach (var item in list)
            {
                List<dynamic> sub = new List<dynamic>();
                GetAll(ref sub, item.Pid, prefix + "—");
                item.Children = sub;
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
                        var model = _dbContext.RecruitmentCates.Where(p => p.Pid == item).FirstOrDefault();
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

                var model = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentCatePid == Pid).ToList();
                var temmpDetail = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.RecruitmentDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.RecruitmentDetailPid,
                            }).ToList();
                if (list.Count() > 0)
                {
                    return new { value = false, type = "warning", messError = "has-child" };
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.RecruitmentCates.Remove(model);
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


                var model = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentCatePid == Pid).ToList();
                var temmpDetail = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.RecruitmentDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.RecruitmentDetailPid,
                            }).ToList();
                //var list = _dbContext.RecruitmentDetails.Where(p => p.RecruitmentCatePid == Pid && p.Deleted==false).ToList();
                foreach (var item in list)
                {
                    var temp = _dbContext.RecruitmentDetails.Where(p => p.Pid == item.Pid).FirstOrDefault();

                    temp.Deleted = true;
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.RecruitmentCates.Remove(model);
                _dbContext.SaveChanges();



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
                        var model = _dbContext.RecruitmentCates.Where(p => p.Pid == item && p.isLocked == false).FirstOrDefault();
                        var temmpCate = _dbContext.RecruitmentCate_RecruitmentDetails.Where(p => p.RecruitmentCatePid == item).ToList();
                        var temmpDetail = _dbContext.RecruitmentDetails.Where(p => p.Deleted == false).ToList();

                        var list = (from p in temmpCate
                                    join e in temmpDetail
                                      on p.RecruitmentDetailPid equals e.Pid
                                    select new
                                    {
                                        Pid = p.RecruitmentDetailPid,
                                    }).ToList();
                        if (list.Count() > 0)
                        {
                            return new { value = false, type = "warning", messError = "has-child", pid = item };
                        }
                        //model.Deleted = true;
                        //model.UpdateDate = DateTime.Now;
                        _dbContext.RecruitmentCates.Remove(model);

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

                var data = _dbContext.MultiLang_RecruitmentCates.Where(p => p.RecruitmentCatePid == Pid).ToList();
                var detail = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var Recruitment = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Recruitment.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Name = item.Name;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Pid = item.Pid;
                        child.RecruitmentCatePid = item.RecruitmentCatePid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        detail.Enabled,
                        detail.ParentId,
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

        public dynamic Insert(RecruitmentCate recruitmentCate, List<MultiLang_RecruitmentCate> multiLang_RecruitmentCate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLang_RecruitmentCate.Where(p => p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    int maxOrder = _dbContext.RecruitmentCates.Max(x => (int?)x.Order) ?? 1;
                    //RecruitmentCate.Pid = 0;

                    recruitmentCate.Order = maxOrder + 1;
                    _dbContext.RecruitmentCates.Add(recruitmentCate);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_RecruitmentCate)
                    {
                        string title = string.IsNullOrEmpty(item.Name) ? defaultData.Name : item.Name;

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.RecruitmentCates
                                         join b in _dbContext.MultiLang_RecruitmentCates on a.Pid equals b.RecruitmentCatePid
                                         where (b.LangKey == item.LangKey && b.Slug == newSlug)
                                         select a).FirstOrDefault();

                        if (existSlug != null)
                        {
                            transaction.Rollback();
                            return new { status = false, mess = "Tiêu đề đã tồn tại" };
                        }
                        else
                        {
                            item.Slug = newSlug;
                        }
                        #endregion

                        item.RecruitmentCatePid = recruitmentCate.Pid;
                        _dbContext.MultiLang_RecruitmentCates.Add(item);
                    }

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
        public dynamic Update(RecruitmentCate recruitmentCate, List<MultiLang_RecruitmentCate> multiLang_RecruitmentCate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.RecruitmentCates.Where(p => p.Pid == recruitmentCate.Pid).FirstOrDefault();
                    model.Enabled = recruitmentCate.Enabled;
                    model.ParentId = recruitmentCate.ParentId;
                    model.UpdateDate = DateTime.Now;
                    foreach (var item in multiLang_RecruitmentCate)
                    {
                        var tempModel = _dbContext.MultiLang_RecruitmentCates.Where(p => p.Pid == item.Pid).FirstOrDefault();
                        if (tempModel != null)
                        {
                            if (model.isLocked == false)
                            {
                                #region check exist slug
                                string newSlug = _common.EncodeTitle(item.Name);
                                var existSlug = (from a in _dbContext.RecruitmentCates
                                                 join b in _dbContext.MultiLang_RecruitmentCates on a.Pid equals b.RecruitmentCatePid
                                                 where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                                 select a).FirstOrDefault();

                                if (existSlug != null)
                                {
                                    transaction.Rollback();
                                    return new { status = false, mess = "Tiêu đề đã tồn tại" };
                                }
                                else
                                {
                                    tempModel.Slug = newSlug;
                                }
                                #endregion

                                tempModel.Name = item.Name;
                            }

                            tempModel.Description = item.Description;
                        }
                        else
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Name);
                            var existSlug = (from a in _dbContext.RecruitmentCates
                                             join b in _dbContext.MultiLang_RecruitmentCates on a.Pid equals b.RecruitmentCatePid
                                             where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                             select a).FirstOrDefault();

                            if (existSlug != null)
                            {
                                transaction.Rollback();
                                return new { status = false, mess = "Tiêu đề đã tồn tại" };
                            }
                            else
                            {
                                item.Slug = newSlug;
                            }
                            #endregion

                            item.RecruitmentCatePid = recruitmentCate.Pid;
                            _dbContext.MultiLang_RecruitmentCates.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }





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
                            var model = _dbContext.RecruitmentCates.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.RecruitmentCates.Max(x => (int?)x.Order) ?? 1;

                            RecruitmentCate addRecruitment = new RecruitmentCate();
                            addRecruitment.Order = maxOrder + 1;
                            addRecruitment.Enabled = false;
                            _dbContext.RecruitmentCates.Add(addRecruitment);
                            _dbContext.SaveChanges();
                            List<MultiLang_RecruitmentCate> detailModel = _dbContext.MultiLang_RecruitmentCates.Where(p => p.RecruitmentCatePid == model.Pid).ToList();
                            foreach (MultiLang_RecruitmentCate itemDetail in detailModel)
                            {

                                MultiLang_RecruitmentCate temp = new MultiLang_RecruitmentCate();
                                temp.Name = itemDetail.Name + "(Coppy)";
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                //temp.Slug = itemDetail.Slug;
                                temp.Slug = _common.EncodeTitle(temp.Name);

                                temp.RecruitmentCatePid = addRecruitment.Pid;
                                _dbContext.MultiLang_RecruitmentCates.Add(temp);
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
                long temp = 0;
                var data = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.RecruitmentCates.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                long temp = 0;
                var data = _dbContext.RecruitmentCates.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.RecruitmentCates.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.RecruitmentCates.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.RecruitmentCates.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.RecruitmentCates.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.RecruitmentCates.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.RecruitmentCates.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
