
using Microsoft.AspNetCore.Http;
using Admin.HomePage.Database;
using Admin.HomePage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Newtonsoft.Json;
using Admin.HomePage.Constants;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Common.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;

namespace Admin.HomePage
{
    public class HomePageRepository : IHomePageRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        HomePageContext _db;
        public HomePageRepository(HomePageContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.HomePageConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //double limitRamGB = 0.11;
            //long UsedMemory = System.Diagnostics.Process.GetCurrentProcess().PagedMemorySize64;
            //double limitRamBytes = limitRamGB * 1024 * 1024 * 1024;
            //if (UsedMemory > limitRamBytes)
            //{
            //    GC.Collect(); // Collect all generations
            //      GC.Collect(2,GCCollectionMode.Forced);
            //}
        }
        public Response<IPagedList<Database.HomePage>> GetList(HomePageModel.ParamSearch search)
        {
            Response<IPagedList<Database.HomePage>> rs = new Response<IPagedList<Database.HomePage>>();
            try
            {
                search.ToString();
                rs.Data = _db.HomePages.Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Name.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[HomePageConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<HomePageModel.HomePageDetail> GetById(int id)
        {
            Response<HomePageModel.HomePageDetail> rs = new Response<HomePageModel.HomePageDetail>();
            HomePageModel.HomePageDetail detail = new HomePageModel.HomePageDetail();
            try
            {

                detail.Detail = _db.HomePages.Where(p => p.Pid == id).FirstOrDefault();


                rs.isError = false;

                rs.StatusCode = 200;
                rs.Data = detail;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());

            }
            return rs;
        }
        public Response<Database.HomePage> Save(HomePageModelEdit data)
        {



            var validator = new HomePageValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<HomePage.Database.HomePage> rs = new Response<HomePage.Database.HomePage>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _db.HomePages.Add(data);

                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.HomePages.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.Name = data.Name;
                            model.Section = data.Section;
                            model.ListItemHtml = data.ListItemHtml;
                            model.SourceData = data.SourceData;

                            _db.SaveChanges();

                        }


                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.isError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

                }
            }
            rs.Data = data;
            return rs;
        }


      
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.HomePages.Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    //_db.HomePages.Remove(model);
                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    //var files = _db.HomePage_Files.Where(p => p.HomePageId == model.Pid).ToList();
                    //if(files!= null)
                    //{
                    //    foreach (var file in files)
                    //    {
                    //        _fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, file.UploadFileName);

                    //    }
                    //    _db.HomePage_Files.RemoveRange(files);

                    //}


                    //

                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }
        public Response Enable(List<int> ids, bool isEnable)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.HomePages.Where(p => p.Pid == id).FirstOrDefault();
                    model.Enabled = isEnable;
                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }
        public Response EnableUpdateOrder()
        {

            Response rs = new Response();

            try
            {
                var list = _db.HomePages.OrderBy(p => p.Order).ToList();
                var order = 1;
                foreach (var item in list)
                {
                    item.Order = order;
                    order = order + 1;
                    _db.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;

        }
        public Response UpdateOrder(int id, double order)
        {

            Response rs = new Response();

            try
            {
                var model = _db.HomePages.Where(p => p.Pid == id).FirstOrDefault();
                model.Order = order;
                _db.SaveChanges();


            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "id:" + id.ToString());

            }
            return rs;

        }
        public Response Move(int fromId, int toId)
        {

            Response rs = new Response();

            try
            {
                var fromModel = _db.HomePages.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.HomePages.Where(p => p.Pid == toId).FirstOrDefault();

                if (fromModel != null && fromModel != null)
                {
                    var fromOrder = fromModel.Order;
                    var toOrder = toModel.Order;
                    if (fromOrder > toOrder)
                    {
                        fromModel.Order = toModel.Order - 0.00001;

                    }
                    else if (fromOrder < toOrder)
                    {
                        fromModel.Order = toModel.Order + 0.00001;
                    }

                    _db.SaveChanges();
                    var list = _db.HomePages.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.HomePages.OrderBy(p => p.Order).ToList();
                //var stt = 1;
                //foreach (var item in list)
                //{
                //    item.Order = stt;
                //    stt = stt + 1;
                //    _db.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<List<HomePage.Database.HomePageConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<HomePage.Database.HomePageConfig>> rs = new Response<List<HomePage.Database.HomePageConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    HomePage.Database.HomePageConfig HomePageConfig = _db.HomePageConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (HomePageConfig != null)
                    {
                        HomePageConfig.Type = tab;
                        HomePageConfig.Value = value;
                        HomePageConfig.UpdateDate = DateTime.Now;
                        HomePageConfig.UpdateUser = "";

                    }
                    else
                    {
                        HomePageConfig = new HomePage.Database.HomePageConfig();
                        HomePageConfig.Type = tab;

                        HomePageConfig.Key = key;
                        HomePageConfig.Group = "";
                        HomePageConfig.Value = value;
                        HomePageConfig.CreateDate = DateTime.Now;
                        HomePageConfig.CreateUser = "";
                        HomePageConfig.UpdateDate = DateTime.Now;
                        HomePageConfig.UpdateUser = "";
                        _db.HomePageConfigs.Add(HomePageConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.HomePageConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<List<HomePage.Database.HomePageConfig>> GetAllConfigs()
        {
            Response<List<HomePage.Database.HomePageConfig>> rs = new Response<List<HomePage.Database.HomePageConfig>>();
            try
            {

                var listConfig = _db.HomePageConfigs.ToList();
                rs.Data = listConfig;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<HomePage.Database.HomePageConfig> GetConfigByKey(string key)
        {
            Response<HomePage.Database.HomePageConfig> rs = new Response<HomePage.Database.HomePageConfig>();
            try
            {

                var config = _db.HomePageConfigs.Where(p => p.Key == key).FirstOrDefault();
                rs.Data = config;
                rs.StatusCode = 200;
                return rs;
            }
            catch (Exception ex)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;
            }
        }
        public Response<string> GetHomePageHTML()
        {
            Response<string> rs = new Response<string>();
            try
            {
                var listSection = _db.HomePages.Where(p => p.Enabled == true).ToList();
                var listSectionHTML = "";

                foreach (var item in listSection) {
                    var data = GetListData(item.SourceData);
                    var listItemHTML = "";
                    foreach (var ele in data)
                    {
                        
                        var itemHTML = item.ListItemHtml.ToString();
                        foreach (var i in ele)
                        {
                            itemHTML = itemHTML.Replace(i.Key, i.Value);

                        }
                        listItemHTML += itemHTML;
                    
                    }
                    var sectionHTML = item.Section.Replace("{{ListItem}}", listItemHTML);
                    listSectionHTML += sectionHTML;
                }

                rs.Data = listSectionHTML;
                return rs;
            }
            catch (Exception)
            {
                rs.isError = true;

                rs.StatusCode = 500;
                rs.Message = "Lỗi không xác định";

                return rs;

            }
        }

        public List<dynamic> GetListData(string DOMAIN)
        {
            List<dynamic> rs = new List<dynamic>();
            try
            {
                var body = "";
                //var options = new RestClientOptions("https://localhost:50181")
                var options = new RestClientOptions(DOMAIN)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);

                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", "Bearer" + " " + access_token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {


                    rs = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);


                }
                return rs;

            }
            catch (Exception ex)
            {
                return rs;

            }
        }


    }

}


