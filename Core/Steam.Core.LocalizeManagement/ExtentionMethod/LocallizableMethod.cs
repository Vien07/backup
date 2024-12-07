
using Newtonsoft.Json.Linq;
using Steam.Core.LocalizeManagement.Services;
using Svg.FilterEffects;
using System.Globalization;
using System.Reflection;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.ExtensionMethod
{
    public static class LocallizableMethod
    {
        private static IContentLocalizationService _srv;

        public static void Initialize(IContentLocalizationService srv)
        {
            _srv = srv;
        }

        public static TEntity GetLocalize<TEntity>(this TEntity entity, long pid, string langCode)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            var entityType = entity.GetType().Name;
            foreach (PropertyInfo property in properties)
            {
                var entityID = property.Name;
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(entity);
                    var tValue = _srv.GetLocalizedProperty(entityType, pid, property.Name, value?.ToString(), langCode);
                    property.SetValue(entity, tValue);

                }



            }
            return entity;
        }
        public static TEntity UpdateLocalize<TEntity>(this TEntity entity, long pid, string langCode)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            var entityType = entity.GetType().Name;
            foreach (PropertyInfo property in properties)
            {
                var isMultilangAttribute = property.GetCustomAttribute<IsMultilangAttribute>();
                if (isMultilangAttribute != null)
                {
                    if (isMultilangAttribute.IsMultilang)
                    {
                        var entityID = property.Name;
                        if (property.PropertyType == typeof(string))
                        {
                            var value = property.GetValue(entity);
                            var tValue = _srv.UpdateLocalizedProperty(entityType, pid, property.Name, value?.ToString(), langCode);
                            //property.SetValue(entity, tValue);

                        }
                    }
                }



            }
            return entity;
        }
    }
}
