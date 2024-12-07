using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using ComponentUILibrary.Models;

namespace Admin.PostsManagement.Services
{
    public interface IPostsManagementService
    {
        public Response<PostsManagement_List> GetList(PostsManagementModel.ParamSearch search);
        public Response<PostsManagementModel.PostsManagementDetail> GetById(int id);
        public Response<Database.PostsManagement> Save(SaveModel data);
        public Response Delete(List<int> ids);
        public List<Admin.PostsCategory.Database.PostsCategory> GetChildrenPostCategory(long parentId);
        public List<SelectControlData> GetPostsCategoryParent();
        public bool SaveImage(SaveModel data);


    }
}