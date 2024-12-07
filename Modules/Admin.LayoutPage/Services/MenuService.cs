using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.LayoutPage.Database;
using Admin.LayoutPage.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Policy;
using Admin.LayoutPage.Constants;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Drawing;
using ComponentUILibrary.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Microsoft.AspNetCore.Components.Forms;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using Steam.Infrastructure.Repository;

namespace Admin.LayoutPage.Services
{
    public class MenuService : IMenuService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;

        private readonly IRepository<Database.Menu> _repMenu;
        private readonly IRepository<Database.MenuStyle> _repMenuStyle;
        private readonly IRepositoryConfig<Database.MenuConfig> _repMenuConfig;
        public MenuService(
            IRepository<Database.Menu> repMenu,
            IRepositoryConfig<Database.MenuConfig> repMenuConfig,
            IRepository<Database.MenuStyle> repMenuStyle,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repMenuStyle = repMenuStyle;
            _repMenu = repMenu;
            _repMenuConfig = repMenuConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repMenuConfig.GetAllConfigs();


        }
        public void GetPid()
        {

        }
        public Response<List<Database.Menu>> GetList(ParamSearch search)
        {
            Response<List<Database.Menu>> rs = new Response<List<Database.Menu>>();
            try
            {

                rs.Data = _repMenu.Query().Where(p => (search.Cate == "0" || p.ParentID == Convert.ToInt32(search.Cate)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
                if (search.Cate != "0")
                {
                    var parrentMenu = _repMenu.Query().Where(p => p.Pid == Convert.ToInt32(search.Cate)).FirstOrDefault();
                    rs.Data.Add(parrentMenu);
                }


            }
            catch (Exception ex)
            {
                rs.IsError = true;
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

                detail = _repMenu.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public void SetChildLevel(long Pid, int Showlevel, bool isEnabled, string path)
        {
            var modelChild = _repMenu.Query().Where(p => p.ParentID == Pid).ToList();
            if (modelChild.Count > 0)
            {
                foreach (var child in modelChild)
                {

                    child.ShowLevel = Showlevel + 1;
                    child.Enabled = isEnabled;
                    child.Path = path + "-" + child.ParentID;
                    _repMenu.SaveChanges();
                    SetChildLevel(child.Pid, child.ShowLevel, child.Enabled, child.Path);

                }
            }


        }


        public Response<Database.Menu> Save(MenuModelEdit input)
        {
            LayoutPage.Database.Menu data = input.GetDatabaseModel();

            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new MenuValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<LayoutPage.Database.Menu> rs = new Response<LayoutPage.Database.Menu>();
            using (var transaction = _repMenu.BeginTransaction())
            {
                string path = "";
                try
                {


                    if (data.Pid == 0)
                    {
                        var tempParrent = _repMenu.Query().Where(p => p.Pid == input.ParentID).FirstOrDefault();
                        data.Order = 0.9;

                        if (tempParrent != null)
                        {
                            data.ShowLevel = tempParrent.ShowLevel + 1;
                            path += tempParrent.Path + "-" + tempParrent.Pid;
                        }
                        data.Path = path;

                       _repMenu.Add(data);
                        _repMenu.SaveChanges();

                    }
                    else
                    {
                        var model = _repMenu.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

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

                                    var modelParent = _repMenu.Query().Where(p => p.Pid == data.ParentID).FirstOrDefault();
                                    model.ShowLevel = modelParent.ShowLevel + 1;
                                    path += modelParent.Path + "-" + modelParent.Pid;
                                }
                                model.ParentID = data.ParentID;
                                model.Path = path;
                                _repMenu.SaveChanges();

                                SetChildLevel(model.Pid, model.ShowLevel, model.Enabled, model.Path);

                            }



                            if (input.files != null)
                            {
                                //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                                //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                            }
                            _repMenu.SaveChanges();

                            //SetChildLevel(model.Pid, model.ShowLevel);
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
        public void SaveListFile(LayoutPage.Database.Menu data, string listFIlesString)
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

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }
        void DeleteMenu(long id)
        {
            var model = _repMenu.Query().Where(p => p.Pid == id).FirstOrDefault();
            var listChild = _repMenu.Query().Where(p => p.ParentID == id).ToList();
            if (listChild.Count() > 0)
            {
                foreach (var item in listChild)
                {
                    DeleteMenu(item.Pid);

                }
            }
            _repMenu.Remove(model);
            _repMenu.SaveChanges();

        }
     
        public Response UpdateParent(int id, int parentId)
        {

            Response rs = new Response();

            try
            {
                var model = _repMenu.Query().Where(p => p.Pid == id).FirstOrDefault();
                if (parentId == 0)
                {
                    model.ParentID = parentId;

                    model.ShowLevel = 0;

                }
                else
                {
                    var parent = _repMenu.Query().Where(p => p.Pid == parentId).FirstOrDefault();

                    model.ParentID = parentId;
                    model.ShowLevel = parent.ShowLevel + 1;
                }

                _repMenu.SaveChanges();
                SetChildLevel(model.Pid, model.ShowLevel, model.Enabled, model.Path);

            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());

            }
            return rs;

        }
   
     


        public Response<List<SelectControlData>> GetMenuParent(int id)
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            List<SelectControlData> listParent = new List<SelectControlData>();
            try
            {
                var e = _repMenu.Query().Where(p => p.Pid == id).FirstOrDefault();
                if (e != null)
                {
                    listParent = _repMenu.Query().Where(p => p.Enabled == true && p.Pid != id && p.ShowLevel <= e.ShowLevel).Select(
                     row => new SelectControlData
                     {
                         Value = row.Pid.ToString(),
                         Name = row.Title
                     }
                    ).ToList<SelectControlData>();
                }
                else
                {
                    listParent = _repMenu.Query().Where(p => p.Enabled == true && p.Pid != id).Select(
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

                rs.IsError = true;
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

                var listMenuStyle = _repMenuStyle.Query().ToList().Select(
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

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        //public Response<List<SelectControlData>> GetMenuParent()
        //{
        //    Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
        //    //List<Database.LayoutPage> listParent = new List<Database.LayoutPage>();
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

        public Response<List<SelectControlData>> GetAllMenuParent()
        {
            Response<List<SelectControlData>> rs = new Response<List<SelectControlData>>();
            try
            {



                var listParent = _repMenu.Query().Where(p => p.Enabled == true).Select(
                   row => new SelectControlData
                   {
                       Value = row.Pid.ToString(),
                       Name = row.Title,
                       ParrentID = row.ParentID
                   }).ToList<SelectControlData>();




                rs.Data = listParent;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
    }

}
