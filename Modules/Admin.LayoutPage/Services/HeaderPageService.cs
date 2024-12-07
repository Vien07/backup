
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
using Steam.Infrastructure.Repository;
using Steam.Core.Base.Constant;
using Steam.Core.Common.SteamDictionary;
namespace Admin.LayoutPage.Services
{
    public class HeaderPageService : IHeaderPageService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.HeaderPage> _repHeaderPage;
        private readonly IRepository<Database.Menu> _repMenu;
        private readonly IRepository<Database.MenuItemStyle> _repMenuItemStyle;
        private readonly IRepository<Database.MenuStyle> _repMenuStyle;
        private readonly IRepositoryConfig<Database.HeaderPageConfig> _repHeaderPageConfig;


        private readonly IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> _repWebsiteKeys;

        public HeaderPageService(
            IRepository<Database.HeaderPage> repHeaderPage,
            IRepositoryConfig<Database.HeaderPageConfig> repHeaderPageConfig,
            IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> repWebsiteKeys,
            IRepository<Database.FooterItem> repFooterItem,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repHeaderPage = repHeaderPage;
            _repHeaderPageConfig = repHeaderPageConfig;
            _repWebsiteKeys = repWebsiteKeys;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repHeaderPageConfig.GetAllConfigs();

        }

        public Response<IPagedList<Database.HeaderPage>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.HeaderPage>> rs = new Response<IPagedList<Database.HeaderPage>>();
            try
            {
                
                search.ToString();
                rs.Data = _repHeaderPage.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Title.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(_repHeaderPageConfig.GetConfigByKey(HeaderPageConstants.Config.Admin.PageSize, DefaultConfig.PAGE_SIZE.ToString())));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<Database.HeaderPage> GetById(int id)
        {
            Response<Database.HeaderPage> rs = new Response<Database.HeaderPage>();
            Database.HeaderPage detail = new Database.HeaderPage();
            try
            {

                detail = _repHeaderPage.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.HeaderPage> Save(Database.HeaderPage data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new HeaderPageValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<LayoutPage.Database.HeaderPage> rs = new Response<LayoutPage.Database.HeaderPage>();
            using (var transaction = _repHeaderPage.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repHeaderPage.Add(data);

                        _repHeaderPage.SaveChanges();
                    }
                    else
                    {
                        var model = _repHeaderPage.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.HeaderBlock = data.HeaderBlock;


                            _repHeaderPage.SaveChanges();

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

                    var model = _repHeaderPage.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repHeaderPage.Remove(model);
                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    //var files = _db.HeaderPage_Files.Where(p => p.HeaderPageId == model.Pid).ToList();
                    //if(files!= null)
                    //{
                    //    foreach (var file in files)
                    //    {
                    //        _fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, file.UploadFileName);

                    //    }
                    //    _db.HeaderPage_Files.RemoveRange(files);

                    //}


                    //

                    _repHeaderPage.SaveChanges();
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
   
        public Response<string> GenerateHeaderHtml(long pid)
        {
            Response<string> rs = new Response<string>();
            try
            {

                HeaderPageHelper _helper = new HeaderPageHelper();
                var listWebsiteKey = _repWebsiteKeys.Query().Where(p => p.isSystemKey == false).ToList();
                var listMenu = _repMenu.Query().OrderBy(p=>p.Order).ToList();
                rs = _helper.GenerateHeader(_repMenu.Query().ToList(), _repMenuItemStyle.Query().ToList(), _repMenuStyle.Query().ToList(), _repHeaderPage.Query().Where(p=>p.Pid==pid).FirstOrDefault(), listWebsiteKey);
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsError = true;

                rs.StatusCode = 500;
                rs.Message = ex.ToString();

                return rs;
            }
        }

    }

}
