using CMS.Areas.Configurations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasData(
                new EmailTemplate { Pid = 1, Code = "EmailContactToAdmin", Title = "Email gửi admin khi liên hệ", Group = "ContactToAdmin" },
                new EmailTemplate { Pid = 2, Code = "EmailContactToCustomer", Title = "Email gửi khách hàng khi liên hệ", Group = "ContactToCustomer" },

                new EmailTemplate { Pid = 3, Code = "EmailActiveAccount", Title = "Email gửi khách hàng kích hoạt tài khoản khi khách hàng đăng ký", Group = "ActiveAccount" },
                new EmailTemplate { Pid = 4, Code = "EmailForgotPassword", Title = "Email gửi khách hàng khi khách hàng quên mật khẩu", Group = "ForgotPassword" },

                new EmailTemplate { Pid = 5, Code = "EmailRecruitToAdmin", Title = "Email gửi admin khi người dùng ứng tuyển ", Enabled = false, Group = "RecruitToAdmin" },
                new EmailTemplate { Pid = 6, Code = "EmailRecruitToCustomer", Title = "Email gửi người dùng khi ứng tuyển", Enabled = false, Group = "RecruitToCustomer" },

                new EmailTemplate { Pid = 7, Code = "EmailCustomerOrder", Title = "Email gửi khách hàng đi ấn nút đặt hàng", Group = "CustomerOrder" },
                new EmailTemplate { Pid = 8, Code = "EmailAdminOrder", Title = "Email gửi khách hàng khi ấn nút send mail trong admin", Group = "AdminOrder" },
                new EmailTemplate { Pid = 9, Code = "EmailAdminVAT", Title = "Email gửi khách hàng VAT", Group = "AdminVAT" },

                new EmailTemplate { Pid = 10, Code = "EmailEnquireToAdmin", Title = "Email gửi admin khi đặt lịch hẹn", Enabled = false, Group = "EnquireToAdmin" },
                new EmailTemplate { Pid = 11, Code = "EmailEnquireToCustomer", Title = "Email gửi khách hàng khi đặt lịch hẹn", Enabled = false, Group = "EnquireToCustomer" },
                new EmailTemplate { Pid = 12, Code = "EmailOrderToAdmin", Title = "Email gửi admin khi khách đặt hàng", Group = "OrderToAdmin" });

        }
    }

    public class MultiLangEmailTemplateConfiguration : IEntityTypeConfiguration<MultiLang_EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<MultiLang_EmailTemplate> builder)
        {

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 1, EmailTemplatePid = 1, Subject = "Có 1 yêu cầu hỗ trợ", FromName = "{{FromName}}", LangKey = "vi", Content = @"<div class=""tt""><div class=""tt"">Xin chào <strong><span class=""pull - right""><a class=""add_merge_field text - primary "" role=""button""> {{FromName}},</a></span></strong></div><p>Có <strong>01 </strong>yêu cầu được gửi tới website!</p><ul><li>Tên Khách hàng: {{FullName}}</li><li>Email: {{Email}}</li><li>Số điện thoại: {{PhoneNumber}}</li><li>Tiêu đề: {{Title}}</li><li>Nội dung: {{Content}}</li></ul><hr><div class=""small text - muted note"">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div></div>" },

                new MultiLang_EmailTemplate { Pid = 2, EmailTemplatePid = 1, Subject = "There is 1 request for support", FromName = "{{FromName}}", LangKey = "en", Content = @"<div class=""tt"">Hello<strong><span class=""pull - right""><a class=""add_merge_field text - primary "" role=""button""> {{FromName}} </a></span></strong></div><p>There is <strong>01</strong> request sent to the website!</p><ul><li>Customer name: {{FullName}}</li><li>Email: {{Email}}</li><li>Phone number: {{PhoneNumber}}</li><li>Title: {{Title}}</li><li>Content: {{Content}}</li></ul><hr><div class=""small text - muted note"">* This is an autoresponder email, please do not reply to this message.</div>" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 3, EmailTemplatePid = 2, Subject = "Cảm ơn bạn đã liên hệ", FromName = "{{FromName}}", LangKey = "vi", Content = @"<div class=""tt"">Xin chào <strong><span class=""pull - right""> <a class=""add_merge_field text - primary "" role=""button""> {{ClientName}},</a></span></strong></div><div class=""tt"">Cảm ơn bạn đã liên hệ, chúng tôi sẽ trả lời bạn sớm nhất có thể.</div><hr><div class=""small text - muted note"">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div>" },
                new MultiLang_EmailTemplate { Pid = 4, EmailTemplatePid = 2, Subject = "Thank you for contacting!", FromName = "{{FromName}}", LangKey = "en", Content = @"<div class=""tt"">Dear<strong><span class=""pull - right"">&nbsp;<a class=""add_merge_field text - primary "" style=""text - decoration: none; "" role=""button"" data-mce-style=""text - decoration: none; "">{{ClientName}},</a></span></strong></div><div class=""tt"">Thank you for your contact, we will reply to you as soon as possible.</div><hr><div class=""small text-muted note"">* This is an autoresponder email, please do not reply to this message.</div>" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 5, EmailTemplatePid = 3, Subject = "Xác nhận tài khoản - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "<a href='{{LinkActiveAccount}}'>Xác nhận tài khoản</a>" },
                new MultiLang_EmailTemplate { Pid = 6, EmailTemplatePid = 3, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 7, EmailTemplatePid = 4, Subject = "Quên mật khẩu - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "Mật khẩu mới của bạn là: {{NewPassword}}" },
                new MultiLang_EmailTemplate { Pid = 8, EmailTemplatePid = 4, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
                 new MultiLang_EmailTemplate { Pid = 9, EmailTemplatePid = 5, Subject = "Có 1 yêu cầu tuyển dụng - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "{FullName}} {{Email}} {{PhoneNumber}} {{Title}} {{Content}} {{Job}}" },
                 new MultiLang_EmailTemplate { Pid = 10, EmailTemplatePid = 5, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 11, EmailTemplatePid = 6, Subject = "Tuyển dụng - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "Cảm ơn bạn đã ứng tuyển , chúng tôi sẽ trả lời bạn sớm nhất có thể." },
                new MultiLang_EmailTemplate { Pid = 12, EmailTemplatePid = 6, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
              new MultiLang_EmailTemplate { Pid = 13, EmailTemplatePid = 7, Subject = "Cảm ơn bạn đã đặt hàng - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "Cảm ơn bạn đã đặt hàng , chúng tôi sẽ trả lời bạn sớm nhất có thể." },
              new MultiLang_EmailTemplate { Pid = 14, EmailTemplatePid = 7, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
              new MultiLang_EmailTemplate { Pid = 15, EmailTemplatePid = 8, Subject = "Xác nhận đơn hàng - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "Xác nhận đơn hàng , chúng tôi sẽ trả lời bạn sớm nhất có thể." },
              new MultiLang_EmailTemplate { Pid = 16, EmailTemplatePid = 8, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
              new MultiLang_EmailTemplate { Pid = 17, EmailTemplatePid = 9, Subject = "Xuất VAT - {{FromName}}", FromName = "{{FromName}}", LangKey = "vi", Content = "Chúng tôi gửi bạn VAT cho mã đơn hàng {{InvoiceCode}}" },
              new MultiLang_EmailTemplate { Pid = 18, EmailTemplatePid = 9, Subject = "", FromName = "", LangKey = "en" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 19, EmailTemplatePid = 10, Subject = "Có 1 khách hàng đặt lịch hẹn", FromName = "{{FromName}}", LangKey = "vi", Content = @"<div class=""tt""><div class=""tt"">Xin chào <strong><span class=""pull - right""><a class=""add_merge_field text - primary "" role=""button""> {{FromName}},</a></span></strong></div><p>Có <strong>01 </strong>yêu cầu được gửi tới website!</p><ul><li>Tên Khách hàng: {{FullName}}</li><li>Email: {{Email}}</li><li>Số điện thoại: {{PhoneNumber}}</li><li>Dịch vụ: {{ServiceName}}</li><li>Ngày đặt lịch hẹn: {{EnquireDate}}</li><li>Nội dung: {{Content}}</li></ul><hr><div class=""small text - muted note"">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div></div>" },

                new MultiLang_EmailTemplate { Pid = 20, EmailTemplatePid = 10, Subject = "There is 1 customer who has booked an appointment", FromName = "{{FromName}}", LangKey = "en", Content = @"<div class=""tt"">Hello<strong><span class=""pull - right""><a class=""add_merge_field text - primary "" role=""button""> {{FromName}} </a></span></strong></div><p>There is <strong>01</strong> request sent to the website!</p><ul><li>Customer name: {{FullName}}</li><li>Email: {{Email}}</li><li>Phone number: {{PhoneNumber}}</li><li>Service: {{ServiceName}}</li><li>Appointment date: {{EnquireDate}}</li><li>Content: {{Content}}</li></ul><hr><div class=""small text - muted note"">* This is an autoresponder email, please do not reply to this message.</div>" });

            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 21, EmailTemplatePid = 11, Subject = "Cảm ơn bạn đã đặt lịch hẹn", FromName = "{{FromName}}", LangKey = "vi", Content = @"<div class=""tt"">Xin chào <strong><span class=""pull - right""> <a class=""add_merge_field text - primary "" role=""button""> {{ClientName}},</a></span></strong></div><div class=""tt"">Cảm ơn bạn đã đặt lịch hẹn, chúng tôi sẽ trả lời bạn sớm nhất có thể.</div><hr><div class=""small text - muted note"">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div>" },
                new MultiLang_EmailTemplate { Pid = 22, EmailTemplatePid = 11, Subject = "Thank you for scheduling your appointment!", FromName = "{{FromName}}", LangKey = "en", Content = @"<div class=""tt"">Dear<strong><span class=""pull - right"">&nbsp;<a class=""add_merge_field text - primary "" style=""text - decoration: none; "" role=""button"" data-mce-style=""text - decoration: none; "">{{ClientName}},</a></span></strong></div><div class=""tt"">Thank you for your contact, we will reply to you as soon as possible.</div><hr><div class=""small text-muted note"">* This is an autoresponder email, please do not reply to this message.</div>" });
            builder.HasData(
                new MultiLang_EmailTemplate { Pid = 23, EmailTemplatePid = 12, Subject = "Có 01 đơn hàng mới", FromName = "{{FromName}}", LangKey = "vi", Content = @"<div class=""e - body""> <div class=""tt"">Xin ch&agrave;o <strong><span class=""pull - right""><a class=""add_merge_field text - primary "" role=""button""> {{FromName}},</a></span></strong></div> <p>C&oacute; <strong>01 </strong>đơn h&agrave;ng mới!</p> <hr /> <div class=""mb - 3 fs - 14""> <div class=""tt tt - color uppercase border - bottom fs - 14"">Th&ocirc;ng tin đơn h&agrave;ng {{InvoiceCode}} <span class=""small fw - normal uppercase - none text - body"">({{DatetimeNow}})</span></div> <table class=""w - 100 mb - 2""> <tbody> <tr> <td class=""col - 6 align - top"" style=""width: 199px; ""> <p><strong>Th&ocirc;ng tin thanh to&aacute;n</strong></p> <p>{{FullName}}</p> <p>{{Email}}</p> <p>{{PhoneNumber}}</p> </td> <td class=""col - 6 align - top"" style=""width: 199px; ""> <p><strong>Địa chỉ giao h&agrave;ng</strong></p> <p>{{FullName}}</p> <p>{{CustomerAddress}}</p> <p>{{PhoneNumber}}</p> </td> </tr> </tbody> </table> <p><strong>Phương thức thanh to&aacute;n: </strong>{{PaymentMethod}}</p> <div class=""tt tt-color uppercase border-bottom fs - 14"">Chi tiết đơn h&agrave;ng</div> <div class=""tt tt-color uppercase border-bottom fs - 14"">{{TableProductList}}</div> <table class=""table - invoid fs - 14 mb - 2"" style=""height: 88px; ""> <tbody> <tr class=""bg - light fw - bold text - red"" style=""height: 22px; ""> <td class=""td p-sm text - right"" style=""width: 499.344px; height: 22px; "" colspan=""4"">Tổng gi&aacute; trị đơn h&agrave;ng</td> <td class=""td p-sm text - right"" style=""width: 94px; height: 22px; "">{{Total}} đ</td> </tr> </tbody> </table> </div> </div>" },
                new MultiLang_EmailTemplate { Pid = 24, EmailTemplatePid = 12, Subject = "New Order Notification", FromName = "{{FromName}}", LangKey = "en", Content = @"<div class=""e-body""> <div class=""tt"">Hello <strong><span class=""pull-right""><a class=""add_merge_field text-primary"" role=""button""> {{FromName}},</a></span></strong></div> <p>You have <strong>01 </strong>new order!</p> <hr /> <div class=""mb-3 fs-14""> <div class=""tt tt-color uppercase border-bottom fs-14"">Order Information {{InvoiceCode}} <span class=""small fw-normal uppercase-none text-body"">({{DatetimeNow}})</span></div> <table class=""w-100 mb-2""> <tbody> <tr> <td class=""col-6 align-top"" style=""width: 199px; ""> <p><strong>Payment Information</strong></p> <p>{{FullName}}</p> <p>{{Email}}</p> <p>{{PhoneNumber}}</p> </td> <td class=""col-6 align-top"" style=""width: 199px; ""> <p><strong>Shipping Address</strong></p> <p>{{FullName}}</p> <p>{{CustomerAddress}}</p> <p>{{PhoneNumber}}</p> </td> </tr> </tbody> </table> <p><strong>Payment Method: </strong>{{PaymentMethod}}</p> <div class=""tt tt-color uppercase border-bottom fs-14"">Order Details</div> <div class=""tt tt-color uppercase border-bottom fs-14"">{{TableProductList}}</div> <table class=""table-invoid fs-14 mb-2"" style=""height: 88px; ""> <tbody> <tr class=""bg-light fw-bold text-red"" style=""height: 22px; ""> <td class=""td p-sm text-right"" style=""width: 499.344px; height: 22px; "" colspan=""4"">Total Order Value</td> <td class=""td p-sm text-right"" style=""width: 94px; height: 22px; "">{{Total}} đ</td> </tr> </tbody> </table> </div> </div>" });

        }
    }
}
