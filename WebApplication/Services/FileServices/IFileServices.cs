using Microsoft.AspNetCore.Http;
using DTO.Common;
namespace CMS.Services.FileServices
{
    public interface IFileServices
    {
        dynamic SaveFileAvatar(IFormFile file, string url, string title);
        dynamic SaveFile(IFormFile file, string url, string title);
        dynamic SaveFileNotRename(IFormFile file, string url, string title);
        dynamic SaveFileNoBackground(IFormFile file, string url, string title);
        dynamic SaveFileNotResize(IFormFile file, string url, string title);
        dynamic SaveFileNotResizeNotRename(IFormFile file, string url, string title);
        dynamic SaveFileNotResizeWidth(IFormFile file, string url, string title, int width);
        dynamic SaveFileNotResizeWidthNoBackground(IFormFile file, string url, string title, int width);
        dynamic SaveFileOriginal(IFormFile file, string url, string filename);
        dynamic SaveCategoryImage(IFormFile file, string url, string filename);
        dynamic UpdateFile(IFormFile file, string url, string title);
        dynamic DeleteFile(string url, string fileName);
        void ResizeThumbImage(IFormFile Images, string url, string title);
        void ResizeThumbImageNoBackground(IFormFile Images, string url, string title);
        dynamic WatermarkImage(string url, string fileName);
        dynamic WatermarkImage(string base64);
        dynamic SaveImagesBase64(ImagesModel listImages, string url, string title);
        dynamic SaveImagesBase64(string tempBase64, string url, string title);
        dynamic ConvertIformfileToBase64(IFormFile imagesFile);
        void SaveTxtFile(string url, string content);
        string CreateWatermarkImage(string originPath, string watermarkPath, string fileName);
        dynamic SaveFileWithWatermark(IFormFile file, string url, string title);
        void ResizeThumbImageWithWatermark(IFormFile Images, string url, string title);
        dynamic SaveImagesBase64WithWatermark(string tempBase64, string url, string title);
    }
}