using Microsoft.AspNetCore.Http;
using Steam.Core.Utilities.SteamModels;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface IFileHelper
    {
        public string UploadImageModule(IFormFile File, string SavePath, string SavePathThumb, int height, int width);
        ResponeSaveFile UploadImageModule(UploadImageInfo image);
        ResponeSaveFile UploadFileModule(UploadFileInfo image);
        void DeleteFile(string dirPath, string fileName);
        void CreateFolder(string pathFolder);
        ResponeSaveFile UploadImage(IFormFile file);
         string UploadImageNotResize(IFormFile file, string dirPath);
        public ResponeSaveFile UploadImagesBase64(UploadImageBase64Info imageinfo);
        public Task<string> UploadImageToServer( UploadImageInfo model);
        public Task<string> UploadImagesBase64ToServer(UploadImageBase64Info imageinfo);


    }
}