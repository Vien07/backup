using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Slide.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Slide
{
    public interface ISlideRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Slide slide, List<MultiLang_Slide> multiLang_Slide, IFormFile images);
        dynamic Update(Models.Slide slide, List<MultiLang_Slide> multiLang_Slide, IFormFile images);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}