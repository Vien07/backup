using Admin.Authorization.Database;
using System.IdentityModel.Tokens.Jwt;

using Steam.Core.Base.Constant;
using Devsense.PHP.Syntax;
using System.Security.Claims;

namespace MasterAdmin
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, AuthorizationContext _authordb)
        {
            try
            {

                //_authordb = db;

               string token = httpContext.Request.Cookies["auth-token"];
                string Path = httpContext.Request.Path.ToString();
                if (Path != null)
                {
                    //check public api
                    if (httpContext.Request.Path.ToString().Contains("/api/"))
                    {
                        await _next(httpContext);
                        return;
                    }
                    // Check static file
                    var ext = System.IO.Path.GetExtension(httpContext.Request.Path);
                    if (string.IsNullOrEmpty(System.IO.Path.GetExtension(httpContext.Request.Path)) || System.IO.Path.GetExtension(httpContext.Request.Path) == ".php")
                    {
                        var splitRequestPath = Path.Split('/');
                        // Check has token
                        if (token != null)
                        {
                            // Check RequestPath
                            if (Path != "/" && Path.ToLower() != SystemInfo.VirtualFolder + "/dashboard" && splitRequestPath[1].ToLower() != "login")
                            {

                                string rolePath = "";
                                //return;
                                // Check RequestPath if contain more than 2 parameter after split
                                if (splitRequestPath.Length > 2)
                                {
                                    rolePath = "/" + splitRequestPath[1];
                                }
                                else
                                {
                                    rolePath = httpContext.Request.Path;
                                }
                                var user = ConvertTokenToUser(token, _authordb);
                                var claims = new List<Claim>
                                {
                                   new Claim(ClaimTypes.Name, user.Name), 
                                     new Claim(ClaimTypes.Role, user.Role),   
                                        };

                                var identity = new ClaimsIdentity(claims, "UserPermission");

                                httpContext.User = new ClaimsPrincipal(identity);
                      
                                var Authorized = CheckPermissionRolePath(ref _authordb, user.Pid, rolePath);
                                if (!Authorized)
                                {
                                    httpContext.Request.Path = SystemInfo.VirtualFolder + "/Account/NoPermission";
                                    await _next(httpContext);
                                    return;
                                }
                                else
                                {

                                }
;                                //var resultAuthorizedParameter = new SqlParameter()
                                //{
                                //    ParameterName = "authorized",
                                //    SqlDbType = System.Data.SqlDbType.Bit,
                                //    Direction = System.Data.ParameterDirection.Output
                                //};
                                //_authordb.Database.ExecuteSqlRaw("steam.[001_CheckPermissionRolePath] @IdUser, @RolePath, @authorized OUTPUT",
                                //    new SqlParameter("@IdUser", user.Pid),
                                //    new SqlParameter("@RolePath", rolePath),
                                //    resultAuthorizedParameter);

                                //if (!(bool)resultAuthorizedParameter.Value)
                                //{
                                //    httpContext.Request.Path = "/Account/NoPermission";
                                //    await _next(httpContext);
                                //    return;
                                //}
                                WriteLog(ref _authordb, Path, user.UserName);

                            }


                        }
                        else
                        {
                            if (splitRequestPath[1].ToUpper() != "LOGIN")
                            {
                                string directPath = SystemInfo.VirtualFolder + "/Login/index?redirectUrl=" + Path.Replace("/", "%2F");
                                httpContext.Request.Path = directPath;
                                httpContext.Response.Redirect(directPath);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (token != null)
                    {
                        httpContext.Items["user"] = ConvertTokenToUser(token, _authordb);
                    }
                    else
                    {
                        httpContext.Request.Path = SystemInfo.VirtualFolder + "/Login";
                    }

                }
            }
            catch (Exception ex)
            {
                httpContext.Request.Path = SystemInfo.VirtualFolder + "/Login";

            }
            await _next(httpContext);
        }
        public void WriteLog(ref AuthorizationContext _authordb, string path, string user)
        {
            try
            {
                var splitRequestPath = path.Split('/');
                var rolePath = "";
                if (splitRequestPath.Count() > 2)
                {
                    rolePath = String.Format("/{0}/{1}", splitRequestPath[1], splitRequestPath[2]);

                }
                var checkModule = _authordb.ModuleRoles.Where(p => p.RolePath == rolePath).FirstOrDefault();
                if (checkModule != null && checkModule.Log == true)
                {
                    LogManagement log = new LogManagement();
                    log.ActionData = "";
                    log.ActionName = checkModule.ActionName;
                    log.ActionUrl = path;
                    log.CreateUser = user;
                    log.CreateDate = DateTime.Now;
                    log.UpdateUser = user;
                    log.UpdateDate = DateTime.Now;
                    _authordb.LogManagements.Add(log);
                    _authordb.SaveChanges();
                }
            }
            catch (Exception ex)
            {


            }
        }
        public bool CheckPermissionRolePath(ref AuthorizationContext _authordb, long idUser, string rolePath)
        {

            var isExistRolePath = _authordb.ModuleRoles.Count(mr => mr.RolePath == rolePath && !mr.Deleted);

            if (isExistRolePath > 0)
            {
                var isAllowAnonymous = _authordb.ModuleRoles
                    .Where(mr => mr.RolePath == rolePath && !mr.Deleted)
                    .Select(mr => mr.AllowAnonymous)
                    .FirstOrDefault();

                if (isAllowAnonymous == true)
                {
                    return true;// cho phép truy cập nhất cấu hình là isAllowAnonymous
                }
                else
                {
                    //var a = _authordb.User_Groups
                    //  .Join(_authordb.ModuleRoleGroups,
                    //        usg => usg.Id_GroupRole,
                    //        mdrg => mdrg.Id_GroupRole,
                    //        (usg, mdrg) => new { UserGroup = usg, ModuleRoleGroup = mdrg });
                    var countRole = _authordb.User_Groups
                        .Join(_authordb.ModuleRoleGroups,
                              usg => usg.Id_GroupRole,
                              mdrg => mdrg.Id_GroupRole,
                              (usg, mdrg) => new { UserGroup = usg, ModuleRoleGroup = mdrg })
                        .Join(_authordb.ModuleRoles,
                              mrg => mrg.ModuleRoleGroup.Id_ModuleRole,
                              mdr => mdr.Pid,
                              (mrg, mdr) => new { mrg.UserGroup, mrg.ModuleRoleGroup, ModuleRole = mdr })
                        .Count(joined => joined.UserGroup.Id_User == idUser && !joined.ModuleRole.Deleted && joined.ModuleRole.RolePath == rolePath);

                    if (countRole > 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

            return false;

        }

        private Admin.Authorization.Database.User ConvertTokenToUser(string token, AuthorizationContext _authordb)
        {
            try
            {
                if (token.StartsWith("Bearer"))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                var handler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwt = handler.ReadJwtToken(token);

                var claims = new Dictionary<string, string>();

                foreach (var claim in jwt.Claims)
                {
                    claims.Add(claim.Type, claim.Value);
                }
                string UserNames = claims["unique_name"].ToString();
                Admin.Authorization.Database.User user = _authordb.Users.Where(s => s.UserName == UserNames && s.Deleted == false).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<AuthorizeMiddleware>();
        }
    }
}
