using Microsoft.EntityFrameworkCore;
using Admin.PostsManagement.Database;
using static Admin.PostsManagement.Constants.PostsManagementConstants;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Admin.PostsManagement.Database
{
    public class PostsManagementContext : DbContext
    {
        private string _connectionString;
        private DbContextOptions<PostsManagementContext> _options;
        public PostsManagementContext(DbContextOptions<PostsManagementContext> options) : base(options)
        {
            _connectionString = Database.GetDbConnection().ConnectionString;
            _options = options;

        }


        public virtual DbSet<PostsManagementConfig> PostsManagementConfigs { get; set; }
        public virtual DbSet<PostsManagement> PostsManagements { get; set; }
        public virtual DbSet<PostsManagement_Files> PostsManagement_Files { get; set; }

        public virtual DbSet<PostsManagementCateConfig> PostsManagementCateConfigs { get; set; }
        public virtual DbSet<PostsManagementCate> PostsManagementCates { get; set; }
        public virtual DbSet<PostsManagement_PostsManagementCate> PostsManagement_PostsManagementCates { get; set; }


        public PostsManagementContext SetConnectionString(string newConnectionString)
        {
            if (newConnectionString != _connectionString)
            {
                _connectionString = newConnectionString;
                var builder = new DbContextOptionsBuilder<PostsManagementContext>(_options);
                builder.UseSqlServer(newConnectionString);
                PostsManagementContext newDb = new PostsManagementContext(builder.Options);
                return newDb;
            }else
            {
                return this;
            }    
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostsManagementContext).Assembly);
            SeedPostsManagementConfig(modelBuilder);

        }
        protected void SeedPostsManagementConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<PostsManagementConfig>().HasData(
                 new PostsManagementConfig
                 {
                     Pid = ++pid,
                     Key = Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Admin.MaxHeight,
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
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Admin.ThumbHeight,
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
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Admin.ThumbWidth,
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
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    }
                          ,
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Website.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }
                             ,
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Website.PreSlug,
                        Value = "/pre-slug/",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }  ,
                    new PostsManagementConfig
                    {
                        Pid = ++pid,
                        Key = Config.Admin.PostType,
                        Value = "Mới:new",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Website"
                    }
             ); ;


        }


    }
}