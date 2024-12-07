using Admin.ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace Admin.ProductManagement.ScheduleTask
{
    public class GetMisaAccessTokenScheduleTask /*: IInvocable*/
    {
        private readonly IMisaApiService _rep;
        public GetMisaAccessTokenScheduleTask(IMisaApiService rep)
        {
            _rep = rep;
        }

        public async Task Invoke()
        {
            try
            {
                var model = _rep.GenerateAccessToken();
                await new Task<string>(() => string.Empty);
            }
            catch
            {

            }
        }
    }
}
