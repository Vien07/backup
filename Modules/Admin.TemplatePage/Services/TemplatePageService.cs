
using Microsoft.AspNetCore.Http;
using Admin.TemplatePage.Database;
using Admin.TemplatePage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.TemplatePage.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Admin.SEO;
using Steam.Infrastructure.Repository;
using Admin.SEO.Database;
using Admin.SEO.Services;

namespace Admin.TemplatePage.Services
{
    public class TemplatePageService : ITemplatePageService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _CONFIG;
        private readonly IRepository<Database.TemplatePage> _repTemplatePage;
        private readonly IRepositoryConfig<Database.TemplatePageConfig> _repTemplatePageConfig;
        private ISEOService _srvSEO;

        public TemplatePageService(
            IRepository<Database.TemplatePage> repTemplatePage,
            IRepositoryConfig<Database.TemplatePageConfig> repTemplatePageConfig,
            IFileHelper fileHelper,
            ISEOService srvSEO,
            ILoggerHelper logger)
        {
            _repTemplatePage = repTemplatePage;
            _repTemplatePageConfig = repTemplatePageConfig;
            _srvSEO = srvSEO;
            _logger = logger;
            _fileHelper = fileHelper;
            _CONFIG = _repTemplatePageConfig.GetAllConfigs();
        }
        public Response<IPagedList<Database.TemplatePage>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.TemplatePage>> rs = new Response<IPagedList<Database.TemplatePage>>();
            try
            {
                search.ToString();
                rs.Data = _repTemplatePage.Query().Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Url.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_CONFIG[TemplatePageConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<TemplatePageDetail> GetById(int id)
        {
            Response<TemplatePageDetail> rs = new Response<TemplatePageDetail>();
            TemplatePageDetail detail = new TemplatePageDetail();
            try
            {

                detail.Detail = _repTemplatePage.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<TemplatePage.Database.TemplatePage> Save(TemplatePageModelEdit input)
        {
            Response<TemplatePage.Database.TemplatePage> rs = new Response<TemplatePage.Database.TemplatePage>();
            Database.TemplatePage modelResponse = new Database.TemplatePage();
            try
            {
                //-------save-file-pond----------
                var img = "";
                var filePath = "";

                SaveImage(ref img, ref filePath,
                    input.fileStatus,
                    input.files,
                    input.FilePath, input.Name);
                //-------end-save-file-pond----------

                var validator = new TemplatePageValidator();

                // Execute the validator
                ValidationResult results = validator.Validate(input);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                if (!success)
                {
                    string mess = string.Join(";", results.Errors);

                    rs.Message = mess;
                    rs.IsError = true;
                    return rs;
                }

                using (var transaction = _repTemplatePage.BeginTransaction())
                {
                    try
                    {
                        modelResponse = input.GetDatabaseModel();

                        if (modelResponse.Pid == 0)
                        {
                            if (img != "")
                            {
                                modelResponse.Image = img;

                            }
                            if (filePath != "")
                            {
                                modelResponse.FilePath = filePath;

                            }
                            modelResponse.Order = 0.9;

                            _repTemplatePage.Add(modelResponse);

                            _repTemplatePage.SaveChanges();
                        }
                        else
                        {
                           var editModel = _repTemplatePage.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (modelResponse != null)
                            {
                                if (img != "")
                                {
                                    editModel.Image = img;

                                }
                                if (filePath != "")
                                {
                                    editModel.FilePath = filePath;

                                }
                                editModel.Name = modelResponse.Name;
                                editModel.Description = modelResponse.Description;
                                editModel.Url = modelResponse.Url;
                                editModel.PageType = modelResponse.PageType;
                                editModel.Parameters = modelResponse.Parameters;
                                _repTemplatePage.SaveChanges();

                            }


                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        rs.IsError = true;
                        rs.Message = ex.Message;
                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, modelResponse.ToJson());

                    }
                }
                rs.Data = modelResponse;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
            }

            return rs;

        }
        public void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
        {
            try
            {
                if (filestatus == "remove")
                {
                    img = "";
                    return;
                }

                if (filestatus != "existed")
                {

                    if (!String.IsNullOrEmpty(filePath))
                    {
                        var arrFile = filePath.Split('/');
                        img = arrFile[arrFile.Length - 1];
                        imgFilePath = SystemInfo.PathFileManager + "/" + filePath.Replace(img, "");
                    }
                    else
                    {
                        if (file != null)
                        {
                            img = _fileHelper.UploadImageModule(
                                  new UploadImageInfo
                                  {
                                      FileName = title.ToSlug(),
                                      Height = Convert.ToInt32(_CONFIG[TemplatePageConstants.Config.Admin.MaxHeight].ToString()),
                                      Width = Convert.ToInt32(_CONFIG[TemplatePageConstants.Config.Admin.MaxWidth].ToString()),
                                      Path = TemplatePageConstants.StaticPath.Asset.Image,
                                      PathThumb = TemplatePageConstants.StaticPath.Asset.ImageThumb,
                                      File = file
                                  }
                                  ).FileName;
                            imgFilePath = TemplatePageConstants.StaticPath.Asset.Image;
                        }

                    }

                    //-------end-save-file-pond----------

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    _srvSEO.Delete(id, TemplatePageConstants.ModuleInfo.ModuleCode);
                    var model = _repTemplatePage.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repTemplatePage.Remove(model);
                    //check and remove images


                    _repTemplatePage.SaveChanges();
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
