using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Website
{
    public class VisitorDto
    {
        public int Online { get; set; }
        public int Total { get; set; }

        public string OnlineString { get; set; } = "0";
        public string TotalString { get; set; } = "0";
        public string BarChart { get; set; }
    }
}
