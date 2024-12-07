
using Microsoft.AspNetCore.Http;
using Admin.Slider.Database;
using Admin.Slider.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.Slider.Constants;
using Steam.Core.Common.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Common.SteamModels;
using Steam.Core.Base.Constant;

namespace Admin.Slider
{
    public class SliderRepository : ISliderRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        SliderContext _db;
        public SliderRepository(SliderContext db, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.SliderConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public Response<IPagedList<Database.Slider>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.Slider>> rs = new Response<IPagedList<Database.Slider>>();
            try
            {
                search.ToString();
                rs.Data = _db.Sliders.Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Title.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[SliderConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<SliderDetail> GetById(int id)
        {
            Response<SliderDetail> rs = new Response<SliderDetail>();
            SliderDetail detail = new SliderDetail();
            try
            {

                detail.Detail = _db.Sliders.Where(p => p.Pid == id).FirstOrDefault();

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
        public Response<Slider.Database.Slider> Save(SliderModelEdit input)
        {
            Response<Slider.Database.Slider> rs = new Response<Slider.Database.Slider>();
            Database.Slider modelResponse = new Database.Slider();
            try
            {
                //-------save-file-pond----------
                var img = "";
                var filePath = "";
                if (input.fileStatus != "existed")
                {
                    if (input.files != null)
                    {
                        if (input.FilePath != null && input.FilePath != "")
                        {
                            var arrFile = input.FilePath.Split('/');
                            img = arrFile[arrFile.Length - 1];
                            filePath = SystemInfo.PathFileManager + "/" + input.FilePath.Replace(img, "");
                        }
                        else
                        {
                            img = _fileHelper.UploadImageModule(
                             new UploadImageInfo
                             {
                                 FileName = input.Title.ToSlug(),
                                 Height = Convert.ToInt32(_config[SliderConstants.Config.Admin.MaxHeight].ToString()),
                                 Width = Convert.ToInt32(_config[SliderConstants.Config.Admin.MaxWidth].ToString()),
                                 ThumbHeight = Convert.ToInt32(_config[SliderConstants.Config.Admin.ThumbHeight].ToString()),
                                 ThumbWidth = Convert.ToInt32(_config[SliderConstants.Config.Admin.ThumbWidth].ToString()),
                                 Path = SliderConstants.StaticPath.Asset.Image,
                                 PathThumb = SliderConstants.StaticPath.Asset.ImageThumb,
                                 File = input.files
                             }
                             ).FileName;
                            filePath = SliderConstants.StaticPath.Asset.Image;
                        }
                    }
                    //-------end-save-file-pond----------

                }
                var validator = new SliderValidator();

                // Execute the validator
                ValidationResult results = validator.Validate(input);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                if (!success)
                {
                    string mess = string.Join(";", results.Errors);

                    rs.Message = mess;
                    rs.isError = true;
                    return rs;
                }

                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        modelResponse = input.GetDatabaseModel();
                        if (img != "")
                        {
                            modelResponse.Images = img;

                        }
                        if (filePath != "")
                        {
                            modelResponse.FilePath = filePath;

                        }
                        if (modelResponse.Pid == 0)
                        {
                            modelResponse.Order = 0.9;
                            modelResponse.Images = img;

                            _db.Sliders.Add(modelResponse);

                            _db.SaveChanges();
                        }
                        else
                        {
                            modelResponse = _db.Sliders.Where(p => p.Pid == input.Pid).FirstOrDefault();

                            if (modelResponse != null)
                            {
                                if (img != "")
                                {
                                    modelResponse.Images = img;

                                }
                                if (filePath != "")
                                {
                                    modelResponse.FilePath = filePath;

                                }
                                modelResponse.Title = input.Title;
                                modelResponse.Description = input.Description;
                                modelResponse.Link = input.Link;
                                modelResponse.Position = input.Position;
                                modelResponse.Images_Alt = input.Images_Alt;
                                modelResponse.TypeMedia = input.TypeMedia;
                                //if (files != null && files.Count > 0)
                                //{
                                //    _fileHelper.DeleteFile(SliderConstants.StaticPath.Asset.Image, model.Images);
                                //    _fileHelper.DeleteFile(SliderConstants.StaticPath.Asset.ImageThumb, model.Images);
                                //    model.Images = img;
                                //}
                                _db.SaveChanges();

                            }


                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        rs.isError = true;
                        rs.Message = ex.Message;
                        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, modelResponse.ToJson());

                    }
                }
                rs.Data = modelResponse;
            }
            catch (Exception ex)
            {
                rs.isError = true;
                rs.Message = ex.Message;
            }

            return rs;

        }
        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _db.Sliders.Where(p => p.Pid == id).FirstOrDefault();
                    _db.Sliders.Remove(model);
                    //check and remove images


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
                    var model = _db.Sliders.Where(p => p.Pid == id).FirstOrDefault();
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
                var list = _db.Sliders.OrderBy(p => p.Order).ToList();
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
                var model = _db.Sliders.Where(p => p.Pid == id).FirstOrDefault();
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
                var fromModel = _db.Sliders.Where(p => p.Pid == fromId).FirstOrDefault();
                var toModel = _db.Sliders.Where(p => p.Pid == toId).FirstOrDefault();

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
                    var list = _db.Sliders.OrderBy(p => p.Order).ToList();
                    var order = 1;
                    foreach (var item in list)
                    {
                        item.Order = order;
                        order = order + 1;
                        _db.SaveChanges();
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
                rs.isError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, fromId.ToString() + "-" + toId.ToString());
            }
            return rs;

        }
        public Response<List<Slider.Database.SliderConfig>> SaveConfig(IFormCollection formData, string tab)
        {
            Response<List<Slider.Database.SliderConfig>> rs = new Response<List<Slider.Database.SliderConfig>>();
            try
            {

                foreach (var item in formData)
                {


                    var key = item.Key;
                    var value = item.Value;
                    Slider.Database.SliderConfig sampleConfig = _db.SliderConfigs.Where(p => p.Key == key).FirstOrDefault();
                    if (sampleConfig != null)
                    {
                        sampleConfig.Type = tab;
                        sampleConfig.Value = value;
                        sampleConfig.UpdateDate = DateTime.Now;
                        sampleConfig.UpdateUser = "";

                    }
                    else
                    {
                        sampleConfig = new Slider.Database.SliderConfig();
                        sampleConfig.Type = tab;

                        sampleConfig.Key = key;
                        sampleConfig.Group = "";
                        sampleConfig.Value = value;
                        sampleConfig.CreateDate = DateTime.Now;
                        sampleConfig.CreateUser = "";
                        sampleConfig.UpdateDate = DateTime.Now;
                        sampleConfig.UpdateUser = "";
                        _db.SliderConfigs.Add(sampleConfig);
                    }
                    _db.SaveChanges();
                }
                var listConfig = _db.SliderConfigs.ToList();
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
        public Response<List<Slider.Database.SliderConfig>> GetAllConfigs()
        {
            Response<List<Slider.Database.SliderConfig>> rs = new Response<List<Slider.Database.SliderConfig>>();
            try
            {

                var listConfig = _db.SliderConfigs.ToList();
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
        public Response<Slider.Database.SliderConfig> GetConfigByKey(string key)
        {
            Response<Slider.Database.SliderConfig> rs = new Response<Slider.Database.SliderConfig>();
            try
            {

                var config = _db.SliderConfigs.Where(p => p.Key == key).FirstOrDefault();
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
