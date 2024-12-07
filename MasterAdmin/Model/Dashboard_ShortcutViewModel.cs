
using Admin.DashBoard.Database;
using Admin.PostsManagement.Constants;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MasterAdmin.Models.Dashboard_ShortcutViewModel
{
    public class ShortCutSaveModel : Dashboard_Shortcut
    {

        public Dashboard_Shortcut GetDatabaseModel()
        {

            Dashboard_Shortcut rs = new Dashboard_Shortcut();

            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Name = this.Name;
            rs.IconSvg = this.IconSvg;

            rs.Description = this.Description;
            rs.Link = this.Link;            

            rs.CreateDate = DateTime.Now;
            rs.UpdateDate = DateTime.Now;
            rs.Enabled = true;

            return rs;
        }

    }
}
