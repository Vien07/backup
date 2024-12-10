using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Common
{
    public class ResponseDto
    {
        public bool isError { get; set; }
        public string statusError { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }
}
