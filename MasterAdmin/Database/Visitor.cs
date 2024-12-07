
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.DashBoard.Database
{
    public class Visitor
    {
        public int Id { get; set; }
        public string RequestId { get; set; }
        public string IpAddress { get; set; }
        public string DeviceName { get; set; }
        public string Browser { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}

