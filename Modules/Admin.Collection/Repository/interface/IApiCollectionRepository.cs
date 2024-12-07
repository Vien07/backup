using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Collection.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.SEO.Models;
using Admin.Collection.Api.Models;
using Admin.Collection.Api.Models.Response;

namespace Admin.Collection
{
    public interface IApiCollectionRepository
    {

        public ResponseList<List<GetDetailCollection>> GetListProductWithCollection();
        public Response<List<GetListCollectionName>> GetListCollectionName();
        public Response<GetDetailCollection> GetDetailCollection(string slug);

    }
}