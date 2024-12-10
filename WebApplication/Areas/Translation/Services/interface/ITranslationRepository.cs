namespace CMS.Areas.Translation
{
    public interface ITranslationRepository
    {
        dynamic LoadData();
        dynamic Update(dynamic data);
        dynamic Validation(dynamic data);
    }
}