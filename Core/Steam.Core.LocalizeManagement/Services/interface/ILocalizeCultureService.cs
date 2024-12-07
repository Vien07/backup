using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Services
{
    public interface ILocalizeCultureService
    {
        public Response<IPagedList<Database.LocalizeCulture>> GetList(ParamSearch search);
        public Response<LocalizeCultureDetail> GetById(int id);
        public Response<LocalizeManagement.Database.LocalizeCulture> Save(LocalizeCultureModelEdit input);
        public Response Delete(List<int> ids);


    }
}