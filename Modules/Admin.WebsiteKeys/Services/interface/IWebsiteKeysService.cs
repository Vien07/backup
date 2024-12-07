using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.WebsiteKeys.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.WebsiteKeys.Service
{
    public interface IWebsiteKeysService
    {
         public Response<IPagedList<Database.WebsiteKeys>> GetList(ParamSearch search);
        public Response<WebsiteKeysDetail> GetById(int id);
        public Response<WebsiteKeys.Database.WebsiteKeys> Save(WebsiteKeysModelEdit input);
        public Response Delete(List<int> ids);


    }
}