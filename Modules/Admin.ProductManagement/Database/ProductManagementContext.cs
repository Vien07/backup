using Microsoft.EntityFrameworkCore;
using Admin.ProductManagement.Constants;

namespace Admin.ProductManagement.Database
{
    public class ProductManagementContext : DbContext
    {
        public ProductManagementContext(DbContextOptions<ProductManagementContext> options) : base(options)
        {

        }

        public virtual DbSet<MisaApiConfig> MisaApiConfigs { get; set; }
        public virtual DbSet<MisaApiTracker> MisaApiTrackers { get; set; }
        public virtual DbSet<ProductSpecificaty> ProductSpecificaties { get; set; }
        public virtual DbSet<MisaBranch> MisaBranchs { get; set; }
        public virtual DbSet<ProductSpecificatyConfig> ProductSpecificatyConfigs { get; set; }
        public virtual DbSet<ProductConfig> ProductConfigs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductDetail_PostDetail> ProductDetail_PostDetails { get; set; }
        public virtual DbSet<Product_Files> Product_Files { get; set; }
        public virtual DbSet<ProductCategoryConfig> ProductCategoryConfigs { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductCategory_Files> ProductCategory_Files { get; set; }
        public virtual DbSet<ProductPolicy> ProductPolicies { get; set; }
        public virtual DbSet<ProductPolicyConfig> ProductPolicyConfigs { get; set; }
        public virtual DbSet<OrderManagement> OrderManagements { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderManagementConfig> OrderManagementConfigs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductManagementContext).Assembly);
            SeedMisaProductConfig(modelBuilder);
            SeedProductSpecificatyConfig(modelBuilder);
            SeedMisaApiConfig(modelBuilder);
            SeedMisaProductCategoryConfigs(modelBuilder);
            SeedProductPolicyConfig(modelBuilder);
            SeedOrderManagementConfig(modelBuilder);
        }

        protected void SeedMisaProductConfig(ModelBuilder modelBuilder)
        {
            var pid = 0;

            modelBuilder.Entity<ProductConfig>().HasData(
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.MaxWidth,
                      Value = "720",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.MaxHeight,
                      Value = "350",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.ThumbHeight,
                      Value = "285",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.ThumbWidth,
                      Value = "456",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.PageSize,
                      Value = "10",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigWebsite.PageSize,
                      Value = "10",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Website"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigWebsite.PreSlug,
                      Value = "/san-pham/",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Website"
                  },
                  new ProductConfig
                  {
                      Pid = ++pid,
                      Key = ProductConstants.ConfigAdmin.ProductTypes,
                      Value = "nam:nam;nu:nu,unisex:unisex",
                      Group = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                      Type = "Admin"
                  }
              );
        }
        protected void SeedProductSpecificatyConfig(ModelBuilder modelBuilder)
        {
            var pid = 0;

            modelBuilder.Entity<ProductSpecificatyConfig>().HasData(
                 new ProductSpecificatyConfig
                 {
                     Pid = ++pid,
                     Key = MisaApiTrackerConstants.PageSize,
                     Value = "12",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                 });
        }
        protected void SeedProductPolicyConfig(ModelBuilder modelBuilder)
        {
            var pid = 0;

            modelBuilder.Entity<ProductPolicyConfig>().HasData(
                 new ProductPolicyConfig
                 {
                     Pid = ++pid,
                     Key = ProductPolicyConstants.Config.Admin.GroupPolicy,
                     Value = "Chính sách:policy;Điều khoản:term",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 });
            modelBuilder.Entity<ProductPolicyConfig>().HasData(
                 new ProductPolicyConfig
                 {
                     Pid = ++pid,
                     Key = ProductPolicyConstants.Config.Admin.PageSize,
                     Value = "20",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 });
        }

        protected void SeedMisaApiConfig(ModelBuilder modelBuilder)
        {
            var pid = 0;
            modelBuilder.Entity<MisaApiConfig>().HasData(
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.AppID,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.Domain,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.SignatureInfo,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.AccessToken,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.LoginTime,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.PageSize,
                      Value = "12",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.SecretKey,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.BaseUrl,
                      Value = "https://graphapi.mshopkeeper.vn",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.CompanyCode,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  },
                  new MisaApiConfig
                  {
                      Pid = ++pid,
                      Key = MisaApiTrackerConstants.Environment,
                      Value = "",
                      CreateUser = "admin",
                      CreateDate = DateTime.Now,
                      LastLogin = DateTime.Now,
                      UpdateUser = "admin",
                      UpdateDate = DateTime.Now,
                  });
        }
        protected void SeedMisaProductCategoryConfigs(ModelBuilder modelBuilder)
        {
            int id = 0;


            modelBuilder.Entity<ProductCategoryConfig>().HasData(
                 new ProductCategoryConfig
                 {
                     Pid = ++id,
                     Key = ProductCategoryConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new ProductCategoryConfig
                    {
                        Pid = ++id,
                        Key = ProductCategoryConstants.Config.Admin.MaxHeight,
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
                    new ProductCategoryConfig
                    {
                        Pid = ++id,
                        Key = ProductCategoryConstants.Config.Admin.ThumbHeight,
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
                    new ProductCategoryConfig
                    {
                        Pid = ++id,
                        Key = ProductCategoryConstants.Config.Admin.ThumbWidth,
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
                    new ProductCategoryConfig
                    {
                        Pid = ++id,
                        Key = ProductCategoryConstants.Config.Admin.PageSize,
                        Value = "10",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    },
                    new ProductCategoryConfig
                    {
                        Pid = ++id,
                        Key = ProductCategoryConstants.Config.Website.PreSlug,
                        Value = "san-pham",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    },
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.ApiUpdateRewriteProductUrl,
                         Value = "{host}/api/ApiMasterPage/UpdateRewriteProductUrl",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     }
                     ,
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.PreSlugCate,
                         Value = "danh-muc-san-pham",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     },
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.PreSlugDetail,
                         Value = "san-pham",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     },
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.Parameter,
                         Value = "page:PageIndex:1;",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     },
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.CatePage,
                         Value = "/ProductPage/Cate",
                         Group = "",
                         CreateUser = "admin",
                         CreateDate = DateTime.Now,
                         LastLogin = DateTime.Now,
                         UpdateUser = "admin",
                         UpdateDate = DateTime.Now,
                         Type = "Admin"
                     },
                     new ProductCategoryConfig
                     {
                         Pid = ++id,
                         Key = ProductCategoryConstants.Config.Website.DetailPage,
                         Value = "/ProductPage/Detail",
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
        protected void SeedOrderManagementConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<OrderManagementConfig>().HasData(
                 new OrderManagementConfig
                 {
                     Pid = ++pid,
                     Key = Constants.OrderManagementConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Admin.MaxHeight,
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
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Admin.ThumbHeight,
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
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Admin.ThumbWidth,
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
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Admin.PageSize,
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
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Website.PageSize,
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
                    new OrderManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.OrderManagementConstants.Config.Website.PreSlug,
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