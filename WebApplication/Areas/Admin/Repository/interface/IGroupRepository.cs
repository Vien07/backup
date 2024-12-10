using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Admin.Models;
using DTO.Common;

namespace CMS.Areas.Admin
{
    public interface IGroupRepository
    {
        string GetData();
        bool Enable(int code, bool active);
        bool Delete(int code);
        string Edit(int code);
        bool Insert(GroupUser data);
        bool Update(GroupUser data);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(GroupUser data);

    }
}