
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class MisaBranch : BaseEntity
    {
        public Guid MisaBranchID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool IsBaseDepot { get; set; }
        public bool IsChainBranch { get; set; }
        public string ProvinceAddr { get; set; }
        public string DistrictAddr { get; set; }
        public string CommuneAddr { get; set; }
        public string Address { get; set; }
    }
}

