namespace Admin.ProductManagement.Models.SearchModels
{
    public class MisaApiTrackerSearchModel
    {
        public string KeySearch { get; set; } = string.Empty;
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public int PageIndex { get; set; } = 1;
    }
}
