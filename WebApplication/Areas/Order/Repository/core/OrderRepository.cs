using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;
using CMS.Services;
using DTO.Cart;
using CMS.Areas.Order.Models;
using static CMS.Services.ExtensionServices;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CMS.Services.EmailServices;
using CMS.Areas.Customer.Models;
using MimeKit;
using System.Security.Policy;
using System.IO;
using SelectPdf;
using System.IO.Compression;
using Microsoft.AspNetCore.Hosting.Server;
using System.Threading;
using QRCoder;
using System.Drawing;

using CMS.Services.TranslateServices;

namespace CMS.Areas.Order
{
    public class OrderRepository : IOrderRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly ICommonServices _common;
        private readonly IEmailServices _emailServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslateServices _translate;

        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlOrderImages = ConstantStrings.UrlOrderImages;
        private string UrlDocs = ConstantStrings.UrlDocs;

        public OrderRepository(DBContext dbContext,
                             IFileServices fileHelper, ICommonServices common, IHttpContextAccessor httpContextAccessor, IEmailServices emailServices, ITranslateServices translate)
        {
            _dbContext = dbContext;
            _fileServices = fileHelper;
            _common = common;
            _httpContextAccessor = httpContextAccessor;
            _emailServices = emailServices;
            _translate = translate;
        }
        public dynamic LoadData(SearchDto SearchDto)
        {
            try
            {
                var enumStateOrderList = new Dictionary<int, string>();
                var stateOrderList = Enum.GetNames(typeof(EnumOrder.OrderState));
                for (var i = 0; i < stateOrderList.Length; i++)
                {
                    enumStateOrderList.Add(i, stateOrderList[i]);
                }

                var enumPaymentMethodList = new Dictionary<int, string>();
                var paymentMethodList = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                for (var i = 0; i < paymentMethodList.Length; i++)
                {
                    enumPaymentMethodList.Add(i, paymentMethodList[i]);
                }

                var data = (from a in _dbContext.Orders
                            where a.Deleted == false
                            select new
                            {
                                Pid = a.Pid,
                                Status = enumStateOrderList.GetValueOrDefault(a.State),
                                StatusId = a.State,
                                IsPaymentId = a.IsPayment,
                                PaymentMethodId = a.PaymentMethod,
                                IsPayment = a.IsPayment ? "Đã thanh toán" : "Chưa thanh toán",
                                PaymentMethod = enumPaymentMethodList.GetValueOrDefault(a.PaymentMethod),
                                Note = a.Note,
                                TotalString = _common.ConvertFormatMoney(a.Total),
                                ShipFeeString = _common.ConvertFormatMoney(a.ShipFee),
                                DepositString = _common.ConvertFormatMoney(a.Deposit),
                                Enabled = a.Enabled,
                                Customer = _dbContext.Customers.Where(x => x.Pid == a.CustomerPid).FirstOrDefault(),
                                CreateDate = a.CreateDate,
                                CreateUser = a.CreateUser,
                                UpdateUser = a.UpdateUser,
                            }).ToList();
                var result = new List<dynamic>();

                if (!string.IsNullOrEmpty(SearchDto.Key))
                {
                    var keySearch = RemoveSign4VietnameseString(SearchDto.Key).ToLower();
                    foreach (var item in data)
                    {
                        if (RemoveSign4VietnameseString(item.Customer.FirstName + " " + item.Customer.LastName).ToLower().Contains(keySearch) || item.Pid.ToString().Contains(keySearch))
                        {
                            result.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        result.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(SearchDto.Cate))
                {
                    var catePid = Convert.ToInt32(SearchDto.Cate);
                    result = result.Where(x => x.StatusId == catePid).ToList();
                }

                if (!string.IsNullOrEmpty(SearchDto.IsPayment))
                {
                    var catePid = Convert.ToInt32(SearchDto.IsPayment);
                    result = result.Where(x => x.IsPaymentId == Convert.ToBoolean(catePid)).ToList();
                }

                if (!string.IsNullOrEmpty(SearchDto.PaymentMethod))
                {
                    var catePid = Convert.ToInt32(SearchDto.PaymentMethod);
                    result = result.Where(x => x.PaymentMethodId == catePid).ToList();
                }

                PagedList<dynamic> dataPaging = new PagedList<dynamic>(result.OrderByDescending(p => p.CreateDate).ToList(), SearchDto.Page, SearchDto.PageNumber);
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
                        var model = _dbContext.Orders.Where(p => p.Pid == item).FirstOrDefault();
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
        public bool ChangeStatus(long[] Pid, int active)
        {
            try
            {
                var user = _common.GetUserAdminInfo();
                if (user == null)
                {
                    return false;
                }

                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Orders.Where(p => p.Pid == item).FirstOrDefault();
                        model.State = active;
                        model.UpdateUser = user.account;
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
        public bool ChangePaymentMethod(long[] Pid, int active)
        {
            try
            {
                var user = _common.GetUserAdminInfo();
                if (user == null)
                {
                    return false;
                }

                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Orders.Where(p => p.Pid == item).FirstOrDefault();
                        model.PaymentMethod = active;
                        model.UpdateUser = user.account;
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
        public bool ChangeIsPayment(long[] Pid, bool active)
        {
            try
            {
                var user = _common.GetUserAdminInfo();
                if (user == null)
                {
                    return false;
                }

                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.Orders.Where(p => p.Pid == item).FirstOrDefault();
                        model.IsPayment = active;
                        model.UpdateUser = user.account;
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
                var model = _dbContext.Orders.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = "Order";
                logObj.Pid = model.Pid;
                logObj.Cate = ConstantStrings.OrderId;
                _common.SaveLog(1, "delete", logObj);
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
                        var model = _dbContext.Orders.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;
                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = "Order";
                            logObj.Pid = model.Pid;
                            logObj.Cate = ConstantStrings.OrderId;
                            _common.SaveLog(1, "delete", logObj);
                        }
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
        public dynamic Edit(int Pid)
        {
            try
            {
                var model = _dbContext.Orders.Where(p => p.Pid == Pid).FirstOrDefault();
                var email = _dbContext.Customers.Where(p => p.Pid == model.CustomerPid).FirstOrDefault().Email;
                var background = _dbContext.OrderImages.Where(p => p.Pid == model.BackgroundPid).Select(p => p.BackgroundImageUrl).FirstOrDefault();
                if (!String.IsNullOrEmpty(background))
                {
                    background = UrlOrderImages + background;
                }
                else
                {
                    background = "/b-admin/dist/img/avatar3.jpg";
                }
                return new
                {
                    obj = new
                    {
                        PaymentStatusPid = model.IsPayment == true ? 1 : 0,
                        PaymentMethod = model.PaymentMethod,
                        OrderStatus = model.State,
                        Enabled = model.Enabled,
                        Note = model.Note,
                        Pid = model.Pid,
                        Email = email,
                        CompanyName = model.CompanyName,
                        CompanyAddress = model.CompanyAddress,
                        TaxCode = model.TaxCode,
                        BackgroundImage = background,
                        Vat = model.VAT,

                    },

                    objDetail = new
                    {
                        ProductCatePid = model.ProductCatePid,
                        ProductPid = model.ProductDetailPid,
                        RegisterDate = model.RegisterDate,
                        ExpiredDate = model.ExpiredDate,
                        Price = model.Total,
                        PriceOrigin = GetPrice(model.ProductDetailPid, model.ProductCatePid),
                        DiscountCode = model.DiscountCode,
                    },
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public dynamic Insert(OrderInformation orderInfo)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var user = _common.GetUserAdminInfo();
                    if (user == null)
                    {
                        messErr = "User not found";
                        return new { status = false, mess = messErr };
                    }
                    #region checkDiscountCode
                    if (!string.IsNullOrEmpty(orderInfo.DiscountCode))
                    {
                        var currentTime = DateTime.Now;
                        var checkDiscountCode = _dbContext.DiscountCodeDetails
                            .FirstOrDefault(x => x.Code == orderInfo.DiscountCode && x.Enabled);
                        if (checkDiscountCode == null ||
                            checkDiscountCode.StartDate > currentTime ||
                            currentTime >= checkDiscountCode.EndDate)
                        {
                            return new { status = false, mess = "Mã giảm giá không hợp lệ" };
                        }
                        if(checkDiscountCode.MaxQuantity > 0)
                        {
                            if (checkDiscountCode.UsedQuantity + 1 > checkDiscountCode.MaxQuantity)
                            {
                                return new { status = false, mess = "Mã giảm giá đã vượt quá giới hạn sử dụng" };
                            }
                        }
                        checkDiscountCode.UsedQuantity += 1;
                        _dbContext.SaveChanges();
                    }
                    #endregion

                    var customer = _dbContext.Customers.Where(x => x.Deleted == false && x.Enabled == true && x.Email.Equals(orderInfo.Email)).FirstOrDefault();
                    if (customer != null)
                    {
                        Random random = new Random();
                        var id = random.Next(1000, 9999);
                        Models.Order order = new Models.Order();
                        order.CustomerPid = customer.Pid;
                        order.ProductDetailPid = orderInfo.ProductDetailPid;
                        order.ProductCatePid = orderInfo.ProductCatePid;
                        order.CustomerPid = customer.Pid;
                        order.State = orderInfo.OrderState;
                        order.IsPayment = orderInfo.IsPayment;
                        order.PaymentMethod = orderInfo.PaymentMethod;
                        order.Note = orderInfo.Note;
                        order.Total = orderInfo.TotalPrice;
                        order.ShipFee = orderInfo.ShipFee;
                        order.Deposit = orderInfo.Deposit;
                        order.CreateUser = user.account;
                        order.UpdateUser = user.account;
                        order.Province = orderInfo.Province;
                        order.District = orderInfo.District;
                        order.VAT = orderInfo.VAT;
                        order.Ward = orderInfo.Ward;
                        order.Address = orderInfo.Address;
                        order.CompanyAddress = orderInfo.CompanyAddress;
                        order.CompanyName = orderInfo.CompanyName;
                        order.TaxCode = orderInfo.TaxCode;
                        order.RegisterDate = orderInfo.RegisterDate;
                        order.ExpiredDate = orderInfo.ExpiredDate;
                        order.DiscountCode = orderInfo.DiscountCode;
                        _dbContext.Orders.Add(order);
                        _dbContext.SaveChanges();


                        order.Code = id + order.Pid.ToString();
                        _dbContext.SaveChanges();
                        #region saveBackground
                        if (orderInfo.Background != null)
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileOriginal(orderInfo.Background, UrlOrderImages, orderInfo.Background.FileName + "-" + order.Pid);
                            if (!saveFileStatus.isError)
                            {
                                OrderImages background = new OrderImages();
                                background.OrderPid = order.Pid;
                                background.BackgroundImageUrl = saveFileStatus.fileName;
                                _dbContext.OrderImages.Add(background);
                                _dbContext.SaveChanges();
                                order.BackgroundPid = background.Pid;
                                _dbContext.SaveChanges();

                            }
                        }

                        #endregion
                        transaction.Commit();

                        return new { status = true, mess = messErr };

                    }
                    else
                    {
                        messErr = "Khách hàng không tồn tại!";
                        transaction.Rollback();
                    }
                    return new { status = false, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";

                    return new { status = false, mess = messErr };
                }
            }
        }
        public dynamic Update(OrderInformation orderInfo)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";
                try
                {
                    var user = _common.GetUserAdminInfo();
                    if (user == null)
                    {
                        messErr = "User not found";
                        return new { status = false, mess = messErr };
                    }

                    var customer = _dbContext.Customers.Where(x => x.Deleted == false && x.Enabled == true && x.Email.Equals(orderInfo.Email)).FirstOrDefault();
                    if (customer != null)
                    {

                        Models.Order order = _dbContext.Orders.Where(x => x.Deleted == false && x.Pid == orderInfo.Pid).FirstOrDefault();
                        #region checkDiscountCode
                        if(order.DiscountCode != orderInfo.DiscountCode && !string.IsNullOrEmpty(orderInfo.DiscountCode))
                        {
                            var currentTime = DateTime.Now;
                            var checkDiscountCode = _dbContext.DiscountCodeDetails
                                .FirstOrDefault(x => x.Code == orderInfo.DiscountCode && x.Enabled);
                            if (checkDiscountCode == null ||
                                checkDiscountCode.StartDate > currentTime ||
                                currentTime >= checkDiscountCode.EndDate)
                            {
                                return new { status = false, mess = "Mã giảm giá không hợp lệ" };
                            }
                            if (checkDiscountCode.MaxQuantity > 0)
                            {
                                if (checkDiscountCode.UsedQuantity + 1 > checkDiscountCode.MaxQuantity)
                                {
                                    return new { status = false, mess = "Mã giảm giá đã vượt quá giới hạn sử dụng" };
                                }
                            }
                            checkDiscountCode.UsedQuantity += 1;
                            _dbContext.SaveChanges();
                        }
                        #endregion
                        if (order != null)
                        {
                            order.ProductDetailPid = orderInfo.ProductDetailPid;
                            order.ProductCatePid = orderInfo.ProductCatePid;
                            order.CustomerPid = customer.Pid;
                            order.State = orderInfo.OrderState;
                            order.IsPayment = orderInfo.IsPayment;
                            order.PaymentMethod = orderInfo.PaymentMethod;
                            order.Note = orderInfo.Note;
                            order.Total = orderInfo.TotalPrice;
                            order.ShipFee = orderInfo.ShipFee;
                            order.Deposit = orderInfo.Deposit;
                            order.CreateUser = user.account;
                            order.UpdateUser = user.account;
                            order.Province = orderInfo.Province;
                            order.District = orderInfo.District;
                            order.VAT = orderInfo.VAT;
                            order.Ward = orderInfo.Ward;
                            order.Address = orderInfo.Address;
                            order.CompanyAddress = orderInfo.CompanyAddress;
                            order.CompanyName = orderInfo.CompanyName;
                            order.TaxCode = orderInfo.TaxCode;
                            order.RegisterDate = orderInfo.RegisterDate;
                            order.ExpiredDate = orderInfo.ExpiredDate;
                            order.DiscountCode = orderInfo.DiscountCode;
                            _dbContext.SaveChanges();
                            #region saveBackground
                            if (orderInfo.Background != null)
                            {
                                dynamic saveFileStatus = _fileServices.SaveFileOriginal(orderInfo.Background, UrlOrderImages, orderInfo.Background.FileName + "-" + order.Pid);
                                if (!saveFileStatus.isError)
                                {
                                    OrderImages background = new OrderImages();
                                    background.OrderPid = order.Pid;
                                    background.BackgroundImageUrl = saveFileStatus.fileName;
                                    _dbContext.OrderImages.Add(background);
                                    _dbContext.SaveChanges();
                                    order.BackgroundPid = background.Pid;
                                    _dbContext.SaveChanges();

                                }
                            }

                            #endregion
                        }
                        else
                        {
                            messErr = "Đơn không tồn tại!";
                            transaction.Rollback();
                            return new { status = true, mess = messErr };
                        }

                        _dbContext.SaveChanges();

                        transaction.Commit();

                        return new { status = true, mess = messErr };

                    }
                    else
                    {
                        messErr = "Khách hàng không tồn tại!";
                        transaction.Rollback();
                    }
                    return new { status = false, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    messErr = "Something Wrong!";
                    return new { status = false, mess = messErr };
                }
            }
        }
        public string LoadProductList(string key, int catePid)
        {
            try
            {
                var data = (from a in _dbContext.ProductDetails
                            join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                            join c in _dbContext.ProductCate_ProductDetails on a.Pid equals c.ProductDetailPid
                            where (!a.Deleted && a.Enabled && b.LangKey == DefaultLang)
                            && (c.ProductCatePid == catePid || catePid == 0) && (b.Title.Contains(key) || string.IsNullOrEmpty(key))
                            orderby a.Order descending
                            select new
                            {
                                pid = a.Pid,
                                title = b.Title,
                                picThumb = UrlProductImages + a.PicThumb,
                                code = a.Code,
                            }).Take(20).Distinct().ToList();


                return JsonConvert.SerializeObject(data);
            }
            catch
            {
                return "[]";
            }
        }
        public string LoadProductDetail(long productId)
        {
            try
            {
                dynamic d = new ExpandoObject();

                var colors = (from a in _dbContext.ProductColors
                              join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
                              where !a.Deleted && !a.isLocked && b.LangKey == DefaultLang
                              select new { pid = a.Pid, title = b.Name }).ToList();
                d.colors = colors;

                var options = (from a in _dbContext.ProductOptions
                               join b in _dbContext.ProductOption_ProductDetails on a.Pid equals b.ProductOptionPid
                               where !a.Deleted && !a.isLocked && b.ProductDetailPid == productId && b.Status
                               select new { pid = a.Pid, code = a.Code, price = b.Price, priceDiscount = b.PriceDiscount }).ToList();
                d.colors = colors;
                d.options = options;

                return JsonConvert.SerializeObject(d);
            }
            catch
            {
                return String.Empty;
            }
        }
        public string LoadOrderTable(string orderString, decimal shipFee, decimal deposit)
        {
            try
            {
                var orderList = JsonConvert.DeserializeObject<List<CartStringDto>>(orderString);
                var result = new CartInfoDto();
                result.items = new List<CartItemDto>();

                foreach (var item in orderList)
                {
                    var product = (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails
                                   on a.Pid equals b.ProductDetailPid
                                   where !a.Deleted && a.Enabled && a.Pid == item.productId
                                   select new
                                   {
                                       title = b.Title,
                                       picThumb = UrlProductImages + a.PicThumb,
                                       slug = b.Slug,
                                       code = a.Code,
                                       price = a.Price,
                                       priceDiscount = a.PriceDiscount
                                   }).FirstOrDefault();

                    //var priceModel = _dbContext.ProductOption_ProductDetails.Where(x => x.ProductDetailPid == item.productId && x.ProductOptionPid == item.optionId).FirstOrDefault();

                    //var colorTitle = (from a in _dbContext.ProductColors
                    //                  join b in _dbContext.MultiLang_ProductColors
                    //                  on a.Pid equals b.ProductColorPid
                    //                  where !a.Deleted && a.Enabled && a.Pid == item.colorId && !a.isLocked
                    //                  select new { title = b.Name }).FirstOrDefault().title;

                    //var optionModel = (from a in _dbContext.ProductOptions
                    //                   join b in _dbContext.MultiLang_ProductOptions
                    //                   on a.Pid equals b.ProductOptionPid
                    //                   where !a.Deleted && a.Enabled && a.Pid == item.optionId && !a.isLocked
                    //                   select new { title = b.Name, code = a.Code }).FirstOrDefault();

                    CartItemDto cartItemDto = new CartItemDto();
                    cartItemDto.title = product.title;
                    cartItemDto.id = item.id;
                    cartItemDto.code = product.code;
                    cartItemDto.slug = product.slug;
                    cartItemDto.picture = product.picThumb;
                    cartItemDto.optionId = item.optionId;
                    cartItemDto.colorId = item.colorId;
                    //cartItemDto.price = (priceModel.PriceDiscount == 0 ? priceModel.Price : priceModel.PriceDiscount) * item.quantity;
                    cartItemDto.price = (product.priceDiscount == 0 ? product.price : product.priceDiscount) * item.quantity;
                    cartItemDto.priceString = _common.ConvertFormatMoney(cartItemDto.price);
                    //cartItemDto.colorTitle = colorTitle;
                    //cartItemDto.optionCode = optionModel.code;
                    cartItemDto.quantity = item.quantity;
                    cartItemDto.productId = item.productId;
                    result.items.Add(cartItemDto);
                }
                result.temporaryPrice = result.items.Sum(x => x.price);
                result.totalPrice = result.temporaryPrice + shipFee - deposit;
                result.temporaryPriceString = _common.ConvertFormatMoney(result.temporaryPrice);
                result.totalPriceString = _common.ConvertFormatMoney(result.totalPrice);

                return JsonConvert.SerializeObject(result);
            }
            catch
            {
                return "";
            }
        }
        public async Task<bool> SendMail(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    await _emailServices.SendMailOrderAdmin(item);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<bool> ExportVAT(long[] Pid, bool isSendMail)
        {
            try
            {
                var pdfResult = new List<dynamic>();

                var absolutepath = Directory.GetCurrentDirectory();
                var zipPath = Path.Combine(absolutepath + "\\wwwroot\\docs\\vat.zip");
                var vatHtmlPath = Path.Combine(absolutepath + "\\wwwroot\\template\\cart", "vat.html");
                var htmlString = "";
                using (StreamReader reader = new StreamReader(vatHtmlPath))
                {
                    htmlString = reader.ReadToEnd();
                }

                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }

                ZipFile.CreateFromDirectory(Path.Combine(absolutepath + "\\wwwroot\\publish\\"), zipPath);
                using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Update);

                foreach (var item in Pid)
                {
                    var htmlResult = htmlString;

                    var fileName = "invoice_code_" + item + ".pdf";
                    var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + UrlDocs, fileName);
                    HtmlToPdf converter = new HtmlToPdf();

                    // convert the url to pdf
                    PdfDocument doc = converter.ConvertHtmlString(htmlResult);

                    // save pdf document
                    doc.Save(filePath);

                    // close pdf document
                    doc.Close();

                    var entry =
                      archive.CreateEntryFromFile(
                          filePath,
                          Path.GetFileName(filePath),
                          CompressionLevel.Optimal
                      );
                    dynamic d = new ExpandoObject();
                    d.id = item;
                    d.path = filePath;

                    pdfResult.Add(d);
                }

                if (isSendMail)
                {
                    foreach (var item in pdfResult)
                    {
                        await _emailServices.SendMailVAT(item.id, item.path);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public dynamic GetInfoCustomer(string email)
        {
            try
            {
                var customerInfo = _dbContext.Customers.Where(x => x.Deleted == false && x.Email.Equals(email)).FirstOrDefault();
                if (customerInfo != null)
                {
                    return customerInfo;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public decimal GetPrice(long ProductPid, long ProductCatePid)
        {
            try
            {
                var model = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductCatePid == ProductCatePid && x.ProductDetailPid == ProductPid).FirstOrDefault();
                if (model != null)
                {
                    return model.Price;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public dynamic InsertCard(CardInformation card)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var user = _common.GetUserAdminInfo();
                    if (user == null)
                    {
                        messErr = "User not found";
                        return new { status = false, mess = messErr };
                    }

                    var order = _dbContext.Orders.Where(x => x.Deleted == false && x.Pid == card.OrderPid).FirstOrDefault();
                    if (order == null)
                    {
                        messErr = "Lỗi đơn hàng";
                        return new { status = false, mess = messErr };
                    }
                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.Url == _common.EncodeTitle(card.Url)).FirstOrDefault();
                    if (duplicateUrl != null)
                    {
                        messErr = "Url trùng lặp";
                        return new { status = false, mess = messErr };
                    }
                    var allCards = _dbContext.OrderDetails.Where(x => x.OrderPid == card.OrderPid).Count();
                    var limit = _dbContext.ProductDetails.Where(x => x.Pid == order.ProductDetailPid).Select(x => x.UserAmount).FirstOrDefault();
                    if (allCards >= limit)
                    {
                        messErr = "Limit reached";
                        return new { status = false, mess = messErr };
                    }

                    OrderDetail newCard = new OrderDetail();
                    newCard.FullName = card.FullName;
                    newCard.PersonalPhone = card.PersonalPhone;
                    newCard.CompanyPhone = card.CompanyPhone;
                    newCard.HomePhone = card.HomePhone;
                    newCard.PersonalEmail = card.PersonalEmail;
                    newCard.WorkEmail = card.WorkEmail;
                    newCard.Position = card.Position;
                    newCard.Url = _common.EncodeTitle(card.Url);
                    newCard.Telegram = card.Telegram;
                    newCard.WhatsApp = card.WhatsApp;
                    newCard.Viber = card.Viber;
                    newCard.Skype = card.Skype;
                    newCard.Zalo = card.Zalo;
                    newCard.LinkedIn = card.LinkedIn;
                    newCard.Facebook = card.Facebook;
                    newCard.Twitter = card.Twitter;
                    newCard.OrderPid = card.OrderPid;
                    newCard.PersonalLink = card.PersonalLink;
                    newCard.CompanyWebsiteLink = card.CompanyWebsiteLink;
                    newCard.PersonalWebsiteLink = card.PersonalWebsiteLink;
                    string customLinksJson = "";
                    if (card.CustomLink != null)
                    {
                        customLinksJson = JsonConvert.SerializeObject(card.CustomLink);
                    }
                    newCard.CustomLink = customLinksJson;
                    _dbContext.OrderDetails.Add(newCard);
                    _dbContext.SaveChanges();

                    if (card.Avatar != null)
                    {

                        dynamic saveFileStatus = _fileServices.SaveFileOriginal(card.Avatar, UrlOrderImages, card.Avatar.FileName + "-" + newCard.Pid);
                        if (!saveFileStatus.isError)
                        {

                            newCard.Avatar = saveFileStatus.fileName;
                            _dbContext.SaveChanges();

                        }


                    }
                    // Generate the QR code
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    string siteUrl = _common.GetConfigValue(ConstantStrings.KeyRootDomain).TrimEnd('/') + _translate.GetUrl("url.name-card");
                    string QRUrl = newCard.Url;
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(siteUrl + QRUrl, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    // Save the QR code image
                    var absolutepath = Directory.GetCurrentDirectory();
                    string qrFileName = "QRCode-" + newCard.Url + ".png";
                    string qrImagePath = Path.Combine(absolutepath + "\\wwwroot\\" + UrlOrderImages, qrFileName);
                    qrCodeImage.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Png);

                    // Store the QR code file name in the database
                    newCard.QrImage = qrFileName;
                    newCard.QrLink = siteUrl + QRUrl;
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
        public dynamic UpdateCard(CardInformation card)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";
                try
                {
                    var user = _common.GetUserAdminInfo();
                    if (user == null)
                    {
                        messErr = "User not found";
                        return new { status = false, mess = messErr };
                    }

                    var order = _dbContext.Orders.Where(x => x.Deleted == false && x.Pid == card.OrderPid).FirstOrDefault();
                    if (order == null)
                    {
                        messErr = "Lỗi đơn hàng";
                        return new { status = false, mess = messErr };
                    }
                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.Url == _common.EncodeTitle(card.Url) && x.Pid != card.Pid).FirstOrDefault();
                    if (duplicateUrl != null)
                    {
                        messErr = "Url trùng lặp";
                        return new { status = false, mess = messErr };
                    }


                    var cardData = _dbContext.OrderDetails.Where(p => p.Pid == card.Pid).FirstOrDefault();
                    if (cardData != null)
                    {
                        cardData.FullName = card.FullName;
                        cardData.PersonalPhone = card.PersonalPhone;
                        cardData.CompanyPhone = card.CompanyPhone;
                        cardData.HomePhone = card.HomePhone;
                        cardData.PersonalEmail = card.PersonalEmail;
                        cardData.WorkEmail = card.WorkEmail;
                        cardData.Position = card.Position;
                        cardData.Telegram = card.Telegram;
                        cardData.WhatsApp = card.WhatsApp;
                        cardData.Viber = card.Viber;
                        cardData.Skype = card.Skype;
                        cardData.Zalo = card.Zalo;
                        cardData.LinkedIn = card.LinkedIn;
                        cardData.Facebook = card.Facebook;
                        cardData.Twitter = card.Twitter;
                        cardData.OrderPid = card.OrderPid;
                        cardData.PersonalLink = card.PersonalLink;
                        cardData.CompanyWebsiteLink = card.CompanyWebsiteLink;
                        cardData.PersonalWebsiteLink = card.PersonalWebsiteLink;
                        cardData.UpdateDate = DateTime.Now;
                        string customLinksJson = "";
                        if (card.CustomLink != null)
                        {
                            customLinksJson = JsonConvert.SerializeObject(card.CustomLink);
                        }
                        cardData.CustomLink = customLinksJson;

                        _dbContext.SaveChanges();

                        if (card.Avatar != null)
                        {


                            dynamic saveFileStatus = _fileServices.SaveFileOriginal(card.Avatar, UrlOrderImages, card.Avatar.FileName + "-" + cardData.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (!String.IsNullOrEmpty(cardData.Avatar))
                                {
                                    _fileServices.DeleteFile(UrlOrderImages, cardData.Avatar);
                                }
                                cardData.Avatar = saveFileStatus.fileName;
                                _dbContext.SaveChanges();

                            }


                        }

                        if (cardData.Url != _common.EncodeTitle(card.Url))
                        {
                            cardData.Url = _common.EncodeTitle(card.Url);
                            // Generate the QR code
                            _fileServices.DeleteFile(UrlOrderImages, cardData.QrImage);
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            string siteUrl = _common.GetConfigValue(ConstantStrings.KeyRootDomain).TrimEnd('/') + _translate.GetUrl("url.name-card");
                            string QRUrl = cardData.Url;
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(siteUrl + QRUrl, QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            Bitmap qrCodeImage = qrCode.GetGraphic(20);

                            // Save the QR code image
                            var absolutepath = Directory.GetCurrentDirectory();
                            string qrFileName = "QRCode-" + cardData.Url + ".png";
                            string qrImagePath = Path.Combine(absolutepath + "\\wwwroot\\" + UrlOrderImages, qrFileName);
                            qrCodeImage.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Png);


                            cardData.QrImage = qrFileName;
                            cardData.QrLink = siteUrl + QRUrl;
                            _dbContext.SaveChanges();
                        }

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

        public dynamic LoadDataCard(SearchDto SearchDto)
        {
            try
            {
                var data = (from a in _dbContext.OrderDetails
                            join b in _dbContext.Orders on a.OrderPid equals b.Pid
                            where a.OrderPid == Convert.ToInt64(SearchDto.OrderPid)
                            select new
                            {
                                Pid = a.Pid,
                                Url = a.Url,
                                FullName = a.FullName,
                                Avatar = UrlOrderImages + a.Avatar,

                            }).ToList().FilterSearch(new string[] { "Url", "FullName" }, SearchDto.Key);
                var result = new List<dynamic>();


                foreach (var item in data)
                {
                    result.Add(item);
                }


                PagedList<dynamic> dataPaging = new PagedList<dynamic>(result.OrderByDescending(p => p.Pid).ToList(), SearchDto.Page, SearchDto.PageNumber);
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

        public dynamic EditCard(int Pid)
        {
            try
            {

                var model = _dbContext.OrderDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                List<CustomLink> allLinks = new List<CustomLink>();
                if (!String.IsNullOrEmpty(model.CustomLink))
                {
                    allLinks = JsonConvert.DeserializeObject<List<CustomLink>>(model.CustomLink);
                }

                return new
                {
                    objCardDetail = new
                    {
                        Pid = model.Pid,
                        FullName = model.FullName,
                        PersonalPhone = model.PersonalPhone,
                        HomePhone = model.HomePhone,
                        CompanyPhone = model.CompanyPhone,
                        PersonalEmail = model.PersonalEmail,
                        WorkEmail = model.WorkEmail,
                        Position = model.Position,
                        Url = model.Url,
                        Telegram = model.Telegram,
                        WhatsApp = model.WhatsApp,
                        Viber = model.Viber,
                        Skype = model.Skype,
                        Zalo = model.Zalo,
                        LinkedIn = model.LinkedIn,
                        Facebook = model.Facebook,
                        Twitter = model.Twitter,
                        CustomLink = allLinks,
                        Avatar = !String.IsNullOrEmpty(model.Avatar) ? UrlOrderImages + model.Avatar : "/img/customers/default-avatar.png",
                        QrCode = !String.IsNullOrEmpty(model.QrImage) ? UrlOrderImages + model.QrImage : "",
                        PersonalLink = model.PersonalLink,
                        CompanyWebsiteLink = model.CompanyWebsiteLink,
                        PersonalWebsiteLink = model.PersonalWebsiteLink,
                    }


                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteCard(int Pid)
        {
            try
            {
                var model = _dbContext.OrderDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                if (!String.IsNullOrEmpty(model.Avatar))
                {
                    _fileServices.DeleteFile(UrlOrderImages, model.Avatar);
                }
                _fileServices.DeleteFile(UrlOrderImages, model.QrImage);
                _dbContext.OrderDetails.Remove(model);
                _dbContext.SaveChanges();

                //dynamic logObj = new ExpandoObject();
                //logObj.Title = "Order";
                //logObj.Pid = model.Pid;
                //logObj.Cate = ConstantStrings.OrderId;
                //_common.SaveLog(1, "delete", logObj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public dynamic DeleteCard(long[] Pid)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.OrderDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            if (!String.IsNullOrEmpty(model.Avatar))
                            {
                                _fileServices.DeleteFile(UrlOrderImages, model.Avatar);
                            }
                            _fileServices.DeleteFile(UrlOrderImages, model.QrImage);
                            _dbContext.OrderDetails.Remove(model);
                            _dbContext.SaveChanges();

                            //dynamic logObj = new ExpandoObject();
                            //logObj.Title = "Order";
                            //logObj.Pid = model.Pid;
                            //logObj.Cate = ConstantStrings.OrderId;
                            //_common.SaveLog(1, "delete", logObj);
                        }
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

        public dynamic UploadCSV(IFormFile File, string OrderPid)
        {
            try
            {
                string messErr = "";
                var user = _common.GetUserAdminInfo();
                if (user == null)
                {
                    messErr = "User not found";
                    return new { isError = true, messError = messErr, data = "" };
                }
                var order = _dbContext.Orders.Where(x => x.Pid == Convert.ToInt64(OrderPid)).FirstOrDefault();
                if (order == null)
                {
                    messErr = "Lỗi đơn hàng";
                    return new { isError = true, messError = messErr, data = "" };
                }
                var inputStream = File.OpenReadStream();
                var records = _common.ReadCsvStream<CsvCardModel>(inputStream, true);
                var allCards = _dbContext.OrderDetails.Where(x => x.OrderPid == Convert.ToInt64(OrderPid)).Count();
                var limit = _dbContext.ProductDetails.Where(x => x.Pid == order.ProductDetailPid).Select(x => x.UserAmount).FirstOrDefault();
                int uploadSuccess = 0;
                int uploadFail = 0;
                int index = 1;
                List<int> failCardIds = new List<int>();
                if (allCards + records.Count > limit)
                {
                    messErr = "Số lượng card đã vượt qua giới hạn tối đa";
                    return new { isError = true, messError = messErr, data = "" };
                }
                else
                {
                    foreach (var item in records)
                    {
                        index++;
                        bool result = InsertCSV(item, OrderPid);
                        if (result)
                        {
                            uploadSuccess++;
                        }
                        else
                        {
                            uploadFail++;
                            failCardIds.Add(index);
                        }
                    }
                }
                return new
                {
                    isError = false,
                    data = new
                    {
                        UploadSuccess = uploadSuccess,
                        UploadFail = uploadFail,
                        FailIds = failCardIds,
                    },
                    messError = messErr


                };
            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "Something is wrong!", data = "" };
            }
        }

        public bool InsertCSV(CsvCardModel card, string OrderPid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.OrderPid == Convert.ToInt64(OrderPid) && x.Url == _common.EncodeTitle(card.Url)).FirstOrDefault();
                    if (duplicateUrl != null)
                    {
                        return false;
                    }
                    OrderDetail newCard = new OrderDetail();
                    newCard.FullName = card.FullName;
                    newCard.PersonalPhone = card.PersonalPhone;
                    newCard.CompanyPhone = card.CompanyPhone;
                    newCard.HomePhone = card.HomePhone;
                    newCard.PersonalEmail = card.PersonalEmail;
                    newCard.WorkEmail = card.WorkEmail;
                    newCard.Position = card.Position;
                    newCard.Url = _common.EncodeTitle(card.Url);
                    newCard.Telegram = card.Telegram;
                    newCard.WhatsApp = card.WhatsApp;
                    newCard.Viber = card.Viber;
                    newCard.Skype = card.Skype;
                    newCard.Zalo = card.Zalo;
                    newCard.LinkedIn = card.LinkedIn;
                    newCard.Facebook = card.Facebook;
                    newCard.Twitter = card.Twitter;
                    ////newCard.PersonalLink = card.PersonalLink;
                    //newCard.CompanyWebsiteLink = card.CompanyWebsiteLink;
                    //newCard.PersonalWebsiteLink = card.PersonalWebsiteLink;
                    newCard.OrderPid = Convert.ToInt64(OrderPid);

                    _dbContext.OrderDetails.Add(newCard);
                    _dbContext.SaveChanges();

                    // Generate the QR code

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    string siteUrl = _common.GetConfigValue(ConstantStrings.KeyRootDomain).TrimEnd('/') + _translate.GetUrl("url.name-card");
                    string QRUrl = newCard.Url;
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(siteUrl + QRUrl, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    // Save the QR code image
                    var absolutepath = Directory.GetCurrentDirectory();
                    string qrFileName = "QRCode-" + newCard.Url + ".png";
                    string qrImagePath = Path.Combine(absolutepath + "\\wwwroot\\" + UrlOrderImages, qrFileName);
                    qrCodeImage.Save(qrImagePath, System.Drawing.Imaging.ImageFormat.Png);


                    newCard.QrImage = qrFileName;
                    newCard.QrLink = siteUrl + QRUrl;
                    _dbContext.SaveChanges();
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

        public dynamic ExportCSV(string OrderPid)
        {
            try
            {
                string messErr = "";
                var user = _common.GetUserAdminInfo();
                if (user == null)
                {
                    messErr = "User not found";
                    return new { isError = true, messError = messErr };
                }
                var order = _dbContext.Orders.Where(x => x.Pid == Convert.ToInt64(OrderPid)).FirstOrDefault();
                if (order == null)
                {
                    messErr = "Lỗi đơn hàng";
                    return new { isError = true, messError = messErr };
                }

                var model = _dbContext.OrderDetails.Where(p => p.OrderPid == Convert.ToInt64(OrderPid)).OrderByDescending(p => p.Pid).Select(
                    p => new CsvExportModel
                    {
                        Name = p.FullName,
                        Position = p.Position,
                        CardUrl = p.QrLink,
                        PersonalPhone = p.PersonalPhone,
                        HomePhone = p.HomePhone,
                        CompanyPhone = p.CompanyPhone,
                        PersonalEmail = p.PersonalEmail,
                        WorkEmail = p.WorkEmail,
                        //CompanyWebsiteLink = p.CompanyWebsiteLink,
                        //PersonalWebsiteLink = p.PersonalWebsiteLink,
                        Telegram = p.Telegram,
                        WhatsApp = p.WhatsApp,
                        Viber = p.Viber,
                        Skype = p.Skype,
                        Zalo = p.Zalo,
                        LinkedIn = p.LinkedIn,
                        Facebook = p.Facebook,
                        Twitter = p.Twitter,
                        CustomLink = p.CustomLink,
                    }
                    ).ToList();
                foreach (var item in model)
                {
                    string customLinkString = "";
                    List<CustomLink> allLinks = new List<CustomLink>();
                    if (!String.IsNullOrEmpty(item.CustomLink))
                    {
                        allLinks = JsonConvert.DeserializeObject<List<CustomLink>>(item.CustomLink);
                        foreach (var link in allLinks)
                        {
                            customLinkString += " - " + link.Link;
                        }
                    }
                    item.CustomLink = customLinkString;
                }
                string csvString = _common.ExportCsv(model);

                return new
                {
                    isError = false,
                    data = csvString,
                    messError = messErr

                };
            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "Something is wrong!" };
            }
        }
        public dynamic ApplyDiscountCode(decimal price, string discountCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(discountCode))
                {
                    #region checkCode
                    var currentTime = DateTime.Now;
                    var checkDiscountCode = _dbContext.DiscountCodeDetails
                        .FirstOrDefault(x => x.Code == discountCode && x.Enabled);
                    if (checkDiscountCode == null ||
                        checkDiscountCode.StartDate > currentTime ||
                        currentTime >= checkDiscountCode.EndDate)
                    {
                        return new { isError = true, messError = "Mã giảm giá không hợp lệ", data = price };
                    }
                    if (checkDiscountCode.MaxQuantity > 0)
                    {
                        if (checkDiscountCode.UsedQuantity + 1 > checkDiscountCode.MaxQuantity)
                        {
                            return new { isError = true, messError = "Mã giảm giá đã vượt quá giới hạn sử dụng", data = price };
                        }
                    }

                    #endregion

                    decimal newPrice = price;
                    if (checkDiscountCode.DiscountCodeType == "%")
                    {
                        newPrice = newPrice - newPrice * (checkDiscountCode.DiscountCodeValue / 100);
                    }
                    else
                    {
                        newPrice = newPrice - checkDiscountCode.DiscountCodeValue;
                    }
                    return new { isError = false, messError = "", data = newPrice };
                }
                else
                {
                    return new { isError = true, messError = "Mã giảm giá không hợp lệ", data = price };
                }
            }
            catch(Exception ex)
            {
                return new { isError = true, messError = "Something is Wrong!", data = price };
            }

        }
    }
}
