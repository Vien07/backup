namespace CMS.Services.TranslateServices
{
    public interface ITranslateServices
    {
        string GetString(string key);
        string GetStringAdmin(string key);
        string GetStringWithLang(string key, string lang);
        string GetStringWithLangAdmin(string key, string lang);
        string GetUrl(string key);
    }
}