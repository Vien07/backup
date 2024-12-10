using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.HomePage.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.HomePage
{
    public interface IHomePageIntroRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.HomePage slide, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images, IFormFile backgroundImage);
        dynamic Update(Models.HomePage slide, List<MultiLang_HomePage> multiLang_HomePage, IFormFile images, IFormFile backgroundImage);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}