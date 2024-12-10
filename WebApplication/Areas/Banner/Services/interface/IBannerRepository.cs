using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Banner.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Banner
{
    public interface IBannerRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(Models.Banner banner, List<MultiLang_Banner> multiLang_Banner, string listPage, IFormFile images);
        dynamic Update(Models.Banner banner, List<MultiLang_Banner> multiLang_Banner, string listPage, IFormFile images);
        bool Count(int code) ;
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);

    }
}