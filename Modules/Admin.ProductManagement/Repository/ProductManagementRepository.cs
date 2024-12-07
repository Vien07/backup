using Admin.ProductManagement.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Admin.ProductManagement.Repository
{
    public class ProductManagementRepository<T> : IProductManagementRepository<T> where T : class
    {
        protected DbContext Context { get; }
        protected DbSet<T> DbSet { get; }

        public ProductManagementRepository(ProductManagementContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public IQueryable<T> Query()
        {
            return DbSet;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            DbSet.AddRange(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            DbSet.RemoveRange(entity);
        }
    }
}
