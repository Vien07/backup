using Microsoft.AspNetCore.Http;
using System.Reflection;
using Admin.WebSetting.Models;
using Admin.Course.Database;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.STeamHelper;
using Admin.WebSetting.Constants;
using Steam.Core.Base.Constant;
using Steam.Core.Utilities.SteamModels;
using Newtonsoft.Json;
using RestSharp;
using System.Dynamic;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Steam.Core.Utilities.STeamHelper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Steam.Infrastructure.Repository;

namespace Admin.WebSetting.Services
{
    public class WebSettingService : IWebSettingService
    {
        ILoggerHelper _logger;
        IFileHelper _fileHelper;
        IMetaHelper _metaHelper;
        private readonly IRepository<Database.WebsiteConfiguration> _repWebsiteConfiguration;
        string AppKey = "";
        string MedidaFileServer = "";
        public WebSettingService(
            IFileHelper fileHelper, 
            IMetaHelper metaHelper, 
            ILoggerHelper logger,
            IRepository<Database.WebsiteConfiguration> repWebsiteConfiguration)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _repWebsiteConfiguration = repWebsiteConfiguration;
            _metaHelper = metaHelper;
            MedidaFileServer = SystemInfo.MedidaFileServer;// configuration["SystemConfig:MedidaFileServer"];

            AppKey = SystemInfo.AppKey;// configuration["Website:AppKey"].ToString();
        }
        public Response Save(WebSettingViewModelModelEdit model)
        {
            Response rs = new Response();
            var input = model.GetDatabaseModel();

            try
            {
                MetaModel modelMeta = new MetaModel();
                modelMeta.PageTitle = model.WebsiteName;
                modelMeta.SiteName = model.WebsiteName;
                modelMeta.Description = model.WebsiteDescription;
                modelMeta.PageUrl = model.RootDomain;
                modelMeta.SiteTitle = model.WebsiteName;
                modelMeta.OgType = "website";
                PropertyInfo[] propertyInfos;
                propertyInfos = input.GetType().GetProperties();
                foreach (PropertyInfo key in propertyInfos)
                {
                    var current = _repWebsiteConfiguration.Query().Where(x => x.Key == key.Name).FirstOrDefault();
                    if (key.Name == nameof(WebSettingDto.ogImage))
                    {
                        modelMeta.ImageUrl = MedidaFileServer + WebSettingConstants.StaticPath.Asset.Image + current.Value;
                    }
                    if (current != null)
                    {
                        if (key.PropertyType.Name == "IFormFile")
                        {
                            var data = key.GetValue(input);
                            if (data != null)
                            {
                                var img = data as IFormFile;
                                if (img.FileName.ToString().ToLower() != "blob")
                                {
                                    var imageName = _fileHelper.UploadImageNotResize(img, WebSettingConstants.StaticPath.Asset.Image);
                                    current.Value = imageName;
                                    if (key.Name == nameof(WebSettingDto.ogImage))
                                    {
                                        modelMeta.ImageUrl = MedidaFileServer + WebSettingConstants.StaticPath.Asset.Image + imageName;
                                    }
                                }

                            }
                        }
                        else
                        {
                            var data = key.GetValue(input);
                            if (data != null)
                            {
                                current.Value = data.ToString();
                            }
                            else
                            {
                                current.Value = "";
                            }
                            if (key.Name == nameof(WebSettingDto.RootDomain))
                            {
                                modelMeta.HomepageUrl = current.Value;

                            }

                        }
                        _repWebsiteConfiguration.SaveChanges();

                    }
                }
                var metaWebsite = _repWebsiteConfiguration.Query().Where(x => x.Key == nameof(WebSettingDto.WebsiteMeta)).FirstOrDefault();
                var OgImage = _repWebsiteConfiguration.Query().Where(x => x.Key == nameof(WebSettingDto.ogImage)).FirstOrDefault();
                modelMeta.OgImage = OgImage.Value ?? "";
                if(model.TypeSave==1)
                {
                    string meta = _metaHelper.GenerateMetaTag(modelMeta) + model.WebsiteMetaExtra;
                    if (!string.IsNullOrEmpty(meta.ToRemoveBreakSympol()))
                    {
                        //model.WebsiteMeta = meta;
                        metaWebsite.Value = meta;
                        _repWebsiteConfiguration.SaveChanges();
                        UpdateHomePageMeta(meta, input.ApiUpdateHomePageMeta);

                    }

                    UpdateRobots(input.Robots, model.RootDomain, input.ApiUpdateRobots, input.TextRobotsOn, input.TextRobotsOff);
                }    

                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, model.ToJson());
            }
            return rs;
        }
        public Response<Dictionary<string, string>> GetWebSetting()
        {
            var rs = new Response<Dictionary<string, string>>();
            try
            {
                var config = _repWebsiteConfiguration.Query().Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
                rs.Data = config;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, rs.Data.ToJson());
            }
            return rs;
        }
        public Response<Dictionary<string, string>> GetWebSiteConfig()
        {
            var rs = new Response<Dictionary<string, string>>();
            try
            {
                var config = _repWebsiteConfiguration.Query().Where(p=>p.Group== "Website").Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
                rs.Data = config;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, rs.Data.ToJson());
            }
            return rs;
        }
        public void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
        {
            try
            {
                if (filestatus != "existed")
                {

                    if (!String.IsNullOrEmpty(filePath))
                    {
                        var arrFile = filePath.Split('/');
                        img = arrFile[arrFile.Length - 1];
                        imgFilePath = SystemInfo.PathFileManager + "/" + filePath.Replace(img, "");
                    }
                    else
                    {
                        img = _fileHelper.UploadFileModule(
                         new UploadFileInfo
                         {

                             Path = "",
                             FileName = "",
                             File = file
                         }
                         ).FileName;
                        imgFilePath = "";
                    }

                    //-------end-save-file-pond----------

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateRobots(string status, string domain, string api, string AllowRobots, string DisallowRobots)
        {
            //string AllowRobots =string.Format("User-agent: * Allow: / \n Sitemap: {0}/sitemap.xml \n Disallow: /s-admin/ \n Disallow: /*_escaped_fragment_", domain);
            //string DisallowRobots = "User-agent:* \n Disallow:/ ";
            try
            {
                dynamic requestModel = new ExpandoObject();
                Response<bool> responseObj = new Response<bool>();
                if (status == "on")
                {
                    requestModel.RobotsData = AllowRobots;

                }
                else
                {
                    requestModel.RobotsData = DisallowRobots;

                }



                var client = new RestClient(api);
                var request = new RestRequest();
                request.AddHeader(nameof(AppKey), AppKey);
                request.Timeout = 2000;
                request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
                var response = client.ExecutePost(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<bool>>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void UpdateHomePageMeta(string meta, string api)
        {
            try
            {
                dynamic requestModel = new ExpandoObject();
                Response<bool> responseObj = new Response<bool>();
                requestModel.HomePageMeta = meta;

                var client = new RestClient(api);
                var request = new RestRequest();
                request.Timeout = 2000;

                request.AddHeader(nameof(AppKey), AppKey);
                request.AddJsonBody(JsonConvert.SerializeObject((object)requestModel));
                var response = client.ExecutePost(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<bool>>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
