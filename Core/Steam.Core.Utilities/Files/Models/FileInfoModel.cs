namespace Steam.Core.Utilities.SteamModels
{
    public class FileInfoModel
    {
        public string? id { get; set; }

        public string? dataUrl { get; set; }
        public string? caption { get; set; }
        public string? description { get; set; }
        public int? order { get; set; }
        public string? name { get; set; }
        public string? size { get; set; }
        public string? status { get; set; }
        public string? alt { get; set; }
    }
    public class FileInfoModel1
    {
        public string name { get; set; }

        public string size { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
    }
    public class ResponeSaveFile
    {

        public string FileName { get; set; }
        public bool isError { get; set; }
        public string MessError { get; set; }
    }

}