using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DTO.Common;

namespace CMS.Areas.Admin
{
    public interface ILogRepository
    {
        dynamic GetList(SearchDto search);
        dynamic GetListError(SearchDto search);
    }
}