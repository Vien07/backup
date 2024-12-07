using Microsoft.EntityFrameworkCore;
using Admin.Collection.Database;
using static Admin.Collection.Constants.CollectionConstants;

namespace Admin.Collection.Database
{
    public class CollectionContext : DbContext
    {
        public CollectionContext(DbContextOptions<CollectionContext> options) : base(options)
        {

        }


        public virtual DbSet<CollectionConfig> CollectionConfigs { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Collection_Files> Collection_Files { get; set; }

        public virtual DbSet<CollectionCateConfig> CollectionCateConfigs { get; set; }
        public virtual DbSet<CollectionCate> CollectionCates { get; set; }
        public virtual DbSet<Collection_CollectionCate> Collection_CollectionCates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollectionContext).Assembly);
            SeedCollectionConfig(modelBuilder);

        }
        protected void SeedCollectionConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CollectionConfig>().HasData(
                 new CollectionConfig
                 {
                     Pid = 1,
                     Key = Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new CollectionConfig
                    {
                        Pid = 2,
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
                    new CollectionConfig
                    {
                        Pid = 3,
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
                    new CollectionConfig
                    {
                        Pid = 4,
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
                    new CollectionConfig
                    {
                        Pid = 5,
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
                    new CollectionConfig
                    {
                        Pid = 6,
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
                    new CollectionConfig
                    {
                        Pid = 7,
                        Key = Config.Website.PreSlug,
                        Value = "/pre-slug/",
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