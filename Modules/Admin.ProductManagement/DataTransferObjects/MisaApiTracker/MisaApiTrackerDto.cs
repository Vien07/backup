using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaApiTracker
{
    public class MisaApiTrackerDto
    {
        public int Pid { get; set; }
        public string? Name { get; set; }
        public string? Endpoint { get; set; }
        public string? Method { get; set; }
        public string? RequestHeaders { get; set; }
        public string? RequestParams { get; set; }
        public string? ResponseStatusCode { get; set; }
        public string? Response { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
