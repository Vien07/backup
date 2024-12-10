using CMS.Areas.Contact.Models;
using DTO.Branch;
using System.Collections.Generic;

namespace CMS.Repository
{
    public interface IContact_Repository
    {
        dynamic GetContactInfo(string lang);
        bool SaveContact(ContactList data);
        bool SaveEnquire(EnquireList data);
        List<BranchDto> GetBranchList(string lang);
    }
}