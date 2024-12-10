using CMS.Areas.Admin.Models;
using CMS.Areas.Contact.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
    {
        public void Configure(EntityTypeBuilder<ContactInfo> builder)
        {
            builder.HasData(
                 new ContactInfo { Pid = 1, Code = "contact-companyName", Type = "text" },
                 new ContactInfo { Pid = 2, Code = "contact-address", Type = "text" },
                 new ContactInfo { Pid = 3, Code = "contact-tel", Type = "text", isMultiLang = false },
                 new ContactInfo { Pid = 4, Code = "contact-fax", Type = "text", isMultiLang = false },
                 new ContactInfo { Pid = 5, Code = "contact-hotline", Type = "text", isMultiLang = false },
                 new ContactInfo { Pid = 6, Code = "contact-noted", Type = "text" },
                 new ContactInfo { Pid = 7, Code = "contact-map", Type = "text", isMultiLang = false },
                 new ContactInfo { Pid = 8, Code = "contact-email", Type = "text", isMultiLang = false },
                 new ContactInfo { Pid = 9, Code = "contact-time", Type = "text" },
                 new ContactInfo { Pid = 10, Code = "contact-intro", Type = "text" });
        }
    }
}
