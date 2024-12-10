using CMS.Areas.Banner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasData(new Page { Pid = 1, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 2, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 3, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 4, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 5, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 6, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 7, Enabled = true, Deleted = false });
            builder.HasData(new Page { Pid = 8, Enabled = true, Deleted = false });
        }
    }

    public class MultiLangPageConfiguration : IEntityTypeConfiguration<MultiLang_Page>
    {
        public void Configure(EntityTypeBuilder<MultiLang_Page> builder)
        {
            builder.HasData(new MultiLang_Page { Pid = 1, Title = "Trang chủ", LangKey = "vi", PagePid = 1 });
            builder.HasData(new MultiLang_Page { Pid = 2, Title = "Home", LangKey = "en", PagePid = 1 });

            builder.HasData(new MultiLang_Page { Pid = 3, Title = "Giới thiệu", LangKey = "vi", PagePid = 2 });
            builder.HasData(new MultiLang_Page { Pid = 4, Title = "About", LangKey = "en", PagePid = 2 });

            builder.HasData(new MultiLang_Page { Pid = 5, Title = "Liên hệ", LangKey = "vi", PagePid = 3 });
            builder.HasData(new MultiLang_Page { Pid = 6, Title = "Contact", LangKey = "en", PagePid = 3 });

            builder.HasData(new MultiLang_Page { Pid = 7, Title = "Sản phẩm", LangKey = "vi", PagePid = 4 });
            builder.HasData(new MultiLang_Page { Pid = 8, Title = "Product", LangKey = "en", PagePid = 4 });

            builder.HasData(new MultiLang_Page { Pid = 9, Title = "Khách hàng", LangKey = "vi", PagePid = 5 });
            builder.HasData(new MultiLang_Page { Pid = 10, Title = "Customer", LangKey = "en", PagePid = 5 });

            builder.HasData(new MultiLang_Page { Pid = 11, Title = "Đơn hàng", LangKey = "vi", PagePid = 6 });
            builder.HasData(new MultiLang_Page { Pid = 12, Title = "Order", LangKey = "en", PagePid = 6 });

            builder.HasData(new MultiLang_Page { Pid = 13, Title = "Tính năng", LangKey = "vi", PagePid = 7 });
            builder.HasData(new MultiLang_Page { Pid = 14, Title = "Feature", LangKey = "en", PagePid = 7 });

            builder.HasData(new MultiLang_Page { Pid = 15, Title = "Tin tức", LangKey = "vi", PagePid = 8 });
            builder.HasData(new MultiLang_Page { Pid = 16, Title = "News", LangKey = "en", PagePid = 8 });

        }
    }
}
