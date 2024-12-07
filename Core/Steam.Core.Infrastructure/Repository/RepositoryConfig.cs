using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Steam.Infrastructure.Repository;
using System.Linq.Expressions;

public class RepositoryConfig<T> : IRepositoryConfig<T> where T : class, new()
{
    private readonly DbContext _context;

    public RepositoryConfig(DbContext context)
    {
        _context = context;
    }
    public IQueryable<T> Query()
    {
        return _context.Set<T>();
    }

    public Dictionary<string, string> GetAllConfigs()
    {
        var result = new Dictionary<string, string>();

        try
        {
            var configs = _context.Set<T>().ToList();

            foreach (var config in configs)
            {
                var keyProperty = config.GetType().GetProperty("Key");
                var valueProperty = config.GetType().GetProperty("Value");

                if (keyProperty != null && valueProperty != null)
                {
                    var key = keyProperty.GetValue(config)?.ToString();
                    var value = valueProperty.GetValue(config)?.ToString();

                    if (key != null)
                    {
                        result[key] = value;
                    }
                }
            }

        }
        catch (Exception ex)
        {

        }
        return result;

    }

    public T GetConfigByKey(string key)
    {
        try
        {
            var propertyName = "Key";
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(key, property.Type);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
            return _context.Set<T>().FirstOrDefault(lambda);

        }
        catch (Exception ex)
        {
            // Handle exception (log or rethrow)
            throw new ApplicationException($"Error retrieving entity with key {key}", ex);
        }
    }
    public string GetConfigByKey(string key, string DefaultValue)
    {
        try
        {
            var propertyName = "Key";
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(key, property.Type);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
            var entity = _context.Set<T>().FirstOrDefault(lambda);

            if (entity != null)
            {
                var valueProperty = typeof(T).GetProperty("Value");
                if (valueProperty != null)
                {
                    var value = valueProperty.GetValue(entity) as string;
                    return value ?? DefaultValue;
                }
            }
            return DefaultValue;

        }
        catch (Exception ex)
        {
           return DefaultValue;
        }
    }
    public List<T> SaveConfig(Dictionary<string, string> configs, string tab)
    {
        List<T> result = new List<T>();
        try
        {
            foreach (var item in configs)
            {
                var key = item.Key;
                var value = item.Value.ToString();

                // Build the expression tree for p => p.Key == key
                var parameter = Expression.Parameter(typeof(T), "p");
                var propertyKey = Expression.Property(parameter, "Key");
                var constantKey = Expression.Constant(key, propertyKey.Type);
                var equal = Expression.Equal(propertyKey, constantKey);
                var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                var entity = _context.Set<T>().FirstOrDefault(lambda);
                if (entity != null)
                {
                    SetProperty(entity, "Type", tab);
                    SetProperty(entity, "Value", value);
                    SetProperty(entity, "UpdateDate", DateTime.Now);
                    SetProperty(entity, "UpdateUser", ""); // You might want to replace with the actual user
                }
                else
                {
                    entity = new T();
                    SetProperty(entity, "Type", tab);
                    SetProperty(entity, "Key", key);
                    SetProperty(entity, "Group", "");
                    SetProperty(entity, "Value", value);
                    SetProperty(entity, "CreateDate", DateTime.Now);
                    SetProperty(entity, "CreateUser", ""); // You might want to replace with the actual user
                    SetProperty(entity, "UpdateDate", DateTime.Now);
                    SetProperty(entity, "UpdateUser", ""); // You might want to replace with the actual user
                    _context.Set<T>().Add(entity);
                }
            }

            _context.SaveChanges();

            result = _context.Set<T>().ToList();
        }
        catch (Exception ex)
        {
            // Optionally log the exception
            // Log.Error(ex, "Error saving configurations");
        }

        return result;
    }

    private void SetProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, Convert.ChangeType(value, property.PropertyType), null);
        }
    }
}
