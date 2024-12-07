
using Microsoft.AspNetCore.Http;
using Admin.HeaderPage.Database;
using Admin.HeaderPage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.HeaderPage.Constants;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Common.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Microsoft.EntityFrameworkCore;

namespace Admin.HeaderPage
{
    public class HeaderPageRepository : IHeaderPageRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        HeaderPageContext _db;

        public HeaderPageRepository(HeaderPageContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.HeaderPageConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);


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
        public Response<IPagedList<Database.HeaderPage>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.HeaderPage>> rs = new Response<IPagedList<Database.HeaderPage>>();
            try
            {
                search.ToString();
                rs.Data = _db.HeaderPages.Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Title.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(_config[HeaderPageConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.isError = true;
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

                detail = _db.HeaderPages.Where(p => p.Pid == id).FirstOrDefault();

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
            Response<HeaderPage.Database.HeaderPage> rs = new Response<HeaderPage.Database.HeaderPage>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _db.HeaderPages.Add(data);

                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.HeaderPages.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.HeaderBlock = data.HeaderBlock;


                            _db.SaveChanges();

                        }


                    }

                    //---------end save list lisst file--------
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

                    var model = _db.HeaderPages.Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _db.HeaderPages.Remove(model);
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
                    var model = _db.HeaderPages.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.HeaderPages.OrderBy(p => p.Order).ToList();
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
                var model = _db.HeaderPages.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.HeaderPages.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.HeaderPages.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.HeaderPages.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.HeaderPages.OrderBy(p => p.Order).ToList();
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
        public Response<List<HeaderPage.Database.HeaderPageConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<HeaderPage.Database.HeaderPageConfig>> rs = new Response<List<HeaderPage.Database.HeaderPageConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    HeaderPage.Database.HeaderPageConfig HeaderPageConfig = _db.HeaderPageConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (HeaderPageConfig != null)
                    {
                        HeaderPageConfig.Type = tab;
                        HeaderPageConfig.Value = value;
                        HeaderPageConfig.UpdateDate = DateTime.Now;
                        HeaderPageConfig.UpdateUser = "";

                    }
                    else
                    {
                        HeaderPageConfig = new HeaderPage.Database.HeaderPageConfig();
                        HeaderPageConfig.Type = tab;

                        HeaderPageConfig.Key = key;
                        HeaderPageConfig.Group = "";
                        HeaderPageConfig.Value = value;
                        HeaderPageConfig.CreateDate = DateTime.Now;
                        HeaderPageConfig.CreateUser = "";
                        HeaderPageConfig.UpdateDate = DateTime.Now;
                        HeaderPageConfig.UpdateUser = "";
                        _db.HeaderPageConfigs.Add(HeaderPageConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.HeaderPageConfigs.ToList();
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
        public Response<List<HeaderPage.Database.HeaderPageConfig>> GetAllConfigs()
        {
            Response<List<HeaderPage.Database.HeaderPageConfig>> rs = new Response<List<HeaderPage.Database.HeaderPageConfig>>();
            try
            {

                var listConfig = _db.HeaderPageConfigs.ToList();
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
        public Response<HeaderPage.Database.HeaderPageConfig> GetConfigByKey(string key)
        {
            Response<HeaderPage.Database.HeaderPageConfig> rs = new Response<HeaderPage.Database.HeaderPageConfig>();
            try
            {

                var config = _db.HeaderPageConfigs.Where(p => p.Key == key).FirstOrDefault();
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
        public Response<string> GenerateHeaderHtml()
        {
            Response<string> rs = new Response<string>();
            try
            {

                HeaderPageHelper _helper = new HeaderPageHelper();
                rs.Data= _helper.GenerateHeader(_db.Menus.ToList(), _db.MenuItemStyles.ToList(), _db.MenuStyles.ToList(), _db.HeaderPages.FirstOrDefault());
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

    }

}
