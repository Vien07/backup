
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Database;
using Admin.LayoutPage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.LayoutPage.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using RestSharp;
using Admin.WebsiteKeys.Database;
using Steam.Core.Utilities.STeamHelper;
using Newtonsoft.Json;
using Steam.Infrastructure.Repository;
using Steam.Core.Base.Constant;

namespace Admin.LayoutPage.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly IRestHelper _restHelper;

        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.HomePage> _repHomePage;
        private readonly IRepositoryConfig<Database.HomePageConfig> _repHomePageConfig;
        private readonly IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> _repWebsiteKeys;


        public HomePageService(
            IRepository<Database.HomePage> repHomePage,
            IRepositoryConfig<Database.HomePageConfig> repHomePageConfig,
           IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> repWebsiteKeys,
            IFileHelper fileHelper, 
            ILoggerHelper logger,
            IRestHelper restHelper)
        {
            _restHelper = restHelper;
            _repHomePage = repHomePage;
            _repWebsiteKeys = repWebsiteKeys;
            _repHomePageConfig = repHomePageConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repHomePageConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.HomePage>> GetList(HomePageModel.ParamSearch search)
        {
            Response<IPagedList<Database.HomePage>> rs = new Response<IPagedList<Database.HomePage>>();
            try
            {
                search.ToString();
                rs.Data = _repHomePage.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32( _repHomePageConfig.GetConfigByKey(HomePageConstants.Config.Admin.PageSize, DefaultConfig.PAGE_SIZE.ToString())));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
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

                detail.Detail =_repHomePage.Query().Where(p => p.Pid == id).FirstOrDefault();


                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = detail;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
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
            Response<Database.HomePage> rs = new Response<Database.HomePage>();
            using (var transaction = _repHomePage.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repHomePage.Add(data);

                        _repHomePage.SaveChanges();
                    }
                    else
                    {
                        var model = _repHomePage.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.Name = data.Name;
                            model.Section = data.Section;
                            model.ListTabHtml = data.ListTabHtml;
                            model.ListItemHtml = data.ListItemHtml;
                            model.ListItemChildHtml = data.ListItemChildHtml;
                            model.TypeView = data.TypeView;
                            model.SourceData = data.SourceData;
                            model.ScriptBlock = data.ScriptBlock;
                            model.StyleBlock = data.StyleBlock;

                            _repHomePage.SaveChanges();

                        }


                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
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
                    var model = _repHomePage.Query().Where(p => p.Pid == id).FirstOrDefault();
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

                    _repHomePage.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }
     
        public Response<string> GenerateHomePageHtml(long pid)
        {
            Response<string> rs = new Response<string>();
            try
            {
                var listSection =_repHomePage.Query().Where(p => p.Enabled == true && p.Pid == pid).OrderBy(p => p.Order).ToList();

                if (pid == 0)
                {
                    listSection =_repHomePage.Query().Where(p => p.Enabled == true).OrderBy(p => p.Order).ToList();

                }
                var listWebsiteKey = _repWebsiteKeys.Query().Where(p => p.isSystemKey == false).ToList();

                HomePageHelper _helper = new HomePageHelper(_restHelper);
                rs = _helper.GenerateHomePage(listSection, listWebsiteKey);

                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = ex.ToString();
                rs.Data = "Lỗi không xác định";

                return rs;

            }
        }

        public List<dynamic> GetListData(string DOMAIN)
        {
            List<dynamic> rs = new List<dynamic>();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                var data = _restHelper.Get<List<dynamic>>(DOMAIN, requestHeaders: headers);
                if (data != null)
                {
                    rs = data;
                }

                //var body = "";
                ////var options = new RestClientOptions("https://localhost:50181")
                //var options = new RestClientOptions(DOMAIN)
                //{
                //    MaxTimeout = -1,
                //};
                //var client = new RestClient(options);
                //var request = new RestRequest("", Method.Post);

                //request.AddHeader("Content-Type", "application/json");
                ////request.AddHeader("Authorization", "Bearer" + " " + access_token);
                //request.AddParameter("application/json", body, ParameterType.RequestBody);

                //RestResponse response = client.Execute(request);

                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //{


                //    rs = JsonConvert.DeserializeObject<List<dynamic>>(response.Content);


                //}
                return rs;

            }
            catch (Exception ex)
            {
                return rs;

            }
        }


    }

}


