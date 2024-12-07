using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.SEO.Models;
using Admin.PostsManagement.Api.Models;
using Admin.PostsManagement.Api.Models.Response;

namespace Admin.PostsManagement.Services
{
    public interface IApiPostsManagementService
    {

        public Response<PostDetailResponse> GetPostBySlug(Api.Models.Request.GetPostBySlugRequest input);
        public ResponseList<List<Post_Item>> GetListPostByCateSlug(Api.Models.Request.GetListPostBySlug input);
        public ResponseList<dynamic> GetListNewPostByCateSlug(Api.Models.Request.GetListNewPostByCateSlug input);
        public ResponseList<dynamic> GetListRelatePostByPostSlug(Api.Models.Request.GetListRelatePostByPostSlug input);

    }
}