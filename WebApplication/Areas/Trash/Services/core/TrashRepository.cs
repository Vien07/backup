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

namespace CMS.Areas.Trash
{
    public class TrashRepository : ITrashRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private int CustomerId = ConstantStrings.CustomerId;
        private int FAQId = ConstantStrings.FAQId;
        private int FeatureId = ConstantStrings.FeatureId;
        private int GalleryId = ConstantStrings.GalleryId;
        private int NewsId = ConstantStrings.NewsId;
        private int AboutId = ConstantStrings.AboutId;
        private int ProductId = ConstantStrings.ProductId;
        private int RecruitmentId = ConstantStrings.RecruitmentId;
        private string DefaultLang = ConstantStrings.DefaultLang;

        public TrashRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices common)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _common = common;
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {

                List<dynamic> listData = new List<dynamic>();
                if (search.Cate == null || search.Cate == "0")
                {
                    listData.AddRange(GetDataAbout());
                    listData.AddRange(GetDataProduct());
                    listData.AddRange(GetDataCustomer());
                    listData.AddRange(GetDataFeature());
                    listData.AddRange(GetDataNews());

                }
                else
                {
                    if (search.Cate == AboutId.ToString())
                    {
                        listData.AddRange(GetDataAbout());
                    }
                    else if (search.Cate == ProductId.ToString())
                    {
                        listData.AddRange(GetDataProduct());
                    }
                    else if (search.Cate == CustomerId.ToString())
                    {
                        listData.AddRange(GetDataCustomer());
                    }
                    else if (search.Cate == FeatureId.ToString())
                    {
                        listData.AddRange(GetDataFeature());
                    }
                    else if (search.Cate == GalleryId.ToString())
                    {
                        listData.AddRange(GetDataGallery());
                    }
                    else if (search.Cate == NewsId.ToString())
                    {
                        listData.AddRange(GetDataNews());
                    }
                    else if (search.Cate == FAQId.ToString())
                    {
                        listData.AddRange(GetDataFAQ());
                    }
                    else if (search.Cate == RecruitmentId.ToString())
                    {
                        listData.AddRange(GetDataRecruitment());
                    }
                }
                if (search.Key != null)
                {
                    var listSearch = new List<dynamic>();
                    //listData = listData.Where(p => p.Title.Contains(search.Key) ||
                    //       p.Description.Contains(search.Key) ||
                    //       p.Content.Contains(search.Key)).ToList();
                    listData.ForEach(item =>
                    {
                        var title = item.Title.ToLower();
                        var description = "";
                        var content = "";
                        try
                        {
                            description = item.Description.ToLower();
                        }
                        catch
                        {
                            description = "";
                        }

                        try
                        {
                            content = item.Content.ToLower();
                        }
                        catch
                        {
                            content = "";
                        }

                        if (_common.RemoveSign4VietnameseString(title).Contains(_common.RemoveSign4VietnameseString(search.Key.ToLower()))
                            || _common.RemoveSign4VietnameseString(description).Contains(_common.RemoveSign4VietnameseString(search.Key.ToLower()))
                            || _common.RemoveSign4VietnameseString(content).Contains(_common.RemoveSign4VietnameseString(search.Key.ToLower())))
                        {
                            listSearch.Add(item);
                        }
                    });
                    listData = listSearch;
                }


                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.UpdateDate), search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);

                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };
                return new { jsData = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public List<dynamic> GetDataAbout()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.AboutDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Title = dataLang.Title;
                        obj.Cate = "Giới thiệu";
                        obj.CateId = AboutId;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataNews()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.NewsDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Tin tức";
                        obj.CateId = NewsId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataCustomer()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.Customers.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Khách hàng";
                        obj.CateId = CustomerId;
                        obj.Title = item.Email;
                        obj.Description = item.Email;
                        obj.Content = item.Email;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataFeature()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.FeatureDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_FeatureDetails.Where(p => p.FeatureDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Tính năng";
                        obj.CateId = FeatureId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataGallery()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.GalleryDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_GalleryDetails.Where(p => p.GalleryDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Thư viện";
                        obj.CateId = GalleryId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataFAQ()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.FAQDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "FAQ's";
                        obj.CateId = FAQId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataProduct()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.ProductDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Sản phẩm";
                        obj.CateId = ProductId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }
        public List<dynamic> GetDataRecruitment()
        {
            List<dynamic> d = new List<dynamic>();

            try
            {
                var data = _dbContext.RecruitmentDetails.Where(p => p.Deleted == true).ToList();

                foreach (var item in data)
                {
                    try
                    {
                        var dataLang = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                        dynamic obj = new ExpandoObject();
                        obj.Pid = item.Pid;
                        obj.Cate = "Tuyển dụng";
                        obj.CateId = RecruitmentId;
                        obj.Title = dataLang.Title;
                        obj.Description = dataLang.Description;
                        obj.Content = dataLang.Content;
                        obj.UpdateDate = item.UpdateDate;
                        d.Add(obj);
                    }
                    catch (Exception)
                    {

                    }

                }
                return d;
            }
            catch (Exception)
            {
                return d;
            }
        }

        public dynamic Undo(int pid, int cateId)
        {
            try
            {
                if (cateId == AboutId)
                {
                    var data = _dbContext.AboutDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == ProductId)
                {
                    var data = _dbContext.ProductDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == NewsId)
                {
                    var data = _dbContext.NewsDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == CustomerId)
                {
                    var data = _dbContext.Customers.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == FeatureId)
                {
                    var data = _dbContext.FeatureDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == FAQId)
                {
                    var data = _dbContext.FAQDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == RecruitmentId)
                {
                    var data = _dbContext.RecruitmentDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }
                else if (cateId == GalleryId)
                {
                    var data = _dbContext.GalleryDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    data.Deleted = false;
                }

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public dynamic Delete(int pid, int cateId)
        {
            try
            {
                if (cateId == AboutId)
                {
                    var data = _dbContext.AboutDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_AboutDetails.RemoveRange(temp);
                    _dbContext.AboutDetails.Remove(data);
                }
                else if (cateId == ProductId)
                {
                    var data = _dbContext.ProductDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_ProductDetails.RemoveRange(temp);
                    _dbContext.ProductDetails.Remove(data);
                }
                else if (cateId == NewsId)
                {
                    var data = _dbContext.NewsDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_NewsDetails.RemoveRange(temp);
                    _dbContext.NewsDetails.Remove(data);
                }
                else if (cateId == FAQId)
                {
                    var data = _dbContext.FAQDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_FAQDetails.RemoveRange(temp);
                    _dbContext.FAQDetails.Remove(data);
                }
                else if (cateId == RecruitmentId)
                {
                    var data = _dbContext.RecruitmentDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_RecruitmentDetails.RemoveRange(temp);
                    _dbContext.RecruitmentDetails.Remove(data);
                }
                else if (cateId == FeatureId)
                {
                    var data = _dbContext.FeatureDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_FeatureDetails.Where(p => p.FeatureDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_FeatureDetails.RemoveRange(temp);
                    _dbContext.FeatureDetails.Remove(data);
                }
                else if (cateId == GalleryId)
                {
                    var data = _dbContext.GalleryDetails.Where(p => p.Pid == pid).FirstOrDefault();
                    var temp = _dbContext.MultiLang_GalleryDetails.Where(p => p.GalleryDetailPid == data.Pid).ToList();
                    _dbContext.MultiLang_GalleryDetails.RemoveRange(temp);
                    _dbContext.GalleryDetails.Remove(data);
                }
                else if (cateId == CustomerId)
                {
                    var data = _dbContext.Customers.Where(p => p.Pid == pid).FirstOrDefault();
                    _dbContext.Customers.Remove(data);
                }

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public dynamic DeleteAll()
        {
            try
            {

                var dataAbout = _dbContext.AboutDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataAbout)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_AboutDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }

                }
                _dbContext.AboutDetails.RemoveRange(dataAbout);

                var dataProduct = _dbContext.ProductDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataProduct)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_ProductDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.ProductDetails.RemoveRange(dataProduct);

                var dataNews = _dbContext.NewsDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataNews)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_NewsDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.NewsDetails.RemoveRange(dataNews);

                var dataGallery = _dbContext.GalleryDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataGallery)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_GalleryDetails.Where(p => p.GalleryDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_GalleryDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.GalleryDetails.RemoveRange(dataGallery);

                var dataFeature = _dbContext.FeatureDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataFeature)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FeatureDetails.Where(p => p.FeatureDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_FeatureDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.FeatureDetails.RemoveRange(dataFeature);

                var dataCustomer = _dbContext.Customers.Where(p => p.Deleted == true).ToList();
                _dbContext.Customers.RemoveRange(dataCustomer);

                var dataFAQ = _dbContext.FAQDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataFAQ)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FAQDetails.Where(p => p.FAQDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_FAQDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.FAQDetails.RemoveRange(dataFAQ);

                var dataRecruitment = _dbContext.RecruitmentDetails.Where(p => p.Deleted == true).ToList();
                foreach (var item in dataRecruitment)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_RecruitmentDetails.Where(p => p.RecruitmentDetailPid == item.Pid).ToList();
                        _dbContext.MultiLang_RecruitmentDetails.RemoveRange(temp);
                    }
                    catch
                    {

                    }
                }
                _dbContext.RecruitmentDetails.RemoveRange(dataRecruitment);

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
