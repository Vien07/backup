using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.ProductPolicy
{
    public class ProductPolicyWithGroupViewModel
    {
        public string Pid { get; set; }
        public string Title { get; set; }
        public string ParentCode { get; set; }
    }
}
