using Microsoft.AspNetCore.Http;
using System.IO;

namespace Steam.Core.FileManager.Helper.Model
{
    public class UploadImageInfo
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public string Path { get; set; }
        public string PathThumb { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
    public class UploadImageBase64Info
    {
        public int Height { get; set; }

        public int Width { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string  Base64 { get; set; }
    }
    public class UploadFileInfo
    {

        public string Path { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }

}