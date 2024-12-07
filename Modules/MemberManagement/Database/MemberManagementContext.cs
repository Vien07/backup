using Microsoft.EntityFrameworkCore;
using Admin.MemberManagement.Models;
using System;
using System.Security.Cryptography;
using Admin.MemberManagement.Constants;

namespace Admin.MemberManagement.Database
{
    public class MemberManagementContext : DbContext
    {
        public MemberManagementContext(DbContextOptions<MemberManagementContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.MemberManagementConfig> MemberManagementConfigs { get; set; }
        public virtual DbSet<Database.MemberManagement> MemberManagements { get; set; }
        public virtual DbSet<Database.MemberManagement_Files> MemberManagement_Files { get; set; }

        public virtual DbSet<FeedbackConfig> FeedbackConfigs { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Feedback_Files> Feedback_Files { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberManagementContext).Assembly);
            SeedMemberManagementConfig(modelBuilder);
            SeedFeedbackConfig(modelBuilder);
            modelBuilder.Entity<MemberManagement>()
         .HasIndex(p => new { p.Email})
         .IsUnique(true);
        }
        protected void SeedMemberManagementConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<MemberManagementConfig>().HasData(
                 new MemberManagementConfig
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
                    new MemberManagementConfig
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
                    new MemberManagementConfig
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
                    new MemberManagementConfig
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
                    new MemberManagementConfig
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
                    new MemberManagementConfig
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
                    new MemberManagementConfig
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
        protected void SeedFeedbackConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<FeedbackConfig>().HasData(
                 new FeedbackConfig
                 {
                     Pid = ++pid,
                     Key = FeedbackConstants.Config.Admin.MaxWidth,
                     Value = "720",
                     Group = "",
                     CreateUser = "admin",
                     CreateDate = DateTime.Now,
                     LastLogin = DateTime.Now,
                     UpdateUser = "admin",
                     UpdateDate = DateTime.Now,
                     Type = "Admin"
                 },
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Admin.MaxHeight,
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
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Admin.ThumbHeight,
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
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Admin.ThumbWidth,
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
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Admin.PageSize,
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
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Website.PageSize,
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
                    new FeedbackConfig
                    {
                        Pid = ++pid,
                        Key = FeedbackConstants.Config.Website.PreSlug,
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