using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using DTO.News;
using DTO;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using static CMS.Services.ExtensionServices;
using DTO.Product;
using System;
using CMS.Services.CommonServices;

namespace CMS.Repository
{
    public class TagKey_Repository : ITagKey_Repository
    {
        private readonly DBContext _dbContext;
        private readonly ICommonServices _common;
        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        public TagKey_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
        }

        public async Task<List<NewsDto>> GetListNews(string lang, string key)
        {
            try
            {
                var keyword = "";
                if (!string.IsNullOrEmpty(key))
                {
                    keyword = key.ToLower();
                }
                CultureInfo vi = new CultureInfo(lang);
                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   && (a.SlugTagKey.Contains(key))
                                   orderby a.CreateDate descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                   }).ToListAsync();
                return model.DistinctBy(x => x.Pid).ToList(); ;
            }
            catch
            {
                return new List<NewsDto>();
            }
        }
        public async Task<List<ProductDto>> GetListProduct(string lang, string key)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var firstOption = _dbContext.ProductOptions.Where(x => x.Enabled && !x.Deleted).OrderByDescending(x => x.Order).FirstOrDefault();

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   join c in _dbContext.ProductOption_ProductDetails on a.Pid equals c.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang) && c.ProductOptionPid == firstOption.Pid && a.SlugTagKey.Contains(key)
                                   orderby a.Order descending
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Code = a.Code,
                                       Slug = b.Slug,
                                       CreateDate = a.CreateDate.Value,
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       IsHot = a.IsHot,
                                       IsNew = a.IsNew,
                                       Price = c.Price,
                                       PriceDiscount = c.PriceDiscount,
                                       PriceString = _common.ConvertFormatMoney(c.Price),
                                       PriceDiscountString = _common.ConvertFormatMoney(c.PriceDiscount)
                                   }).ToListAsync();

                return model.DistinctBy(x => x.Pid).ToList();
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            }
        }

    }
}
