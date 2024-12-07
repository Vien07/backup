using Microsoft.AspNetCore.Http;

namespace Steam.Core.Utilities.SteamModels
{
    public class UploadImageInfo
    {
        public int Height { get; set; }

        public int Width { get; set; }
        public int ThumbHeight { get; set; }
        public int ThumbWidth { get; set; }
        public string Path { get; set; }
        public string PathThumb { get; set; }
        public string FileName { get; set; }
        public string Absolutepath { get; set; } = Directory.GetCurrentDirectory();
        public string VitrualFolder { get; set; } = "";
        public IFormFile File { get; set; }
    }
    public class UploadImageBase64Info
    {
        public int Height { get; set; }

        public int Width { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string  Base64 { get; set; }
        public string PathThumb { get; set; }

    }
    public class UploadFileInfo
    {

        public string Path { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }

}