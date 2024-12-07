
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Steam.Core.FileManager.ExendtionMethod
{
    public static partial class ExendtionMethod
    {
        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }

    }
}
