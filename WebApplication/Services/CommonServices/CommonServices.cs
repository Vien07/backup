using CMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CmsUtilities.SecurityHelper;
using DTO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using Microsoft.CodeAnalysis;
using CMS.Areas.Shared.Models;
using DTO.Customer;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using CMS.Areas.Product.Models;
using CMS.Areas.Promotion.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using DTO.Product;
using DTO.Common;
using System.Globalization;
using System.Reflection;

namespace CMS.Services.CommonServices
{
    public class CommonServices : ICommonServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memory;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private readonly DBContext _dbContext;
        private string FormatMoney = "";
        public CommonServices(IHttpContextAccessor httpContextAccessor, DBContext dbContext, IMemoryCache memory)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            FormatMoney = GetConfigValue(ConstantStrings.KeyFormatMoney);
            _memory = memory;
        }
        public void SaveLog(int status, string action, dynamic data)
        {
            try
            {
                string code = GetUserAdminInfo().account;
                var log = new Log();
                log.AdminUser = code;
                log.IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                log.LoginTime = DateTime.Now;
                log.Status = status;
                log.Type = action;
                log.Description = data.Title;
                log.PidDetail = Convert.ToInt32(data.Pid);
                log.PidCate = Convert.ToInt32(data.Cate);
                _dbContext.Logs.Add(log);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public void SaveLogAdmin(string code, int status)
        {
            try
            {
                var data = new Log();
                data.AdminUser = code;
                data.IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                data.LoginTime = DateTime.Now;
                data.Status = status;
                data.Type = "login";
                _dbContext.Logs.Add(data);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public string GetConfigValue(string key)
        {
            try
            {
                var rs = _dbContext.Configurations.Where(p => p.NameKey == key).FirstOrDefault();
                if (rs != null)
                {
                    return rs.Value;
                }
                return "";
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        public string GetHashSha256(string text)
        {
            return SecurityHelper.Sha256(text);
        }
        public string EncodeTitle(string s)
        {
            try
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD);
                string tempString = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                string rs = tempString.ToLower().Trim().Replace(" ", "-");
                char[] chars = @"$%#@ø–!*?;:~`+=()[]{}|\'<>,“”/^&"".&‘’‚“”„†‡‰‹›♠♣♥♦←↑→↓™!#$%'()*+,-./:;<=>?@[\]^_`{|}~-–—-¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿þÿΑαΒβΓγΔδΕεΖζΗηΘθΙιΚκΛλΜμΝνΞξΟοΠπΡρΣσΤτΥυΦφΧχΨψΩω●•".ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    string strChar = chars.GetValue(i).ToString();
                    if (rs.Contains(strChar))
                    {
                        rs = rs.Replace(strChar, "-");
                    }
                }
                rs = rs.Replace(" ", "-");
                var arr = rs.ToCharArray();
                int kt = 0;
                int tempPos = 2;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (kt > 1)
                    {
                        rs = rs.Remove(i - tempPos, 1);
                        tempPos++;
                    }

                    if (arr[i].ToString() == "-")
                    {
                        kt++;
                    }
                    else
                    {
                        kt = 0;
                    }
                }
                while (rs.ToCharArray()[rs.ToCharArray().Length - 1].ToString() == "-")
                {
                    var rsArr = rs.ToCharArray();
                    if (rsArr[rsArr.Length - 1].ToString() == "-")
                    {
                        rs = rs.Remove(rsArr.Length - 1, 1);
                    }
                    if (rsArr[0].ToString() == "-")
                    {
                        rs = rs.Remove(0, 1);
                    }
                }
                return rs;
            }
            catch (Exception)
            {

                return "";
            }

        }
        public string RemoveHtmlTag(string input)
        {
            try
            {
                return Regex.Replace(input, "<.*?>", String.Empty);
            }
            catch
            {
                return "";
            }
        }
        public string RemoveScripsTag(string input)
        {
            try
            {
                Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                var output = rRemScript.Replace(input, "");
                return output;
            }
            catch
            {
                return "";
            }
        }
        public string ConvertFormatToCultureDateTime(string format)
        {
            try
            {
                format = format.ToLower();
                if (format == "dd/mm/yyyy")
                {
                    return "dd/M/yyyy";

                }
                else if (format == "dd-mm-yyyy")
                {
                    return "dd-M-yyyy";

                }
                else if (format == "dd.mm.yyyy")
                {
                    return "dd.M.yyyy";
                }
                return "dd/M/yyyy";
            }
            catch (Exception ex)
            {
                return "dd/M/yyyy";
            }
        }
        public string ConvertFormatMoney(dynamic money)
        {
            try
            {
                money = money.ToString("#,##0").Replace(",", FormatMoney);
                return money;

            }
            catch (Exception)
            {

                return money;
            }
        }
        public string BizmacEncrytion(string plainText)
        {
            return SecurityHelper.EncryptString(plainText);
        }
        public string BizmacDecrytion(string encryptText)
        {
            return SecurityHelper.DecryptString(encryptText);
        }
        public string GetWordsInStringWithSymbol(string q, string start, string end)
        {
            try
            {
                string rs = "";
                string pattern = start + @"[\w-" + end + "]+";
                Regex rgx = new Regex(pattern);
                string sentence = q; ;

                foreach (Match match in rgx.Matches(sentence))
                {
                    rs += match.Value + ";";
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> GetCallAPI(string requestUri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var responseTask = await client.GetAsync(requestUri);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        return responseTask.Content.ReadAsStringAsync().Result;
                    }
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public string RemoveSign4VietnameseString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] VietnameseSigns = new string[] {

                    "aAeEoOuUiIdDyY",

                    "áàạảãâấầậẩẫăắằặẳẵ",

                    "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

                    "éèẹẻẽêếềệểễ",

                    "ÉÈẸẺẼÊẾỀỆỂỄ",

                    "óòọỏõôốồộổỗơớờợởỡ",

                    "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

                    "úùụủũưứừựửữ",

                    "ÚÙỤỦŨƯỨỪỰỬỮ",

                    "íìịỉĩ",

                    "ÍÌỊỈĨ",

                    "đ",

                    "Đ",

                    "ýỳỵỷỹ",

                    "ÝỲỴỶỸ"
                };

                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
                return str;
            }
            return "";
        }
        public string GetIdFromDateTimeNow()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                       DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                       DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }
        public void RenderCatetories<T>(List<T> categories, ref string result, string moduleUrl)
        {
            try
            {
                StringBuilder sb = new StringBuilder("<ul>");
                foreach (var item in categories)
                {
                    sb.AppendFormat("<li><a href='{0}' title='{1}' alt='{1}' target='_self'>{1}</a>", moduleUrl + item.GetType().GetProperty("Slug").GetValue(item) + "/", item.GetType().GetProperty("Title").GetValue(item));

                    if ((item.GetType().GetProperty("Children").GetValue(item) as List<T>).Count > 0)
                    {
                        string html = "";
                        RenderCatetories(item.GetType().GetProperty("Children").GetValue(item) as List<T>, ref html, moduleUrl);
                        sb.Append(html);
                    }
                    sb.Append("</li>");
                }

                sb.Append("</ul>");
                result += sb.ToString();
            }
            catch
            {

            }
        }
        public void SaveLogError(Exception ex)
        {
            try
            {
                var err = new LogError();
                err.Trace = ex.StackTrace.ToString();
                err.Message = ex.Message;
                _dbContext.LogErrors.Add(err);
                _dbContext.SaveChanges();
            }
            catch (Exception exe)
            {

            }

        }
        public string GetProvince(string code)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + string.Format("\\wwwroot\\data\\VietNam\\{0}.json", "province"));

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    dynamic items = JsonConvert.DeserializeObject<dynamic>(json);
                    return items[code]["name_with_type"];
                }
            }
            catch
            {
                return "";
            }
        }
        public string GetDistrict(string parentCode, string code)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + string.Format("\\wwwroot\\data\\VietNam\\district\\{0}.json", parentCode));

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    dynamic items = JsonConvert.DeserializeObject<dynamic>(json);
                    return items[code]["name_with_type"];
                }
            }
            catch
            {
                return "";
            }
        }
        public string GetWard(string parentCode, string code)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + string.Format("\\wwwroot\\data\\VietNam\\ward\\{0}.json", parentCode));

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    dynamic items = JsonConvert.DeserializeObject<dynamic>(json);
                    return items[code]["name_with_type"];
                }
            }
            catch
            {
                return "";
            }
        }
        public string GetAddress(string address, string ward, string district, string city)
        {
            try
            {
                var rs = new List<string>();

                if (!string.IsNullOrEmpty(address))
                    rs.Add(address);

                if (!string.IsNullOrEmpty(ward))
                    rs.Add(ward);

                if (!string.IsNullOrEmpty(district))
                    rs.Add(district);

                if (!string.IsNullOrEmpty(city))
                    rs.Add(city);

                return string.Join(", ", rs);
            }
            catch
            {
                return "";
            }

        }
        public CustomerDto GetProfile(long pid)
        {
            try
            {
                var customer = _dbContext.Customers.Where(x => x.Pid == pid && !x.Deleted && x.Enabled).Select(x => new CustomerDto
                {
                    Pid = x.Pid,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    Province = x.Province,
                    District = x.District,
                    Ward = x.Ward,
                    PicThumb = !String.IsNullOrEmpty(x.PicThumb) ? UrlCustomerImages + x.PicThumb : "",
                }).FirstOrDefault();

                return customer;
            }
            catch
            {
                return null;
            }
        }
        public CookieDto GetUserAdminInfo()
        {
            try
            {
                var getUserCookie = _httpContextAccessor.HttpContext.Request.Cookies["BizMaC"];
                var userData = new CookieDto();
                if (!string.IsNullOrEmpty(getUserCookie))
                {
                    userData = JsonConvert.DeserializeObject<CookieDto>(BizmacDecrytion(getUserCookie));
                }
                else
                {
                    var getUserSession = _httpContextAccessor.HttpContext.Session.GetString("BizMaC");
                    userData = JsonConvert.DeserializeObject<CookieDto>(BizmacDecrytion(getUserSession));
                }

                return userData;
            }
            catch
            {
                return null;
            }
        }
        public List<Promotion_Product> getAllPromotions()
        {
            if (!_memory.TryGetValue(ConstantStrings.CachePromotionName, out List<Promotion_Product> cacheValue))
            {
                cacheValue = (from a in _dbContext.PromotionDetails
                              join b in _dbContext.Promotion_Products on a.Pid equals b.PromotionPid
                              where !a.Deleted && a.Enabled && a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now
                              select b).ToList();
                _memory.Set(ConstantStrings.CachePromotionName, cacheValue);
            }
            return cacheValue;
        }
        public List<OptionProduct> getAllProductOptions()
        {
            var cacheValue = (from a in _dbContext.ProductOptions
                              join b in _dbContext.ProductOption_ProductDetails on a.Pid equals b.ProductOptionPid
                              where !a.Deleted && a.Enabled && b.Status
                              select new OptionProduct
                              {
                                  Order = a.Order,
                                  ProductDetailPid = b.ProductDetailPid,
                                  ProductOptionPid = b.ProductOptionPid,
                                  Price = b.Price,
                                  PriceDiscount = b.PriceDiscount,
                              }).OrderByDescending(x => x.Order).ToList();
            return cacheValue;
        }
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
        public IList<T> ReadCsvStream<T>(Stream stream, bool skipFirstLine = true, string csvDelimiter = ",") where T : new()
        {
            var records = new List<T>();
            using (var reader = new StreamReader(stream, Encoding.UTF8, true)) // Specify UTF8 encoding
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new string[] { csvDelimiter }, StringSplitOptions.None);
                    if (skipFirstLine)
                    {
                        skipFirstLine = false;
                    }
                    else
                    {
                        var item = new T();
                        var properties = item.GetType().GetProperties();
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (i < properties.Length)
                            {
                                var property = properties[i];
                                var convertedValue = Convert.ChangeType(values[i], property.PropertyType, CultureInfo.GetCultureInfo("vi-VN"));
                                property.SetValue(item, convertedValue, null);
                            }
                        }

                        records.Add(item);
                    }
                }
            }

            return records;
        }
        public string ExportCsv<T>(IList<T> data, bool includeHeader = true, string csvDelimiter = ",")
        {
            var type = data.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            using (var stringWriter = new StringWriter())
            {
                if (includeHeader)
                {
                    stringWriter.WriteLine(
                        string.Join<string>(
                            csvDelimiter, itemType.GetProperties().Select(x => x.Name)
                        )
                    );
                }

                foreach (var obj in data)
                {
                    var vals = obj.GetType().GetProperties().Select(pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    }
                    );

                    string line = string.Empty;
                    foreach (var val in vals)
                    {
                        if (val.Value != null)
                        {
                            var escapeVal = val.Value.ToString();
                            // Check if the value contans a comma and place it in quotes if so
                            if (escapeVal.Contains(",", StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = string.Concat("\"", escapeVal, "\"");
                            }

                            // Replace any \r or \n special characters from a new line with a space
                            if (escapeVal.Contains("\r", StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = escapeVal.Replace("\r", " ", StringComparison.OrdinalIgnoreCase);
                            }

                            if (escapeVal.Contains("\n", StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = escapeVal.Replace("\n", " ", StringComparison.OrdinalIgnoreCase);
                            }

                            line = string.Concat(line, escapeVal, csvDelimiter);
                        }
                        else
                        {
                            line = string.Concat(line, string.Empty, csvDelimiter);
                        }
                    }

                    stringWriter.WriteLine(line.TrimEnd(csvDelimiter.ToCharArray()));
                }

                return stringWriter.ToString();
            }
        }
    }
}
