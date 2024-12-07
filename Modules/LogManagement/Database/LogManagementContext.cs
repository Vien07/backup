using Microsoft.EntityFrameworkCore;
using Admin.LogManagement.Models;
using System;

namespace Admin.LogManagement.Database
{
    public class LogManagementContext : DbContext
    {
        public LogManagementContext(DbContextOptions<LogManagementContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.LogManagementConfig> LogManagementConfigs { get; set; }
        public virtual DbSet<Database.LogAdminActivity> LogManagements { get; set; }
        //public virtual DbSet<Database.LogManagement_Files> LogManagement_Files { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        protected void SeedLogManagementConfig(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<LogManagementConfig>().HasData(
                 new LogManagementConfig
                 {
                     Pid = 1,
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
                    new LogManagementConfig
                    {
                        Pid = 2,
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
                    new LogManagementConfig
                    {
                        Pid = 3,
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
                    new LogManagementConfig
                    {
                        Pid = 4,
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
                    new LogManagementConfig
                    {
                        Pid = 5,
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
                    new LogManagementConfig
                    {
                        Pid = 6,
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
                    new LogManagementConfig
                    {
                        Pid = 7,
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


    }
}