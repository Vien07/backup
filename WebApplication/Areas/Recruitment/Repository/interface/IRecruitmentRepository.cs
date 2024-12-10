
using Microsoft.AspNetCore.Http;
using CMS.Areas.Shared.Models;
using CMS.Areas.Recruitment.Models;
using System.Collections.Generic;

using DTO.Common;

namespace CMS.Areas.Recruitment
{
    public interface IRecruitmentRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(RecruitmentDetail newsDetail, List<MultiLang_RecruitmentDetail> multiLangRecruitmentDetail,
                           IFormFile Images, List<Temp_Images> listImagesRecruitment,
                           List<Temp_MultiLang_Images> listLangImagesRecruitment, string listCates);
        dynamic Update(RecruitmentDetail newsDetail,
            List<MultiLang_RecruitmentDetail> multiLangRecruitmentDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesRecruitment, List<Temp_MultiLang_Images> listLangImagesRecruitment, string listCates);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
    }
}