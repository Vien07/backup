using Microsoft.EntityFrameworkCore;
using Admin.Course.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.Course.Database
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions<CourseContext> options) : base(options)
        {

        }

        public virtual DbSet<CourseConfig> CourseConfigs { get; set; }
        public virtual DbSet<Database.Course> Courses { get; set; }

        public virtual DbSet<Database.CourseChapter> CourseChapters { get; set; }
        public virtual DbSet<Database.Course_Lecture> Course_Lectures { get; set; }
        public virtual DbSet<Database.CourseCategory> CourseCategories { get; set; }

        public virtual DbSet<LectureConfig> LectureConfigs { get; set; }
        public virtual DbSet<Database.Lecture> Lectures { get; set; }

        public virtual DbSet<MemberConfig> MemberConfigs { get; set; }
        public virtual DbSet<Database.Member> Members { get; set; }

        public virtual DbSet<Database.Course_Member> Course_Members { get; set; }
        public virtual DbSet<Database.Lecture_Member> Lecture_Member { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseContext).Assembly);

            //config
            modelBuilder.Entity<Member>().HasIndex(u => u.Email).IsUnique();


            //seed data

            modelBuilder.Entity<CourseConfig>().HasData(
                new CourseConfig { Pid = 1, Key = "MinWidth", Value = "400", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new CourseConfig { Pid = 2, Key = "MaxWidth", Value = "800", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new CourseConfig { Pid = 3, Key = "PageSizeAdmin", Value = "12", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new CourseConfig { Pid = 4, Key = "PageSizeClient", Value = "12", Group = "text", CreateUser = "admin", UpdateUser = "admin" });

            modelBuilder.Entity<LectureConfig>().HasData(
                new LectureConfig { Pid = 1, Key = "MinWidth", Value = "400", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new LectureConfig { Pid = 2, Key = "MaxWidth", Value = "800", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new LectureConfig { Pid = 3, Key = "PageSizeAdmin", Value = "12", Group = "text", CreateUser = "admin", UpdateUser = "admin" });

            modelBuilder.Entity<MemberConfig>().HasData(
                new MemberConfig { Pid = 1, Key = "MinWidth", Value = "400", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new MemberConfig { Pid = 2, Key = "MaxWidth", Value = "800", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new MemberConfig { Pid = 3, Key = "PageSizeAdmin", Value = "12", Group = "text", CreateUser = "admin", UpdateUser = "admin" });
        }
    }
}