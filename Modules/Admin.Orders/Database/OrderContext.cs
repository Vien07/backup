using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Orders.Database
{
    public class OrderContext : DbContext
    {
        private string _connectionString;
        private DbContextOptions<OrderContext> _options;

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            _connectionString = Database.GetDbConnection().ConnectionString;
            _options = options;
        }


        public virtual DbSet<TransactionOrder> TransactionOrders { get; set; }
        public virtual DbSet<OrderConfig> OrderConfigs { get; set; }

        public OrderContext SetConnectionString(string newConnectionString)
        {
            if (newConnectionString != _connectionString)
            {
                _connectionString = newConnectionString;
                var builder = new DbContextOptionsBuilder<OrderContext>(_options);
                builder.UseSqlServer(newConnectionString);
                OrderContext newDb = new OrderContext(builder.Options);
                return newDb;
            }
            else
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
            //SeedOrderConfig(modelBuilder);

        }
    }


}
