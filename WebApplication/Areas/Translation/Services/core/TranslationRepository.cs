using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CMS.Areas.Translation
{
    public class TranslationRepository : ITranslationRepository
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _core;
        string messErr = "";

        private readonly DBContext _dbContext;
        IFileServices _fileServices;

        public TranslationRepository(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IFileServices fileServices, ICommonServices core)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _fileServices = fileServices;
            _core = core;
        }
        public dynamic LoadData()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\lang.json";
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    dynamic obj = JValue.Parse(json);
                    dynamic dynJson = JsonConvert.SerializeObject(obj);

                    return dynJson;
                }
            }
            catch (Exception ex)
            {
                return "[]";
            }
        }

        public dynamic Update(dynamic data)
        {
            try
            {
                data = JsonConvert.DeserializeObject(data);
                data = JsonConvert.SerializeObject(data, Formatting.Indented);
                var path = Directory.GetCurrentDirectory() + @"\wwwroot\lang\lang.json";
                System.IO.File.WriteAllText(path, data);
                return new { status = true, mess = messErr };
            }
            catch (Exception ex)
            {
                messErr = "Something Wrong!";
                return new { status = false, mess = messErr };
            }
        }

        public dynamic Validation(dynamic data)
        {
            try
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
            catch (Exception ex)
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
        }
    }
}
