
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Database;
using Admin.PostsManagement.Models;
using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.PostsManagement.Constants;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Admin.PostsCategory.Database;
using Steam.Core.Common;
using Microsoft.Extensions.Configuration;
using Admin.PostsCategory.Constants;
using Admin.PostsManagement.Api.Models.Response;
using Admin.SEO;
using Admin.PostsCategory;
using Admin.PostsManagement.Api.Models.Request;
using Steam.Infrastructure.Repository;
using Admin.SEO.Services;
using Admin.PostsCategory.Services;
namespace Admin.PostsManagement.Services
{
    public class ApiPostsManagementService : IApiPostsManagementService
    {
        private readonly IConfiguration configuration;

        private ILoggerHelper _logger;
        Dictionary<string, string> _CONFIG;
        string MedidaFileServer = "";
        private readonly IRepositoryConfig<Database.PostsManagementConfig> _repPostsManagementConfig;
        private readonly IRepository<Database.PostsManagement> _repPostsManagement;
        private readonly IRepository<Admin.SEO.Database.SEO> _repSEO;
        private readonly IRepository<Admin.PostsCategory.Database.PostsCategory> _repPostsCategory;

        public ApiPostsManagementService(
             IRepository<Database.PostsManagement> repPostsManagement,
            IRepository<Admin.PostsCategory.Database.PostsCategory> repPostsCategory,
            IRepository<Admin.SEO.Database.SEO> repSEO, 
            IRepositoryConfig<Database.PostsManagementConfig> repPostsManagementConfig,
            ILoggerHelper logger)
        {

            _repPostsManagement = repPostsManagement;
            _repPostsManagementConfig = repPostsManagementConfig;
            _repSEO = repSEO;
            _repPostsCategory = repPostsCategory;
            _logger = logger;
            _CONFIG = _repPostsManagementConfig.GetAllConfigs();


        }
        public Response<PostDetailResponse> GetPostBySlug(GetPostBySlugRequest input)
        {
            Response<PostDetailResponse> rs = new Response<PostDetailResponse>();
            PostDetailResponse post = new PostDetailResponse();
            try
            {
                //long postPid = 0;
                //var tempSEO = new object();//;_srvSEO.GetSEO(input.PostSlug, PostsManagementConstants.ModuleInfo.ModuleCode);

                //if (tempSEO != null)
                //{
                //    var tempPost = _repPostsManagement.Query().Where(p => p.Enabled == true).Where(p => p.Pid == tempSEO.FirstOrDefault().PostPid).ToList();

                //     post = (from a in tempSEO
                //                 join b in tempPost on a.PostPid equals b.Pid
                //                 select new PostDetailResponse
                //                 {
                //                     Description = a.Description,
                //                     Content = b.Content,
                //                     Author = b.Author,
                //                     LinkAuthor = b.LinkAuthor,
                //                     Images_Caption = b.Images_Caption,
                //                     Images_Alt = b.Images_Alt,
                //                     Images_Description = b.Images_Description,
                //                     ImagesPath = (SystemInfo.MedidaFileServer + b.FilePath + b.Images),
                //                     PublishDate = b.PublishDate,
                //                     SeeMore = b.SeeMore,
                //                     Slug = a.PostSlug,
                //                     TableOfContent = b.TableOfContent,
                //                     Meta = a.Meta.ToRemoveBreakSympol()
                //                 }).Single();



                //}
                //rs.IsError = false;

                //rs.StatusCode = 200;
                //rs.Data = post;
                //try { _repSEO.SaveChanges(); } catch (Exception ex) { }
                //_srvSEO.CountSEO(input.PostSlug, PostsManagementConstants.ModuleInfo.ModuleCode);
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        public ResponseList<List<Post_Item>> GetListPostByCateSlug(Api.Models.Request.GetListPostBySlug input)
        {
            ResponseList<List<Post_Item>> rs = new ResponseList<List<Post_Item>>();
            try
            {

                //long catePid = 0;
                //var tempRootCate = _srvCate.GetRootCategories(input.RootSlug).FirstOrDefault();
                //if (tempRootCate != null)
                //{
                //    catePid = tempRootCate.Pid;
                //}
                //if (!String.IsNullOrEmpty(input.CateSlug))
                //{
                //    var cate = _srvCate.GetCategories(input.CateSlug).FirstOrDefault();
                //    var cateSEO = _srvSEO.GetSEO(input.CateSlug, PostsCategoryConstants.ModuleInfo.ModuleCode).FirstOrDefault(); 
                //    rs.Meta = cateSEO.Meta.ToRemoveBreakSympol();

                //    catePid = cate.Pid;

                //}
                //var listSeo =_srvSEO.GetSEOsByModuleCode(PostsManagementConstants.ModuleInfo.ModuleCode);
                //var listPost = _repPostsManagement.Query().Where(p => p.Enabled == true).Where(p => p.Enabled == true).Where(p => p.CateID == catePid).ToList();
                //var posts = (
                //       from a in listPost
                //       join b in listSeo on a.Pid equals b.PostPid
                //       select new Api.Models.Response.Post_Item
                //       {
                //           Title = a.Title,
                //           Slug = b.PostSlug,
                //           Description = a.Description,
                //           CateSlug = input.RootSlug,
                //           Images = a.Images,
                //           ImagesPath = MedidaFileServer + a.FilePath + a.Images,
                //           PublishDate = a.PublishDate,
                //           Images_Alt = a.Images_Alt
                //       }).ToList().ToPagedList(input.PageIndex, input.PageSize);

                //rs.IsError = false;

                //rs.StatusCode = 200;
                //rs.PageCount = posts.PageCount;
                //rs.PageIndex = posts.PageNumber;
                //rs.PageSize = posts.PageSize;
                //rs.TotalItem = posts.LastItemOnPage;
                //rs.Data = posts.DeepClone<List<Post_Item>>();
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListNewPostByCateSlug(Api.Models.Request.GetListNewPostByCateSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();
            try
            {

                long catePid = 0;
                var tempRootCate = _repPostsCategory.Query().Where(p => p.Enabled == true).Where(p => p.Slug == input.RootSlug && p.ParentID == 0).FirstOrDefault();
                if (tempRootCate != null)
                {
                    catePid = tempRootCate.Pid;
                }
                if (!String.IsNullOrEmpty(input.CateSlug))
                {
                    var cate = _repPostsCategory.Query().Where(p => p.Enabled == true).Where(p => p.Slug == input.CateSlug).FirstOrDefault();
                    catePid = cate.Pid;

                }
                var listPost = _repPostsManagement.Query().Where(p => p.Enabled == true).Where(p => p.CateID == catePid)
                    .Where(p => ("," + p.TypePost + ",").Contains(",new,"))
                    .OrderBy(r => Guid.NewGuid()).Take(10).ToList();
                var listSeo = _repSEO.Query().Where(p => p.ModuleCode == PostsManagementConstants.ModuleInfo.ModuleCode).ToList();
                var posts = (
                       from a in listPost
                       join b in listSeo on a.Pid equals b.PostPid
                       where (a.CateID == catePid)
                       select new Api.Models.Response.GetListPostBySlug
                       {
                           Title = a.Title,
                           Slug = b.PostSlug,
                           Description = a.Description,
                           Images = a.Images,
                           CateSlug = input.RootSlug,
                           ImagesPath = MedidaFileServer + a.FilePath + a.Images,
                           Images_Alt = a.Images_Alt
                       }).ToList();
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = posts;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListRelatePostByPostSlug(Api.Models.Request.GetListRelatePostByPostSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();
            try
            {
                var listPost = new List<Database.PostsManagement>();

                long postPid = 0;
                var postSEO = _repSEO.Query().Where(p => p.PostSlug == input.PostSlug && p.ModuleCode == PostsManagementConstants.ModuleInfo.ModuleCode).FirstOrDefault();
                if (postSEO != null)
                {
                    postPid = postSEO.PostPid;
                    var post = _repPostsManagement.Query().Where(p => p.Pid == postPid).FirstOrDefault();
                    var cateSub = post.SubCate;
                    long catePid = post.CateID;
                    if (!String.IsNullOrEmpty(cateSub))
                    {
                        var listCateSub = cateSub.Split(',');
                        var tempN = 0;
                        for (int i = 0; i <= input.TakeItem; i++)
                        {
                            var temp = false;
                            while (!temp)
                            {
                                var randomCate = listCateSub.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                                var randomPost = _repPostsManagement.Query().Where(p => p.Pid != postPid && ("," + p.SubCate + ",").Contains("," + randomCate + ",")).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                                var temppost = listPost.Where(p => p.Pid == randomPost.Pid).FirstOrDefault();
                                if (temppost == null)
                                {
                                    temp = true;

                                    listPost.Add(randomPost);
                                }
                                if (tempN >= input.TakeItem)
                                {
                                    temp = true;

                                }
                                tempN++;

                            }

                            if (tempN >= input.TakeItem)
                            {
                                break;

                            }
                        }
                    }
                    else
                    {
                        listPost = _repPostsManagement.Query().Where(p => p.Enabled == true).Where(p => p.CateID == catePid && p.Pid != postPid).OrderBy(x => Guid.NewGuid()).Take(input.TakeItem).ToList();


                    }
                    if (listPost.Count < input.TakeItem)
                    {
                        //var randomPost = _db.PostsManagements.Where(p => p.CateID == catePid && p.Pid != postPid).OrderBy(x => Guid.NewGuid()).Take(input.TakeItem).ToList();

                    }

                }
                else
                {



                }
                var listSeo = _repSEO.Query().ToList();

                var posts = (
                    from a in listPost
                    join b in listSeo on a.Pid equals b.PostPid
                    select new Api.Models.Response.GetListPostBySlug
                    {
                        Title = a.Title,
                        Slug = b.PostSlug,
                        Description = a.Description,
                        CateSlug = input.RootSlug,
                        Images = a.Images,
                        ImagesPath = MedidaFileServer + a.FilePath + a.Images,
                        Images_Alt = a.Images_Alt
                    }).ToList();
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = posts;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
    }

}
