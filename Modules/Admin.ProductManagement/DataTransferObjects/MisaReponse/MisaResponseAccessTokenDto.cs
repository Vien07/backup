using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaReponse
{
    public class MisaResponseAccessTokenDto
    {
        public string Domain { get; set; }
        public string AppID { get; set; }
        public string AccessToken { get; set; }
        public string CompanyCode { get; set; }
        public string Environment { get; set; }
    }
}
