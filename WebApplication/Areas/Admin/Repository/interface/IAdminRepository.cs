using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Areas.Admin
{
    public interface IAdminRepository
    {
        bool Login(string code, string password, string ip, bool rememberMe = false);
        bool RecoveryPassword(string email);

        bool ValidateRecoveryPassword(string key);
        List<string> GetPermissionForUser();

    }
}