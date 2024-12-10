using DTO.Common;

namespace CMS.Areas.Contact
{
    public interface IEnquireListRepository
    {
        dynamic ReadContact(long Pid);
        dynamic GetData(SearchDto search);
        dynamic Delete(long[] Pid);
        dynamic Seen(long[] Pid);
        dynamic NotSeen(long[] Pid);
    }
}