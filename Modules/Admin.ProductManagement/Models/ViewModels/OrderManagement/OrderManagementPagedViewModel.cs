using Admin.ProductManagement.DataTransferObjects.OrderManagement;
using Admin.ProductManagement.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.OrderManagement
{
    public class OrderManagementPagedViewModel
    {
        public List<OrderManagementDto> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
