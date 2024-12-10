using System.Collections.Generic;
using DTO.Recruitment;
using DTO.News;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public interface IRecruitment_Repository
    {
        dynamic GetList(string lang, int page, string key);
        dynamic GetListBySlug(string lang, int page, string cate);
        string RemoveHtml(string input);
        dynamic GetRecruitmentPreview(string detailJson, string listJson, string base64, string lang);
        dynamic GetRecruitment(string slug, string lang);
        List<dynamic> GetRelateList(string slug, string lang);
        dynamic GetCate(string slug, string lang);
        dynamic GetCate(string lang);
        long GetPidCateBySlug(string slug);
        dynamic GetCateByRecruitmentPid(long pid, string lang);
        dynamic GetCateByPid(long pid, string lang);
        bool SendCV(CVDto model);
        Task<RecruitmentDto> GetRecruit(string slug, string lang);

        List<NewsDto> GetListHotAndNew(string lang);
        Dictionary<string, RecruitmentDto> GetPreNextRecruit(string slug, string lang);
        List<NewsCateDto> GetCateNews(string lang);
        bool checkRecruitIsExist(string lang);
    }
}