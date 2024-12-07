using Admin.ProductManagement.DataTransferObjects.OrderManagement;
using Admin.ProductManagement.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.OrderManagement
{
    public class OrderManagementConfigViewModel
    {
        public List<OrderManagementConfigDto> Items { get; set; }
        public OrderManagementConfigDto Item { get; set; }
    }
}
