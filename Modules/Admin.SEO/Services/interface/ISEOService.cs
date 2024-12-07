using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.SEO.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.SEO.Api.Models;
using System.Collections;

namespace Admin.SEO.Services
{
    public interface ISEOService
    {
        //public Response<IPagedList<Database.SEO>> GetList(ParamSearch search);
        public Response<SEO_List> GetList(ParamSearch search);

        public Response<SEODetail> GetById(int id);
        public Response<SEO.Database.SEO> SaveSEO(SEO.Database.SEO data);    
        public Response<SEO.Database.SEO> Save(SEO.Database.SEO data);
        public Response Delete(List<int> ids);

        public Response<dynamic> GetPostBySlug(string postSlug);
        public Response<List<Response_GetAllListSEOActive>> GetAllListSEOActive();
        public Response Delete(int id, string moduleCode);
        public List<Database.SEO> GetSEOsByModuleCode(string moduleCode);
        public Database.SEO GetSEO(long pid, string moduleCode);
        public List<Database.SEO> GetSEO(string Slug, string moduleCode);
        public bool CountSEO(string Slug, string moduleCode);
    }
}