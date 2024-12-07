using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsCategory.Models;
using X.PagedList;
using ComponentUILibrary.Models;
using Steam.Core.Base.Models;

namespace Admin.PostsCategory.Services
{
    public interface IPostsCategoryService
    {
        //public Response<List<MultiLevelTreeList<Database.PostsCategory>>> GetList(ParamSearch search);
        //public Response<List<Database.PostsCategory>> GetList(ParamSearch search);
        public Response<List<PostsCategory_Item>> GetList(ParamSearch search);

        public Response<PostsCategoryDetail> GetById(int id);
        public Response<PostsCategory.Database.PostsCategory> Save(PostsCategoryModelEdit data);
        public Response Delete(int id);
        public Response<List<SelectControlData>> GetPostsCategoryParent(int id);
        public Response<List<SelectControlData>> GetListTemplatePage(string type);
        public Response<List<SelectControlData>> GetPostsCategoryTreeChildrenByParentId(long ParentId);
        public List<Admin.PostsCategory.Database.PostsCategory> GetChildrenPostCategory(long parentId);
        //public Response<List<SelectControlData>> GetPostsCategoryParent();
        public Response<string> GenerateXMLRewriteUrl();
        public List<Database.PostsCategory> GetCategories(string slug);
        public List<Database.PostsCategory> GetCategories(int pid);
        public List<Database.PostsCategory> GetRootCategories(string slug);

    }
}