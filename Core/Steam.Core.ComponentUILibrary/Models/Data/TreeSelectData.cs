using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentUILibrary.Models
{
    public class TreeSelectData
    {
        public string value { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public List<TreeSelectData> children { get; set; } = new List<TreeSelectData>();
    }
}
