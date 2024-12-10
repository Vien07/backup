using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Areas.Contact.Models;
using DTO.About;

namespace CMS.Repository
{
    public interface IAbout_Repository
    {
        Task<AboutDto> GetAbout(string slug, string lang);
        Task<string> GetSlugDefault(string lang);
        AboutDto GetAboutPreview();
    }
}