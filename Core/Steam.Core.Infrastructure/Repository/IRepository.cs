using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Steam.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Query();

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        IDbContextTransaction BeginTransaction();

        void SaveChanges();

        Task SaveChangesAsync();

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
        bool Enable(List<long> ids, bool isEnable, string userName);
        bool Move(long fromID, long toID);
        bool EnableUpdateOrder();
        bool UpdateOrder(long id, double order);
        T GetById(long id);
        List<T> GetListByKey(object value, string key);
        bool SaveListFile(long pid, string path, string listFilesString, int maxHeight, int maxWidth);

    }
    public interface IRepositoryConfig<T>
    {
        public Dictionary<string, string> GetAllConfigs();
        T GetConfigByKey(string key);
        string GetConfigByKey(string key,string DefaultValue);
        List<T> SaveConfig(Dictionary<string, string> configs, string tab);
        IQueryable<T> Query();

    }

}
