
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
using Steam.Infrastructure.Repository;

namespace Admin.LayoutPage.Services
{
    public class MenuStyleService : IMenuStyleService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.MenuStyle> _repMenuStyle;
        private readonly IRepository<Database.MenuItemStyle> _repMenuItemStyle;
        private readonly IRepositoryConfig<Database.MenuStyleConfig> _repMenuStyleConfig;
        public MenuStyleService(
           IRepository<Database.MenuStyle> repMenuStyle,
           IRepository<Database.MenuItemStyle> repMenuItemStyle,
           IRepositoryConfig<Database.MenuStyleConfig> repMenuStyleConfig,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repMenuStyle = repMenuStyle;
            _repMenuItemStyle = repMenuItemStyle;
            _repMenuStyleConfig = repMenuStyleConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repMenuStyleConfig.GetAllConfigs();

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
                rs.Data = _repMenuStyle.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Title.Contains(search.KeySearch)))
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
        public Response<List<Database.MenuItemStyle>> GetChildList(long? Pid)
        {
            Response<List<Database.MenuItemStyle>> rs = new Response<List<Database.MenuItemStyle>>();
            try
            {
                //search.ToString();
                rs.Data = _repMenuItemStyle.Query().Where(p => p.Deleted == false && p.MenuStylePid == Pid)
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
        public Response<Database.MenuStyle> GetById(int id)
        {
            Response<Database.MenuStyle> rs = new Response<Database.MenuStyle>();
            Database.MenuStyle detail = new Database.MenuStyle();
            try
            {

                detail = _repMenuStyle.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.MenuItemStyle> GetChildById(int id)
        {
            Response<Database.MenuItemStyle> rs = new Response<Database.MenuItemStyle>();
            Database.MenuItemStyle detail = new Database.MenuItemStyle();
            try
            {

                detail = _repMenuItemStyle.Query().Where(p => p.Pid == id).FirstOrDefault();

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
            using (var transaction = _repMenuStyle.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repMenuStyle.Add(data);

                        _repMenuStyle.SaveChanges();
                    }
                    else
                    {
                        var model = _repMenuStyle.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.MenuBlock = data.MenuBlock;


                            _repMenuStyle.SaveChanges();

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
            using (var transaction = _repMenuItemStyle.BeginTransaction())
            {


                try
                {
                    if (data.Pid == 0)
                    {
                        var modelGroup = _repMenuItemStyle.Query().Where(p => p.MenuStylePid == data.MenuStylePid).OrderByDescending(p => p.Level).ToList();
                        if (modelGroup.Count() > 0)
                        {
                            data.Level = modelGroup[0].Level + 1; // Add a null-conditional operator
                        }
                        else
                        {
                            data.Level = 0;
                        }
                        data.Order = 0.9;
                        
                        _repMenuItemStyle.Add(data);

                        _repMenuItemStyle.SaveChanges();
                    }

                    else
                    {
                        var model = _repMenuItemStyle.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();                        


                        if (model != null)
                        {
                            model.Level = data.Level;
                            model.ItemBlock = data.ItemBlock;


                            _repMenuItemStyle.SaveChanges();

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

                    var model = _repMenuStyle.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repMenuStyle.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _repMenuItemStyle.Query().Where(p => p.MenuStylePid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _repMenuItemStyle.RemoveRange(menuItems);

                    }


                    //

                    _repMenuItemStyle.SaveChanges();
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

                    var model = _repMenuItemStyle.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repMenuItemStyle.Remove(model);


                    _repMenuItemStyle.SaveChanges();
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
       
    

    }

}
