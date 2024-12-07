
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Reflection;

namespace Steam.Core.Common
{
    public static class ExtensionMethod
    {
        public static Image Resize(this Image image, int width, int height)
        {
            var res = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, width, height);
            }
            return res;
        }
        public static Image ResizeWidth(this Image image, int width)
        {
            int height = (image.Height * width) / image.Width;
            var res = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, width, height);
            }
            return res;
        }
        public static long GetDirectorySize(this System.IO.DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            if (directoryInfo == null || !directoryInfo.Exists)
                return startDirectorySize; //Return 0 while Directory does not exist.

            //Add size of files in the Current Directory to main size.
            foreach (var fileInfo in directoryInfo.GetFiles())
                System.Threading.Interlocked.Add(ref startDirectorySize, fileInfo.Length);

            if (recursive) //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
                System.Threading.Tasks.Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
            System.Threading.Interlocked.Add(ref startDirectorySize, GetDirectorySize(subDirectory, recursive)));

            return startDirectorySize;  //Return full Size of this Directory.
        }
        public static string GetConfigValue<T>(this List<T> modelList, string key)
        {
            if (modelList == null)
            {
                throw new ArgumentNullException(nameof(modelList));
            }

            var keyProperty = typeof(T).GetProperty("Key", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (keyProperty != null)
            {
                var item = modelList.FirstOrDefault(x =>
                    keyProperty.GetValue(x)?.ToString()?.Equals(key, StringComparison.OrdinalIgnoreCase) ?? false);

                if (item != null)
                {
                    var valueProperty = typeof(T).GetProperty("Value", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var value = valueProperty?.GetValue(item);
                    return value != null ? value.ToString() : "";
                }
            }

            return "";
        }
        public static T DeepClone<T>(this object model) where T : class, new()
        {
            T newModel = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(model));
            return newModel;
        }
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> listParam, int size)
        {
            if (listParam == null || size <= 0)
            {
                yield return listParam;
            }
            else
            {
                using (IEnumerator<T> enumerator = listParam.GetEnumerator())
                {
                    bool flag = enumerator.MoveNext();
                    while (flag)
                    {
                        List<T> listReturn = new List<T>();
                        while (listReturn.Count<T>() < size && flag)
                        {
                            listReturn.Add(enumerator.Current);
                            flag = enumerator.MoveNext();
                        }
                        yield return listReturn;
                    }
                }
            }
            yield break;
        }
    }
}
