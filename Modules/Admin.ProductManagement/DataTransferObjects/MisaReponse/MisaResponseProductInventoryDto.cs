using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaReponse
{
    public class MisaResponseProductInventoryDto
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal OnHand { get; set; }
        public decimal Ordered { get; set; }
        public decimal PreOrdered { get; set; }
    }
}
