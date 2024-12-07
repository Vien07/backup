
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
using Steam.Core.Base.Constant;
using Admin.WebsiteKeys.Database;
using Steam.Infrastructure.Repository;

namespace Admin.LayoutPage.Services
{
    public class QuickToolBarService : IQuickToolBarService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.QuickToolBar> _repQuickToolBar;
        private readonly IRepository<Database.QuickToolBarItem> _repQuickToolBarItem;
        private readonly IRepositoryConfig<Database.QuickToolBarConfig> _repQuickToolBarConfig;
        private readonly IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> _repWebsiteKeys;

        public QuickToolBarService(
           IRepository<Database.QuickToolBarItem> repQuickToolBarItem,
          IRepository<Database.QuickToolBar> repQuickToolBar,
          IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> repWebsiteKeys,
          IRepositoryConfig<Database.QuickToolBarConfig> repQuickToolBarConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repQuickToolBarItem = repQuickToolBarItem;
            _repQuickToolBar = repQuickToolBar;
            _repWebsiteKeys = repWebsiteKeys;
            _repQuickToolBarConfig = repQuickToolBarConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repQuickToolBarConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.QuickToolBar>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.QuickToolBar>> rs = new Response<IPagedList<Database.QuickToolBar>>();
            try
            {
                search.ToString();
                rs.Data = _repQuickToolBar.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
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
        public Response<List<Database.QuickToolBarItem>> GetChildList(long? Pid)
        {
            Response<List<Database.QuickToolBarItem>> rs = new Response<List<Database.QuickToolBarItem>>();
            try
            {
                //search.ToString();
                rs.Data = _repQuickToolBarItem.Query().Where(p => p.Deleted == false && p.QuickToolBarPid == Pid)
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
        public Response<Database.QuickToolBar> GetById(int id)
        {
            Response<Database.QuickToolBar> rs = new Response<Database.QuickToolBar>();
            Database.QuickToolBar detail = new Database.QuickToolBar();
            try
            {

                detail = _repQuickToolBar.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.QuickToolBarItem> GetChildById(int id)
        {
            Response<Database.QuickToolBarItem> rs = new Response<Database.QuickToolBarItem>();
            Database.QuickToolBarItem detail = new Database.QuickToolBarItem();
            try
            {

                detail = _repQuickToolBarItem.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.QuickToolBar> Save(QuickToolBarModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new QuickToolBarValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.QuickToolBar> rs = new Response<Database.QuickToolBar>();
            using (var transaction = _repQuickToolBar.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repQuickToolBar.Add(data);

                        _repQuickToolBar.SaveChanges();
                    }
                    else
                    {
                        var model = _repQuickToolBar.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;
                            model.QuickToolBarBlock = data.QuickToolBarBlock;
                            _repQuickToolBar.SaveChanges();

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
        public Response<Database.QuickToolBarItem> SaveChild(QuickToolBarItemModelEdit input)
        {
            //-------save-file-pond----------
            var img = "";
            //var videolink = "";
            var filePath = "";

            //-------end-save-file-pond----------


            var validator = new QuickToolBarItemValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(input);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.QuickToolBarItem> rs = new Response<Database.QuickToolBarItem>();
           var data = input.GetDatabaseModel();

            using (var transaction = _repQuickToolBarItem.BeginTransaction())
            {


                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repQuickToolBarItem.Add(data);

                        _repQuickToolBarItem.SaveChanges();
                    }

                    else
                    {
                        data = _repQuickToolBarItem.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                        if (data != null)
                        {

                            data.Title = input.Title;

                            data.Value = input.Value;
                            data.Key = input.Key;
                            data.Icon = input.Icon;
                            data.ItemBlock = input.ItemBlock;

                            _repQuickToolBarItem.SaveChanges();

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

                    var model =_repQuickToolBar.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repQuickToolBar.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _repQuickToolBarItem.Query().Where(p => p.QuickToolBarPid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _repQuickToolBarItem.RemoveRange(menuItems);

                    }


                    //

                    _repQuickToolBar.SaveChanges();
                    _repQuickToolBarItem.SaveChanges();
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

                    var model = _repQuickToolBarItem.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repQuickToolBarItem.Remove(model);


                    _repQuickToolBarItem.SaveChanges();
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

        public Response MoveChild(int fromId, int toId)
        {

            Response rs = new Response();

            try
            {
                var fromModel =_repQuickToolBarItem.Query().Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _repQuickToolBarItem.Query().Where(p => p.Pid == toId).FirstOrDefault();

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

                    _repQuickToolBarItem.SaveChanges();
                    var list = _repQuickToolBarItem.Query().OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _repQuickToolBarItem.SaveChanges();
                    }

                }
                //var list = _db.QuickToolBars.OrderBy(p => p.Order).ToList();
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
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<string> GenerateQuickToolBarHtml(long pid)
        {
            Response<string> rs = new Response<string>();
            try
            {

                QuickToolBarHelper _helper = new QuickToolBarHelper();
                var QuickToolBar = _repQuickToolBar.Query().Where(p => p.Pid == pid).FirstOrDefault();
                var QuickToolBarItem = _repQuickToolBarItem.Query().Where(p => p.QuickToolBarPid == QuickToolBar.Pid).ToList();
                var listWebsiteKey = _repWebsiteKeys.Query().Where(p => p.isSystemKey == false).ToList();

                rs = _helper.GenerateQuickToolBar(QuickToolBar, QuickToolBarItem, listWebsiteKey);
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
