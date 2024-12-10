using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.Services
{
    public static class ExtensionServices
    {
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };
        public static string RemoveSign4VietnameseString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
                return str;
            }
            return "";
        }
        public static List<T> FilterSearch<T>(this List<T> source, string[] keyWords, string keySearch)
        {
            try
            {
                if (source.Any() && !string.IsNullOrEmpty(keySearch))
                {
                    keySearch = RemoveSign4VietnameseString(keySearch).ToLower();
                    List<T> results = new List<T>();
                    foreach (var element in source)
                    {
                        foreach (var word in keyWords)
                        {
                            var value = element.GetType().GetProperty(word).GetValue(element);
                            if (value != null)
                            {
                                var valueRemovedSign = RemoveSign4VietnameseString(value.ToString()).ToLower();
                                if (valueRemovedSign.Contains(keySearch))
                                {
                                    results.Add(element);
                                    break;
                                }
                            }
                        }
                    }
                    return results;
                }
                return source;
            }
            catch (Exception ex)
            {
                return source;
            }
        }
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize).Select(x => x.Select(v => v.Value));
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static List<T> RandomList<T>(this List<T> source, int size)
        {
            Random random = new Random();
            var length = source.Count();
            List<T> results = new List<T>();
            for (int i = 0; i < size; i++)
            {
                try
                {
                    var ramdomItem = source[random.Next(length)];
                    source.Remove(ramdomItem);
                    length--;
                    results.Add(ramdomItem);
                }
                catch
                {

                }
            }
            return results;
        }
        public static string RemoveHtmlTag(string input)
        {
            try
            {
                return Regex.Replace(input, "<.*?>", String.Empty);
            }
            catch
            {
                return "";
            }
        }

        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException("Call LoadStoredProc before using this method");
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }
        private static List<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
              .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
              .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        try
                        {
                            var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                            prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                        catch
                        {

                        }
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }
        public static async Task<List<T>> ExecuteStoredProc<T>(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        return reader.MapToList<T>();
                    }
                }
                catch (Exception e)
                {
                    throw (e);
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }
    }
}
