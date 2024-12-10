using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Contact.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Contact
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ICommonServices _common;
        private string messErr = "";
        private string BannerBranchWidth = "0";

        //private string UrlBranchImages = ConstantStrings.UrlBranchImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        //private string KeyBannerBranchWidth = ConstantStrings.KeyBannerBranchWidth;
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private int NewsId = ConstantStrings.NewsId;
        private int RootNewsCatePid = ConstantStrings.RootNewsCatePid;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        public BranchRepository(DBContext dbContext, IFileServices fileServices, ICommonServices common)
        {
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
            //BannerBranchWidth = _common.GetConfigValue(KeyBannerBranchWidth);
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                //var data = _dbContext.ProjectDetails.Where(p => p.Deleted != true).ToList();
                var listProjectPid = _dbContext.MultiLang_Branchs.Select(p => new { p.BranchPid, p.Title, p.Address })
                    .Distinct().ToList().FilterSearch(new string[] { "Address", "Title" }, search.Key).Select(x => x.BranchPid).Distinct().ToList();
                List<Models.Branch> data = new List<Models.Branch>();

                foreach (var item in listProjectPid)
                {
                    var temp = _dbContext.Branchs.Where(p => (p.Enabled == search.Enable || search.Enable == null)
                                                                && p.Deleted != true && p.Pid == item).FirstOrDefault();
                    if (temp != null)
                    {
                        data.Add(temp);
                    }
                }
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_Branchs.Where(p => p.LangKey == DefaultLang && p.BranchPid == item.Pid).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == item.Pid).FirstOrDefault();

                    }
                    dynamic child = new ExpandoObject();
                    child.Title = temp.Title;
                    //child.Images = UrlBranchImages + Fullmages + item.Images;
                    //child.ImagesName = Fullmages + item.Images;
                    child.Link = item.Link;
                    child.PhoneNumber = item.PhoneNumber;
                    child.Address = temp.Address;
                    //child.Description = temp.Description;
                    //if (temp.Description.Length > 200)
                    //{
                    //    child.Description = temp.Description.Substring(0, 150) + "...";
                    //}

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
                        var model = _dbContext.Branchs.Where(p => p.Pid == item).FirstOrDefault();
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
                var model = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();
                //_fileServices.DeleteFile(UrlBranchImages, Fullmages + model.Images);
                var multiLang = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == Pid).ToList();
                _dbContext.MultiLang_Branchs.RemoveRange(multiLang);
                _dbContext.Branchs.Remove(model);
                _dbContext.SaveChanges();
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
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Branchs.Where(p => p.Pid == item).FirstOrDefault();
                        //_fileServices.DeleteFile(UrlBranchImages, Fullmages + model.Images);
                        var multiLang = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == item).ToList();
                        _dbContext.MultiLang_Branchs.RemoveRange(multiLang);
                        _dbContext.Branchs.Remove(model);
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
        public dynamic Edit(long Pid)
        {
            try
            {

                var data = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == Pid).ToList();
                var detail = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var branch = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (branch.Deleted != true)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.LangKey = item.LangKey;
                        //child.Description = item.Description;
                        child.Address = item.Address;
                        child.Pid = item.Pid;
                        child.BranchPid = item.BranchPid;
                        listData.Add(child);
                    }
                }
                return new
                {
                    detail = new
                    {
                        //picThumb = UrlBranchImages + Fullmages + detail.Images,
                        //ImagesName = Fullmages + detail.Images,
                        detail.Link,
                        detail.PhoneNumber,
                        detail.Enabled,
                        detail.Pid,
                        //detail.Position
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
        public dynamic Insert(Models.Branch branch, List<MultiLang_Branch> multiLang_Branch, IFormFile images)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    var defaultData = multiLang_Branch.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    //int maxOrder = _dbContext.Branchs.Select(p => p.Order).DefaultIfEmpty(0).Max();
                    int maxOrder = _dbContext.Branchs.Max(x => (int?)x.Order) ?? 1;

                    branch.Order = maxOrder + 1;
                    //dynamic kt = _fileServices.SaveFileNotResizeWidth(images, UrlBranchImages, defaultData.Title, Convert.ToInt32(BannerBranchWidth));

                    //if (!kt.isError)
                    //{
                    //    branch.Images = kt.fileName;
                    //}
                    _dbContext.Branchs.Add(branch);
                    _dbContext.SaveChanges();
                    foreach (var item in multiLang_Branch)
                    {
                        // string title = string.IsNullOrEmpty(item.Tite) ? defaultData.Tite : item.Name;
                        item.BranchPid = branch.Pid;
                        _dbContext.MultiLang_Branchs.Add(item);
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
        public dynamic Update(Models.Branch branch, List<MultiLang_Branch> multiLang_Branch, IFormFile images)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var defaultData = multiLang_Branch.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    var model = _dbContext.Branchs.Where(p => p.Pid == branch.Pid).FirstOrDefault();
                    //if (images != null)
                    //{
                    //    _fileServices.DeleteFile(UrlBranchImages, Fullmages + model.Images);
                    //    dynamic kt = _fileServices.SaveFileNotResizeWidth(images, UrlBranchImages, defaultData.Title, Convert.ToInt32(BannerBranchWidth));

                    //    if (!kt.isError)
                    //    {
                    //        model.Images = kt.fileName;
                    //    }
                    //}
                    model.Link = branch.Link;
                    model.PhoneNumber = branch.PhoneNumber;
                    //model.Position = branch.Position;
                    model.Enabled = branch.Enabled;



                    _dbContext.SaveChanges();
                    #region multi lang
                    foreach (var item in multiLang_Branch)
                    {
                        var multiModel = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == branch.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            multiModel.Title = item.Title;
                            //multiModel.Description = item.Description;
                            multiModel.Address = item.Address;
                        }
                        else
                        {
                            var tempdefaultData = _dbContext.MultiLang_Branchs.Where(p => p.BranchPid == branch.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? tempdefaultData.Title : item.Title;
                            item.Title = title;
                            item.BranchPid = branch.Pid;
                            _dbContext.MultiLang_Branchs.Add(item);
                        }
                    }
                    _dbContext.SaveChanges();

                    #endregion


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
        public bool Up(long Pid)
        {
            try
            {
                int temp = 0;
                var data = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Branchs.Where(p => p.Order > data.Order).OrderBy(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();
                temp = data.Order;
                var changeData = _dbContext.Branchs.Where(p => p.Order < data.Order).OrderByDescending(p => p.Order).FirstOrDefault();
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
                var data = _dbContext.Branchs.Where(p => p.Pid == Pid).FirstOrDefault();
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
                var toRow = _dbContext.Branchs.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.Branchs.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.Branchs.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.Branchs.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
                    var listData = _dbContext.Branchs.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
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
