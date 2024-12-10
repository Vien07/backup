using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DTO;
using CMS.Areas.Shared.Models;
using CMS.Areas.Convenience.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Convenience
{
    public interface IConvenienceRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Convenience convenience, List<MultiLang_Convenience> multiLang_Convenience, IFormFile images);
        dynamic Update(Models.Convenience Convenience, List<MultiLang_Convenience> multiLang_Convenience, IFormFile images);
        bool Count(int code);
        string SearchDto(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}