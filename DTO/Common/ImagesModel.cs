using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Common
{
    public class ImagesModel
    {
        public string base64 { get; set; }
        public int Id { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string subDescription { get; set; }
    }
    public class Temp_MultiLang_Images
    {
        public string ImagesPid { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
        public string Pid { get; set; }
    }
    public class Temp_Images
    {
        public string Pid { get; set; }
        public string Status { get; set; }
        public string Images { get; set; }
        public int Order { get; set; }
    }
}
