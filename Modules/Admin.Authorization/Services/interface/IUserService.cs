using Steam.Core.Base.Models;

namespace Admin.Authorization.Services
{
    public interface IUserService
    {
        public Response<Admin.Authorization.Database.User> Login(string email, string password);
        public Response<Admin.Authorization.Database.User> Register(Admin.Authorization.Database.User user);
    }
}