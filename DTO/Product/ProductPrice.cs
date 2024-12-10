using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductPrice
    {
        public int Pid { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Enable { get; set; } = false;
    }
}
