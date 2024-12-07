using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsCategory.Models;
using X.PagedList;
using ComponentUILibrary.Models;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Repository;

namespace Admin.PostsCategory.Repository
{
    public interface IPostsCategoryRepository : IRepository<Database.PostsCategory>
    {

    }    

}