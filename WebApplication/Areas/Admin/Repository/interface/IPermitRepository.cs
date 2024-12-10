using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CMS.Areas.Admin
{
    public interface IPermitRepository
    {
        string GetDataGroup(int? groupUserCode, string txtSearch);
        string GetDataUser(int? userCode, string txtSearch);
        string InsertGroupPermission(int groupUserCode, string id, bool isCheked);
        string InsertGroupPermissionUser(int userCode, string id, bool isCheked);
    }
}