using Admin.WebSetting.Models;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Repository;


namespace Admin.WebSetting.Repository
{
    public interface IWebSettingRepository : IRepository<Database.WebsiteConfiguration>
    {


    }
}