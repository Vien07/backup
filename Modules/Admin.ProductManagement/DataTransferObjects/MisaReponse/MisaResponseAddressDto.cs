using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaReponse
{
    public class MisaResponseAddressDto
    {
        public string Id { get; set; }
        public int Kind { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}
