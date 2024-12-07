using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steam.Core.FileManager.Helper;
using Steam.Core.FileManager.Model;

namespace Steam.Core.FileManager.API.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiFileManagerController : ControllerBase
    {
        public AdminLogin adminLogin { get; }
        IFileHelper _fileHelper;
        public ApiFileManagerController(IOptions<AdminLogin> adminLogin, IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }
        [HttpGet("GetToken")]
        //[CustomAuthorization]
        public string GetToken(string u, string p)
        {
            try
            {



            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost("UploadImage")]
        public async Task<string> UploadImage([FromForm] UploadImageInfo model)
        {
            try
            {
                var Absolutepath = Directory.GetCurrentDirectory();
                if (model == null || model.File == null || model.File.Length == 0)
                {
                    return string.Empty;
                }
                var fileName = _fileHelper.UploadImageModule(model).FileName;
                //string fileName = $"{Guid.NewGuid().ToString()}_{model.File.FileName}";
                //string filePath = Path.Combine(Absolutepath, model.Path, fileName);

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await model.File.CopyToAsync(stream);
                //}


                //string thumbFileName = $"thumb_{fileName}";
                //string thumbFilePath = Path.Combine(Absolutepath, model.PathThumb, thumbFileName);


                //System.IO.File.Copy(filePath, thumbFilePath);

                // Update the model with file paths
                //model.Path = filePath;
                //model.PathThumb = thumbFilePath;

                return fileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        [HttpPost("UploadImageBase64")]
        public async Task<string> UploadImageBase64(UploadImageBase64Info model)
        {
            try
            {

                var fileName = _fileHelper.UploadImagesBase64(model).FileName;


                return fileName.Trim('"');
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}