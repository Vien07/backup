using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Steam.Core.Base.Models
{

    public class PagedListDto<T>
    {
        public List<T> Items { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }

    } 


}