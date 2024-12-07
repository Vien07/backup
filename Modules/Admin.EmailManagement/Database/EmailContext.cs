using Microsoft.EntityFrameworkCore;
using Admin.EmailManagement.Models;
using Admin.EmailManagement.Constants;

namespace Admin.EmailManagement.Database
{
    public class EmailContext : DbContext
    {
        public EmailContext(DbContextOptions<EmailContext> options) : base(options)
        {

        }


        public virtual DbSet<Database.EmailConfig> EmailConfigs { get; set; }
        public virtual DbSet<Database.EmailAdmin> EmailAdmins { get; set; }
        public virtual DbSet<Database.EmailMailBox> EmailMailBoxes { get; set; }
        public virtual DbSet<Database.EmailTemplate> EmailTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmailContext).Assembly);
            SeedEmailConfig(modelBuilder);

        }
        protected void SeedEmailConfig(ModelBuilder modelBuilder)
        {
            long pid = 0;
            modelBuilder.Entity<EmailConfig>().HasData(
                 new EmailConfig
                 {
                     Pid = ++pid,
                     Key = EmailAdminConstans.Config.Admin.MaxWidth,
                     Value = "1024",
                     Group="",
                     CreateUser="admin",
                     CreateDate=DateTime.Now,
                     LastLogin=DateTime.Now,
                     UpdateUser= "admin",
                     UpdateDate= DateTime.Now,
                     Type="Admin"
                 },
                    new EmailConfig
                    {
                        Pid = ++pid,
                        Key = EmailAdminConstans.Config.Admin.MaxHeight,
                        Value = "720",
                        Group = "",
                        CreateUser = "admin",
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now,
                        UpdateUser = "admin",
                        UpdateDate = DateTime.Now,
                        Type = "Admin"
                    },
                    new EmailConfig
                    {
                        Pid = ++pid,
                        Key = EmailAdminConstans.Config.Admin.PageSize,
                        Value = "100",
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