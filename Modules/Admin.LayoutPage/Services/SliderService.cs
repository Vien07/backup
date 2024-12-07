
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
    public class SliderService : ISliderService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;

        private readonly IRepository<Database.Slider> _repSlider;
        private readonly IRepository<Database.SliderItem> _repSliderItems;
        private readonly IRepositoryConfig<Database.SliderConfig> _repSliderConfig;
        private readonly IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> _repWebsiteKeys;

        public SliderService(
            IRepository<Database.Slider> repSlider,
            IRepository<Database.SliderItem> repSliderItems,
            IRepositoryConfig<Database.SliderConfig> repSliderConfig,
            IRepository<Admin.WebsiteKeys.Database.WebsiteKeys> repWebsiteKeys,
            IFileHelper fileHelper, 
            ILoggerHelper logger)
        {
            _repSlider = repSlider;
            _repSliderItems = repSliderItems;
            _repSliderConfig = repSliderConfig;
            _repWebsiteKeys = repWebsiteKeys;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repSliderConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.Slider>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.Slider>> rs = new Response<IPagedList<Database.Slider>>();
            try
            {
                search.ToString();
                rs.Data = _repSlider.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.Name.Contains(search.KeySearch)))
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
        public Response<List<Database.SliderItem>> GetChildList(long? Pid)
        {
            Response<List<Database.SliderItem>> rs = new Response<List<Database.SliderItem>>();
            try
            {
                //search.ToString();
                rs.Data = _repSliderItems.Query().Where(p => p.Deleted == false && p.SliderPid == Pid)
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
        public Response<Database.Slider> GetById(int id)
        {
            Response<Database.Slider> rs = new Response<Database.Slider>();
            Database.Slider detail = new Database.Slider();
            try
            {

                detail = _repSlider.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.SliderItem> GetChildById(int id)
        {
            Response<Database.SliderItem> rs = new Response<Database.SliderItem>();
            Database.SliderItem detail = new Database.SliderItem();
            try
            {

                detail = _repSliderItems.Query().Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Database.Slider> Save(SliderModelEdit data)
        {
            //-------save-file-pond----------
            var img = "";

            //-------end-save-file-pond----------


            var validator = new SliderValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.Slider> rs = new Response<Database.Slider>();
            using (var transaction = _repSlider.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repSlider.Add(data);

                        _repSlider.SaveChanges();
                    }
                    else
                    {
                        var model = _repSlider.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Name = data.Name;
                            model.SliderBlock = data.SliderBlock;
                            _repSlider.SaveChanges();

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
        public Response<Database.SliderItem> SaveChild(SliderItemModelEdit input)
        {
            //-------save-file-pond----------
            var img = "";
            //var videolink = "";
            var filePath = "";
            SaveImage(ref img, ref filePath,
                       input.fileStatus,
                       input.files,
                       input.FilePath, input.Title);
            //-------end-save-file-pond----------


            var validator = new SliderItemValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(input);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Database.SliderItem> rs = new Response<Database.SliderItem>();
           var data = input.GetDatabaseModel();

            using (var transaction = _repSliderItems.BeginTransaction())
            {


                try
                {
                    if (img != "")
                    {
                        data.Images = img;

                    }
                    else
                    {
                        data.Images = "";
                    }
                    if (filePath != "")
                    {
                        data.FilePath = filePath;

                    }
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repSliderItems.Add(data);

                        _repSliderItems.SaveChanges();
                    }

                    else
                    {
                        data = _repSliderItems.Query().Where(p => p.Pid == input.Pid).FirstOrDefault();

                        if (data != null)
                        {
                            if (img != "")
                            {
                                data.Images = img;

                            }
                            else
                            {
                                data.Images = "";
                            }
                            if (filePath != "")
                            {
                                data.FilePath = filePath;

                            }
                            data.Title = input.Title;
                            data.Description = input.Description;
                            data.Link = input.Link;
                            data.VideoLink = input.VideoLink;
                            data.Position = input.Position;
                            data.Images_Alt = input.Images_Alt;
                            data.TypeMedia = input.TypeMedia;
                            data.ItemBlock = input.ItemBlock;

                            _repSliderItems.SaveChanges();

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
        public void SaveImage(ref string img, ref string imgFilePath, string filestatus, IFormFile file, string filePath, string title)
        {
            try
            {

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
                                     Height = Convert.ToInt32(_config[SliderConstants.Config.Admin.MaxHeight].ToString()),
                                     Width = Convert.ToInt32(_config[SliderConstants.Config.Admin.MaxWidth].ToString()),
                                     Path = SliderConstants.StaticPath.Asset.Image,
                                     PathThumb = SliderConstants.StaticPath.Asset.ImageThumb,
                                     File = file
                                 }
                                 ).FileName;
                            imgFilePath = SliderConstants.StaticPath.Asset.Image;
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

                    var model = _repSlider.Query().Where(p => p.Pid == id).FirstOrDefault();
                    //model.Deleted = true;
                    _repSlider.Remove(model);

                    //check and remove images
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.Image, model.Images);
                    //_fileHelper.DeleteFile(Constants.StaticPath.Asset.ImageThumb, model.Images);
                    //
                    //check and remove file
                    var menuItems = _repSliderItems.Query().Where(p => p.SliderPid == model.Pid).ToList();
                    if (menuItems != null)
                    {

                        _repSliderItems.RemoveRange(menuItems);

                    }


                    //

                    _repSliderItems.SaveChanges();
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

                    var model =_repSliderItems.Query().Where(p => p.Pid == id).FirstOrDefault();
                    _repSliderItems.Remove(model);


                    _repSliderItems.SaveChanges();
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
                var fromModel = _repSliderItems.Query().Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _repSliderItems.Query().Where(p => p.Pid == toId).FirstOrDefault();

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

                    _repSliderItems.SaveChanges();
                    var list = _repSliderItems.Query().OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _repSliderItems.SaveChanges();
                    }

                }
                //var list = _db.Sliders.OrderBy(p => p.Order).ToList();
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
        public Response<string> GenerateSliderHtml(long pid)
        {
            Response<string> rs = new Response<string>();
            try
            {

                SliderHelper _helper = new SliderHelper();
                var slider = _repSlider.Query().Where(p => p.Pid == pid).FirstOrDefault();
                var sliderItem = _repSliderItems.Query().Where(p => p.SliderPid == slider.Pid).OrderBy(p => p.Order).ToList();
                var listWebsiteKey = _repWebsiteKeys.Query().Where(p => p.isSystemKey == false).ToList();

                rs = _helper.GenerateSlider(slider, sliderItem, listWebsiteKey);
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
