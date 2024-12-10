using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class PromoProduct
    {
        public long Pid { get; set; }
        public string PicThumb { get; set; }
        public string Title { get; set; }
        public decimal PriceOrigin { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public List<PromoProductOption> Options { get; set; }
    }

    public class PromoProductOption
    {
        public long OptionPid { get; set; }
        public string OptionCode { get; set; }
        public decimal PriceOrigin { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
}
