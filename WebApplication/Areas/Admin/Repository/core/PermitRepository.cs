using CMS.Areas.Admin.Models;
using CMS.Services.TranslateServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Areas.Admin
{
    public class PermitRepository : IPermitRepository
    {

        private readonly DBContext _dBContext;
        private readonly ITranslateServices _translate;

        public PermitRepository(DBContext dbContext, ITranslateServices translate)
        {
            _dBContext = dbContext;
            _translate = translate;
        }
        private class Module
        {
            public string ModuleCode { get; set; }
            public string ModuleName { get; set; }
            public List<Permission> Permissons { get; set; }
        }
        private class Permission
        {
            public string PermissionCode { get; set; }
            public string PermissionName { get; set; }
            public bool IsChecked { get; set; }
        }

        private class GenernalPermission
        {
            public string ModuleCode { get; set; }
            public string PermissionCode { get; set; }
            public bool? Status { get; set; }
        }
        public string GetDataGroup(int? groupUserCode, string txtSearch)
        {
            using (_dBContext)
            {
                try
                {
                    var dataGroupPermission = _dBContext.GroupPermissons.Where(x => x.Deleted == false && x.Enabled == true && x.GroupUserCode == groupUserCode).ToList();
                    var dataModule = _dBContext.Modules.Where(x => x.Deleted == false && x.Enabled == true && x.Locked == false).OrderBy(x => x.Order).ToList();
                    var dataPermission = _dBContext.Permissions.Where(x => x.Locked == false).ToList();

                    var listResult = new List<Module>();
                    foreach (var item in dataModule)
                    {
                        var module = new Module();
                        module.ModuleCode = item.Code;
                        module.ModuleName = item.ModuleName;
                        var listPermission = new List<Permission>();

                        foreach (var p in dataPermission)
                        {
                            var permission = new Permission();
                            permission.PermissionCode = p.Code;
                            permission.PermissionName = _translate.GetStringAdmin("permission." + p.Name);
                            var isChecked = dataGroupPermission.Where(x => x.ModuleCode == item.Code && x.PermissonCode == p.Code).FirstOrDefault();
                            if (isChecked != null)
                            {
                                permission.IsChecked = true;
                            }
                            else
                            {
                                permission.IsChecked = false;
                            }
                            listPermission.Add(permission);
                        }

                        module.Permissons = listPermission;
                        listResult.Add(module);
                    }

                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        listResult = listResult.Where(x => x.ModuleName.ToLower().Contains(txtSearch.ToLower())).ToList();
                    }

                    return JsonConvert.SerializeObject(listResult);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public string GetDataUser(int? userCode, string txtSearch)
        {
            using (_dBContext)
            {
                try
                {
                    var userInfo = _dBContext.Users.FirstOrDefault(x => x.Pid == userCode && !x.Deleted);
                    var dataUserPermission = _dBContext.UserPermissions.Where(x => x.Deleted == false && x.Enabled == true && x.UserCode == userCode).ToList();
                    var dataGroupPermission = _dBContext.GroupPermissons.Where(x => x.Deleted == false && x.Enabled == true && x.GroupUserCode == userInfo.GroupUserCode).ToList();

                    List<GenernalPermission> genernalPermissions = new List<GenernalPermission>();

                    if (dataGroupPermission.Any())
                    {
                        foreach (var p in dataGroupPermission)
                        {
                            GenernalPermission genernal = new GenernalPermission();
                            genernal.ModuleCode = p.ModuleCode;
                            genernal.PermissionCode = p.PermissonCode;
                            genernal.Status = true;
                            genernalPermissions.Add(genernal);
                        }
                    }

                    if (dataUserPermission.Any())
                    {
                        foreach (var p in dataUserPermission)
                        {
                            var checkExist = genernalPermissions.Where(x => x.PermissionCode == p.PermissonCode && x.ModuleCode == p.ModuleCode).FirstOrDefault();
                            if (checkExist != null)
                            {
                                genernalPermissions.Remove(checkExist);
                            }
                            GenernalPermission genernal = new GenernalPermission();
                            genernal.ModuleCode = p.ModuleCode;
                            genernal.PermissionCode = p.PermissonCode;
                            genernal.Status = p.Status;
                            genernalPermissions.Add(genernal);
                        }
                    }


                    var dataModule = _dBContext.Modules.Where(x => x.Deleted == false && x.Enabled == true && x.Locked == false).ToList();
                    var dataPermission = _dBContext.Permissions.Where(x => x.Locked == false).ToList();

                    var listResult = new List<Module>();

                    foreach (var item in dataModule)
                    {
                        var module = new Module();
                        module.ModuleCode = item.Code;
                        module.ModuleName = item.ModuleName;
                        var listPermission = new List<Permission>();

                        foreach (var p in dataPermission)
                        {
                            var permission = new Permission();
                            permission.PermissionCode = p.Code;
                            permission.PermissionName = p.Name;
                            var isChecked = genernalPermissions.Where(x => x.ModuleCode == item.Code && x.PermissionCode == p.Code).FirstOrDefault();
                            permission.IsChecked = isChecked != null ? isChecked.Status.Value : false;
                            listPermission.Add(permission);
                        }

                        module.Permissons = listPermission;
                        listResult.Add(module);
                    }

                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        listResult = listResult.Where(x => x.ModuleName.ToLower().Contains(txtSearch.ToLower())).ToList();
                    }

                    return JsonConvert.SerializeObject(listResult);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public string InsertGroupPermission(int groupUserCode, string id, bool isCheked)
        {
            using (_dBContext)
            {
                try
                {
                    var moduleCode = id.Split("_")[0].ToString();
                    var permissionCode = id.Split("_")[1].ToString();
                    var model = _dBContext.GroupPermissons.Where(x => x.Deleted == false && x.Enabled == true
                        && x.ModuleCode == moduleCode && x.PermissonCode == permissionCode && x.GroupUserCode == groupUserCode).FirstOrDefault();
                    if (model != null)
                    {
                        if (!isCheked)
                        {
                            _dBContext.GroupPermissons.Remove(model);
                            _dBContext.SaveChanges();
                        }
                    }
                    else
                    {
                        if (isCheked)
                        {
                            var data = new GroupPermisson();
                            data.GroupUserCode = groupUserCode;
                            data.ModuleCode = moduleCode;
                            data.PermissonCode = permissionCode;
                            data.Enabled = true;
                            data.Deleted = false;
                            data.UpdateDate = DateTime.Now;
                            data.CreateDate = DateTime.Now;
                            data.UpdateUser = null;
                            data.CreateUser = null;
                            _dBContext.GroupPermissons.Add(data);
                            _dBContext.SaveChanges();
                        }
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
        }
        public string InsertGroupPermissionUser(int userCode, string id, bool isCheked)
        {
            using (_dBContext)
            {
                try
                {
                    var moduleCode = id.Split("_")[0].ToString();
                    var permissionCode = id.Split("_")[1].ToString();
                    var model = _dBContext.UserPermissions.Where(x => x.Deleted == false && x.Enabled == true
                        && x.ModuleCode == moduleCode && x.PermissonCode == permissionCode && x.UserCode == userCode).FirstOrDefault();
                    if (model != null)
                    {
                        model.Status = isCheked;
                        _dBContext.SaveChanges();
                    }
                    else
                    {
                        var data = new UserPermission();
                        data.UserCode = userCode;
                        data.ModuleCode = moduleCode;
                        data.PermissonCode = permissionCode;
                        data.Enabled = true;
                        data.Deleted = false;
                        data.UpdateDate = DateTime.Now;
                        data.CreateDate = DateTime.Now;
                        data.UpdateUser = null;
                        data.CreateUser = null;
                        data.Status = isCheked;
                        _dBContext.UserPermissions.Add(data);
                        _dBContext.SaveChanges();
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
        }
    }
}
