using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Steam.Infrastructure.Repository;
using System.Linq.Expressions;
using Steam.Core.Base;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.STeamHelper;
using System.Text.Json;
using System.Reflection;
using Newtonsoft.Json;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private ILoggerHelper _logger;
    private IFileHelper _fileHelper;
    public Repository(DbContext context, ILoggerHelper logger, IFileHelper fileHelper)
    {
        _context = context;
        _logger = logger;
        _fileHelper = fileHelper;
    }

    public IQueryable<T> Query()
    {
        return _context.Set<T>();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
    #region common action
    public List<T> GetListByKey(object value, string key)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value, property.Type);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return _context.Set<T>().Where(lambda).ToList();
        }
        catch (Exception ex)
        {
            return new List<T>();
        }
    }


    public T GetById(long id)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var propertyKey = Expression.Property(parameter, "Pid");
            var constantKey = Expression.Constant(id, propertyKey.Type);
            var equal = Expression.Equal(propertyKey, constantKey);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            var entity = _context.Set<T>().FirstOrDefault(lambda);


            return entity;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public bool Enable(List<long> ids, bool status, string userName)
    {
        try
        {
            foreach (var id in ids)
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var propertyKey = Expression.Property(parameter, "Pid");
                var constantKey = Expression.Constant(id, propertyKey.Type);
                var equal = Expression.Equal(propertyKey, constantKey);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                var entity = _context.Set<T>().FirstOrDefault(lambda);
                if (entity != null)
                {
                    SetProperty(entity, "Enabled", status);
                    SetProperty(entity, "UpdateDate", DateTime.Now);
                    SetProperty(entity, "UpdateUser", userName);
                }
                _context.SaveChanges();

            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public bool Move(long fromID, long toID)
    {
        try
        {

            var parameter = Expression.Parameter(typeof(T), "p");
            var propertyKey = Expression.Property(parameter, "Pid");

            var constantKeyTo = Expression.Constant(toID, propertyKey.Type);
            var constantKeyFrom = Expression.Constant(toID, propertyKey.Type);

            var equalFrom = Expression.Equal(propertyKey, constantKeyFrom);
            var equalTo = Expression.Equal(propertyKey, constantKeyTo);

            var lambdaTo = Expression.Lambda<Func<T, bool>>(equalTo, parameter);
            var lambdaFrom = Expression.Lambda<Func<T, bool>>(equalFrom, parameter);

            var entityTo = _context.Set<T>().FirstOrDefault(lambdaTo);
            var entityFrom = _context.Set<T>().FirstOrDefault(lambdaFrom);
            if (entityTo != null && entityFrom != null)
            {
                var fromOrder = Convert.ToDouble(GetPropertyValue(entityFrom, "Order").ToString());
                var toOrder = Convert.ToDouble(GetPropertyValue(entityTo, "Order").ToString());
                if (fromOrder > toOrder)
                {
                    SetProperty(entityFrom, "Order", toOrder - 0.00001);

                }
                else if (fromOrder < toOrder)
                {

                    SetProperty(entityFrom, "Order", toOrder + 0.00001);

                }
            }

            _context.SaveChanges();



            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public bool EnableUpdateOrder()
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var list = _context.Set<T>().ToList();

                var order = 1;
                foreach (var item in list)
                {
                    SetProperty(item, "Order", order);
                    order++;
                }

                _context.SaveChanges();

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }
    }
    public bool UpdateOrder(long id, double order)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var propertyKey = Expression.Property(parameter, "Pid");
            var constantKey = Expression.Constant(id, propertyKey.Type);
            var equal = Expression.Equal(propertyKey, constantKey);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            var entity = _context.Set<T>().FirstOrDefault(lambda);
            if (entity != null)
            {
                SetProperty(entity, "Order", order);
                SetProperty(entity, "UpdateDate", DateTime.Now);
            }
            _context.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    public bool SaveListFile(long pid, string path, string listFilesString, int maxHeight, int maxWidth)
    {
        try
        {
            List<FileInfoModel> listFiles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFilesString);
            var filesToAdd = new List<T>();
            var fileIdsToDelete = new List<int>();
            var filesToUpdate = new List<T>();

            foreach (var item in listFiles)
            {
                switch (item.status)
                {
                    case "new":
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = maxHeight,
                            Width = maxWidth,
                            FileName = Guid.NewGuid().ToString(),
                            Path = path
                        });

                        if (!saveFile.isError)
                        {
                            var newFile = Activator.CreateInstance<T>();
                            SetProperty(newFile, "SampleId", pid);
                            SetProperty(newFile, "Caption", item.caption);
                            SetProperty(newFile, "Description", item.description);
                            SetProperty(newFile, "UploadFileName", saveFile.FileName);
                            SetProperty(newFile, "Order", item.order);

                            filesToAdd.Add(newFile);
                        }
                        break;

                    case "delete":
                        _fileHelper.DeleteFile(path, item.name);
                        fileIdsToDelete.Add(Convert.ToInt32(item.id));
                        break;

                    case "edit":
                        var fileToEdit = _context.Set<T>().FirstOrDefault(p => (long)p.GetType().GetProperty("Pid").GetValue(p) == Convert.ToInt32(item.id));
                        if (fileToEdit != null)
                        {
                            SetProperty(fileToEdit, "Caption", item.caption);
                            SetProperty(fileToEdit, "Description", item.description);
                            SetProperty(fileToEdit, "Order", item.order);
                            filesToUpdate.Add(fileToEdit);
                        }
                        break;
                }
            }
            try
            {
                foreach (var file in filesToAdd)
                {
                    try
                    {
                        _context.Set<T>().Add(file);

                    }
                    catch (Exception)
                    {

                        _fileHelper.DeleteFile(path, GetPropertyValue(file, "UploadFileName").ToString());
                    }
                }

                foreach (var file in filesToUpdate)
                {
                    try
                    {
                        _context.Set<T>().Update(file);

                    }
                    catch (Exception)
                    {

                        _fileHelper.DeleteFile(path, GetPropertyValue(file, "UploadFileName").ToString());
                    }
                }

                if (fileIdsToDelete.Any())
                {
                    var filesToDelete = _context.Set<T>().Where(p => fileIdsToDelete.Contains((int)p.GetType().GetProperty("Pid").GetValue(p))).ToList();
                    foreach (var file in filesToDelete)
                    {
                        _context.Set<T>().Remove(file);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //_logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
            //_logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message);
        }
        return true;
    }


    private void SetProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, Convert.ChangeType(value, property.PropertyType), null);
        }
    }

    public object GetPropertyValue(object entity, string propertyName)
    {
        try
        {

            if (entity != null)
            {
                var propertyInfo = entity.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(entity);
                }
            }
        }
        catch (Exception ex)
        {

        }

        return null;
    }
    #endregion
}
