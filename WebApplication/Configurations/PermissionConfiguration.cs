using CMS.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(
               new Permission { Code = "VIEW", Name = "view", Locked = false },
               new Permission { Code = "EDIT", Name = "edit", Locked = false },
               new Permission { Code = "DELETE", Name = "delete", Locked = false },
               new Permission { Code = "ADD", Name = "add", Locked = false },
               new Permission { Code = "ONLYSUBADMIN", Name = "onlysubadmin", Locked = true });
        }
    }
}
