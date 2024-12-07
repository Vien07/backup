
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;


namespace Admin.HeaderPage.Database
{
    public class MenuConfig:BaseDatabaseModel
    {


        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }



    }
}
