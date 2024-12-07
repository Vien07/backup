using Steam.Core.FileManager.Helper.Model;
using Microsoft.AspNetCore.Http;

namespace Steam.Core.FileManager.Helper
{
    public interface IFileHelper
    {
        public ResponeSaveFile UploadImageModule(Steam.Core.FileManager.Model.UploadImageInfo imageInfo);
        void DeleteFile(string dirPath, string fileName);
        void CreateFolder(string pathFolder);
        ResponeSaveFile UploadImage(IFormFile file);
         string UploadImageNotResize(IFormFile file, string dirPath);
        public ResponeSaveFile UploadImagesBase64(Steam.Core.FileManager.Model.UploadImageBase64Info imageinfo);


    }
}