using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaReponse
{
    public class MisaResponseBranchDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsBaseDepot { get; set; }
        public bool IsChainBranch { get; set; }
        public string ProvinceAddr { get; set; }
        public string DistrictAddr { get; set; }
        public string CommuneAddr { get; set; }
        public string Address { get; set; }
    }
}
