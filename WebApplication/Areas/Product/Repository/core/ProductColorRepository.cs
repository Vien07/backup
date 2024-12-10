using CMS.Areas.Product.Models;
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

namespace CMS.Areas.Product
{
    public class ProductColorRepository : IProductColorRepository
    {
        private readonly DBContext _dbContext;
        private readonly IFileServices _fileSrv;
        private readonly ICommonServices _common;

        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlProductColorImages = ConstantStrings.UrlProductColorImages;
        private string Fullmages = ConstantStrings.Fullmages;

        public ProductColorRepository(DBContext dbContext, IFileServices fileSrv, ICommonServices common)
        {
            _dbContext = dbContext;
            _fileSrv = fileSrv;
            _common = common;
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
            var data = (from a in _dbContext.ProductColors
                        join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
                        where (!a.Deleted && !a.isLocked && a.ParentId == parentId) && (b.LangKey == DefaultLang)
                        orderby a.Order descending
                        select new
                        {
                            Name = b.Name,
                            Pid = a.Pid,
                            Code = a.Code,
                            Order = a.Order,
                            PicThumb = UrlProductColorImages + a.PicThumb,
                            Enabled = a.Enabled
                        }).ToList();

            foreach (var item in data)
            {
                dynamic d = new ExpandoObject();
                d.Name = prefix + item.Name;
                d.CountPost = (from a in _dbContext.ProductColor_ProductDetails
                               where a.ProductColorPid == item.Pid
                               join b in _dbContext.ProductDetails on a.ProductDetailPid equals b.Pid
                               where b.Deleted == false
                               select new
                               {
                                   Pid = b.Pid
                               }).Distinct().ToList().Count();
                d.Pid = item.Pid;
                d.Order = item.Order;
                d.Code = item.Code;
                d.PicThumb = item.PicThumb;
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
                        var model = _dbContext.ProductColors.Where(p => p.Pid == item).FirstOrDefault();
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

                var model = _dbContext.ProductColors.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.ProductColor_ProductDetails.Where(p => p.ProductColorPid == Pid).ToList();
                var temmpDetail = _dbContext.ProductDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.ProductDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.ProductDetailPid,
                            }).ToList();
                if (list.Count() > 0)
                {
                    return new { value = false, type = "warning", messError = "has-child" };
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.ProductColors.Remove(model);
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


                var model = _dbContext.ProductColors.Where(p => p.Pid == Pid && p.isLocked == false).FirstOrDefault();
                var temmpCate = _dbContext.ProductColor_ProductDetails.Where(p => p.ProductColorPid == Pid).ToList();
                var temmpDetail = _dbContext.ProductDetails.Where(p => p.Deleted == false).ToList();

                var list = (from p in temmpCate
                            join e in temmpDetail
                              on p.ProductDetailPid equals e.Pid
                            select new
                            {
                                Pid = p.ProductDetailPid,
                            }).ToList();
                //var list = _dbContext.ProductDetails.Where(p => p.ProductColorPid == Pid && p.Deleted==false).ToList();
                foreach (var item in list)
                {
                    var temp = _dbContext.ProductDetails.Where(p => p.Pid == item.Pid).FirstOrDefault();

                    temp.Deleted = true;
                }
                //model.Deleted = true;
                //model.UpdateDate = DateTime.Now;
                _dbContext.ProductColors.Remove(model);
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
                        var model = _dbContext.ProductColors.Where(p => p.Pid == item && p.isLocked == false).FirstOrDefault();
                        var temmpCate = _dbContext.ProductColor_ProductDetails.Where(p => p.ProductColorPid == item).ToList();
                        var temmpDetail = _dbContext.ProductDetails.Where(p => p.Deleted == false).ToList();

                        var list = (from p in temmpCate
                                    join e in temmpDetail
                                      on p.ProductDetailPid equals e.Pid
                                    select new
                                    {
                                        Pid = p.ProductDetailPid,
                                    }).ToList();
                        if (list.Count() > 0)
                        {
                            return new { value = false, type = "warning", messError = "has-child", pid = item };
                        }
                        //model.Deleted = true;
                        //model.UpdateDate = DateTime.Now;
                        _dbContext.ProductColors.Remove(model);

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

                var data = _dbContext.MultiLang_ProductColors.Where(p => p.ProductColorPid == Pid).ToList();
                var detail = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var Product = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Product.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Name = item.Name;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.Pid = item.Pid;
                        child.ProductColorPid = item.ProductColorPid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        detail.Enabled,
                        detail.ParentId,
                        detail.Pid,
                        detail.Code,
                        PicThumb = UrlProductColorImages + detail.PicThumb
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
        public dynamic Insert(ProductColor productColor, List<MultiLang_ProductColor> multiLang_ProductColor, IFormFile PicThumb)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var arr = new List<int>();
                    if (productColor.ParentId != 0)
                    {
                        recurGetParentCate(ref arr, productColor.ParentId);
                    }

                    var defaultData = multiLang_ProductColor.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    int maxOrder = _dbContext.ProductColors.Max(x => (int?)x.Order) ?? 1;
                    //ProductColor.Pid = 0;

                    productColor.Order = maxOrder + 1;
                    productColor.ParentRoute = arr.Count > 0 ? string.Join("_", arr) : "";
                    _dbContext.ProductColors.Add(productColor);
                    _dbContext.SaveChanges();

                    foreach (var item in multiLang_ProductColor)
                    {
                        string title = string.IsNullOrEmpty(item.Name) ? defaultData.Name : item.Name;

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.ProductColors
                                         join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
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

                        item.ProductColorPid = productColor.Pid;
                        _dbContext.MultiLang_ProductColors.Add(item);
                    }

                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileSrv.SaveCategoryImage(PicThumb, UrlProductColorImages, defaultData.Name + "-" + productColor.Pid);

                        if (!saveFileStatus.isError)
                        {
                            productColor.PicThumb = saveFileStatus.fileName;
                            _dbContext.SaveChanges();

                        }
                        _dbContext.SaveChanges();
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
        public dynamic Update(ProductColor productColor, List<MultiLang_ProductColor> multiLang_ProductColor, IFormFile PicThumb)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.ProductColors.Where(p => p.Pid == productColor.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_ProductColors.Where(p => p.ProductColorPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Name;


                    var childCateList = new List<long>();
                    var cateChildren = _dbContext.ProductColors.Where(p => p.ParentId == model.Pid && !p.Deleted && !p.isLocked).ToList();
                    foreach (var item in cateChildren)
                    {
                        childCateList.Add(item.Pid);
                        recurGetChildrenCate(ref childCateList, item.Pid);
                    }


                    if (childCateList.Contains(productColor.ParentId))
                    {
                        messErr = "Bạn không thể thêm như vậy!";
                        return new { status = false, mess = messErr };
                    }

                    var arr = new List<int>();
                    if (productColor.ParentId != 0)
                    {
                        recurGetParentCate(ref arr, productColor.ParentId);
                    }

                    model.Enabled = productColor.Enabled;
                    model.ParentId = productColor.ParentId;
                    model.UpdateDate = DateTime.Now;
                    model.Code = productColor.Code;
                    model.ParentRoute = arr.Count > 0 ? string.Join("_", arr) : "";

                    foreach (var item in multiLang_ProductColor)
                    {
                        var tempModel = _dbContext.MultiLang_ProductColors.Where(p => p.Pid == item.Pid).FirstOrDefault();
                        if (tempModel != null)
                        {
                            if (model.isLocked == false)
                            {
                                #region check exist slug
                                string newSlug = _common.EncodeTitle(item.Name);
                                var existSlug = (from a in _dbContext.ProductColors
                                                 join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
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
                            var existSlug = (from a in _dbContext.ProductColors
                                             join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
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

                            item.ProductColorPid = productColor.Pid;
                            _dbContext.MultiLang_ProductColors.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }

                    if (PicThumb != null)
                    {
                        dynamic kt = _fileSrv.SaveCategoryImage(PicThumb, UrlProductColorImages, nameImages);
                        if (!kt.isError)
                        {
                            _fileSrv.DeleteFile(UrlProductColorImages, model.PicThumb);
                            _fileSrv.DeleteFile(UrlProductColorImages, Fullmages + model.PicThumb);
                            model.PicThumb = kt.fileName;
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
                            var model = _dbContext.ProductColors.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.ProductColors.Max(x => (int?)x.Order) ?? 1;

                            ProductColor addProduct = new ProductColor();
                            addProduct.Order = maxOrder + 1;
                            addProduct.Enabled = false;
                            _dbContext.ProductColors.Add(addProduct);
                            _dbContext.SaveChanges();
                            List<MultiLang_ProductColor> detailModel = _dbContext.MultiLang_ProductColors.Where(p => p.ProductColorPid == model.Pid).ToList();
                            foreach (MultiLang_ProductColor itemDetail in detailModel)
                            {

                                MultiLang_ProductColor temp = new MultiLang_ProductColor();
                                temp.Name = itemDetail.Name + "(Coppy)";
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                //temp.Slug = itemDetail.Slug;
                                temp.Slug = _common.EncodeTitle(temp.Name);

                                temp.ProductColorPid = addProduct.Pid;
                                _dbContext.MultiLang_ProductColors.Add(temp);
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
                var data = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.ProductColors.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.ProductColors.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var toRow = _dbContext.ProductColors.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.ProductColors.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.ProductColors.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.ProductColors.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.ProductColors.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
        public bool SaveShowHome(long Pid, bool value)
        {
            try
            {
                var data = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();
                if (data != null)
                {
                    data.IsShowHome = value;
                    _dbContext.SaveChanges();
                }
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
                var parent = _dbContext.ProductColors.Where(x => !x.Deleted && x.Enabled && !x.isLocked && x.Pid == childId).FirstOrDefault();
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
                var cateChildren = _dbContext.ProductColors.Where(p => p.ParentId == parentId && !p.Deleted && !p.isLocked).ToList();
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
                var data = _dbContext.ProductColors.Where(p => p.Pid == Pid).FirstOrDefault();
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
