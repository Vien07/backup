using CMS.Areas.Admin.Models;
using CMS.Areas.Contact.Models;
using CMS.Areas.News.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasData(
              //new Module { Code = "Dashboard", ModuleName = "Dashboard", Url = "b-admin/Dashboard/Index", Order = 1, Locked = false, Enabled = true, Deleted = false },
              new Module { Code = "HomePage", ModuleName = "Trang chủ (Homepage)", Url = "b-admin/HomePage/", Order = 3, Enabled = true, Deleted = false },
              new Module { Code = "About", ModuleName = "Giới thiệu (About)", Url = "b-admin/About/", Order = 5, Enabled = true, Deleted = false },
              new Module { Code = "FAQ", ModuleName = "FAQ's", Url = "b-admin/FAQ/", Order = 7, Enabled = true, Deleted = false },
              new Module { Code = "Convenience", ModuleName = "Tiện ích (Convenience)", Url = "b-admin/Convenience/Index", Order = 9, Enabled = true, Deleted = false },
              new Module { Code = "Partner", ModuleName = "Đối tác (Partner)", Url = "b-admin/Partner/Index", Order = 10, Enabled = true, Deleted = false },
              //new Module { Code = "Comment", ModuleName = "Feedback (Comment)", Url = "b-admin/Comment/Index", Order = 11, Enabled = true, Deleted = false },
              //new Module { Code = "NewsCate", ModuleName = "Danh mục tin tức (News category)", Url = "b-admin/NewsCate/Index", Order = 13, Enabled = true, Deleted = false },
              new Module { Code = "News", ModuleName = "Tin tức (News)", Url = "b-admin/News/", Order = 15, Enabled = true, Deleted = false },
              //new Module { Code = "FeatureCate", ModuleName = "Danh mục dịch vụ (Feature category)", Url = "b-admin/FeatureCate/Index", Order = 17, Enabled = true, Deleted = false },
              new Module { Code = "Feature", ModuleName = "Tính năng (Feature)", Url = "b-admin/Feature/", Order = 19, Enabled = true, Deleted = false },
              //new Module { Code = "GalleryCate", ModuleName = "Danh mục thư viện (Gallery category)", Url = "b-admin/GalleryCate/Index", Order = 21, Enabled = true, Deleted = false },
              //new Module { Code = "Gallery", ModuleName = "Thư viện (Gallery)", Url = "b-admin/Gallery/", Order = 23, Enabled = true, Deleted = false },
              new Module { Code = "ProductCate", ModuleName = "Chu kỳ sản phẩm (ProductCycle)", Url = "b-admin/ProductCate/Index", Order = 25, Enabled = true, Deleted = false },
              new Module { Code = "Product", ModuleName = "Sản phẩm (Product)", Url = "b-admin/Product/Index", Order = 27, Enabled = true, Deleted = false },
              //new Module { Code = "ProductOption", ModuleName = "Tùy chọn sản phẩm (ProductOption)", Url = "b-admin/ProductColor/Index", Order = 29, Enabled = true, Deleted = false },
              //new Module { Code = "ProductType", ModuleName = "Loại sản phẩm (ProductType)", Url = "b-admin/ProductType/Index", Order = 31, Enabled = true, Deleted = false },
              //new Module { Code = "ProductColor", ModuleName = "Màu sản phẩm (ProductColor)", Url = "b-admin/ProductColor/Index", Order = 33, Enabled = true, Deleted = false },
              //new Module { Code = "Promotion", ModuleName = "Khuyến mãi (Promotion)", Url = "b-admin/Promotion/Index", Order = 34, Enabled = true, Deleted = false },
              new Module { Code = "DiscountCode", ModuleName = "DiscountCode", Url = "b-admin/DiscountCode/Index", Order = 34, Enabled = true, Deleted = false },
              new Module { Code = "Customer", ModuleName = "Khách hàng (Customer)", Url = "b-admin/Customer/Index", Order = 35, Enabled = true, Deleted = false },
              new Module { Code = "Order", ModuleName = "Đơn hàng", Url = "b-admin/Order/Index", Order = 37, Enabled = true, Deleted = false },
              //new Module { Code = "Recruitment", ModuleName = "Tuyển dụng (Recruitment)", Url = "b-admin/Recruitment/", Order = 38, Enabled = true, Deleted = false },
              //new Module { Code = "Candidate", ModuleName = "Ứng viên ứng tuyển (Candidate)", Url = "b-admin/Candidate/", Order = 38, Enabled = true, Deleted = false },
              new Module { Code = "Slide", ModuleName = "Slide", Url = "b-admin/Slide/", Order = 38, Enabled = true, Deleted = false },
              new Module { Code = "Banner", ModuleName = "Banner", Url = "b-admin/Banner/", Order = 41, Enabled = true, Deleted = false },
              new Module { Code = "Advertisement", ModuleName = "Quảng cáo (Advertisement)", Url = "b-admin/Advertisement/Index", Order = 43, Enabled = true, Deleted = false },
              new Module { Code = "Popup", ModuleName = "Popup", Url = "b-admin/Popup/Index", Order = 45, Enabled = true, Deleted = false },
              new Module { Code = "Calendar", ModuleName = "Calendar", Url = "b-admin/Calendar/", Order = 47, Enabled = true, Deleted = false },
              new Module { Code = "Contact", ModuleName = "Liên hệ (Contact)", Url = "b-admin/Contact/Index", Order = 49, Enabled = true, Deleted = false },
              //new Module { Code = "Branch", ModuleName = "Chi nhánh (Branch)", Url = "b-admin/Branch/Index", Order = 51, Enabled = true, Deleted = false },
              new Module { Code = "ContactList", ModuleName = "Danh sách liên hệ (Contact list)", Url = "b-admin/ContactList/Index", Order = 53, Enabled = true, Deleted = false },
              //new Module { Code = "EnquireList", ModuleName = "Danh sách đặt lịch hẹn", Url = "b-admin/EnquireList/Index", Order = 55, Enabled = true, Deleted = false },
              new Module { Code = "GeneralConfiguration", ModuleName = "Cài đặt (Setting)", Url = "b-admin/GeneralConfiguration/Index", Order = 57, Enabled = true, Deleted = false },
              new Module { Code = "Translation", ModuleName = "Dịch ngôn ngữ", Url = "b-admin/Translation/Index", Order = 59, Enabled = true, Deleted = false },
              new Module { Code = "Group", ModuleName = "Nhóm quản lý (Group)", Url = "b-admin/Group/Index", Order = 61, Enabled = true, Deleted = false },
              new Module { Code = "Users", ModuleName = "Nhân viên quản lý (Users)", Url = "b-admin/Users/Index", Order = 63, Enabled = true, Deleted = false },
              new Module { Code = "Permit", ModuleName = "Phân quyền (Permission)", Url = "b-admin/Permit/Index", Order = 65, Enabled = true, Deleted = false },
              new Module { Code = "Log", ModuleName = "Log", Url = "b-admin/Log/Index", Order = 67, Enabled = true, Deleted = false },
              new Module { Code = "Trash", ModuleName = "Thùng rác (Trash)", Url = "b-admin/Trash/", Order = 69, Enabled = true, Deleted = false });
        }
    }
}
