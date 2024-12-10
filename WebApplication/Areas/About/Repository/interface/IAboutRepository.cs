using CMS.Areas.About.Models;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CMS.Areas.About
{
    public interface IAboutRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(AboutDetail aboutDetail, List<MultiLang_AboutDetail> multiLangAboutDetail);
        dynamic Update(AboutDetail aboutDetail, List<MultiLang_AboutDetail> multiLangAboutDetail);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
        bool Preview(string obj, string objDetail, IFormFile PicThumb);
        bool SaveStatus(long pid, bool value, string type);
    }
}