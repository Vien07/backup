using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Database
{
    public class PaymentContext : DbContext
    {
        private string _connectionString;
        private DbContextOptions<PaymentContext> _options;

        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
            _connectionString = Database.GetDbConnection().ConnectionString;
            _options = options;
        }


        public virtual DbSet<PaymentConfig> PaymenConfigs { get; set; }

        public PaymentContext SetConnectionString(string newConnectionString)
        {
            if (newConnectionString != _connectionString)
            {
                _connectionString = newConnectionString;
                var builder = new DbContextOptionsBuilder<PaymentContext>(_options);
                builder.UseSqlServer(newConnectionString);
                PaymentContext newDb = new PaymentContext(builder.Options);
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentContext).Assembly);
            //SeedOrderConfig(modelBuilder);

        }
    }


}
