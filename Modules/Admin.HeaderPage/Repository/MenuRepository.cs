using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.HeaderPage.Database;
using Admin.HeaderPage.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Policy;
using Admin.HeaderPage.Constants;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Drawing;
using ComponentUILibrary.Models;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Microsoft.AspNetCore.Components.Forms;

namespace Admin.HeaderPage
{
    public class MenuRepository : IMenuRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        HeaderPageContext _db;
        public MenuRepository(HeaderPageContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.MenuConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public void GetPid()
        {

        }
        public Response<List<Database.Menu>> GetList(ParamSearch search)
        {
            Response<List<Database.Menu>> rs = new Response<List<Database.Menu>>();
            try
            {

                rs.Data = _db.Menus.Where(p => (search.Cate == "0" || p.ParentID == Convert.ToInt32(search.Cate)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
                if (search.Cate != "0")
                {
                    var parrentMenu = _db.Menus.Where(p => p.Pid == Convert.ToInt32(search.Cate)).FirstOrDefault();
                    rs.Data.Add(parrentMenu);
                }


            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }

        public Response<Database.Menu> GetById(int id)
        {
            Response<Database.Menu> rs = new Response<Database.Menu>();
            Database.Menu detail = new Database.Menu();
            try
            {

                detail = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();

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
        public void SetChildLevel(long Pid, int Showlevel, bool isEnabled, string path)
        {
            var modelChild = _db.Menus.Where(p => p.ParentID == Pid).ToList();
            if (modelChild.Count > 0)
            {
                foreach (var child in modelChild)
                {

                    child.ShowLevel = Showlevel + 1;
                    child.Enabled = isEnabled;
                    child.Path = path + "-" + child.ParentID;
                    _db.SaveChanges();
                    SetChildLevel(child.Pid, child.ShowLevel, child.Enabled, child.Path);

                }
            }


        }

        
        public Response<Database.Menu> Save(MenuModelEdit input)
        {
            HeaderPage.Database.Menu data = input.GetDatabaseModel();

            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new MenuValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<HeaderPage.Database.Menu> rs = new Response<HeaderPage.Database.Menu>();
            using (var transaction = _db.Database.BeginTransaction())
            {
                string path = "";
                try
                {
                    

                    if (data.Pid == 0)
                    {
                        var tempParrent = _db.Menus.Where(p => p.Pid == input.ParentID).FirstOrDefault();
                        data.Order = 0.9;
                        
                        if (tempParrent != null)
                        {
                            data.ShowLevel = tempParrent.ShowLevel + 1;
                            path += tempParrent.Path + "-" + tempParrent.Pid;
                        }
                        data.Path = path;

                        _db.Menus.Add(data);
                        _db.SaveChanges();

                    }
                    else
                    {
                        var model = _db.Menus.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.Description = data.Description;
                            model.URL = data.URL;
                            model.Event = data.Event;
                            model.MenuStylePid = data.MenuStylePid;
                            //model.ParentID = data.ParentID;


                            if (model.ParentID != data.ParentID)
                            {
                                
                                if (data.ParentID == 0)
                                {
                                    model.ShowLevel = 0;
                                    path += data.Pid.ToString() + "-";
                                    //SetChildLevel(model.Pid, model.ShowLevel);
                                }
                                else
                                {
                                   
                                    var modelParent = _db.Menus.Where(p => p.Pid == data.ParentID).FirstOrDefault();
                                    model.ShowLevel = modelParent.ShowLevel + 1;
                                    path += modelParent.Path + "-" + modelParent.Pid;
                                }
                                model.ParentID = data.ParentID;
                                model.Path = path;
                                _db.SaveChanges();

                                SetChildLevel(model.Pid, model.ShowLevel, model.Enabled,model.Path);

                            }



                            if (input.files != null)
                            {
                                //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                                //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                            }
                            _db.SaveChanges();

                            //SetChildLevel(model.Pid, model.ShowLevel);
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
        public void SaveListFile(HeaderPage.Database.Menu data, string listFIlesString)
        {
            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);

            try
            {


            }
            catch (Exception ex)
            {

            }
        }
        public Response Delete(int id)
        {

            Response rs = new Response();

            try
            {

                DeleteMenu(id);




            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }
        void DeleteMenu(long id)
        {
            var model = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();
            var listChild = _db.Menus.Where(p => p.ParentID == id).ToList();
            if (listChild.Count() > 0)
            {
                foreach (var item in listChild)
                {
                    DeleteMenu(item.Pid);

                }
            }
            _db.Menus.Remove(model);
            _db.SaveChanges();

        }
        public Response Enable(List<int> ids, bool isEnable)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();
                    model.Enabled = isEnable;
                    _db.SaveChanges();
                    //SetChildLevel(model.Pid, model.ShowLevel, model.Enabled, model.Path);
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
        public Response UpdateParent(int id, int parentId)
        {

            Response rs = new Response();

            try
            {
                var model = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();
                model.ParentID = parentId;
                _db.SaveChanges();
                SetChildLevel(model.Pid, model.ShowLevel, model.Enabled, model.Path);

            }
            catch (Exception ex)
            {

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }
        public Response EnableUpdateOrder()
        {

            Response rs = new Response();

            try
            {
                var list = _db.Menus.OrderBy(p => p.Order).ToList();
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
                var model = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.Menus.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Menus.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.Menus.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
                    }
                }
                //var list = _db.Menus.OrderBy(p => p.Order).ToList();
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
        public Response<List<HeaderPage.Database.MenuConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<HeaderPage.Database.MenuConfig>> rs = new Response<List<HeaderPage.Database.MenuConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    HeaderPage.Database.MenuConfig sampleConfig = _db.MenuConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (sampleConfig != null)
                    {
                        sampleConfig.Type = tab;
                        sampleConfig.Value = value;
                        sampleConfig.UpdateDate = DateTime.Now;
                        sampleConfig.UpdateUser = "";

                    }
                    else
                    {
                        sampleConfig = new HeaderPage.Database.MenuConfig();
                        sampleConfig.Type = tab;

                        sampleConfig.Key = key;
                        sampleConfig.Group = "";
                        sampleConfig.Value = value;
                        sampleConfig.CreateDate = DateTime.Now;
                        sampleConfig.CreateUser = "";
                        sampleConfig.UpdateDate = DateTime.Now;
                        sampleConfig.UpdateUser = "";
                        _db.MenuConfigs.Add(sampleConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.MenuConfigs.ToList();
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
        public Response<List<HeaderPage.Database.MenuConfig>> GetAllConfigs()
        {
            Response<List<HeaderPage.Database.MenuConfig>> rs = new Response<List<HeaderPage.Database.MenuConfig>>();
            try
            {

                var listConfig = _db.MenuConfigs.ToList();
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
        public Response<HeaderPage.Database.MenuConfig> GetConfigByKey(string key)
        {
            Response<HeaderPage.Database.MenuConfig> rs = new Response<HeaderPage.Database.MenuConfig>();
            try
            {

                var config = _db.MenuConfigs.Where(p => p.Key == key).FirstOrDefault();
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



        public Response<List<SelectControlData>> GetMenuParent(int id)
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            List<SelectControlData> listParent = new List<SelectControlData>();
            try
            {
                var e = _db.Menus.Where(p => p.Pid == id).FirstOrDefault();
                if (e != null)
                {
                    listParent = _db.Menus.Where(p => p.Enabled == true && p.Pid != id && p.ShowLevel <= e.ShowLevel).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title
                     }
                    ).ToList<SelectControlData>();
                }
                else
                {
                    listParent = _db.Menus.Where(p => p.Enabled == true && p.Pid != id).Select(
                       row => new SelectControlData
                       {
                           Value = row.Pid.ToString(),
                           Name = row.Title
                       }
                   ).ToList<SelectControlData>();
                }



                rs.Data = listParent;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }      
        public Response<List<SelectControlData>> GetListMenuStyle()
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            try
            {

                   var  listMenuStyle = _db.MenuStyles.ToList().Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title
                     }
                    ).ToList<SelectControlData>();



                rs.Data = listMenuStyle;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        //public Response<List<SelectControlData>> GetMenuParent()
        //{
        //    Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
        //    //List<Database.HeaderPage> listParent = new List<Database.HeaderPage>();
        //    List<SelectControlData> listParent = new List<SelectControlData>();
        //    try
        //    {
        //        listParent = _db.Menus.Where(p => p.Enabled == true && p.ParentID == 0).Select(
        //           row => new SelectControlData
        //           {
        //               Value = row.Pid.ToString(),
        //               Name = row.Title,
        //               Order = row.Order
        //           }
        //       ).OrderBy(p => p.Order).ToList<SelectControlData>();
        //        listParent.Add(new SelectControlData { Value = "0", Name = "--Chọn menu cha", Order = 0 });
        //        listParent = listParent.OrderBy(p => p.Order).ToList();

        //        rs.Data = listParent;
        //        return rs;

        //    }
        //    catch (Exception ex)
        //    {
        //        rs.StatusCode = 500;

        //        rs.isError = true;
        //        rs.Message = ex.Message;
        //        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

        //    }
        //    return rs;
        //}
    }

}
