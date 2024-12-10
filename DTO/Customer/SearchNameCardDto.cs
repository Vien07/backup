using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class SearchNameCardDto
    {
        public string SearchKeyword { get; set; }
        public int PageIndex { get; set; }
        public string OrderId { get; set; }
    }
}
