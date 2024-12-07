
using System.Reflection;


namespace Steam.Core.Common.SteamDictionary
{
    public static class ExtensionMethod
    {
        public static T ToObject<T>(this IDictionary<string, string> source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                try
                {
                    var obj = someObjectType
                    .GetProperty(item.Key);
                    if (obj != null)
                    {
                        obj.SetValue(someObject, item.Value, null);
                    }
                }
                catch
                {

                }
            }

            return someObject;
        }
        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }
        
        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <param name="dictionary">Dictionary to check and get</param>
        /// <param name="key">Key to find the value</param>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary != null)
                return dictionary.TryGetValue(key, out var obj) ? obj : default;
            return default;
        }
        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// </summary>
        /// <param name="dictionary">Dictionary to check and get</param>
        /// <param name="key">Key to find the value</param>
        /// <param name="factory">A factory method used to create the value if not found in the dictionary</param>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <returns>Value if found, default if can not found.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            if (dictionary.TryGetValue(key, out var obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }
    }
}
