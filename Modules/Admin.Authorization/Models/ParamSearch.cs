
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Authorization.Models
{
    public class ParamSearch
    {

        public string KeySearch { get; set; }=String.Empty;
        public string FromDate { get; set; } = DateTime.Now.AddMonths(-1).ToString("dd/M/yyyy");
        public string ToDate { get; set; } = DateTime.Now.ToString("dd/M/yyyy");
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public int PageIndex { get; set; } = 1;
    }
}
