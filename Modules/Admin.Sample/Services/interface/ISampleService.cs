using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Sample.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.Sample.Database;

namespace Admin.Sample
{
    public interface ISampleService
    {
        public PagedListDto<SampleDetailDTO> GetList(ParamSearch search);

        public long Save(SampleModelEdit input);
        public Sample.Database.SampleDetail Delete(List<int> ids);



    }
}