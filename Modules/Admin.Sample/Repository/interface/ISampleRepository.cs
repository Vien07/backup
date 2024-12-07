using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Sample.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Repository;
namespace Admin.Sample.Repository
{
    public interface ISampleRepository: IRepository<Database.SampleDetail>
    {


    }
}