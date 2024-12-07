using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Steam.Core.LocalizeManagement.Database;
using Steam.Infrastructure.Models;

namespace Steam.Core.LocalizeManagement.Services
{
    public class ContentLocalizationService : IContentLocalizationService
    {
        private readonly ILocalizedContentPropertyService _entityRepository;
        public ContentLocalizationService(ILocalizedContentPropertyService entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public TEntity GetLocalize<TEntity>(IContentLocalizationService _srv, long pid, string langCode) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            var entityType = typeof(TEntity).Name;

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(entity);
                    var tValue = _srv.GetLocalizedProperty(entityType, pid, property.Name, value?.ToString(), langCode);
                    property.SetValue(entity, tValue);
                }
            }
            return entity;
        }

        public string GetLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue, string cultureId)
        {

            return _entityRepository.Translate(entityId, propertyName, entityType, cultureId, propertyValue);


        }      
        public bool UpdateLocalizedProperty(string entityType, long entityId, string propertyName, string propertyValue, string cultureId)
        {

            return _entityRepository.SetLocallize(entityId, propertyName, entityType, cultureId, propertyValue);


        }

    }
}
