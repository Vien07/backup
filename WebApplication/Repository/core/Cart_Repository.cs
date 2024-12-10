using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using CMS.Areas.Contact.Models;
using X.PagedList;
using System.Globalization;
using Newtonsoft.Json;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Cart;
using DTO.Customer;
using AutoMapper;
using DTO.Product;
using CMS.Services.TranslateServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;
using CMS.Services.EmailServices;

namespace CMS.Repository
{
    public class Cart_Repository : ICart_Repository
    {
        private string DefaultOgImage = "";
        private string PageLimitDetail = "";
        private string PageLimit = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;
        private readonly IEmailServices _emailServices;
        private readonly IMapper _mapper;
        private readonly ITranslateServices _translate;

        private readonly DBContext _dbContext;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Thumb = ConstantStrings.Thumb;
        private string UrlConfigurationImages = ConstantStrings.UrlConfigurationImages;
        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string KeyDefaultOgImage = ConstantStrings.KeyDefaultOgImage;
        private string KeyPageLimitDetail = ConstantStrings.KeyPageLimitDetail;
        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        public Cart_Repository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, ICommonServices core, IMapper mapper, ITranslateServices translate, IEmailServices emailServices)
        {
            _translate = translate;
            _emailServices = emailServices;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _common = core;
            _mapper = mapper;
            DefaultOgImage = _common.GetConfigValue(KeyDefaultOgImage);
            PageLimitDetail = _common.GetConfigValue(KeyPageLimitDetail);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<string> GetCartInfo(string cartStr, string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var cartList = JsonConvert.DeserializeObject<List<CartStringDto>>(cartStr);
                var result = new CartInfoDto();
                result.items = new List<CartItemDto>();

                foreach (var item in cartList)
                {
                    var product = await (from a in _dbContext.ProductDetails
                                         join b in _dbContext.MultiLang_ProductDetails
                                         on a.Pid equals b.ProductDetailPid
                                         where !a.Deleted && a.Enabled && a.Pid == item.productId
                                         select new { title = b.Title, picThumb = UrlProductImages + a.PicThumb, slug = b.Slug, code = a.Code }).FirstOrDefaultAsync();

                    var temp = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == item.productId 
                                                                            && (item.productCateId == 0 || item.productCateId == x.ProductCatePid)).FirstOrDefault();
                    var originalPrice = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == item.productId).FirstOrDefault();
                    CartItemDto cartItemDto = new CartItemDto();
                    cartItemDto.title = product.title;
                    cartItemDto.id = item.id;
                    cartItemDto.code = product.code;
                    cartItemDto.slug = product.slug;
                    cartItemDto.productCateId = temp.ProductCatePid;
                    cartItemDto.price = temp.Price;
                    cartItemDto.priceString = _common.ConvertFormatMoney(cartItemDto.price);
                    cartItemDto.priceOriginal = originalPrice.Price;
                    cartItemDto.priceOriginalString = _common.ConvertFormatMoney(cartItemDto.priceOriginal);
                    cartItemDto.total = cartItemDto.price;
                    cartItemDto.totalString = _common.ConvertFormatMoney(cartItemDto.total);
                    cartItemDto.quantity = item.quantity;
                    cartItemDto.productId = item.productId;
                    cartItemDto.months = _dbContext.ProductCates.Where(p => p.Pid == temp.ProductCatePid).Select(p => p.Months).FirstOrDefault();
                    result.items.Add(cartItemDto);
                }
                result.temporaryPrice = result.items.Sum(x => x.total);
                result.totalPrice = result.temporaryPrice;
                result.temporaryPriceString = _common.ConvertFormatMoney(result.temporaryPrice);
                result.totalPriceString = _common.ConvertFormatMoney(result.totalPrice);

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public async Task<dynamic> SaveOrder(string cartStr, string lang, OrderInformation information)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var customer = _dbContext.Customers.Where(x => x.Email.Equals(information.Email)).FirstOrDefault();
                    var cartDataString = await GetCartInfo(cartStr, lang);

                    var cartData = JsonConvert.DeserializeObject<CartInfoDto>(cartDataString);

                    CMS.Areas.Order.Models.Order order = new CMS.Areas.Order.Models.Order();
                    int months = _dbContext.ProductCates.Where(p => p.Pid == cartData.items[0].productCateId).Select(p => p.Months).FirstOrDefault();
                    Random random = new Random();
                    var id = random.Next(1000, 9999);
                    order.CustomerPid = customer.Pid;
                    order.ProductDetailPid = cartData.items[0].productId;
                    order.ProductCatePid = cartData.items[0].productCateId;
                    order.CustomerPid = customer.Pid;
                    order.State = 0;
                    order.IsPayment = information.IsPayment;
                    order.PaymentMethod = information.PaymentMethod;
                    order.VAT = information.VAT;
                    order.Total = cartData.totalPrice;
                    order.CompanyAddress = information.CompanyAddress;
                    order.CompanyName = information.CompanyName;
                    order.TaxCode = information.TaxCode;
                    order.RegisterDate = DateTime.Now;
                    order.ExpiredDate = DateTime.Now.AddMonths(months);
                    await _dbContext.Orders.AddAsync(order);
                    await _dbContext.SaveChangesAsync();
                    order.Code = id + order.Pid.ToString();
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                    await _emailServices.SendMailOrderCustomer(order.Pid);
                    await _emailServices.SendMailOrderToAdmin(order.Pid);
                    return new { IsError = true, OrderId = order.Pid };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new { IsError = true, OrderId = 0 };
                }
            }
        }
        public async Task<OrderDto> GetOrder(string Pid)
        {
            OrderDto order = new OrderDto();
            try
            {
                
                var orderId = Convert.ToInt64(Pid);
                
                order = await _dbContext.Orders.Where(p => p.Pid == orderId).Select(p => new OrderDto
                {
                    Pid = p.Pid,
                    Total = p.Total,
                    TotalString = _common.ConvertFormatMoney(p.Total),
                    CustomerId = p.CustomerPid,
                }).FirstOrDefaultAsync();
                var customer = _dbContext.Customers.Where(p => p.Pid == order.CustomerId).FirstOrDefault();
                order.Email = customer.Email;
                //order.FullName = customer.FullName;
                order.FullName = customer.FirstName + " " + customer.LastName;
                order.PhoneNumber = customer.PhoneNumber;
                order.FullAddress = _common.GetAddress(customer.Address, _common.GetWard(customer.District, customer.Ward), _common.GetDistrict(customer.Province, customer.District), _common.GetProvince(customer.Province));
                return order;
            }
            catch
            {
                return order;
            }
        }
    }
}
