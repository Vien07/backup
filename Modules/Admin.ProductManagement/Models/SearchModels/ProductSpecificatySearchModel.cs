using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SearchModels
{
    public class ProductSpecificatySearchModel
    {
        public string KeySearch { get; set; } = String.Empty;
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public string Group { get; set; } = "";
        public int PageIndex { get; set; } = 1;
    }
}
