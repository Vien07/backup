

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Steam.Core.Base
{
    public class MultiLangService
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<DbContextOptions<DbContext>> _dbContextOptions;

        public MultiLangService(IConfiguration configuration, IOptionsMonitor<DbContextOptions<DbContext>> dbContextOptions)
        {
            _configuration = configuration;
            _dbContextOptions = dbContextOptions;
        }

        public void ChangeConnectionString(string newConnectionString)
        {
            var options = _dbContextOptions.Get("PostsManagementContext");
            var builder = new DbContextOptionsBuilder<DbContext>(options);
            builder.UseSqlServer(newConnectionString);

            // Apply the new options to the DbContext
            _dbContextOptions.OnChange((newOptions, _) =>
            {
                options = newOptions;
            });
        }

    }

}
