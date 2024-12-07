
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
    public class MenuStyleRepository : IMenuStyleRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        HeaderPageContext _db;

        public MenuStyleRepository(HeaderPageContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.MenuStyleConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public Response<IPagedList<Database.MenuStyle>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.MenuStyle>> rs = new Response<IPagedList<Database.MenuStyle>>();
            try
            {
                search.ToString();
                rs.Data = _db.MenuStyles.Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Title.Contains(search.KeySearch)))
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
        public Response<List<Database.MenuItemStyle>> GetChildList(long? Pid)
        {
            Response<List<Database.MenuItemStyle>> rs = new Response<List<Database.MenuItemStyle>>();
            try
            {
                //search.ToString();
                rs.Data = _db.MenuItemStyles.Where(p => p.Deleted == false && p.MenuStylePid == Pid)
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
        public Response<Database.MenuStyle> GetById(int id)
        {
            Response<Database.MenuStyle> rs = new Response<Database.MenuStyle>();
            Database.MenuStyle detail = new Database.MenuStyle();
            try
            {

                detail = _db.MenuStyles.Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.MenuItemStyle> GetChildById(int id)
        {
            Response<Database.MenuItemStyle> rs = new Response<Database.MenuItemStyle>();
            Database.MenuItemStyle detail = new Database.MenuItemStyle();
            try
            {

                detail = _db.MenuItemStyles.Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.MenuStyle> Save(MenuStyleModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new MenuStyleValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.MenuStyle> rs = new Response<Database.MenuStyle>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _db.MenuStyles.Add(data);

                        _db.SaveChanges();
                    }
                    else
                    {
                        var model = _db.MenuStyles.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.MenuBlock = data.MenuBlock;


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
        public Response<Database.MenuItemStyle> SaveChild(MenuItemStyleModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new MenuItemStyleValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.MenuItemStyle> rs = new Response<Database.MenuItemStyle>();
            using (var transaction = _db.Database.BeginTransaction())
            {


                try
                {
                    if (data.Pid == 0)
                    {
                        var modelGroup = _db.MenuItemStyles.Where(p => p.MenuStylePid == data.MenuStylePid).OrderByDescending(p => p.Level).ToList();
                        if (modelGroup.Count() > 0)
                        {
                            data.Level = modelGroup[0].Level + 1; // Add a null-conditional operator
                        }
                        else
                        {
                            data.Level = 0;
                        }
                        data.Order = 0.9;
                        
                        _db.MenuItemStyles.Add(data);

                        _db.SaveChanges();
                    }

                    else
                    {
                        var model = _db.MenuItemStyles.Where(p => p.Pid == data.Pid).FirstOrDefault();                        


                        if (model != null)
                        {
                            model.Level = data.Level;
                            model.ItemBlock = data.ItemBlock;


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

                    var model = _db.MenuStyles.Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _db.MenuStyles.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _db.MenuItemStyles.Where(p => p.MenuStylePid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _db.MenuItemStyles.RemoveRange(menuItems);

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

                    var model = _db.MenuItemStyles.Where(p => p.Pid == id).FirstOrDefault();
                    _db.MenuItemStyles.Remove(model);


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
                    var model = _db.MenuStyles.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.MenuStyles.OrderBy(p => p.Order).ToList();
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
                var model = _db.MenuStyles.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.MenuStyles.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.MenuStyles.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.MenuStyles.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.MenuStyles.OrderBy(p => p.Order).ToList();
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
        public Response<List<Database.MenuStyleConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<Database.MenuStyleConfig>> rs = new Response<List<Database.MenuStyleConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    MenuStyleConfig MenuStyleConfig = _db.MenuStyleConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (MenuStyleConfig != null)
                    {
                        MenuStyleConfig.Type = tab;
                        MenuStyleConfig.Value = value;
                        MenuStyleConfig.UpdateDate = DateTime.Now;
                        MenuStyleConfig.UpdateUser = "";

                    }
                    else
                    {
                        MenuStyleConfig = new Database.MenuStyleConfig();
                        MenuStyleConfig.Type = tab;

                        MenuStyleConfig.Key = key;
                        MenuStyleConfig.Group = "";
                        MenuStyleConfig.Value = value;
                        MenuStyleConfig.CreateDate = DateTime.Now;
                        MenuStyleConfig.CreateUser = "";
                        MenuStyleConfig.UpdateDate = DateTime.Now;
                        MenuStyleConfig.UpdateUser = "";
                        _db.MenuStyleConfigs.Add(MenuStyleConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.MenuStyleConfigs.ToList();
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
        public Response<List<Database.MenuStyleConfig>> GetAllConfigs()
        {
            Response<List<Database.MenuStyleConfig>> rs = new Response<List<Database.MenuStyleConfig>>();
            try
            {

                var listConfig = _db.MenuStyleConfigs.ToList();
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
        public Response<Database.MenuStyleConfig> GetConfigByKey(string key)
        {
            Response<Database.MenuStyleConfig> rs = new Response<Database.MenuStyleConfig>();
            try
            {

                var config = _db.MenuStyleConfigs.Where(p => p.Key == key).FirstOrDefault();
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
