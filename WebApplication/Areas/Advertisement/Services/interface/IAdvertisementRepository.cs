using CMS.Areas.Advertisement.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Advertisement
{
    public interface IAdvertisementRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Advertisement slide, List<MultiLang_Advertisement> multiLang_Advertisement, IFormFile images, string listCates);
        dynamic Update(Models.Advertisement slide, List<MultiLang_Advertisement> multiLang_Advertisement, IFormFile images, string listCates);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}