using CMS.Areas.About.Models;
using CMS.Areas.FAQ.Models;
using CMS.Areas.Gallery.Models;
using CMS.Areas.News.Models;
using CMS.Areas.Product.Models;
using CMS.Areas.Feature.Models;
using CMS.Areas.DiscountCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class NewsCateConfiguration : IEntityTypeConfiguration<NewsCate>
    {
        public void Configure(EntityTypeBuilder<NewsCate> builder)
        {
            builder.HasData(new NewsCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
        }
    }

    public class GalleryCateConfiguration : IEntityTypeConfiguration<GalleryCate>
    {
        public void Configure(EntityTypeBuilder<GalleryCate> builder)
        {
            builder.HasData(new GalleryCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
        }
    }

    public class DiscountCodeCateConfiguration : IEntityTypeConfiguration<DiscountCodeCate>
    {
        public void Configure(EntityTypeBuilder<DiscountCodeCate> builder)
        {
            builder.HasData(new DiscountCodeCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
        }
    }

    public class FAQCateConfiguration : IEntityTypeConfiguration<FAQCate>
    {
        public void Configure(EntityTypeBuilder<FAQCate> builder)
        {
            builder.HasData(new FAQCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
        }
    }

    public class FeatureCateConfiguration : IEntityTypeConfiguration<FeatureCate>
    {
        public void Configure(EntityTypeBuilder<FeatureCate> builder)
        {
            builder.HasData(new FeatureCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
        }
    }

    public class AboutCateConfiguration : IEntityTypeConfiguration<AboutCate>
    {
        public void Configure(EntityTypeBuilder<AboutCate> builder)
        {
            builder.HasData(new AboutCate { Pid = 1, Code = "/", Order = 0 });
        }
    }

    public class ProductCateConfiguration : IEntityTypeConfiguration<ProductCate>
    {
        public void Configure(EntityTypeBuilder<ProductCate> builder)
        {
            builder.HasData(new ProductCate { Pid = 1, Code = "/", isLocked = true, Order = 0 });
            builder.HasData(new ProductCate { Pid = 2, Code = "1", Enabled = true, Deleted = false, Order = 1, Months = 1 });
            builder.HasData(new ProductCate { Pid = 3, Code = "2", Enabled = true, Deleted = false, Order = 2, Months = 6 });
            builder.HasData(new ProductCate { Pid = 4, Code = "3", Enabled = true, Deleted = false, Order = 3, Months = 9 });
            builder.HasData(new ProductCate { Pid = 5, Code = "4", Enabled = true, Deleted = false, Order = 4, Months = 18 });


        }
    }
    public class MultiLangProductCateConfiguration: IEntityTypeConfiguration<MultiLang_ProductCate>
    {
        public void Configure(EntityTypeBuilder<MultiLang_ProductCate> builder)
        {
            builder.HasData(
                new MultiLang_ProductCate { Pid = 1, Name = "1 tháng", LangKey = "vi", ProductCatePid = 2 },
                new MultiLang_ProductCate { Pid = 2, Name = "1 month", LangKey = "en", ProductCatePid = 2 },
                new MultiLang_ProductCate { Pid = 3, Name = "6 tháng", LangKey = "vi", ProductCatePid = 3 },
                new MultiLang_ProductCate { Pid = 4, Name = "6 month", LangKey = "en", ProductCatePid = 3 },
                new MultiLang_ProductCate { Pid = 5, Name = "9 tháng", LangKey = "vi", ProductCatePid = 4 },
                new MultiLang_ProductCate { Pid = 6, Name = "9 month", LangKey = "en", ProductCatePid = 4 },
                new MultiLang_ProductCate { Pid = 7, Name = "18 tháng", LangKey = "vi", ProductCatePid = 5 },
                new MultiLang_ProductCate { Pid = 8, Name = "18 month", LangKey = "en", ProductCatePid = 5 }

                );
        }
    }
    public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.HasData(new ProductOption { Pid = 1, Order = 1, Code = "default" });

        }
    }

    public class MultiLangProductOptionConfiguration : IEntityTypeConfiguration<MultiLang_ProductOption>
    {
        public void Configure(EntityTypeBuilder<MultiLang_ProductOption> builder)
        {
            builder.HasData(
                new MultiLang_ProductOption { Pid = 1, ProductOptionPid = 1, Name = "Default", Description = "", Slug = "default", LangKey = "vi" },
                new MultiLang_ProductOption { Pid = 2, ProductOptionPid = 1, Name = "Default", Description = "", Slug = "default", LangKey = "en" });
        }
    }

    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.HasData(new ProductColor { Pid = 1, Order = 1, Code = "#000000", PicThumb = "default.png" });
        }
    }

    public class MultiLangProductColorConfiguration : IEntityTypeConfiguration<MultiLang_ProductColor>
    {
        public void Configure(EntityTypeBuilder<MultiLang_ProductColor> builder)
        {
            builder.HasData(
                new MultiLang_ProductColor { Pid = 1, ProductColorPid = 1, Name = "Mặc định", Description = "", Slug = "default", LangKey = "vi" },
                new MultiLang_ProductColor { Pid = 2, ProductColorPid = 1, Name = "Default", Description = "", Slug = "default", LangKey = "en" });
        }
    }
}
