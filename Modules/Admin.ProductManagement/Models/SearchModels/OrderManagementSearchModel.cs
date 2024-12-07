using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SearchModels
{
    public class OrderManagementSearchModel
    {
        public string KeySearch { get; set; } = String.Empty;
        public string FromDate { get; set; } = DateTime.Now.AddMonths(-1).ToString("dd/M/yyyy");
        public string ToDate { get; set; } = DateTime.Now.ToString("dd/M/yyyy");
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public int PageIndex { get; set; } = 1;
    }
}
