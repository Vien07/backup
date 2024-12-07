using Microsoft.EntityFrameworkCore;

namespace Admin.DashBoard.Database
{
    public class DashBoardContext : DbContext
    {
        public DashBoardContext(DbContextOptions<DashBoardContext> options) : base(options)
        {

        }


        //public virtual DbSet<Database.Visitor> Visitors { get; set; }
        //public virtual DbSet<Database.DashBoardConfig> DashBoardConfigs { get; set; }
        public virtual DbSet<Database.Dashboard_Shortcut> Dashboard_Shortcuts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashBoardContext).Assembly);
            SeedDashBoardConfig(modelBuilder);

        }
        protected void SeedDashBoardConfig(ModelBuilder modelBuilder)
        {




        }


    }
}