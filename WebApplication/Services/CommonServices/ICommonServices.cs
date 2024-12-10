using CMS.Areas.Product.Models;
using CMS.Areas.Promotion.Models;
using DTO.Common;
using DTO.Customer;
using DTO.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CMS.Services.CommonServices
{
    public interface ICommonServices
    {
        CookieDto GetUserAdminInfo();
        void SaveLogError(Exception ex);
        string GetConfigValue(string key);
        void SaveLog(int status, string action, dynamic data);
        void SaveLogAdmin(string code, int status);
        string GetHashSha256(string text);
        string EncodeTitle(string s);
        string RemoveHtmlTag(string input);
        string RemoveScripsTag(string input);
        string ConvertFormatToCultureDateTime(string format);
        string ConvertFormatMoney(dynamic money);
        string BizmacEncrytion(string plainText);
        string BizmacDecrytion(string encryptText);
        string GetWordsInStringWithSymbol(string q, string start, string end);
        Task<string> GetCallAPI(string requestUri);
        string RemoveSign4VietnameseString(string str);
        string GetIdFromDateTimeNow();
        void RenderCatetories<T>(List<T> categories, ref string result, string moduleUrl);
        string GetProvince(string code);
        string GetDistrict(string parentCode, string code);
        string GetWard(string parentCode, string code);
        string GetAddress(string address, string ward, string district, string city);

        CustomerDto GetProfile(long pid);
        List<Promotion_Product> getAllPromotions();
        List<OptionProduct> getAllProductOptions();
        string GenerateRandomString(int length);
        IList<T> ReadCsvStream<T>(Stream stream, bool skipFirstLine = true, string csvDelimiter = ",") where T : new();
        string ExportCsv<T>(IList<T> data, bool includeHeader = true, string csvDelimiter = ",");
    }
}