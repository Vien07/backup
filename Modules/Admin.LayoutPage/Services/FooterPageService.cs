
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
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Microsoft.EntityFrameworkCore;
using Admin.WebsiteKeys.Database;
using Steam.Infrastructure.Repository;

namespace Admin.LayoutPage.Services
{
    public class FooterPageService : IFooterPageService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.FooterPage> _repFooterPage;
        private readonly IRepository<Database.FooterItem> _repFooterItem;
        private readonly IRepositoryConfig<Database.FooterPageConfig> _repFooterPageConfig;


        private readonly IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> _repWebsiteKeys;


        public FooterPageService(
            IRepository<Database.FooterPage> repFooterPage,
            IRepositoryConfig<Database.FooterPageConfig> repFooterPageConfig,
            IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> repWebsiteKeys,
            IRepository<Database.FooterItem> repFooterItem,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repFooterItem = repFooterItem;
            _repFooterPageConfig = repFooterPageConfig;
            _repFooterPage = repFooterPage;
            _repWebsiteKeys = repWebsiteKeys;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repFooterPageConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.FooterPage>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.FooterPage>> rs = new Response<IPagedList<Database.FooterPage>>();
            try
            {
                search.ToString();
                rs.Data = _repFooterPage.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(100));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<List<Database.FooterItem>> GetChildList(long? Pid)
        {
            Response<List<Database.FooterItem>> rs = new Response<List<Database.FooterItem>>();
            try
            {
                //search.ToString();
                rs.Data = _repFooterItem.Query().Where(p => p.Deleted == false && p.FooterPagePid == Pid)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<Database.FooterPage> GetById(int id)
        {
            Response<Database.FooterPage> rs = new Response<Database.FooterPage>();
            Database.FooterPage detail = new Database.FooterPage();
            try
            {

                detail =_repFooterPage.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.FooterItem> GetChildById(int id)
        {
            Response<Database.FooterItem> rs = new Response<Database.FooterItem>();
            Database.FooterItem detail = new Database.FooterItem();
            try
            {

                detail = _repFooterItem.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.FooterPage> Save(FooterPageModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new FooterPageValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.FooterPage> rs = new Response<Database.FooterPage>();
            using (var transaction = _repFooterPage.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repFooterPage.Add(data);

                        _repFooterPage.SaveChanges();
                    }
                    else
                    {
                        var model = _repFooterPage.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;
                            model.FooterBlock = data.FooterBlock;
                            model.FooterPluginBlock = data.FooterPluginBlock;


                            _repFooterPage.SaveChanges();

                        }


                    }

                    //---------end save list lisst file--------
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
        public Response<Database.FooterItem> SaveChild(FooterItemModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new FooterItemValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.FooterItem> rs = new Response<Database.FooterItem>();
            using (var transaction = _repFooterItem.BeginTransaction())
            {


                try
                {
                    if (data.Pid == 0)
                    {
                        var modelGroup = _repFooterItem.Query().Where(p => p.FooterPagePid == data.FooterPagePid).ToList();
                        data.Order = 0.9;
                        
                        _repFooterItem.Add(data);

                        _repFooterItem.SaveChanges();
                    }

                    else
                    {
                        var model = _repFooterItem.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();                        


                        if (model != null)
                        {
                            model.ItemBlock = data.ItemBlock;
                            model.Key = data.Key;


                            _repFooterItem.SaveChanges();

                        }


                    }

                    //---------end save list lisst file--------
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

                    var model = _repFooterPage.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repFooterPage.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _repFooterItem.Query().Where(p => p.FooterPagePid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _repFooterItem.RemoveRange(menuItems);

                    }


                    //

                    _repFooterPage.SaveChanges();
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
        public Response DeleteChild(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {

                    var model = _repFooterItem.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repFooterItem.Remove(model);


                    _repFooterItem.SaveChanges();
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
    
       

        public Response<string> GenerateFooterHtml(long pid)
        {
            Response<string> rs = new Response<string>();
            try
            {

                FooterPageHelper _helper = new FooterPageHelper();
                var footer = _repFooterPage.Query().Where(p => p.Pid == pid).FirstOrDefault();
                var sliderItem = _repFooterItem.Query().Where(p => p.FooterPagePid == footer.Pid).ToList();
                var listWebsiteKey = _repWebsiteKeys.Query().Where(p => p.isSystemKey == false).ToList();
                rs = _helper.GenerateFooter(footer, sliderItem, listWebsiteKey);
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
    }

}
