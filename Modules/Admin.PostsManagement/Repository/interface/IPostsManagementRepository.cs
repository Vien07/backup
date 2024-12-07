using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Database;
using X.PagedList;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Repository;
namespace Admin.PostsManagement.Repository
{
    public interface IPostsManagementRepository : IRepository<Database.PostsManagement>
    {


    }
}