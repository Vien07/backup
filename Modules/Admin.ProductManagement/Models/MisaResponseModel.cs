using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DTOs
{
    public class MisaResponseModel<TModel> where TModel : class
    {
        public int Code { get; set; }
        public TModel Data { get; set; }
        public decimal Total { get; set; }
        public bool Success { get; set; }
        public string Environment { get; set; }
    }
}
