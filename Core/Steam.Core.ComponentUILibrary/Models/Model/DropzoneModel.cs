using Microsoft.AspNetCore.Http;
using Steam.Core.Utilities.SteamModels;
using System.Text.Json;
using static ComponentUILibrary.Models.FileUploadControlModel;

namespace ComponentUILibrary.Models
{
    public class DropzoneModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; }=string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Files { get; set; } = string.Empty;
        public bool IsMultiple { get; set; } = false;
        public bool FirstLoadLib { get; set; } = false;

        public class ListFiles
        {
            public string Status { get; set; }
            public List<File> Files { get; set; }
        }
        public class File
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public string Status { get; set; }
            public long Pid { get; set; }
            public FileInfoModel FileInfo { get; set; }
        }
        public class SaveImageModel
        {
            public string UploadThumbPath { get; set; }
            public string UploadPath { get; set; }
            public string ImageName { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }
    }

}