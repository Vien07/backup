using CMS.Services.CommonServices;
using Coravel.Invocable;
using System.Net.Http;
using System.Threading.Tasks;
using DTO;

namespace CMS.Services.WebsiteServices
{
    public class DomainRequestServices : IInvocable
    {
        private readonly ICommonServices _common;
        public DomainRequestServices(ICommonServices common)
        {
            _common = common;
        }
        public async Task Invoke()
        {
            try
            {
                string domain = _common.GetConfigValue(ConstantStrings.KeyRootDomain);
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(domain);
                    await new Task<string>(() => string.Empty);
                }
            }
            catch
            {

            }
        }
    }
}
