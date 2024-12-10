using CMS.Areas.Admin.Models;
using CMS.Areas.Configurations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CMS.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Pid = 1,
                Code = "bizmac",
                FirstName = "bizmac",
                LastName = "ecommerce ",
                FullName = "bizmac ecommerce",
                Email = "info@bizmac.com.vn",
                Password = "559F52E363C3AA964CBD64481E3E7BDC6E99A9A127BCCD8E010685AC31B2E949",
                GroupUserCode = 1,
                Avatar = "",
                Order = 1
            });
        }
    }

    public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.HasData(
             new GroupUser { Code = 1, Name = "Super Admin", Role = "Super Admin", Order = 1, View = false },
             new GroupUser { Code = 2, Name = "Admin", Role = "Admin", Order = 2, View = true },
             new GroupUser { Code = 3, Name = "User", Role = "Staff", Order = 3, View = true });
        }
    }
}
