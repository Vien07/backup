using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DTO;
using CMS.Areas.Shared.Models;
using CMS.Areas.Partner.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Partner
{
    public interface IPartnerRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Partner partner, List<MultiLang_Partner> multiLang_Partner, IFormFile images);
        dynamic Update(Models.Partner Partner, List<MultiLang_Partner> multiLang_Partner, IFormFile images);
        bool Count(int code);
        string SearchDto(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}