using DTO.Website;
using System.Threading.Tasks;

namespace CMS.Services.WebsiteServices
{
    public interface IWebsiteServices
    {
        void StartUp();
        void SetVisitor();
        Task<VisitorDto> GetVisitor();
    }
}