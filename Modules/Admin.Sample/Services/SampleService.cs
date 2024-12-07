
using Microsoft.AspNetCore.Http;
using Admin.Sample.Database;
using Admin.Sample.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.Sample.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using System.Text.Json;
using Steam.Infrastructure.Repository;
using AutoMapper;
using Config = Admin.Sample.Constants.SampleConstants.Config.Admin;
using PathAsset = Admin.Sample.Constants.SampleConstants.StaticPath.Asset;
using ImageProcessor.Imaging.Formats;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Steam.Core.LocalizeManagement.Services;
using Steam.Core.LocalizeManagement.ExtensionMethod;
namespace Admin.Sample
{
    public class SampleService : ISampleService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _CONFIG;
        private readonly IRepository<Database.SampleDetail> _repSampleDetail;
        private readonly IRepository<Database.Sample_Files> _repSample_Files;
        private readonly IRepositoryConfig<Database.SampleConfig> _repSampleConfig;
        private readonly IMapper _mapper;
        private IContentLocalizationService _localize;
        public SampleService(IRepository<Database.SampleDetail> repSampleDetail,
            IRepository<Database.Sample_Files> repSample_Files,
              IRepositoryConfig<Database.SampleConfig> repSampleConfig,
            IFileHelper fileHelper,
            IContentLocalizationService localize,
            ILoggerHelper logger)
        {
            _repSampleDetail = repSampleDetail;
            _repSample_Files = repSample_Files;
            _repSampleConfig = repSampleConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _localize = localize;
            _CONFIG = _repSampleConfig.GetAllConfigs();
            _mapper = AutoMapperConfig.Initialize();
            Steam.Core.LocalizeManagement.ExtensionMethod.LocallizableMethod.Initialize(_localize);
        }

        public PagedListDto<SampleDetailDTO> GetList(ParamSearch search)
        {
            PagedListDto<SampleDetailDTO> rs = new PagedListDto<SampleDetailDTO>();

            try
            {
                // Use search parameters properly
                var query = _repSampleDetail.Query().Where(
                    p => p.Deleted == false);

                if (!string.IsNullOrEmpty(search.KeySearch))
                {
                    query = query.Where(p => p.Title.Contains(search.KeySearch));
                }

                // OrderBy and paging
                var pagedEntities = query.OrderBy(p => p.Order)
                         .ThenBy(p => p.UpdateDate).ToPagedList(search.PageIndex, Convert.ToInt32(_CONFIG[Config.PageSize]));
                var pagedDtos = pagedEntities.Select(entity => _mapper.Map<SampleDetailDTO>(entity)).ToList();

                rs = new PagedListDto<SampleDetailDTO>
                {
                    Items = pagedDtos,
                    PageNum = pagedEntities.PageNumber,
                    PageSize = pagedEntities.PageSize,
                    Total = pagedEntities.TotalItemCount,
                    PageCount = pagedEntities.PageCount
                };
            }
            catch (Exception ex)
            {

            }

            // Return the result
            return rs;//?? new PagedList<Database.SampleDetail>(new List<Database.SampleDetail>(), 1, 10); // Return empty paged list if rs is null
        }

        public long Save(SampleModelEdit input)
        {
            Admin.Sample.Database.SampleDetail entity = new SampleDetail() ;

            try
            {
                string img = string.Empty;
                string filePath = string.Empty;
                bool stateNewFile = false;

                #region save file pond
                FileUploadFilePondComponent filePond = new FileUploadFilePondComponent();
                DropzoneComponent dropzone = new DropzoneComponent();

                #endregion
                entity = input.GetDatabaseModel(_mapper);

                using (var transaction = _repSampleDetail.BeginTransaction())
                {
                    try
                    {

                        if (!string.IsNullOrEmpty(img)) entity.Images = img;
                        if (!string.IsNullOrEmpty(filePath)) entity.FilePath = filePath;
                        if (!String.IsNullOrEmpty(input.LangCode) && input.LangCode == DefaultConfig.DEFAULT_LANGUAGE)
                        {

                            if (entity.Pid == 0)
                            {
                                entity.Order = 0.9;
                                _repSampleDetail.Add(entity);
                            }
                            else
                            {
                                entity = _repSampleDetail.Query().FirstOrDefault(p => p.Pid == input.Pid);

                                if (entity != null)
                                {
                                    if (!string.IsNullOrEmpty(img)) entity.Images = img;
                                    if (!string.IsNullOrEmpty(filePath)) entity.FilePath = filePath;

                                    entity.Title = input.Title;
                                    entity.Description = input.Description;
                                    entity.Link = input.Link;
                                    entity.Position = input.Position;
                                }
                            }
                        }
                        else
                        {
                            entity.UpdateLocalize<Database.SampleDetail>(entity.Pid, input.LangCode);
                        }
                        entity.Images = "";
                        _repSampleDetail.SaveChanges();

                        #region image
                        var image = filePond.SaveImage(new FileUploadControlModel.SaveImageModel
                        {
                            Title = entity.Title.ToSlug(),
                            File = input.files,
                            FilePath = input.FilePath,
                            UploadThumbPath = PathAsset.ImageThumb,
                            UploadPath = PathAsset.Image,
                            Filestatus = input.fileStatus,
                            Height = Convert.ToInt32(_CONFIG[Config.MaxHeight].ToString()),
                            Width = Convert.ToInt32(_CONFIG[Config.MaxWidth].ToString()),
                        });
                        #endregion
                        entity.Images = image.ImageName ?? "";
                        entity.FilePath = image.FilePath;
                        _repSampleDetail.SaveChanges();

                        transaction.Commit();

                        if (!string.IsNullOrEmpty(input.ListFiles))
                        {
                            _repSample_Files.SaveListFile(entity.Pid,
                                PathAsset.Image,
                                input.ListFiles,
                                Convert.ToInt32(_CONFIG[Config.MaxHeight].ToString()),
                                Convert.ToInt32(_CONFIG[Config.MaxWidth].ToString()));
                            // SaveListFile(modelResponse, input.ListFiles);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        if (stateNewFile)
                        {
                            _fileHelper.DeleteFile(PathAsset.Image, img);
                            _fileHelper.DeleteFile(PathAsset.ImageThumb, img);
                        }

                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, entity.ToJson());
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return entity.Pid;
        }
        public void SaveListFile(Sample.Database.SampleDetail data, string listFilesString)
        {
            try
            {
                var listFiles = JsonSerializer.Deserialize<List<FileInfoModel>>(listFilesString);
                var filesToAdd = new List<Sample.Database.Sample_Files>();

                foreach (var item in listFiles)
                {
                    switch (item.status)
                    {
                        case "new":
                            var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                            {
                                Base64 = item.dataUrl,
                                Height = Convert.ToInt32(_CONFIG[Config.MaxHeight].ToString()),
                                Width = Convert.ToInt32(_CONFIG[Config.MaxWidth].ToString()),
                                FileName = data.Title.ToSlug(),
                                Path = SampleConstants.StaticPath.Asset.Image
                            });

                            if (!saveFile.isError)
                            {
                                filesToAdd.Add(new Sample.Database.Sample_Files
                                {
                                    SampleId = data.Pid,
                                    Caption = item.caption,
                                    Description = item.description,
                                    UploadFileName = saveFile.FileName,
                                    Order = item.order
                                });
                            }
                            break;

                        case "delete":
                            _fileHelper.DeleteFile(SampleConstants.StaticPath.Asset.Image, item.name);
                            var imageToDelete = _repSample_Files.Query().FirstOrDefault(p => p.Pid == Convert.ToInt32(item.id));
                            if (imageToDelete != null)
                            {
                                _repSample_Files.Remove(imageToDelete);
                            }
                            break;

                        case "edit":
                            var imageToEdit = _repSample_Files.Query().FirstOrDefault(p => p.Pid == Convert.ToInt32(item.id));
                            if (imageToEdit != null)
                            {
                                imageToEdit.Caption = item.caption;
                                imageToEdit.Description = item.description;
                                imageToEdit.Order = item.order;
                            }
                            break;
                    }
                }

                if (filesToAdd.Any())
                {
                    _repSample_Files.AddRange(filesToAdd);
                }

                _repSample_Files.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());
            }
        }

        public Sample.Database.SampleDetail Delete(List<int> ids)
        {

            Sample.Database.SampleDetail rs = new Sample.Database.SampleDetail();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repSampleDetail.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repSampleDetail.Remove(model);
                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var files = _repSample_Files.Query().Where(p => p.SampleId == model.Pid).ToList();
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            _fileHelper.DeleteFile(SampleConstants.StaticPath.Asset.Image, file.UploadFileName);

                        }
                        _repSample_Files.RemoveRange(files);

                    }


                    //

                    _repSample_Files.SaveChanges();
                }


            }
            catch (Exception ex)
            {


                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }



    }

}
