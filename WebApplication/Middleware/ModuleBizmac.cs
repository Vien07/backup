using CMS.Areas.About;
using CMS.Areas.Admin;
using CMS.Areas.Advertisement;
using CMS.Areas.Banner;
using CMS.Areas.Calendar;
using CMS.Areas.Comment;
using CMS.Areas.Configurations;
using CMS.Areas.Contact;
using CMS.Areas.Customer;
using CMS.Areas.Dashboard;
using CMS.Areas.FAQ;
using CMS.Areas.Gallery;
using CMS.Areas.HomePage;
using CMS.Areas.News;
using CMS.Areas.Order;
using CMS.Areas.Convenience;
using CMS.Areas.Popup;
using CMS.Areas.Product;
using CMS.Areas.Promotion;
using CMS.Areas.Recruitment;
using CMS.Areas.Feature;
using CMS.Areas.Slide;
using CMS.Areas.Translation;
using CMS.Areas.Trash;
using CMS.Areas.DiscountCode;
using CMS.Helper;
using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.EmailServices;
using CMS.Services.FileServices;
using CMS.Services.PageServices;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using CMS.Areas.Partner;

namespace CMS.Middleware
{
    public static class ModuleBizmac
    {
        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //common
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IWebsiteServices, WebsiteServices>();
            services.AddScoped<IPageServices, PageServices>();
            services.AddScoped<ITranslateServices, TranslateServices>();
            services.AddScoped<DomainRequestServices>();
            services.AddSingleton<TokenHelper>();

            //Website
            services.AddScoped<IAbout_Repository, About_Repository>();
            services.AddScoped<IContact_Repository, Contact_Repository>();
            services.AddScoped<IHome_Repository, Home_Repository>();
            services.AddScoped<INews_Repository, News_Repository>();
            services.AddScoped<IGallery_Repository, Gallery_Repository>();
            services.AddScoped<IFAQ_Repository, FAQ_Repository>();
            services.AddScoped<IFeature_Repository, Feature_Repository>();
            //services.AddScoped<IProject_Repository, Project_Repository>();
            services.AddScoped<ISearch_Repository, Search_Repository>();
            services.AddScoped<ICustomer_Repository, Customer_Repository>();
            services.AddScoped<IProduct_Repository, Product_Repository>();
            services.AddScoped<ICart_Repository, Cart_Repository>();
            //services.AddScoped<ITagKey_Repository, TagKey_Repository>();
            //services.AddScoped<IRecruitment_Repository, Recruitment_Repository>();
            //services.AddScoped<IComment_Repository, Comment_Repository>();


            //admin
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IPartnerRepository, PartnerRepository>();
            services.AddTransient<IConvenienceRepository, ConvenienceRepository>();
            services.AddTransient<ITranslationRepository, TranslationRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IPermitRepository, PermitRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IContactInfoRepository, ContactInfoRepository>();
            services.AddTransient<IContactListRepository, ContactListRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IEnquireListRepository, EnquireListRepository>();
            services.AddTransient<IHomePageIntroRepository, HomePageIntroRepository>();
            services.AddTransient<IHomePageFeatureRepository, HomePageFeatureRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<ICalendarRepository, CalendarRepository>();
            services.AddTransient<ITrashRepository, TrashRepository>();
            services.AddTransient<IBannerRepository, BannerRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<INewsCateRepository, NewsCateRepository>();
            services.AddTransient<IGalleryRepository, GalleryRepository>();
            services.AddTransient<IGalleryCateRepository, GalleryCateRepository>();
            services.AddTransient<IDiscountCodeRepository, DiscountCodeRepository>();
            //services.AddTransient<IDiscountCodeCateRepository, DiscountCodeCateRepository>();
            services.AddTransient<IFAQRepository, FAQRepository>();
            //services.AddTransient<IFAQCateRepository, FAQCateRepository>();
            services.AddTransient<IFeatureRepository, FeatureRepository>();
            //services.AddTransient<IServiceCateRepository, ServiceCateRepository>();
            services.AddTransient<IPromotionRepository, PromotionRepository>();
            //services.AddTransient<IPromotionCateRepository, PromotionCateRepository>();
            services.AddTransient<IAboutRepository, AboutRepository>();
            services.AddTransient<IGeneralConfigurationRepository, GeneralConfigurationRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();
            services.AddTransient<IPopupRepository, PopupRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductCateRepository, ProductCateRepository>();
            //services.AddTransient<IProductOptionRepository, ProductOptionRepository>();
            //services.AddTransient<IProductColorRepository, ProductColorRepository>();
            //services.AddTransient<IProductCommentRepository, ProductCommentRepository>();
            //services.AddTransient<IRecruitmentCateRepository, RecruitmentCateRepository>();
            services.AddTransient<IRecruitmentRepository, RecruitmentRepository>();
            services.AddTransient<ICandidateRepository, CandidateRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            return services;
        }
    }
}
