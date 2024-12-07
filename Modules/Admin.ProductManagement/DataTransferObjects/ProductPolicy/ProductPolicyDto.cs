using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.ProductPolicy
{
    public class ProductPolicyDto
    {
        public long Pid { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }
        public string? Content { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
