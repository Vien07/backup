using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using CMS.Areas.Product.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using static CMS.Services.ExtensionServices;
using CMS.Areas.Shared.Models;
using Newtonsoft.Json;
using DTO.Product;
using CMS.Areas.News.Models;
using CMS.Services.TranslateServices;

namespace CMS.Areas.Product
{
    public class ProductCommentRepository : IProductCommentRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ITranslateServices _translate;
        private readonly ICommonServices _common;

        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int ProductId = ConstantStrings.ProductId;
        private int RootProductCatePid = ConstantStrings.RootProductCatePid;

        public ProductCommentRepository(DBContext dbContext, IFileServices fileSrv, ICommonServices common, ITranslateServices translate)
        {
            _dbContext = dbContext;
            _fileServices = fileSrv;
            _common = common;
            _translate = translate;
        }
        public dynamic LoadData(SearchDto SearchDto)
        {
            try
            {
                var listBookPid = _dbContext.ProductComments.Select(p => new { p.Pid, p.Comment })
                    .Distinct().ToList().FilterSearch(new string[] { "Comment" }, SearchDto.Key).Select(x => x.Pid).Distinct().ToList();

                var data = (from a in _dbContext.Customers
                            join b in _dbContext.ProductComments on a.Pid equals b.CustomerPid
                            join c in _dbContext.ProductDetails on b.ProductDetailPid equals c.Pid
                            join d in _dbContext.MultiLang_ProductDetails on c.Pid equals d.ProductDetailPid
                            where (b.Enabled == SearchDto.Enable || SearchDto.Enable == null) && (!a.Deleted && a.Enabled)
                            && (!b.Deleted && listBookPid.Contains(b.Pid)) && (!c.Deleted && c.Enabled) && (d.LangKey == DefaultLang)
                            select new
                            {
                                Name = a.LastName + " " + a.FirstName,
                                CreateDate = b.CreateDate,
                                Comment = b.Comment,
                                Enabled = b.Enabled,
                                ProductName = d.Title,
                                Slug = d.Slug,
                                Star = b.Star,
                                Pid = b.Pid,
                            }).ToList();

                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Comment = item.Comment;
                    child.Name = item.Name;
                    child.ProductName = item.ProductName;
                    child.Star = item.Star;
                    child.CreateDate = item.CreateDate;
                    child.Pid = item.Pid;
                    child.Enabled = item.Enabled;
                    child.Slug = _common.GetConfigValue(ConstantStrings.KeyRootDomain) + _translate.GetUrl("url.product") + item.Slug + ".html";
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.CreateDate), SearchDto.Page, SearchDto.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);
                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = SearchDto.Page,
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
                        var model = _dbContext.ProductComments.Where(p => p.Pid == item).FirstOrDefault();
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

                var model = _dbContext.ProductComments.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
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
                var model = _dbContext.ProductComments.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                _dbContext.SaveChanges();
                return new { value = true };
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
                        var model = _dbContext.ProductComments.Where(p => p.Pid == item).FirstOrDefault();
                        model.Deleted = true;
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return new { value = false, type = "error" };

                    }
                }
                return new { value = true };
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
    }
}
