using System;
using System.Linq;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;
using DTO.Customer;
using CMS.Areas.Customer.Models;
using Newtonsoft.Json;
using CMS.Services.EmailServices;
using System.Web;
using Microsoft.AspNetCore.Http;
using CMS.Services.FileServices;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CMS.Helper;
using CMS.Services;
using System.Collections.Generic;
using DTO.Cart;
using X.PagedList;
using CMS.Services.PageServices;
using CMS.Areas.Order.Models;
using QRCoder;
using CMS.Services.TranslateServices;

namespace CMS.Repository
{
    public class Customer_Repository : ICustomer_Repository
    {

        public string PageLimit = "";

        private readonly ICommonServices _common;
        private readonly IEmailServices _emailServices;
        private readonly IPageServices _pageServices;
        private readonly ITranslateServices _translate;
        private readonly DBContext _dbContext;
        private readonly IFileServices _file;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenHelper;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private string UrlOrderImages = ConstantStrings.UrlOrderImages;

        public Customer_Repository(DBContext dbContext, ICommonServices common,
            IEmailServices emailServices, IHttpContextAccessor httpContextAccessor, IFileServices file, IMapper mapper, TokenHelper tokenHelper, IPageServices pageServices, ITranslateServices translate)
        {
            _common = common;
            _pageServices = pageServices;
            _translate = translate;
            _dbContext = dbContext;
            _emailServices = emailServices;
            _httpContextAccessor = httpContextAccessor;
            _file = file;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }

        public CustomerDto GetProfile()
        {
            try
            {
                if (!_httpContextAccessor.HttpContext.Items.TryGetValue(ConstantStrings.CustomerCookieName, out object customer))
                {
                    var session = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.CustomerCookieName);
                    var coookie = _httpContextAccessor.HttpContext.Request.Cookies[ConstantStrings.CustomerCookieName];
                    var token = "";
                    if (!string.IsNullOrEmpty(session)) token = session; else token = coookie;
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (_tokenHelper.IsTokenValid(token))
                        {
                            var userId = Convert.ToInt32(_tokenHelper.GetValuePayload(token, "user_id"));

                            customer = _common.GetProfile(userId);
                        }
                    }
                }

                var model = (CustomerDto)customer;
                if (model != null)
                {
                    model.FullAddress = _common.GetAddress(model.Address, _common.GetWard(model.District, model.Ward), _common.GetDistrict(model.Province, model.District), _common.GetProvince(model.Province));
                    return model;
                }
                return new CustomerDto();
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return new CustomerDto();
            }
        }
        public async Task<List<OrderListDto>> GetOrderList()
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

                CustomerDto customer = GetProfile();


                var data = await (from a in _dbContext.Orders
                                  join pd in _dbContext.ProductDetails on a.ProductDetailPid equals pd.Pid
                                  join mpd in _dbContext.MultiLang_ProductDetails on pd.Pid equals mpd.ProductDetailPid
                                  join pc in _dbContext.ProductCates on a.ProductCatePid equals pc.Pid
                                  where a.Deleted == false && a.CustomerPid == customer.Pid && mpd.LangKey == "vi"
                                  select new OrderListDto
                                  {
                                      Status = enumStateOrderList.GetValueOrDefault(a.State),
                                      IsPayment = a.IsPayment ? "Đã thanh toán" : "Chưa thanh toán",
                                      PaymentMethod = enumPaymentMethodList.GetValueOrDefault(a.PaymentMethod),
                                      TotalString = _common.ConvertFormatMoney(a.Total),
                                      ShipFeeString = _common.ConvertFormatMoney(a.ShipFee),
                                      DepositString = _common.ConvertFormatMoney(a.Deposit),
                                      OrderDate = a.CreateDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                      ExpiredDate = a.ExpiredDate,
                                      RegisterDateString = a.RegisterDate.ToString("dd/MM/yyyy"),
                                      ExpiredDateString = a.ExpiredDate.ToString("dd/MM/yyyy"),
                                      Code = a.Pid.ToString(),
                                      Months = pc.Months,
                                      ProductId = a.ProductDetailPid,
                                      ProductTitle = mpd.Title,
                                      UserLimit = pd.UserAmount,
                                      CompanyName = a.CompanyName,
                                  }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return null;
            }
        }
        public async Task<ResponseDto> UpdateProfile(CustomerUpdateDto modelUpdate, IFormFile avatar)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    CustomerDto customer = GetProfile();

                    if (customer.Pid != 0)
                    {
                        var currentModel = await _dbContext.Customers.Where(x => x.Pid == customer.Pid && !x.Deleted && x.Enabled).FirstOrDefaultAsync();
                        if (currentModel != null)
                        {
                            _mapper.Map<CustomerUpdateDto, Customer>(modelUpdate, currentModel);
                            if (avatar != null)
                            {
                                var img = _file.SaveFileAvatar(avatar, UrlCustomerImages, modelUpdate.FullName);
                                if (!img.isError)
                                {
                                    if (currentModel.PicThumb != ConstantStrings.DefaultAvatar)
                                    {
                                        _file.DeleteFile(UrlCustomerImages, currentModel.PicThumb);
                                    }
                                    currentModel.PicThumb = img.fileName;
                                }
                            }
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();

                            return new ResponseDto { isError = false, statusError = "OK", message = "code.success" };
                        }
                    }
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
            }

        }
        public async Task<ResponseDto> UpdatePassword(string currentPassword, string newPassword)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    CustomerDto model = GetProfile();

                    if (model.Pid != 0)
                    {
                        var customer = await _dbContext.Customers.Where(x => x.Pid == model.Pid && !x.Deleted && x.Enabled).FirstOrDefaultAsync();
                        if (customer != null)
                        {
                            if (customer.Password == _common.GetHashSha256(currentPassword))
                            {
                                customer.Password = _common.GetHashSha256(newPassword);
                                await _dbContext.SaveChangesAsync();
                                transaction.Commit();
                                return new ResponseDto { isError = false, statusError = "OK", message = "code.success" };
                            }
                            return new ResponseDto { isError = true, statusError = "Error", message = "code.not-current-password", errorCode = "old-password" };
                        }
                    }
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
            }
        }
        public async Task<bool> ChangePassword(string email, string code)
        {
            try
            {
                email = HttpUtility.UrlDecode(email);
                var customer = await _dbContext.Customers.Where(x => x.Email.Equals(email) && !x.Deleted).FirstOrDefaultAsync();
                if (customer != null && code == customer.ActivationCode)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public ResponseDto SendMailForgotPassword(string email, string newPassword)
        {
            try
            {
                if (newPassword != "")
                {
                    var customer = _dbContext.Customers.Where(x => !x.Deleted && x.Enabled && x.Email.Equals(email)).FirstOrDefault();
                    if (customer != null)
                    {
                        var result = _emailServices.SendMailForgotPasswordCustomer(email, newPassword);
                        if (result)
                        {
                            return new ResponseDto { isError = false, statusError = "OK", message = "code.mail-success" };
                        }
                        else
                        {
                            return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                        }
                    }
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.not-customer", errorCode = "email" };
                }
                else
                {
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
            }
            catch
            {
                return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
            }
        }
        public async Task<ResponseDto> Register(CustomerDto register)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var checkValid = await _dbContext.Customers.Where(x => x.Email.Equals(register.Email) && x.Deleted == false).FirstOrDefaultAsync();
                    if (checkValid == null)
                    {
                        register.Password = _common.GetHashSha256(register.Password);
                        var customerEntity = _mapper.Map<Customer>(register);
                        _dbContext.Customers.Add(customerEntity);
                        await _dbContext.SaveChangesAsync();
                        var sendMailStatus = await _emailServices.SendMailActiveCustomer(customerEntity.Email);
                        if (sendMailStatus)
                        {
                            transaction.Commit();
                            return new ResponseDto { isError = false, statusError = "OK", message = "code.register" };
                        }
                        else
                        {
                            transaction.Rollback();
                            return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                        }
                    }
                    return new ResponseDto { isError = true, statusError = "DuplicateEmail", message = "code.duplicate" };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
            }
        }
        public async Task<ResponseDto> Login(string email, string password, bool rememberMe)
        {
            try
            {
                var customer = await _dbContext.Customers.Where(x => x.Email.Equals(email) && !x.Deleted && x.Enabled).FirstOrDefaultAsync();
                if (customer != null)
                {
                    if (customer.Password == _common.GetHashSha256(password))
                    {
                        var token = _tokenHelper.GenerateToken(new TokenPayload() { UserId = customer.Pid });
                        //_httpContextAccessor.HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);

                        SetTokenForUser(token, rememberMe);

                        return new ResponseDto { isError = false, statusError = "OK", message = "code.login-success" };
                    }
                    else
                    {
                        return new ResponseDto { isError = true, statusError = "Error", message = "code.login-fail", errorCode = "password" };
                    }

                }
                return new ResponseDto { isError = true, statusError = "Error", message = "code.login-fail", errorCode = "email" };
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return new ResponseDto { isError = true, statusError = "Error", message = "process-err" };
            }
        }
        public async Task<bool> LoginFB(string verifyString)
        {
            try
            {
                var result = await _common.GetCallAPI($"https://graph.facebook.com/v12.0/me?fields=id%2Cname%2Cemail&access_token={verifyString}");
                if (!string.IsNullOrEmpty(result))
                {
                    var model = JsonConvert.DeserializeObject<FacebookLoginDto>(result);
                    model.email = !string.IsNullOrEmpty(model.email) ? model.email : "";
                    var customer = await _dbContext.Customers.Where(x => !x.Deleted && x.FacebookId.Equals(model.id)).FirstOrDefaultAsync();
                    if (customer != null)
                    {
                        var token = _tokenHelper.GenerateToken(new TokenPayload() { UserId = customer.Pid });
                        SetTokenForUser(token, true);
                        return true;
                    }
                    else
                    {
                        var newCustomer = new Customer();
                        newCustomer.Password = "";
                        newCustomer.FacebookId = model.id;
                        newCustomer.Email = model.email;
                        newCustomer.LastName = "";
                        newCustomer.FirstName = model.name;
                        newCustomer.Enabled = true;
                        newCustomer.CreateDate = DateTime.Now;
                        newCustomer.PicThumb = ConstantStrings.DefaultAvatar;


                        await _dbContext.Customers.AddAsync(newCustomer);
                        await _dbContext.SaveChangesAsync();

                        var token = _tokenHelper.GenerateToken(new TokenPayload() { UserId = newCustomer.Pid });
                        SetTokenForUser(token, true);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }
        public async Task<bool> LoginGoogle(string verifyString)
        {
            try
            {
                var result = await _common.GetCallAPI("https://oauth2.googleapis.com/tokeninfo?id_token=" + verifyString);
                var model = JsonConvert.DeserializeObject<GoogleLoginDto>(result);
                if (string.IsNullOrEmpty(model.error))
                {
                    var customer = _dbContext.Customers.Where(x => !x.Deleted && x.Email.Equals(model.email)).FirstOrDefault();
                    if (customer != null)
                    {
                        var token = _tokenHelper.GenerateToken(new TokenPayload() { UserId = customer.Pid });
                        SetTokenForUser(token, true);
                        return true;
                    }
                    else
                    {
                        var newCustomer = new Customer();
                        newCustomer.Password = "";
                        newCustomer.GoogleId = model.sub;
                        newCustomer.Email = model.email;
                        newCustomer.LastName = model.family_name;
                        newCustomer.FirstName = model.given_name;
                        newCustomer.Enabled = true;
                        newCustomer.CreateDate = DateTime.Now;
                        newCustomer.PicThumb = ConstantStrings.DefaultAvatar;

                        await _dbContext.Customers.AddAsync(newCustomer);
                        await _dbContext.SaveChangesAsync();

                        var token = _tokenHelper.GenerateToken(new TokenPayload() { UserId = newCustomer.Pid });
                        SetTokenForUser(token, true);

                        return true;

                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }
        public async Task<bool> Active(string email, string code)
        {
            try
            {
                email = HttpUtility.UrlDecode(email);
                var customer = await _dbContext.Customers.Where(x => x.Email.Equals(email) && !x.Deleted).FirstOrDefaultAsync();
                if (customer != null && code == customer.ActivationCode)
                {
                    var random = new Random();
                    var randomCode = random.Next(100000, 600000).ToString();
                    customer.ActivationCode = randomCode;
                    customer.Enabled = true;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }
        public async Task<ResponseDto> EditPassword(string code, string email, string password)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var customer = await _dbContext.Customers.Where(x => x.Email.Equals(email) && !x.Deleted && x.Enabled).FirstOrDefaultAsync();

                    if (customer != null && code == customer.ActivationCode)
                    {
                        var random = new Random();
                        var randomCode = random.Next(100000, 600000).ToString();
                        customer.ActivationCode = randomCode;
                        customer.Password = _common.GetHashSha256(password);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        return new ResponseDto { isError = false, statusError = "OK", message = "code.success" };
                    }
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    return new ResponseDto { isError = true, statusError = "Error", message = "code.process-err" };
                }
            }
        }
        private void SetTokenForUser(string token, bool rememberMe = false)
        {
            if (rememberMe)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(7);
                option.IsEssential = true;
                _httpContextAccessor.HttpContext.Response.Cookies.Append(ConstantStrings.CustomerCookieName, token, option);
            }
            else
            {
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.CustomerCookieName, token);
            }
        }
        public async Task<string> ChangePasswordForEmail(string email)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    email = HttpUtility.UrlDecode(email);
                    string newPassword = "";
                    var customer = await _dbContext.Customers.Where(x => x.Email.Equals(email) && !x.Deleted).FirstOrDefaultAsync();
                    if (customer != null)
                    {
                        newPassword = _common.GenerateRandomString(6);
                        customer.Password = _common.GetHashSha256(newPassword);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return newPassword;
                    }
                    else
                    {
                        return "email-error";
                    }
                }
                catch
                {
                    return "";
                }
            }



        }

        public async Task<OrderListDto> GetOrderByPid(string pid)
        {
            try
            {
                var orderPid = Convert.ToInt64(pid);
                var enumStateOrderList = new Dictionary<int, string>();
                var stateOrderList = Enum.GetNames(typeof(EnumOrder.OrderState));
                for (var i = 0; i < stateOrderList.Length; i++)
                {
                    enumStateOrderList.Add(i, stateOrderList[i]);
                }
                //var productTitle = await (from a in _dbContext.ProductDetails
                //                          join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                //                          join c in _dbContext.Orders on a.Pid equals c.ProductDetailPid
                //                          where c.Pid == orderPid && b.LangKey == "vi"
                //                          select new
                //                          {
                //                              Title = b.Title,
                //                          }).FirstOrDefaultAsync();
                var order = await _dbContext.Orders.Where(p => p.Pid == orderPid)
                    .Select(p => new OrderListDto
                    {

                        CompanyName = p.CompanyName,
                        TaxCode = p.TaxCode,
                        CompanyAddress = p.CompanyAddress,
                        UserLimit = _dbContext.ProductDetails.Where(a => a.Pid == p.ProductDetailPid).Select(x => x.UserAmount).FirstOrDefault(),
                        CurrentUserAmount = _dbContext.OrderDetails.Where(a => a.OrderPid == orderPid).Count(),
                        RegisterDateString = p.RegisterDate.ToString("dd/MM/yyyy"),
                        ExpiredDateString = p.ExpiredDate.ToString("dd/MM/yyyy"),
                        ExpiredDate = p.ExpiredDate,
                        ProductId = p.ProductDetailPid,
                        ProductTitle = _dbContext.MultiLang_ProductDetails.Where(a => a.ProductDetailPid == p.ProductDetailPid && a.LangKey == "vi").Select(a => a.Title).FirstOrDefault(),
                        Status = enumStateOrderList.GetValueOrDefault(p.State),
                        Months = _dbContext.ProductCates.Where(a => a.Pid == p.ProductCatePid).Select(x => x.Months).FirstOrDefault(),
                        BackgroundImage = (!String.IsNullOrEmpty(_dbContext.OrderImages.Where(a => a.Pid == p.BackgroundPid).Select(x => x.BackgroundImageUrl).FirstOrDefault()) ?
                                            UrlOrderImages + _dbContext.OrderImages.Where(a => a.Pid == p.BackgroundPid).Select(x => x.BackgroundImageUrl).FirstOrDefault() : "")

                    }).FirstOrDefaultAsync();
                return order;
            }
            catch
            {
                return new OrderListDto();
            }
        }
        public async Task<NameCard> GetNameCard(string url)
        {
            try
            {

                var nameCard = await (
                    from a in _dbContext.Orders
                    join b in _dbContext.OrderDetails on a.Pid equals b.OrderPid
                    where b.Url == url && a.ExpiredDate > DateTime.Now
                    select new NameCard
                    {
                        Pid = b.Pid,
                        FullName = b.FullName,
                        CompanyName = a.CompanyName,
                        CompanyAddress = a.CompanyAddress,
                        Position = b.Position,
                        PersonalPhone = b.PersonalPhone,
                        HomePhone = b.HomePhone,
                        CompanyPhone = b.CompanyPhone,
                        PersonalEmail = b.PersonalEmail,
                        WorkEmail = b.WorkEmail,
                        Zalo = b.Zalo,
                        Facebook = b.Facebook,
                        Twitter = b.Twitter,
                        LinkedIn = b.LinkedIn,
                        Telegram = b.Telegram,
                        WhatsApp = b.WhatsApp,
                        Skype = b.Skype,
                        Viber = b.Viber,
                        PersonalLink = b.PersonalLink,
                        CompanyWebsiteLink = b.CompanyWebsiteLink,
                        PersonalWebsiteLink = b.PersonalWebsiteLink,
                        Avatar = string.IsNullOrEmpty(b.Avatar) ? "" : UrlOrderImages + b.Avatar,
                        QrImage = string.IsNullOrEmpty(b.QrImage) ? "" : UrlOrderImages + b.QrImage,
                        QrLink = b.QrLink,
                        Background = UrlOrderImages + _dbContext.OrderImages.Where(p => p.Pid == a.BackgroundPid).Select(p => p.BackgroundImageUrl).FirstOrDefault(),
                        CustomLink = string.IsNullOrEmpty(b.CustomLink) ? new List<CustomLink>() : JsonConvert.DeserializeObject<List<CustomLink>>(b.CustomLink)
                    }
                ).FirstOrDefaultAsync();

                return nameCard;
            }
            catch
            {
                return null;
            }

        }

        public async Task<List<OrderImage>> GetOrderImages(string pid)
        {
            try
            {
                var orderPid = Convert.ToInt64(pid);
                var selectedImageId = _dbContext.Orders.Where(p => p.Pid == orderPid).Select(p => p.BackgroundPid).FirstOrDefault();
                var orderImages = await _dbContext.OrderImages.Where(p => p.OrderPid == orderPid)
                    .Select(p => new OrderImage
                    {
                        Pid = p.Pid,
                        Active = p.Pid == selectedImageId,
                        BackgroundImageUrl = UrlOrderImages + p.BackgroundImageUrl
                    }).OrderByDescending(p => p.Pid).Take(6).ToListAsync();
                //if (orderImages.Count > 6)
                //{
                //    var pidsToDelete = orderImages.OrderBy(p => p.Pid).Select(p => p.Pid).Take(orderImages.Count - 6);
                //    foreach (var pidToDelete in pidsToDelete)
                //    {
                //        var imageToDelete = await _dbContext.OrderImages.FirstOrDefaultAsync(p => p.Pid == pidToDelete);
                //        if (imageToDelete != null)
                //        {
                //            _file.DeleteFile(UrlOrderImages, imageToDelete.BackgroundImageUrl);
                //            _dbContext.OrderImages.Remove(imageToDelete);
                //        }
                //    }
                //    await _dbContext.SaveChangesAsync();
                //    orderImages = orderImages.OrderByDescending(p => p.Pid).Take(6).ToList();
                //}
                return orderImages;
            }
            catch
            {
                return new List<OrderImage>();
            }
        }

        public async Task<(string, string)> RenderHtmlForTable(IPagedList<NameCard> data)
        {
            var htmlTableBody = await _pageServices.RenderViewToStringAsync("/Views/Customer/Modal/_OrderDetailBody.cshtml", data);
            var htmlPaging = await _pageServices.RenderViewToStringAsync("/Views/Customer/Modal/_OrderDetailBodyPagination.cshtml", new NameCardPaginationModel { PagedList = data });
            return (htmlTableBody, htmlPaging);
        }
        public async Task<IPagedList<NameCard>> GetListNameCardByOrderId(string orderId, int pageIndex, int pageSize, string searchKeyword = "")
        {
            try
            {
                var orderPid = Convert.ToInt64(orderId);
                var dataList = await _dbContext.OrderDetails
                    .Where(p => p.OrderPid == orderPid)
                    .Select(p => new NameCard
                    {
                        Pid = p.Pid,
                        FullName = p.FullName,
                        Position = p.Position,
                    })
                    .OrderByDescending(p => p.Pid)
                    .ToListAsync();

                var filteredData = dataList
                    .FilterSearch(new string[] { "FullName", "Position" }, searchKeyword)
                    .ToPagedList(pageIndex, pageSize);

                return filteredData;
            }
            catch
            {
                return null;
            }
        }

        public async Task<dynamic> SearchListCard(SearchNameCardDto model)
        {
            try
            {
                var data = await GetListNameCardByOrderId(model.OrderId, model.PageIndex, 20, model.SearchKeyword);
                var (htmlTableBody, htmlPaging) = await RenderHtmlForTable(data);
                var currentUserAmount = _dbContext.OrderDetails.Where(p => p.OrderPid == Convert.ToInt64(model.OrderId)).Count();
                return new
                {
                    isError = true,
                    data = new
                    {
                        htmlTableBody = htmlTableBody,
                        htmlPaging = htmlPaging,
                        htmlCurrentUserAmount = currentUserAmount
                    }

                };
            }
            catch
            {
                return new
                {
                    isError = false
                };
            }
        }
        public dynamic UploadBackgroundImage(IFormFile background, string orderId)
        {
            try
            {
                var orderPid = Convert.ToInt64(orderId);
                var order = _dbContext.Orders.Where(p => p.Pid == orderPid).FirstOrDefault();
                long currentBgId = 0;
                if (background != null)
                {
                    dynamic saveFileStatus = _file.SaveFileOriginal(background, UrlOrderImages, background.FileName + "-" + order.Pid);
                    if (!saveFileStatus.isError)
                    {
                        OrderImages newBackground = new OrderImages();
                        newBackground.OrderPid = order.Pid;
                        newBackground.BackgroundImageUrl = saveFileStatus.fileName;
                        _dbContext.OrderImages.Add(newBackground);
                        _dbContext.SaveChanges();
                        order.BackgroundPid = newBackground.Pid;
                        currentBgId = newBackground.Pid;
                        _dbContext.SaveChanges();

                    }
                    else
                    {
                        return new { isError = false, messError = "Có lỗi khi lưu ảnh!", data = new { } };
                    }
                    var allImages = _dbContext.OrderImages.Where(p => p.OrderPid == orderPid).ToList();
                    if (allImages.Count > 6)
                    {
                        var pidsToDelete = allImages.OrderBy(p => p.Pid).Select(p => p.Pid).Take(allImages.Count - 6);
                        foreach (var pidToDelete in pidsToDelete)
                        {
                            var imageToDelete = _dbContext.OrderImages.FirstOrDefault(p => p.Pid == pidToDelete);
                            if (imageToDelete != null)
                            {
                                _file.DeleteFile(UrlOrderImages, imageToDelete.BackgroundImageUrl);
                                _dbContext.OrderImages.Remove(imageToDelete);
                            }
                        }
                        _dbContext.SaveChanges();
                        allImages = allImages.OrderByDescending(p => p.Pid).Take(6).ToList();

                    }
                    var result = allImages.Select(p => new OrderImage
                    {
                        Pid = p.Pid,
                        Active = p.Pid == currentBgId,
                        BackgroundImageUrl = UrlOrderImages + p.BackgroundImageUrl,

                    }).OrderByDescending(a => a.Pid).ToList();
                    return new { isError = true, messError = "", data = result };
                }
                else
                {
                    return new { isError = false, messError = "Chưa chọn ảnh!", data = new { } };
                }
            }
            catch
            {
                return new { isError = false, messError = "Something is wrong!", data = new { } };
            }
        }
        public dynamic UpdateBackgroundImageFromList(string imageId, string orderId)
        {
            try
            {
                var orderPid = Convert.ToInt64(orderId);
                var imagePid = Convert.ToInt64(imageId);
                var order = _dbContext.Orders.Where(p => p.Pid == orderPid).FirstOrDefault();
                var image = _dbContext.OrderImages.Where(p => p.Pid == imagePid).FirstOrDefault();
                order.BackgroundPid = image.Pid;
                _dbContext.SaveChanges();
                return new { isError = true, messError = "", data = new { url = UrlOrderImages + image.BackgroundImageUrl } };
            }
            catch
            {
                return new { isError = false, messError = "Something is wrong!", data = new { } };
            }
        }

        public dynamic UploadCSV(IFormFile File, string OrderPid)
        {
            try
            {
                string messErr = "";
                var order = _dbContext.Orders.Where(x => x.Pid == Convert.ToInt64(OrderPid)).FirstOrDefault();
                if (order == null)
                {
                    messErr = "Lỗi đơn hàng";
                    return new { isError = true, data = "", messError = messErr };
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
                    return new { isError = true, data = "", messError = messErr };
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

                return new { isError = true, messError = ex.Message, data = "" };
            }
        }

        public bool InsertCSV(CsvCardModel card, string OrderPid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.Url == _common.EncodeTitle(card.Url)).FirstOrDefault();
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
                    //newCard.PersonalLink = card.PersonalLink;
                    //newCard.CompanyWebsiteLink = card.CompanyWebsiteLink;
                    //newCard.PersonalWebsiteLink = card.PersonalWebsiteLink;
                    newCard.Url = _common.EncodeTitle(card.Url);
                    newCard.Telegram = card.Telegram;
                    newCard.WhatsApp = card.WhatsApp;
                    newCard.Viber = card.Viber;
                    newCard.Skype = card.Skype;
                    newCard.Zalo = card.Zalo;
                    newCard.LinkedIn = card.LinkedIn;
                    newCard.Facebook = card.Facebook;
                    newCard.Twitter = card.Twitter;
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
                var order = _dbContext.Orders.Where(x => x.Pid == Convert.ToInt64(OrderPid)).FirstOrDefault();
                if (order == null)
                {
                    messErr = "Lỗi đơn hàng";
                    return new { isError = true, messError = messErr };
                }
                var model = _dbContext.OrderDetails.Where(p => p.OrderPid == Convert.ToInt64(OrderPid)).OrderByDescending(p=>p.Pid).Select(
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
                        Avatar = !String.IsNullOrEmpty(model.Avatar) ? UrlOrderImages + model.Avatar : "/css/skin/user-default.png",
                        QrCode = !String.IsNullOrEmpty(model.QrImage) ? UrlOrderImages + model.QrImage : "",
                        PersonalLink = model.PersonalLink,
                        CompanyWebsiteLink = model.CompanyWebsiteLink,
                        PersonalWebsiteLink = model.PersonalWebsiteLink,
                        QrLink = model.QrLink,
                    }


                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public dynamic InsertCard(CardInformation card)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var order = _dbContext.Orders.Where(x => x.Deleted == false && x.Pid == card.OrderPid).FirstOrDefault();
                    if (order == null)
                    {
                        messErr = "Lỗi đơn hàng";
                        return new { status = false, mess = messErr, code = "orderError" };
                    }
                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.Url == _common.EncodeTitle(card.Url)).FirstOrDefault();
                    if (duplicateUrl != null)
                    {
                        messErr = "Url trùng lặp";
                        return new { status = false, mess = messErr, code = "duplicateUrl" };
                    }
                    var allCards = _dbContext.OrderDetails.Where(x => x.OrderPid == card.OrderPid).Count();
                    var limit = _dbContext.ProductDetails.Where(x => x.Pid == order.ProductDetailPid).Select(x => x.UserAmount).FirstOrDefault();
                    if (allCards >= limit)
                    {
                        messErr = "Đã đạt số lượng card tối đa!";
                        return new { status = false, mess = messErr, code = "limitReached" };
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

                        dynamic saveFileStatus = _file.SaveFileOriginal(card.Avatar, UrlOrderImages, card.Avatar.FileName + "-" + newCard.Pid);
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

                    return new { status = false, mess = messErr, code = "general" };
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

                    var order = _dbContext.Orders.Where(x => x.Deleted == false && x.Pid == card.OrderPid).FirstOrDefault();
                    if (order == null)
                    {
                        messErr = "Lỗi đơn hàng";
                        return new { status = false, mess = messErr, code = "orderError" };
                    }
                    var duplicateUrl = _dbContext.OrderDetails.Where(x => x.Url == _common.EncodeTitle(card.Url) && x.Pid != card.Pid).FirstOrDefault();
                    if (duplicateUrl != null)
                    {
                        messErr = "Url trùng lặp";
                        return new { status = false, mess = messErr, code = "duplicateUrl" };
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


                            dynamic saveFileStatus = _file.SaveFileOriginal(card.Avatar, UrlOrderImages, card.Avatar.FileName + "-" + cardData.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (!String.IsNullOrEmpty(cardData.Avatar))
                                {
                                    _file.DeleteFile(UrlOrderImages, cardData.Avatar);
                                }
                                cardData.Avatar = saveFileStatus.fileName;
                                _dbContext.SaveChanges();

                            }


                        }

                        if (cardData.Url != _common.EncodeTitle(card.Url))
                        {
                            cardData.Url = _common.EncodeTitle(card.Url);
                            // Generate the QR code
                            _file.DeleteFile(UrlOrderImages, cardData.QrImage);
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
                    messErr = "Something is wrong!";

                    return new { status = false, mess = messErr, code = "general" };
                }
            }
        }
        public dynamic DeleteCard(int Pid)
        {
            try
            {

                var model = _dbContext.OrderDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                _file.DeleteFile(UrlOrderImages, model.Avatar);
                _file.DeleteFile(UrlOrderImages, model.QrImage);
                _dbContext.OrderDetails.Remove(model);
                _dbContext.SaveChanges();
                return new { isError = true, messError = "" };
            }
            catch (Exception ex)
            {
                return new { isError = false, messError = "Something is wrong!" };
            }
        }
        public NameCard GetNameCardById(string cardId)
        {
            NameCard nameCard = new NameCard();
            var cardData = _dbContext.OrderDetails.Where(p => p.Pid == Convert.ToInt64(cardId)).FirstOrDefault();

            if (cardData != null)
            {
                var order = _dbContext.Orders.Where(p => p.Pid == cardData.OrderPid).FirstOrDefault();
                nameCard.FullName = cardData.FullName;
                nameCard.CompanyName = order.CompanyName;
                nameCard.CompanyAddress = order.CompanyAddress;
                nameCard.Position = cardData.Position;
                nameCard.PersonalPhone = cardData.PersonalPhone;
                nameCard.HomePhone = cardData.HomePhone;
                nameCard.CompanyPhone = cardData.CompanyPhone;
                nameCard.PersonalEmail = cardData.PersonalEmail;
                nameCard.WorkEmail = cardData.WorkEmail;
                nameCard.Zalo = cardData.Zalo;
                nameCard.Facebook = cardData.Facebook;
                nameCard.Twitter = cardData.Twitter;
                nameCard.LinkedIn = cardData.LinkedIn;
                nameCard.Telegram = cardData.Telegram;
                nameCard.WhatsApp = cardData.WhatsApp;
                nameCard.Skype = cardData.Skype;
                nameCard.Viber = cardData.Viber;
                nameCard.PersonalLink = cardData.PersonalLink;
                nameCard.CompanyWebsiteLink = cardData.CompanyWebsiteLink;
                nameCard.PersonalWebsiteLink = cardData.PersonalWebsiteLink;
                nameCard.CustomLink = string.IsNullOrEmpty(cardData.CustomLink) ? new List<CustomLink>() : JsonConvert.DeserializeObject<List<CustomLink>>(cardData.CustomLink);
            }

            return nameCard;
        }
    }
}
