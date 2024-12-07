using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace Admin.Authorization.Models
{


    public class AuthorizationViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }
    public class Data
    {
        public int Pid { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        public string? Link { get; set; } = "";
        public string? Position { get; set; } = String.Empty;
        public double Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public string Lang { get; set; }

    }

    public class ModuleRoleDetail
    {
        //public List<Database.Authorization_Files> ListFiles { get; set; } = new List<Database.Authorization_Files>();
        public Database.ModuleRole Detail { get; set; } = new Database.ModuleRole();
    }

    public class GroupRoleDetail
    {
        //public List<Database.Authorization_Files> ListFiles { get; set; } = new List<Database.Authorization_Files>();
        public Database.GroupRole Detail { get; set; } = new Database.GroupRole();
    }

    public class ModuleRoleGroupDetail
    {
        //public List<Database.Authorization_Files> ListFiles { get; set; } = new List<Database.Authorization_Files>();
        public Database.Group_ModuleRole Detail { get; set; } = new Database.Group_ModuleRole();
    }
}
