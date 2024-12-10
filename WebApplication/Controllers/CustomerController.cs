using System;
using Microsoft.AspNetCore.Mvc;
using CMS.Repository;
using CMS.Services.WebsiteServices;
using CMS.Services.PageServices;
using DTO.Website;
using DTO;
using CMS.Services.CommonServices;
using Microsoft.AspNetCore.Http;
using CMS.Services.TranslateServices;
using DTO.Customer;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CMS.Middleware;
using DTO.Common;
using System.Text;
using DTO.Cart;
using System.Linq;

namespace CMS.Controllers
{
    public class CustomerController : Controller
    {

        private readonly IWebsiteServices _website;
        private readonly ICustomer_Repository _rep;
        private readonly ICommonServices _common;
        private readonly IPageServices _page;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslateServices _translate;
        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string OgTypeWebsite = ConstantStrings.OgTypeWebsite;
        private string DefaultLang = ConstantStrings.DefaultLang;
        public CustomerController(ICustomer_Repository rep, IWebsiteServices website, ICommonServices common,
            IPageServices page, IHttpContextAccessor httpContextAccessor, ITranslateServices translate)
        {
            _rep = rep;
            _common = common;
            _website = website;
            _page = page;
            _translate = translate;
            _httpContextAccessor = httpContextAccessor;
            _website.StartUp();
        }

        #region Inside
        [Auth("")]
        [HttpGet]
        public async Task<IActionResult> Index(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var customer = _rep.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.CustomerInfo = customer; 
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.CustomerId);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.setting-account");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View();
            }
            catch (Exception ex)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }

        [Auth("")]
        [HttpGet]
        public async Task<IActionResult> OrderManagement(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var customer = _rep.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Orderlist = await _rep.GetOrderList();
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.CustomerId);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.order-management");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View();
            }
            catch (Exception ex)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }

        [Auth("")]
        [HttpGet]
        public async Task<IActionResult> OrderDetail(string lang, string pid)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var customer = _rep.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.CustomerId);
                ViewBag.Pid = pid;
                var order = await _rep.GetOrderByPid(pid);
                if(order.ExpiredDate < DateTime.Now || order.Status != "Success")
                {
                    return Redirect("/");
                }
                ViewBag.Order = order;
                ViewBag.OrderImages = await _rep.GetOrderImages(pid);
                var nameCardData = await _rep.GetListNameCardByOrderId(pid, 1, 20);
                var (htmlTableBody, htmlPaging) = await _rep.RenderHtmlForTable(nameCardData);
                ViewBag.TableBody = htmlTableBody;
                ViewBag.Paging = htmlPaging;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.order-detail");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View();
            }
            catch (Exception ex)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }

        [Auth("")]
        [HttpGet]
        public IActionResult ChangeInfo(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var customer = _rep.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.CustomerInfo = customer;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.setting-account");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View();
            }
            catch (Exception ex)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }

        [Auth("")]
        [HttpPost]
        public async Task<JsonResult> UpdateProfile(CustomerUpdateDto model, IFormFile avatar)
        {
            var response = await _rep.UpdateProfile(model, avatar);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }

        [Auth("")]
        [HttpPost]
        public async Task<JsonResult> UpdatePassword(string currentPassword, string newPassword)
        {
            var response = await _rep.UpdatePassword(currentPassword, newPassword);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }
        #endregion

        #region View
        public async Task<IActionResult> SignUp(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.sign-up");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return await Task.Run(() => View());
            }
            catch
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        public async Task<IActionResult> SignIn(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.sign-in");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return await Task.Run(() => View());
            }
            catch
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        public async Task<IActionResult> NameCard(string lang, string url)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                var namecard = await _rep.GetNameCard(url);
                if(namecard == null)
                {
                    return Redirect("/");
                }
                ViewBag.NameCard = namecard;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = namecard.FullName;
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return await Task.Run(() => View());
            }
            catch
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        public async Task<IActionResult> ForgotPassword(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.forgot-password");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return await Task.Run(() => View());
            }
            catch
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        public async Task<IActionResult> ChangePassword(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var customer = _rep.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                ViewBag.CustomerInfo = customer;
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.CustomerId);

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.change-password");
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);
                return await Task.Run(() => View());
            }
            catch
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        #endregion

        [HttpPost]
        public async Task<JsonResult> SignInGoogle(string token)
        {
            try
            {
                var result = await _rep.LoginGoogle(token);
                if (result)
                {
                    return Json(_translate.GetUrl("url.setting-account"));
                }
                return Json(_translate.GetUrl("url.home"));
            }
            catch
            {
                return Json(_translate.GetUrl("url.home"));
            }
        }
        [HttpPost]
        public async Task<JsonResult> SignInFB(string token)
        {
            try
            {
                var result = await _rep.LoginFB(token);
                if (result)
                {
                    return Json(_translate.GetUrl("url.setting-account"));
                }
                return Json(_translate.GetUrl("url.home"));
            }
            catch
            {
                return Json(_translate.GetUrl("url.home"));
            }
        }
        #region Action Outside
        [HttpPost]
        public async Task<JsonResult> Register(CustomerDto model)
        {
            var response = await _rep.Register(model);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> Login(string email, string password, bool rememberMe)
        {
            var response = await _rep.Login(email, password, rememberMe);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }
        public async Task<IActionResult> Active(string email, string lang, string code)
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = DefaultLang;
            }
            _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

            var resp = await _rep.Active(email, code);
            if (resp)
            {
                return Redirect(_translate.GetUrl("url.sign-in"));
            }
            else
            {
                return Redirect(_translate.GetUrl("url.contact"));
            }
        }
        public IActionResult LogOut()
        {
            _httpContextAccessor.HttpContext.Session.Remove(ConstantStrings.CustomerCookieName);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(ConstantStrings.CustomerCookieName);
            return Redirect(_translate.GetUrl("url.home"));
        }
        [HttpPost]
        public async Task<JsonResult> SendMailForgotPassword(string email)
        {
            string newPassword = await _rep.ChangePasswordForEmail(email);
            if (newPassword == "email-error")
            {
                var responseError = new ResponseDto();
                responseError.isError = true;
                responseError.statusError = "Error";
                responseError.message = "";
                responseError.errorCode = "email";
                var resultError = JsonConvert.SerializeObject(responseError);
                return Json(resultError);
            }
            var response = _rep.SendMailForgotPassword(email, newPassword);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> EditPassword(string code, string email, string password)
        {
            var response = await _rep.EditPassword(code, email, password);
            response.message = _translate.GetString(response.message);
            var result = JsonConvert.SerializeObject(response);
            return Json(result);
        }
        [HttpPost]
        public IActionResult GetVCard(string CardId)
        {
            var nameCard = _rep.GetNameCardById(CardId);
            const string vCardVersion = "3.0";
            const string mimeType = "text/vcard";

            var vCard = new StringBuilder();
            vCard.AppendLine("BEGIN:VCARD");
            vCard.AppendLine($"VERSION:{vCardVersion}");
            vCard.AppendLine($"FN:{nameCard.FullName}");
            vCard.AppendLine($"N:{nameCard.FullName}");
            vCard.AppendLine($"ORG:{nameCard.CompanyName}");
            vCard.AppendLine($"TITLE:{nameCard.Position}");

            AppendIfNotEmpty(vCard, "TEL;TYPE=CELL,VOICE", nameCard.PersonalPhone);
            AppendIfNotEmpty(vCard, "TEL;TYPE=WORK,VOICE", nameCard.CompanyPhone);
            AppendIfNotEmpty(vCard, "TEL;TYPE=HOME,VOICE", nameCard.HomePhone);
            AppendIfNotEmpty(vCard, "EMAIL;TYPE=WORK", nameCard.WorkEmail);
            AppendIfNotEmpty(vCard, "EMAIL;TYPE=HOME", nameCard.PersonalEmail);
            // Explicitly label each URL field with a different type
            //AppendIfNotEmpty(vCard, "URL;TYPE=PersonalLink", nameCard.PersonalLink);
            //AppendIfNotEmpty(vCard, "URL;TYPE=PersonalWebsite", nameCard.PersonalWebsiteLink);
            //AppendIfNotEmpty(vCard, "URL;TYPE=CompanyWebsite", nameCard.CompanyWebsiteLink);

            if (nameCard.CustomLink != null && nameCard.CustomLink.Count > 0)
            {
                foreach (var item in nameCard.CustomLink)
                {
                    AppendIfNotEmpty(vCard, $"URL;TYPE=OTHER", item.Link);
                }
            }

            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Zalo", $"https://zalo.me/{nameCard.Zalo}");
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Facebook", nameCard.Facebook);
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Linkedin", nameCard.LinkedIn);
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Twitter", nameCard.Twitter);
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Telegram", $"https://t.me/{nameCard.Telegram}");
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Whatsapp", $"https://wa.me/{nameCard.WhatsApp}");
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Skype", $"skype:{nameCard.Skype}?chat");
            AppendIfNotEmpty(vCard, "X-SOCIALPROFILE;TYPE=Viber", $"viber://add?number={nameCard.Viber}");

            vCard.AppendLine("END:VCARD");

            var vCardBytes = Encoding.UTF8.GetBytes(vCard.ToString());
            return File(vCardBytes, mimeType, "contact.vcf");
        }

        private void AppendIfNotEmpty(StringBuilder vCard, string prefix, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                vCard.AppendLine($"{prefix}:{value}");
            }
        }

        public async Task<JsonResult> SearchListCard([FromForm] SearchNameCardDto model)
        {
            try
            {
                var response = await _rep.SearchListCard(model);
                return Json(new { isError = response.isError, data = response.data });
            }
            catch
            {
                return Json(new { isError = false });
            }
           
        }
        public JsonResult UploadBackgroundImage(IFormFile Background, string OrderPid)
        {
            try
            {
                var response = _rep.UploadBackgroundImage(Background, OrderPid);
                return Json(new { isError = response.isError, data = response.data, messError = response.messError});
            }
            catch
            {
                return Json(new { isError = false, messError = "Something is wrong!" });
            }

        }
        public JsonResult UpdateBackgroundImageFromList(string ImageId, string OrderPid)
        {
            try
            {
                var response = _rep.UpdateBackgroundImageFromList(ImageId, OrderPid);
                return Json(new { isError = response.isError, messError = response.messError ,data = response.data });
            }
            catch
            {
                return Json(new { isError = false, messError = "Something is wrong!" });
            }

        }

        public JsonResult UploadCSV(IFormFile File, string OrderPid)
        {
            var result = _rep.UploadCSV(File, OrderPid);
            return Json(new { isError = result.isError, data = result.data, messError = result.messError }); ;

        }
        public JsonResult ExportCSV(string OrderPid)
        {
            var result = _rep.ExportCSV(OrderPid);
            return Json(new { isError = result.isError, data = result.data, messError = result.messError }); ;

        }
        public JsonResult EditCard(int pid)
        {
            return Json(_rep.EditCard(pid));
        }
        public JsonResult InsertCard(CardInformation card)
        {
            return Json(new { error = _rep.InsertCard(card) });
        }
        public JsonResult UpdateCard(CardInformation card)
        {
            return Json(new { error = _rep.UpdateCard(card) });
        }

        public JsonResult DeleteCard(int Pid)
        {
            var result = _rep.DeleteCard(Pid);
            return Json(new { isError = result.isError, messError = result.messError });
        }
        #endregion
    }
}