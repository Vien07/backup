
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Collection.Api.Models.Request
{
    //public class Request
    //{

    //    public string PostSlug { get; set; }=String.Empty;
    //}  
    public class GetListPostBySlug
    {

        public string RootSlug { get; set; }=String.Empty;
        public string? CateSlug { get; set; }=String.Empty;
        public int PageIndex { get; set; }=1;
        public int PageSize { get; set; }=10;
    }
}
