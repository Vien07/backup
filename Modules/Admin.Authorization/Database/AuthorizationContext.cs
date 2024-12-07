using Microsoft.EntityFrameworkCore;
using Admin.Authorization.Models;


namespace Admin.Authorization.Database
{
    public class AuthorizationContext : DbContext
    {
        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.AuthorizationConfig> AuthorizationConfigs { get; set; }
        public virtual DbSet<Database.ModuleRole> ModuleRoles { get; set; }
        public virtual DbSet<Database.GroupRole> GroupRoles { get; set; }
        public virtual DbSet<Database.Group_ModuleRole> ModuleRoleGroups { get; set; }
        public virtual DbSet<Database.User_Groups> User_Groups { get; set; }
        public virtual DbSet<Database.User> Users { get; set; }
        public virtual DbSet<LogManagementConfig> LogManagementConfigs { get; set; }
        public virtual DbSet<LogManagement> LogManagements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizationContext).Assembly);
            SeedAuthorizationConfig(modelBuilder);
            SeedLogManagementConfig(modelBuilder);

        }
        protected void SeedAuthorizationConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;

            // Seed User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Pid = 1,
                    UserName = "admin",
                    Name = "Steam",
                    Role = "",
                    IsActive = true,
                    Token = "",
                    Password = "$2a$11$V46eggVJFbCYted8lbIJtuxAYuQOrNK0fdAkLi13MBiThnU/K8oam",
                    LastLogin = DateTime.Now,
                    LastIPLogin = "*",
                    CreateUser = "admin",
                    UpdateUser = "admin"

                }

            );

            // Seed configs
            modelBuilder.Entity<AuthorizationConfig>().HasData(
                 new AuthorizationConfig
                 {
                     Pid = ++pid,
                     Key = Constants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Admin.MaxHeight,
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
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Admin.ThumbHeight,
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
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Admin.ThumbWidth,
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
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Admin.PageSize,
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
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Website.PageSize,
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
                    new AuthorizationConfig
                    {
                        Pid = ++pid,
                        Key = Constants.Config.Website.PreSlug,
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
        protected void SeedLogManagementConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<LogManagementConfig>().HasData(
                 new LogManagementConfig
                 {
                     Pid = ++pid,
                     Key = Constants.LogManagementConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Admin.MaxHeight,
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
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Admin.ThumbHeight,
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
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Admin.ThumbWidth,
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
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Admin.PageSize,
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
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Website.PageSize,
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
                    new LogManagementConfig
                    {
                        Pid = ++pid,
                        Key = Constants.LogManagementConstants.Config.Website.PreSlug,
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