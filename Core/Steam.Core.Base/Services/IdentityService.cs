

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steam.Core.Base.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Steam.Core.Base
{
    public class IdentityService: IIdentityService
    {
        IHttpContextAccessor _httpContext;

        public IdentityService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        //public UserModel GetUser()
        //{
        //    UserModel rs = new UserModel();
        //    try
        //    {
        //        var user = _httpContext.HttpContext.User;

        //        if (user.Identity.IsAuthenticated)
        //        {
        //            var userName = user.Identity.Name;
        //            rs.UserName = userName;

        //            var roleClaim = user.FindFirst(ClaimTypes.Role);
        //            if (roleClaim != null)
        //            {
        //                var userRole = roleClaim.Value;
        //                rs.UserRole = userRole;

        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

                
        //    }
        //    return rs;
        //}
        public UserModel GetUser()
        {
            UserModel rs = new UserModel();
            try
            {
                var cookieValue = _httpContext.HttpContext.Request.Cookies["auth-token"];
                string UserName = "Unknown";
                string Role = "Unknown";
                string DisplayName = "Unknown";
                rs.DisplayName = "Unknown";
                rs.UserRole = "Unknown";
                rs.UserName = "Unknown";
                if (cookieValue != null)
                {
                    var handler = new JwtSecurityTokenHandler();

                    JwtSecurityToken jwt = handler.ReadJwtToken(cookieValue);

                    var claims = new Dictionary<string, string>();

                    foreach (var claim in jwt.Claims)
                    {
                        claims.Add(claim.Type, claim.Value);
                    }
                    //DisplayName = claims["given_name"].ToString();
                    //Role = claims["role"].ToString();
                    //UserName = claims["unique_name"].ToString();
                    //DisplayName = claims["role"].ToString();
                    rs.DisplayName = claims["given_name"].ToString();
                    rs.UserRole = claims["role"].ToString();
                    rs.UserName = claims["unique_name"].ToString();
                }


            }
            catch (Exception)
            {


            }
            return rs;
        }
    }

}
