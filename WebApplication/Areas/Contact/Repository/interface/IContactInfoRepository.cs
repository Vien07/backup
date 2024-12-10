
using Microsoft.AspNetCore.Http;

namespace CMS.Areas.Contact
{
    public interface IContactInfoRepository
    {
        dynamic Update(IFormCollection data, string langKey);
        string GetData(string langKey);

    }
}