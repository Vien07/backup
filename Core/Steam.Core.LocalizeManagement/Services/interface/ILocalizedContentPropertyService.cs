using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Services
{
    public interface ILocalizedContentPropertyService
    {
         public Response<IPagedList<Database.LocalizedContentProperty>> GetList(ParamSearch search);
        public Response<LocalizeManagement.Database.LocalizedContentProperty> Update(LocalizedContentPropertyModelEdit input);
        public Response Delete(List<int> ids);
        public string Translate(long entityID, string properyName, string entityType, string cultureID, string value);
        public bool SetLocallize(long entityID, string properyName, string entityType, string cultureID, string value);


    }
}