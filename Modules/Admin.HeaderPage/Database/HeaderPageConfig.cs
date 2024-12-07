
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.HeaderPage.Database
{
    public class HeaderPageConfig:BaseDatabaseModel
    {


        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }



    }
}
