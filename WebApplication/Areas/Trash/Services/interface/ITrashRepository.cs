using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Trash
{
    public interface ITrashRepository
    {
        dynamic LoadData(SearchDto search);
        List<dynamic> GetDataAbout();
        List<dynamic> GetDataNews();
        dynamic Undo(int pid, int cateId);
        dynamic Delete(int pid, int cateId);
        dynamic DeleteAll();
    }
}