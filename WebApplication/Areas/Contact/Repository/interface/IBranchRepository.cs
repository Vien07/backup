using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Contact.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Contact
{
    public interface IBranchRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Branch slide, List<MultiLang_Branch> multiLang_Branch, IFormFile images);
        dynamic Update(Models.Branch slide, List<MultiLang_Branch> multiLang_Branch, IFormFile images);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}