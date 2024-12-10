using CMS.Services.CommonServices;
using DTO;
using DTO.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Shared.Controllers
{
    [Area("Shared")]
    public class GetSelectController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ICommonServices _common;
        public GetSelectController(DBContext dBContext, ICommonServices common)
        {
            _dbContext = dBContext;
            _common = common;
        }
        public JsonResult GroupUser()
        {
            try
            {
                var data = _dbContext.GroupUsers.Where(p => p.View == true && p.Deleted == false && p.Enabled == true).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Value = item.Code;
                    child.Name = item.Name;
                    listData.Add(child);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult Page(string lang)
        {
            try
            {
                var data = _dbContext.Pages.Where(p => p.Deleted == false && p.Enabled).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_Pages.Where(p => p.PagePid == item.Pid
                                                                && p.LangKey == lang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_Pages.Where(p => p.PagePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Title;
                    listData.Add(child);


                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult ProductCateParent(string lang)
        {
            try
            {
                var data = _dbContext.ProductCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurProductCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult ProductCate(string lang)
        {
            try
            {
                var data = _dbContext.ProductCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.ProductCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurProductCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurProductCate(ref List<dynamic> newsCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.ProductCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurProductCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        newsCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult ProductColor(string lang)
        {
            try
            {
                var data = _dbContext.ProductColors.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_ProductColors.Where(p => p.ProductColorPid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_ProductColors.Where(p => p.ProductColorPid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;
                    listData.Add(child);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult NewsCateParent(string lang)
        {
            try
            {
                var data = _dbContext.NewsCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurNewsCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult NewsCate(string lang)
        {
            try
            {
                var data = _dbContext.NewsCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.NewsCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurNewsCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurNewsCate(ref List<dynamic> newsCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.NewsCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_NewsCates.Where(p => p.NewsCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurNewsCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        newsCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult GalleryCateParent(string lang)
        {
            try
            {
                var data = _dbContext.GalleryCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurGalleryCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult GalleryCate(string lang)
        {
            try
            {
                var data = _dbContext.GalleryCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.GalleryCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurGalleryCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurGalleryCate(ref List<dynamic> galleryCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.GalleryCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_GalleryCates.Where(p => p.GalleryCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurGalleryCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        galleryCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult DiscountCodeCateParent(string lang)
        {
            try
            {
                var data = _dbContext.DiscountCodeCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurDiscountCodeCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult DiscountCodeCate(string lang)
        {
            try
            {
                var data = _dbContext.DiscountCodeCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.DiscountCodeCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurDiscountCodeCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurDiscountCodeCate(ref List<dynamic> discountCodeCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.DiscountCodeCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_DiscountCodeCates.Where(p => p.DiscountCodeCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurDiscountCodeCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        discountCodeCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult FAQCateParent(string lang)
        {
            try
            {
                var data = _dbContext.FAQCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurFAQCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult FAQCate(string lang)
        {
            try
            {
                var data = _dbContext.FAQCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.FAQCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurFAQCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurFAQCate(ref List<dynamic> faqCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.FAQCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_FAQCates.Where(p => p.FAQCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurFAQCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        faqCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult FeatureCateParent(string lang)
        {
            try
            {
                var data = _dbContext.FeatureCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurFeatureCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult FeatureCate(string lang)
        {
            try
            {
                var data = _dbContext.FeatureCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.FeatureCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurFeatureCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurFeatureCate(ref List<dynamic> serviceCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.FeatureCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_FeatureCates.Where(p => p.FeatureCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurFeatureCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        serviceCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult PromotionCateParent(string lang)
        {
            try
            {
                var data = _dbContext.PromotionCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurPromotionCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        listData.Add(child);
                    }
                    catch
                    {

                    }
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult PromotionCate(string lang)
        {
            try
            {
                var data = _dbContext.PromotionCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == 0).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid
                                                                && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid).FirstOrDefault();
                    }

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;

                    var dataChild = _dbContext.PromotionCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == item.Pid).ToList();
                    List<dynamic> listDataChildren = new List<dynamic>();
                    RecurPromotionCate(ref listDataChildren, lang, item.Pid);
                    child.Children = listDataChildren;
                    listData.Add(child);

                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public void RecurPromotionCate(ref List<dynamic> newsCates, string lang, long cate)
        {
            try
            {
                var data = _dbContext.PromotionCates.Where(p => p.Deleted == false && p.isLocked == false && p.ParentId == cate).ToList();
                foreach (var item in data)
                {
                    try
                    {
                        var temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid
                                            && p.LangKey == ConstantStrings.DefaultLang).FirstOrDefault();
                        if (temp == null)
                        {
                            temp = _dbContext.MultiLang_PromotionCates.Where(p => p.PromotionCatePid == item.Pid).FirstOrDefault();
                        }
                        var list = new List<dynamic>();
                        RecurPromotionCate(ref list, lang, item.Pid);
                        dynamic child = new ExpandoObject();
                        child.Value = item.Pid;
                        child.Name = temp.Name;
                        child.Children = list;
                        newsCates.Add(child);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult OrderState()
        {
            try
            {
                var data = Enum.GetNames(typeof(EnumOrder.OrderState));
                List<dynamic> listData = new List<dynamic>();
                for (int i = 0; i < data.Length; i++)
                {
                    dynamic d = new ExpandoObject();
                    d.Value = i;
                    d.Name = data[i];
                    listData.Add(d);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult PaymentMethod()
        {
            try
            {
                var data = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                List<dynamic> listData = new List<dynamic>();
                for (int i = 0; i < data.Length; i++)
                {
                    dynamic d = new ExpandoObject();
                    d.Value = i;
                    d.Name = data[i];
                    listData.Add(d);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult EmailCustomer(string lang)
        {
            try
            {
                //if (lang == null)
                //{
                //    lang = DefaultLang;
                //}
                var data = _dbContext.Customers.Where(p => p.Deleted == false && p.Enabled == true).Select(x => new { x.Email, x.FirstName, x.LastName, x.FullName, x.Pid}).OrderByDescending(x=> x.Pid).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Value = item.Email;
                    //child.Name = item.FullName + " (" + item.Email + ")";
                    child.Name = item.FirstName + " " + item.LastName + " (" + item.Email + ")";
                    listData.Add(child);


                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }

        public JsonResult GetProduct(string lang)
        {
            try
            {
                //if (lang == null)
                //{
                //    lang = DefaultLang;
                //}
                var data = _dbContext.ProductDetails.Where(p => p.Deleted == false && p.Enabled == true).OrderByDescending(x => x.Order).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid
                                                              && p.LangKey == lang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid).FirstOrDefault();
                    }
                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Title;
                    child.MaxCard = item.UserAmount;
                    //child.Lock = item.Lock;
                    listData.Add(child);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult GetProductCate(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = "vi";
                }
                var data = _dbContext.ProductCates.Where(p => p.Deleted == false && p.Enabled == true && !p.isLocked).OrderBy(p=>p.Months).ToList();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    var temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid
                                                              && p.LangKey == lang).FirstOrDefault();
                    if (temp == null)
                    {
                        temp = _dbContext.MultiLang_ProductCates.Where(p => p.ProductCatePid == item.Pid).FirstOrDefault();
                    }
                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = temp.Name;
                    child.Months = item.Months;
                    listData.Add(child);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata});
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult GetPaymentMethod(string lang)
        {
            try
            {
                var data = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                List<dynamic> listData = new List<dynamic>();
                for (int i = 0; i < data.Length; i++)
                {
                    dynamic d = new ExpandoObject();
                    d.Value = i;
                    d.Name = data[i];
                    listData.Add(d);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
        public JsonResult GetPaymentStatus(string lang)
        {
            try
            {
                var data = Enum.GetNames(typeof(EnumOrder.PaymentStatus));
                List<dynamic> listData = new List<dynamic>();
                for (int i = 0; i < data.Length; i++)
                {
                    dynamic d = new ExpandoObject();
                    d.Value = i;
                    d.Name = data[i];
                    listData.Add(d);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }

        }
        public async Task<JsonResult> GetProductCateWithPrice(string lang, int productId)
        {
            try
            {
                if (lang == null)
                {
                    lang = "vi";
                }
                var data = await (from a in _dbContext.ProductCates
                                   join b in _dbContext.ProductCate_ProductDetails on a.Pid equals b.ProductCatePid
                                   join c in _dbContext.MultiLang_ProductCates on a.Pid equals c.ProductCatePid
                                   where !a.Deleted && a.Enabled && !a.isLocked && c.LangKey == lang && b.ProductDetailPid == productId && b.Enable
                                   select new
                                   {
                                       Pid = a.Pid,
                                       Name = c.Name,
                                       Months = a.Months,
                                       Price = b.Price,
                                   }).OrderBy(p => p.Months).ToListAsync();
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {

                    dynamic child = new ExpandoObject();
                    child.Value = item.Pid;
                    child.Name = item.Name;
                    child.Months = item.Months;
                    child.PriceString = _common.ConvertFormatMoney(item.Price);
                    listData.Add(child);
                }
                var jsdata = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
                return Json(new { jsData = jsdata });
            }
            catch (Exception ex)
            {
                return Json(new { jsData = "[]" });
            }
        }
    }
}