
using DTO.Common;

namespace CMS.Areas.Recruitment
{
    public interface ICandidateRepository
    {
        dynamic GetData(SearchDto search);
        dynamic Delete(long[] Pid);
        dynamic ReadContact(long Pid);
        dynamic Seen(long[] Pid);
        dynamic NotSeen(long[] Pid);
    }
}