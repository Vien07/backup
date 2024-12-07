
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.HeaderPage.Database
{
    public class Menu_Files: BaseDatabaseModel
    {

        public long MenuId { get; set; }

        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
