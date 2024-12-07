using Admin.ProductManagement.DataTransferObjects.MisaApiTracker;

namespace Admin.ProductManagement.Models.ViewModels.MisaApiTracker
{
    public class MisaApiTrackerPagedViewModel
    {
        public List<MisaApiTrackerDto> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
