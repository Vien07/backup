using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Contact
{
    public interface IContactListRepository
    {
        dynamic Update(IFormCollection data, string langKey);

        dynamic ReadContact(long Pid);
        dynamic GetData(SearchDto search);
        dynamic Delete(long[] Pid);
        dynamic Seen(long[] Pid);
        dynamic NotSeen(long[] Pid);
    }
}