using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.ProductSpecificaty
{
    public class ProductSpecificatyDto
    {
        public long Pid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
