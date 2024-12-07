

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steam.Core.Base.Models;
using System.Security.Claims;

namespace Steam.Core.Base
{
    public interface IIdentityService
    {
        public UserModel GetUser();

    }

}
