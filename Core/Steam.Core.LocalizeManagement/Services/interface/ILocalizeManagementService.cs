using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Services
{
    public interface ILocalizeManagementService
    {
         public Response<IPagedList<Database.LocalizeManagement>> GetList(ParamSearch search);
        public Response<LocalizeManagementDetail> GetById(int id);
        public Response<LocalizeManagement.Database.LocalizeManagement> Save(LocalizeManagementModelEdit input);
        public Response Delete(List<int> ids);

    }
}