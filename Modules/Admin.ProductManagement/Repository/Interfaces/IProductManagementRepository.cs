using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Repository
{
    public interface IProductManagementRepository<T>
    {
        IQueryable<T> Query();

        void Add(T entity);

        void AddRange(IEnumerable<T> entity);

        IDbContextTransaction BeginTransaction();

        void SaveChanges();

        Task SaveChangesAsync();

        void Remove(T entity);   

        void RemoveRange(IEnumerable<T> entity);
    }
}
