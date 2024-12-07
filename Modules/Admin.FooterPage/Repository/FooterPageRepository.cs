
using Microsoft.AspNetCore.Http;
using Admin.FooterPage.Database;
using Admin.FooterPage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.FooterPage.Constants;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Common.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Microsoft.EntityFrameworkCore;

namespace Admin.FooterPage
{
    public class FooterPageRepository : IFooterPageRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        FooterPageContext _db;

        public FooterPageRepository(FooterPageContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.FooterPageConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public Response<IPagedList<Database.FooterPage>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.FooterPage>> rs = new Response<IPagedList<Database.FooterPage>>();
            try
            {
                search.ToString();
                rs.Data = _db.FooterPages.Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(100));
            }
            catch (Exception ex)
            {
                rs.isError = true;
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
                rs.Data = _db.FooterItems.Where(p => p.Deleted == false && p.FooterPagePid == Pid)
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
            }
            catch (Exception ex)
            {
                rs.isError = true;
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

                detail = _db.FooterPages.Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.FooterItem> GetChildById(int id)
        {
            Response<Database.FooterItem> rs = new Response<Database.FooterItem>();
            Database.FooterItem detail = new Database.FooterItem();
            try
            {

                detail = _db.FooterItems.Where(p => p.Pid == id).FirstOrDefault();

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
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _db.FooterPages.Add(data);

                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.FooterPages.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;
                            model.FooterBlock = data.FooterBlock;
                            model.FooterPluginBlock = data.FooterPluginBlock;


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
            using (var transaction = _db.Database.BeginTransaction())
            {


                try
                {
                    if (data.Pid == 0)
                    {
                        var modelGroup = _db.FooterItems.Where(p => p.FooterPagePid == data.FooterPagePid).ToList();
                        data.Order = 0.9;
                        
                        _db.FooterItems.Add(data);

                        _db.SaveChanges();
                    }

                    else
                    {
                        var model = _db.FooterItems.Where(p => p.Pid == data.Pid).FirstOrDefault();                        


                        if (model != null)
                        {
                            model.ItemBlock = data.ItemBlock;
                            model.Key = data.Key;


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

                    var model = _db.FooterPages.Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _db.FooterPages.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _db.FooterItems.Where(p => p.FooterPagePid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _db.FooterItems.RemoveRange(menuItems);

                    }

                    
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
        public Response DeleteChild(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {

                    var model = _db.FooterItems.Where(p => p.Pid == id).FirstOrDefault();
                    _db.FooterItems.Remove(model);


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
                    var model = _db.FooterPages.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.FooterPages.OrderBy(p => p.Order).ToList();
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
                var model = _db.FooterPages.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.FooterPages.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.FooterPages.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.FooterPages.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.FooterPages.OrderBy(p => p.Order).ToList();
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
        public Response<List<Database.FooterPageConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<Database.FooterPageConfig>> rs = new Response<List<Database.FooterPageConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    FooterPageConfig FooterPageConfig = _db.FooterPageConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (FooterPageConfig != null)
                    {
                        FooterPageConfig.Type = tab;
                        FooterPageConfig.Value = value;
                        FooterPageConfig.UpdateDate = DateTime.Now;
                        FooterPageConfig.UpdateUser = "";

                    }
                    else
                    {
                        FooterPageConfig = new Database.FooterPageConfig();
                        FooterPageConfig.Type = tab;

                        FooterPageConfig.Key = key;
                        FooterPageConfig.Group = "";
                        FooterPageConfig.Value = value;
                        FooterPageConfig.CreateDate = DateTime.Now;
                        FooterPageConfig.CreateUser = "";
                        FooterPageConfig.UpdateDate = DateTime.Now;
                        FooterPageConfig.UpdateUser = "";
                        _db.FooterPageConfigs.Add(FooterPageConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.FooterPageConfigs.ToList();
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
        public Response<List<Database.FooterPageConfig>> GetAllConfigs()
        {
            Response<List<Database.FooterPageConfig>> rs = new Response<List<Database.FooterPageConfig>>();
            try
            {

                var listConfig = _db.FooterPageConfigs.ToList();
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
        public Response<Database.FooterPageConfig> GetConfigByKey(string key)
        {
            Response<Database.FooterPageConfig> rs = new Response<Database.FooterPageConfig>();
            try
            {

                var config = _db.FooterPageConfigs.Where(p => p.Key == key).FirstOrDefault();
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

    }

}
