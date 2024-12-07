using Admin.ProductManagement.DataTransferObjects.MisaApiTracker;

namespace Admin.ProductManagement.Models.ViewModels.MisaApiTracker
{
    public class MisaApiConfigViewModel
    {
        public string SignatureInfo { get; set; }
        public string CompanyCode { get; set; }
        public string Environment { get; set; }
        public string LoginTime { get; set; }
        public string AccessToken { get; set; }
        public List<MisaApiConfigDto> Items { get; set; }
    }
}
