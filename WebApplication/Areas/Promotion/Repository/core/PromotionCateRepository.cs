﻿using CMS.Areas.Promotion.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;

namespace CMS.Areas.Promotion
{
    public class PromotionCateRepository : IPromotionCateRepository
    {
        private readonly DBContext _dbContext;
        private readonly IFileServices _fileSrv;
        private readonly ICommonServices _common;
        private readonly IMemoryCache _memoryCache;


        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;

        public PromotionCateRepository(DBContext dbContext, IFileServices fileSrv, ICommonServices common, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _fileSrv = fileSrv;
            _common = common;
            _memoryCache = memoryCache;


        }

        public dynamic LoadData(SearchDto search)
        {

            List<dynamic> dataList = new List<dynamic>();

            List<dynamic> data = new List<dynamic>();
            GetAll(ref data, 0, "");

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
            var data = (from a in _dbContext.PromotionCates
                        join b in _dbContext.MultiLang_PromotionCates on a.Pid equals b.PromotionCatePid
                        where (!a.Deleted && !a.isLocked && a.ParentId == parentId) && (b.LangKey == DefaultLang)
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
                d.CountPost = (from a in _dbContext.PromotionCate_PromotionDetails
                               where a.PromotionCatePid == item.Pid
                               join b in _dbContext.PromotionDetails on a.PromotionDetailPid equals b.Pid
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
                        var model = _dbContext.PromotionCates.Where(p => p.Pid == item).FirstOrDefault();
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

                var model = _dbContext.PromotionCates.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.PromotionCate_PromotionDetails.Where(p => p.PromotionCatePid == Pid).ToList();
                var temmpDetail = _dbContext.PromotionDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.PromotionDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.PromotionDetailPid,
                            }).ToList();
                if (list.Count() > 0)
                {
                    return new { value = false, type = "warning", messError = "has-child" };
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.PromotionCates.Remove(model);
                _dbContext.SaveChanges();
                return new { value = true, type = "success", messError = "" };
            }
            catch (Exception ex)
            {

                return new { value = false, type = "error", messError = ex.Message };
            }

        }
        public dynamic DeleteAll(int Pid)
        {
            try
            {


                var model = _dbContext.PromotionCates.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.PromotionCate_PromotionDetails.Where(p => p.PromotionCatePid == Pid).ToList();
                var temmpDetail = _dbContext.PromotionDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.PromotionDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.PromotionDetailPid,
                            }).ToList();
                //var list = _dbContext.PromotionDetails.Where(p => p.PromotionCatePid == Pid && p.Deleted==false).ToList();
                foreach (var item in list)
                {
                    var temp = _dbContext.PromotionDetails.Where(p => p.Pid == item.Pid).FirstOrDefault();

                    temp.Deleted = true;
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.PromotionCates.Remove(model);
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
                        var model = _dbContext.PromotionCates.Where(p => p.Pid == item && p.isLocked == false).FirstOrDefault();
                        var temmpCate = _dbContext.PromotionCate_PromotionDetails.Where(p => p.PromotionCatePid == item).ToList();
                        var temmpDetail = _dbContext.PromotionDetails.Where(p => p.Deleted == false).ToList();

                        var list = (from p in temmpCate
                                    join e in temmpDetail
                                      on p.PromotionDetailPid equals e.Pid
                                    select new
                                    {
                                        Pid = p.PromotionDetailPid,
                                    }).ToList();
                        if (list.Count() > 0)
                        {
                            return new { value = false, type = "warning", messError = "has-child", pid = item };
                        }
                        //model.Deleted = true;
                        //model.UpdateDate = DateTime.Now;
                        _dbContext.PromotionCates.Remove(model);

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

                var data = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == Pid).ToList();
                var detail = _dbContext.PromotionCates.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var Promotion = _dbContext.PromotionCates.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Promotion.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Name = item.Name;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Pid = item.Pid;
                        child.PromotionCatePid = item.PromotionCatePid;
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
        public dynamic Insert(PromotionCate promotionCate, List<MultiLang_PromotionCate> multiLang_promotionCate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var arr = new List<int>();
                    if (promotionCate.ParentId != 0)
                    {
                        recurGetParentCate(ref arr, promotionCate.ParentId);
                    }
                    var defaultData = multiLang_promotionCate.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    int maxOrder = _dbContext.PromotionCates.Max(x => (int?)x.Order) ?? 1;
                    //promotionCate.Pid = 0;

                    promotionCate.Order = maxOrder + 1;
                    promotionCate.ParentRoute = arr.Count > 0 ? string.Join("_", arr) : "";
                    _dbContext.PromotionCates.Add(promotionCate);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_promotionCate)
                    {
                        string title = string.IsNullOrEmpty(item.Name) ? defaultData.Name : item.Name;

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.PromotionCates
                                         join b in _dbContext.MultiLang_PromotionCates on a.Pid equals b.PromotionCatePid
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

                        item.PromotionCatePid = promotionCate.Pid;
                        _dbContext.MultiLang_PromotionCates.Add(item);
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
        public dynamic Update(PromotionCate promotionCate, List<MultiLang_PromotionCate> multiLang_promotionCate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {

                    var model = _dbContext.PromotionCates.Where(p => p.Pid == promotionCate.Pid).FirstOrDefault();


                    var childCateList = new List<long>();
                    var cateChildren = _dbContext.PromotionCates.Where(p => p.ParentId == model.Pid && !p.Deleted && !p.isLocked).ToList();
                    foreach (var item in cateChildren)
                    {
                        childCateList.Add(item.Pid);
                        recurGetChildrenCate(ref childCateList, item.Pid);
                    }


                    if (childCateList.Contains(promotionCate.ParentId))
                    {
                        messErr = "Bạn không thể thêm như vậy!";
                        return new { status = false, mess = messErr };
                    }


                    var arr = new List<int>();
                    if (promotionCate.ParentId != 0)
                    {
                        recurGetParentCate(ref arr, promotionCate.ParentId);
                    }
                    model.Enabled = promotionCate.Enabled;
                    model.ParentId = promotionCate.ParentId;
                    model.UpdateDate = DateTime.Now;
                    model.ParentRoute = arr.Count > 0 ? string.Join("_", arr) : "";
                    foreach (var item in multiLang_promotionCate)
                    {
                        var tempModel = _dbContext.MultiLang_PromotionCates.Where(p => p.Pid == item.Pid).FirstOrDefault();
                        if (tempModel != null)
                        {
                            if (model.isLocked == false)
                            {
                                #region check exist slug
                                string newSlug = _common.EncodeTitle(item.Name);
                                var existSlug = (from a in _dbContext.PromotionCates
                                                 join b in _dbContext.MultiLang_PromotionCates on a.Pid equals b.PromotionCatePid
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
                            var existSlug = (from a in _dbContext.PromotionCates
                                             join b in _dbContext.MultiLang_PromotionCates on a.Pid equals b.PromotionCatePid
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

                            item.PromotionCatePid = promotionCate.Pid;
                            _dbContext.MultiLang_PromotionCates.Add(item);
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
                            var model = _dbContext.PromotionCates.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.PromotionCates.Max(x => (int?)x.Order) ?? 1;

                            PromotionCate addPromotion = new PromotionCate();
                            addPromotion.Order = maxOrder + 1;
                            addPromotion.Enabled = false;
                            _dbContext.PromotionCates.Add(addPromotion);
                            _dbContext.SaveChanges();
                            List<MultiLang_PromotionCate> detailModel = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == model.Pid).ToList();
                            foreach (MultiLang_PromotionCate itemDetail in detailModel)
                            {

                                MultiLang_PromotionCate temp = new MultiLang_PromotionCate();
                                temp.Name = itemDetail.Name + "(Coppy)";
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                //temp.Slug = itemDetail.Slug;
                                temp.Slug = _common.EncodeTitle(temp.Name);

                                temp.PromotionCatePid = addPromotion.Pid;
                                _dbContext.MultiLang_PromotionCates.Add(temp);
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
                var data = _dbContext.PromotionCates.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.PromotionCates.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.PromotionCates.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.PromotionCates.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var toRow = _dbContext.PromotionCates.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.PromotionCates.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.PromotionCates.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.PromotionCates.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.PromotionCates.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
        public void recurGetParentCate(ref List<int> arr, int childId)
        {
            try
            {
                var parent = _dbContext.PromotionCates.Where(x => !x.Deleted && x.Enabled && !x.isLocked && x.Pid == childId).FirstOrDefault();
                if (parent != null)
                {
                    arr.Add(childId);
                    recurGetParentCate(ref arr, parent.ParentId);
                }
            }
            catch (Exception ex)
            {

            }

        }
        public void recurGetChildrenCate(ref List<long> arr, long parentId)
        {
            try
            {
                var cateChildren = _dbContext.PromotionCates.Where(p => p.ParentId == parentId && !p.Deleted && !p.isLocked).ToList();
                foreach (var cateChild in cateChildren)
                {
                    arr.Add(cateChild.Pid);
                    recurGetChildrenCate(ref arr, cateChild.Pid);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public bool UpdateOrder(long Pid, int order)
        {
            try
            {
                var data = _dbContext.PromotionCates.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
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
