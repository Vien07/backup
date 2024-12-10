using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Admin
{
    public interface IUsersRepository
    {
        string GetData();
        bool Enable(string code, bool active);
        bool Delete(string code);
        string Edit(int Pid);
        bool Insert(User data);
        bool Update(User data, IFormFile Images, string newPassWord, int type);
        string Search(SearchDto searchData);
        string GetUserInfo();
        dynamic Validation(User data);
    }
}