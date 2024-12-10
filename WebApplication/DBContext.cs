using CMS.Areas.About.Models;
using CMS.Areas.Admin.Models;
using CMS.Areas.Advertisement.Models;
using CMS.Areas.Banner.Models;
using CMS.Areas.Calendar.Models;
using CMS.Areas.Comment.Models;
using CMS.Areas.Configurations.Models;
using CMS.Areas.Contact.Models;
using CMS.Areas.Customer.Models;
using CMS.Areas.FAQ.Models;
using CMS.Areas.Gallery.Models;
using CMS.Areas.HomePage.Models;
using CMS.Areas.News.Models;
using CMS.Areas.Order.Models;
using CMS.Areas.Convenience.Models;
using CMS.Areas.Popup.Models;
using CMS.Areas.Product.Models;
using CMS.Areas.Promotion.Models;
using CMS.Areas.Recruitment.Models;
using CMS.Areas.Feature.Models;
using CMS.Areas.Shared.Models;
using CMS.Areas.Slide.Models;
using CMS.Areas.DiscountCode.Models;
using CMS.Configurations;
using Microsoft.EntityFrameworkCore;
using CMS.Areas.Partner.Models;

namespace CMS
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public virtual DbSet<LogError> LogErrors { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<MultiLang_Comment> MultiLang_Comments { get; set; }

        public virtual DbSet<Convenience> Conveniences { get; set; }
        public virtual DbSet<MultiLang_Convenience> MultiLang_Conveniences { get; set; }

        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<MultiLang_Partner> MultiLang_Partners { get; set; }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderImages> OrderImages { get; set; }

        public virtual DbSet<ModulePreview> ModulePreviews { get; set; }

        public virtual DbSet<EmailTempateVariable> EmailTempateVariables { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<MultiLang_EmailTemplate> MultiLang_EmailTemplates { get; set; }

        public virtual DbSet<RecruitmentCate> RecruitmentCates { get; set; }
        public virtual DbSet<MultiLang_RecruitmentCate> MultiLang_RecruitmentCates { get; set; }
        public virtual DbSet<RecruitmentDetail> RecruitmentDetails { get; set; }
        public virtual DbSet<MultiLang_RecruitmentDetail> MultiLang_RecruitmentDetails { get; set; }
        public virtual DbSet<Images_Recruitment> Images_Recruitments { get; set; }
        public virtual DbSet<MultiLang_Images_Recruitment> MultiLang_Images_Recruitments { get; set; }
        public virtual DbSet<RecruitmentCate_RecruitmentDetail> RecruitmentCate_RecruitmentDetails { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }

        public virtual DbSet<Visitor> Visitors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<GroupPermisson> GroupPermissons { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<GroupAdminMenu> GroupAdminMenus { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<MultiLang_ContactInfo> MultiLang_ContactInfos { get; set; }
        public virtual DbSet<ContactList> ContactLists { get; set; }
        public virtual DbSet<EnquireList> EnquireLists { get; set; }


        public virtual DbSet<PromotionCate> PromotionCates { get; set; }
        public virtual DbSet<PromotionDetail> PromotionDetails { get; set; }
        public virtual DbSet<PromotionCate_PromotionDetail> PromotionCate_PromotionDetails { get; set; }
        public virtual DbSet<MultiLang_PromotionCate> MultiLang_PromotionCates { get; set; }
        public virtual DbSet<MultiLang_PromotionDetail> MultiLang_PromotionDetails { get; set; }
        public virtual DbSet<Images_Promotion> Images_Promotiones { get; set; }
        public virtual DbSet<MultiLang_Images_Promotion> MultiLang_Images_Promotiones { get; set; }

        public virtual DbSet<ProductComment> ProductComments { get; set; }
        public virtual DbSet<ProductCate> ProductCates { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductCate_ProductDetail> ProductCate_ProductDetails { get; set; }
        public virtual DbSet<MultiLang_ProductCate> MultiLang_ProductCates { get; set; }
        public virtual DbSet<MultiLang_ProductOption> MultiLang_ProductOptions { get; set; }
        public virtual DbSet<MultiLang_ProductColor> MultiLang_ProductColors { get; set; }
        public virtual DbSet<MultiLang_ProductDetail> MultiLang_ProductDetails { get; set; }
        public virtual DbSet<Images_Product> Images_Products { get; set; }
        public virtual DbSet<MultiLang_Images_Product> MultiLang_Images_Products { get; set; }
        public virtual DbSet<ProductOption_ProductDetail> ProductOption_ProductDetails { get; set; }
        public virtual DbSet<ProductColor_ProductDetail> ProductColor_ProductDetails { get; set; }
        public virtual DbSet<Promotion_Product> Promotion_Products { get; set; }

        public virtual DbSet<NewsCate> NewsCates { get; set; }
        public virtual DbSet<NewsDetail> NewsDetails { get; set; }
        public virtual DbSet<NewsCate_NewsDetail> NewsCate_NewsDetails { get; set; }
        public virtual DbSet<MultiLang_NewsCate> MultiLang_NewsCates { get; set; }
        public virtual DbSet<MultiLang_NewsDetail> MultiLang_NewsDetails { get; set; }
        public virtual DbSet<Images_News> Images_Newses { get; set; }
        public virtual DbSet<MultiLang_Images_News> MultiLang_Images_Newses { get; set; }

        public virtual DbSet<GalleryCate> GalleryCates { get; set; }
        public virtual DbSet<GalleryDetail> GalleryDetails { get; set; }
        public virtual DbSet<GalleryCate_GalleryDetail> GalleryCate_GalleryDetails { get; set; }
        public virtual DbSet<MultiLang_GalleryCate> MultiLang_GalleryCates { get; set; }
        public virtual DbSet<MultiLang_GalleryDetail> MultiLang_GalleryDetails { get; set; }
        public virtual DbSet<Images_Gallery> Images_Galleries { get; set; }
        public virtual DbSet<MultiLang_Images_Gallery> MultiLang_Images_Galleries { get; set; }

        public virtual DbSet<DiscountCodeCate> DiscountCodeCates { get; set; }
        public virtual DbSet<DiscountCodeDetail> DiscountCodeDetails { get; set; }
        public virtual DbSet<DiscountCodeCate_DiscountCodeDetail> DiscountCodeCate_DiscountCodeDetails { get; set; }
        public virtual DbSet<MultiLang_DiscountCodeCate> MultiLang_DiscountCodeCates { get; set; }
        public virtual DbSet<MultiLang_DiscountCodeDetail> MultiLang_DiscountCodeDetails { get; set; }
        public virtual DbSet<Images_DiscountCode> Images_DiscountCodes { get; set; }
        public virtual DbSet<MultiLang_Images_DiscountCode> MultiLang_Images_DiscountCodes { get; set; }

        public virtual DbSet<FAQCate> FAQCates { get; set; }
        public virtual DbSet<FAQDetail> FAQDetails { get; set; }
        public virtual DbSet<FAQCate_FAQDetail> FAQCate_FAQDetails { get; set; }
        public virtual DbSet<MultiLang_FAQCate> MultiLang_FAQCates { get; set; }
        public virtual DbSet<MultiLang_FAQDetail> MultiLang_FAQDetails { get; set; }
        public virtual DbSet<Images_FAQ> Images_FAQs { get; set; }
        public virtual DbSet<MultiLang_Images_FAQ> MultiLang_Images_FAQs { get; set; }

        public virtual DbSet<FeatureCate> FeatureCates { get; set; }
        public virtual DbSet<FeatureDetail> FeatureDetails { get; set; }
        public virtual DbSet<FeatureCate_FeatureDetail> FeatureCate_FeatureDetails { get; set; }
        public virtual DbSet<MultiLang_FeatureCate> MultiLang_FeatureCates { get; set; }
        public virtual DbSet<MultiLang_FeatureDetail> MultiLang_FeatureDetails { get; set; }
        public virtual DbSet<Images_Feature> Images_Features { get; set; }
        public virtual DbSet<MultiLang_Images_Feature> MultiLang_Images_Features { get; set; }


        public virtual DbSet<AboutCate> AboutCates { get; set; }
        public virtual DbSet<AboutDetail> AboutDetails { get; set; }
        public virtual DbSet<MultiLang_AboutDetail> MultiLang_AboutDetails { get; set; }

        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<MultiLang_Banner> MultiLang_Banners { get; set; }

        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<MultiLang_Slide> MultiLang_Slides { get; set; }

        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<MultiLang_Calendar> MultiLang_Calendars { get; set; }

        public virtual DbSet<Branch> Branchs { get; set; }
        public virtual DbSet<MultiLang_Branch> MultiLang_Branchs { get; set; }

        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<MultiLang_Page> MultiLang_Pages { get; set; }
        public virtual DbSet<Banner_Page> Banner_Pages { get; set; }

        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<Advertisement_Page> Advertisement_Pages { get; set; }
        public virtual DbSet<MultiLang_Advertisement> MultiLang_Advertisements { get; set; }

        public virtual DbSet<Popup> Popups { get; set; }
        public virtual DbSet<Popup_Page> Popup_Pages { get; set; }
        public virtual DbSet<MultiLang_Popup> MultiLang_Popups { get; set; }

        public virtual DbSet<HomePage> HomePages { get; set; }
        public virtual DbSet<MultiLang_HomePage> MultiLang_HomePages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigConfiguration());

            modelBuilder.ApplyConfiguration(new PageConfiguration());
            modelBuilder.ApplyConfiguration(new MultiLangPageConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleConfiguration());

            modelBuilder.ApplyConfiguration(new GroupUserConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());

            modelBuilder.ApplyConfiguration(new ContactInfoConfiguration());

            modelBuilder.ApplyConfiguration(new NewsCateConfiguration());
            modelBuilder.ApplyConfiguration(new GalleryCateConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountCodeCateConfiguration());
            modelBuilder.ApplyConfiguration(new FAQCateConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureCateConfiguration());
            modelBuilder.ApplyConfiguration(new AboutCateConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCateConfiguration());
            modelBuilder.ApplyConfiguration(new MultiLangProductCateConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOptionConfiguration());
            modelBuilder.ApplyConfiguration(new MultiLangProductOptionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductColorConfiguration());
            modelBuilder.ApplyConfiguration(new MultiLangProductColorConfiguration());

            modelBuilder.ApplyConfiguration(new EmailTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new MultiLangEmailTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new EmailTempateVariableConfiguration());
        }
    }
}
