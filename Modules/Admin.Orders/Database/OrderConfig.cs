
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Orders.Database
{
    public class OrderConfig : BaseEntity
    {
        public string Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Group { get; set; }
    }
}
