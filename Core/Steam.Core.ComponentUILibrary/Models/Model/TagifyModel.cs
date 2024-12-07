using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ComponentUILibrary.Models
{
    public class TagifyModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Value { get; set; }
        public string SeparateSympol { get; set; } = ";";
        public bool FirstLoadLib { get; set; } = false;
        public int Type { get; set; } = 0;
        public string ConvertValueToJsonArray()
        {
            string jsonResult = "[]";
            try
            {
                if(!string.IsNullOrEmpty(this.Value))
                {
                    var array = this.Value.Split(this.SeparateSympol);
                    array = array.Where(p => p != string.Empty).ToArray();
                    jsonResult = JsonConvert.SerializeObject(array);
                }    

            }
            catch (Exception)
            {
                jsonResult = "[]";
            }
            return jsonResult;
        }

    }
}