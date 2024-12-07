
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class MisaApiTracker 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestParams { get; set; }
        public string ResponseStatusCode { get; set; }
        public string Response { get; set; }
        public DateTime RequestDate { get; set; }
    }
}

