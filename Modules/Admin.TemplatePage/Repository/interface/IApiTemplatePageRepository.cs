using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.TemplatePage.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Repository;

namespace Admin.TemplatePage
{
    public interface IApiTemplatePageRepository : IRepository<Database.TemplatePage>
    {


    }
}