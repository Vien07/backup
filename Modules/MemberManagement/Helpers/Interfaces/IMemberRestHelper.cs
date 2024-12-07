using Steam.Core.Base.Models;

namespace Admin.MemberManagement.Helpers
{
    public interface IMemberRestHelper
    {
        Response POST(string name, string baseURL, string requestURL, Dictionary<string, string> parameters);
    }
}
