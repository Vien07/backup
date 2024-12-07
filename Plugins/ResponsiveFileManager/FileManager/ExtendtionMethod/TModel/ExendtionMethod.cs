

using Newtonsoft.Json;

namespace Steam.Core.FileManager.ExendtionMethod
{
    public static partial class ExendtionMethod
    {

        public static T ToObject<T>(this object model) where T : class, new()
        {
            T post = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(model));


            return post;
        }

    }
}
