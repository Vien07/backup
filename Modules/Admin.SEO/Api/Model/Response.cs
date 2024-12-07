
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.SEO.Api.Models
{
    public class Response_GetAllListSEOActive
    {

        public string PostSlug { get; set; }
        public string CateSlug { get; set; } 
        public long PostPid { get; set; } 
        public long? CatePid { get; set; }
    }
}
