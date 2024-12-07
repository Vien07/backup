using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using X.PagedList;

namespace Admin.Authorization.Models
{

    public class AccountModelEdit : Database.User
    {

        public IFormFile file { get; set; }
        public string GroupRoleID { get; set; }
        public Database.User GetDatabaseModel()
        {
            Database.User rs = new Database.User();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.UserName = this.UserName;
            rs.Name = this.Name;
            rs.Role = this.Role;
            rs.IsActive = this.IsActive;
            rs.Token = this.Token;
            rs.Password = this.Password;
            rs.LastIPLogin = this.LastIPLogin;
            rs.LastLogin = this.LastLogin;
            rs.TokenUpdate = this.TokenUpdate;
            rs.LasTimeChangePassword = this.LasTimeChangePassword;
            rs.Image = this.Image;
            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }
    }

}
