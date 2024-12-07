using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ComponentUILibrary.Models
{
    public class FileUploadControlModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string FileUrl { get; set; }
        public string Images_Description { get; set; }
        public string Images_Caption { get; set; }
        public string Images_Alt { get; set; }
        public string SelectedFiles { get; set; }
        public string MaxFileSize { get; set; } = "5MB";
        public string AcceptImage { get; set; } = "image/*";
        public string AcceptFile { get; set; } = "";
        public bool IsMultiple { get; set; } = false;
        public bool AcceptVideo { get; set; } = false;
        public bool FirstLoadLib { get; set; } = false;
        public bool ChooseFromServer { get; set; } = true;


        public string GenerateFileJson(List<string> listFiles)
        {
            try
            {
                var rs = new List<Files>();
                foreach (var file in listFiles)
                {
                    var options = new
                    {
                        type = @"remote",
                        file = new
                        {
                            name = file

                        },
                        metadata = new
                        {
                            poster = file
                        }

                    };
                    rs.Add(new Files { source = "filePond", options = options });
                }


                var jsonString = JsonSerializer.Serialize(rs);

                return JsonSerializer.Serialize(jsonString);

            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
        public class Files
        {
            public string source { get; set; }
            public dynamic options { get; set; }
        }
        public class SaveImageModel
        {
            public string Filestatus { get; set; }
            public IFormFile File { get; set; }
            public string FilePath { get; set; }
            public string UploadThumbPath { get; set; }
            public string UploadPath { get; set; }
            public string PathThumb { get; set; }
            public string Title { get; set; }
            public string ImageName { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
            public bool IsExisted { get; set; } = false;
        }

    }

}