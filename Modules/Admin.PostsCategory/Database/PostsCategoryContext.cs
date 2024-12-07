using Microsoft.EntityFrameworkCore;
using Admin.PostsCategory.Models;
using Admin.PostsCategory.Constants;
namespace Admin.PostsCategory.Database
{
    public class PostsCategoryContext : DbContext
    {
        public PostsCategoryContext(DbContextOptions<PostsCategoryContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.PostsCategoryConfig> PostsCategoryConfigs { get; set; }
        public virtual DbSet<Database.PostsCategory> PostsCategories { get; set; }
        public virtual DbSet<Database.PostsCategory_Files> PostsCategory_Files { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostsCategoryContext).Assembly);
            SeedPostsCategoryConfig(modelBuilder);
        }

        protected void SeedPostsCategoryConfig(ModelBuilder modelBuilder)
        {
            int id = 0;


            modelBuilder.Entity<PostsCategoryConfig>().HasData(
                 new PostsCategoryConfig
                 {
                     Pid = ++id,
                     Key = PostsCategoryConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new PostsCategoryConfig
                    {
                        Pid = ++id,
                        Key = PostsCategoryConstants.Config.Admin.MaxHeight,
                        Value = "350",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                    ,
                    new PostsCategoryConfig
                    {
                        Pid = ++id,
                        Key = PostsCategoryConstants.Config.Admin.ThumbHeight,
                        Value = "285",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                      ,
                    new PostsCategoryConfig
                    {
                        Pid = ++id,
                        Key = PostsCategoryConstants.Config.Admin.ThumbWidth,
                        Value = "456",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                      ,
                    new PostsCategoryConfig
                    {
                        Pid = ++id,
                        Key = PostsCategoryConstants.Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }   ,
                    new PostsCategoryConfig
                    {
                        Pid = ++id,
                        Key = PostsCategoryConstants.Config.Website.PreSlug,
                        Value = "danh-muc",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    },
                     new PostsCategoryConfig
                     {
                         Pid = ++id,
                         Key = PostsCategoryConstants.Config.Website.ApiUpdateRewritePostUrl,
                         Value = "{host}/api/ApiMasterPage/UpdateRewritePostUrl",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     }

             ); ;


        }


    }
}