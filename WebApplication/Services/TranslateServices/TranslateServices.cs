using Microsoft.AspNetCore.Http;
using DTO;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CMS.Services.TranslateServices
{
    public class TranslateServices : ITranslateServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TranslateServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //public string GetString(string key)
        //{
        //    try
        //    {
        //        var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
        //        if (string.IsNullOrEmpty(lang))
        //        {
        //            lang = ConstantStrings.DefaultLang;
        //        }
        //        string[] keys = key.Split('.');
        //        var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\" + lang + ".json";
        //        using (StreamReader r = new StreamReader(path))
        //        {
        //            string json = r.ReadToEnd();
        //            dynamic obj = JValue.Parse(json);
        //            return obj[keys[0]][keys[1]].ToString();
        //        }
        //    }
        //    catch
        //    {
        //        return key;
        //    }
        //}

        public string GetUrl(string key)
        {
            try
            {
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                string[] keys = key.Split('.');
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\" + lang + ".json";
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    return obj[keys[0]][keys[1]].ToString();
                }
            }
            catch
            {
                return key;
            }
        }
        public string GetString(string key)
        {
            try
            {
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                string[] keys = key.Split('.');
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\lang.json";
                string rs = key;
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    foreach (var item in obj)
                    {
                        if (item.key == keys[0])
                        {
                            foreach (var item2 in item.value)
                            {
                                if (item2.key == keys[1])
                                {
                                    if (lang == "vi")
                                    {
                                        rs = item2.value.vi;
                                    }
                                    else if (lang == "en")
                                    {
                                        rs = item2.value.en;
                                    }
                                }
                            }
                        }
                    }
                }
                return rs;
            }
            catch
            {
                return key;
            }
        }
        public string GetStringWithLang(string key, string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                string[] keys = key.Split('.');
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\" + lang + ".json";
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    return obj[keys[0]][keys[1]];
                }
            }
            catch
            {
                return key;
            }
        }
        public string GetStringAdmin(string key)
        {
            try
            {
                //var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                //if (string.IsNullOrEmpty(lang))
                //{
                //    lang = ConstantStrings.DefaultLang;
                //}
                var lang = ConstantStrings.DefaultLang;
                string[] keys = key.Split('.');
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\b-admin\lang\" + lang + ".json";
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    return obj[keys[0]][keys[1]].ToString();
                }
            }
            catch
            {
                return key;
            }
        }
        public string GetStringWithLangAdmin(string key, string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                string[] keys = key.Split('.');
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\b-admin\lang\" + lang + ".json";
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    return obj[keys[0]][keys[1]];
                }
            }
            catch
            {
                return key;
            }
        }
    }
}
