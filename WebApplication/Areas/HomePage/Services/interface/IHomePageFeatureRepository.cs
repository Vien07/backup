using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.HomePage.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.HomePage
{
    public interface IHomePageFeatureRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.HomePage slide, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images);
        dynamic Update(Models.HomePage slide, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}