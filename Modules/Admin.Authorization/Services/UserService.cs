using Microsoft.Extensions.Configuration;
using Admin.Authorization.Database;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Admin.Authorization.Services
{
    using BCrypt.Net;
    using Steam.Core.Base.Models;
    using Steam.Core.Utilities.STeamHelper;
    using Steam.Infrastructure.Repository;

    public class UserService : IUserService
    {
        private ILoggerHelper _logger;
        public readonly IConfiguration configuration;
        private readonly IRepository<User> _repUser;

        AuthorizationContext _db;
        public UserService(
           IRepository<User> repUser,
            ILoggerHelper logger)
        {
            _repUser = repUser;
            _logger = logger;
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());

        }
        public  Response<Admin.Authorization.Database.User> Login(string email, string password)
        {
            Response<Admin.Authorization.Database.User> rs = new Response<Admin.Authorization.Database.User>();

            try
            {
                Admin.Authorization.Database.User user = _repUser.Query().Where(s => s.UserName == email && s.Deleted == false).FirstOrDefault();
                //var Password = BCrypt.HashPassword(password);
                if (user == null || BCrypt.Verify(password, user.Password) == false)
                {
                    rs.StatusCode = 0;
                    rs.IsError = true;
                    rs.Message = "Tài khoản hoặc mật khẩu không đúng!";
                    return rs;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["JWT:SecretKey"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                    }),
                    IssuedAt = DateTime.Now,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                //user.IsActive = true;
                _repUser.SaveChanges();
                rs.StatusCode = 1;
                rs.IsError = false;
                rs.Data = user;
            }
            catch (Exception)
            {
                rs.StatusCode = -1;
                rs.IsError= true;
                rs.Message = "Lỗi không xác định!";

            }

            return rs;

        }

        public  Response<Admin.Authorization.Database.User> Register(Admin.Authorization.Database.User user)
        {
            Response<Admin.Authorization.Database.User> rs = new Response<Admin.Authorization.Database.User>();

            user.Password = BCrypt.HashPassword(user.Password);
            _repUser.Add(user);
            _repUser.SaveChanges();

            return rs;
        }

    }

}
