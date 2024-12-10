
using CMS.Areas.Recruitment.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Recruitment
{
    public interface IRecruitmentCateRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(RecruitmentCate RecruitmentCate, List<MultiLang_RecruitmentCate> multiLang_RecruitmentCate);
        dynamic Update(RecruitmentCate RecruitmentCate, List<MultiLang_RecruitmentCate> multiLang_RecruitmentCate);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool MoveRow(long from, long to);

    }
}