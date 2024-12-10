using CMS.Areas.Configurations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Configurations
{
    public class EmailTempateVariableConfiguration : IEntityTypeConfiguration<EmailTempateVariable>
    {
        public void Configure(EntityTypeBuilder<EmailTempateVariable> builder)
        {
            builder.HasData(
                  new EmailTempateVariable { Pid = 1, Code = "{{Logo}}", Name = "Logo", Group = "ContactToCustomer" },
                  new EmailTempateVariable { Pid = 2, Code = "{{Hotline}}", Name = "Hotline", Group = "ContactToCustomer" },
                  new EmailTempateVariable { Pid = 3, Code = "{{CompanyName}}", Name = "CompanyName", Group = "ContactToCustomer" },
                  new EmailTempateVariable { Pid = 4, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "ContactToCustomer" },
                  new EmailTempateVariable { Pid = 5, Code = "{{FromName}}", Name = "FromName", Group = "ContactToCustomer" },
                  new EmailTempateVariable { Pid = 6, Code = "{{ClientName}}", Name = "ClientName", Group = "ContactToCustomer" });

            builder.HasData(
                new EmailTempateVariable { Pid = 7, Code = "{{Logo}}", Name = "Logo", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 8, Code = "{{Hotline}}", Name = "Hotline", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 9, Code = "{{CompanyName}}", Name = "CompanyName", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 10, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 11, Code = "{{FromName}}", Name = "FromName", Group = "ContactToAdmin" },

                new EmailTempateVariable { Pid = 12, Code = "{{FullName}}", Name = "FullName", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 13, Code = "{{Email}}", Name = "Email", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 14, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 15, Code = "{{Title}}", Name = "Title", Group = "ContactToAdmin" },
                new EmailTempateVariable { Pid = 16, Code = "{{Content}}", Name = "Content", Group = "ContactToAdmin" });

            builder.HasData(
                new EmailTempateVariable { Pid = 17, Code = "{{Logo}}", Name = "Logo", Group = "ActiveAccount" },
                new EmailTempateVariable { Pid = 18, Code = "{{Hotline}}", Name = "Hotline", Group = "ActiveAccount" },
                new EmailTempateVariable { Pid = 19, Code = "{{CompanyName}}", Name = "CompanyName", Group = "ActiveAccount" },
                new EmailTempateVariable { Pid = 20, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "ActiveAccount" },
                new EmailTempateVariable { Pid = 21, Code = "{{FromName}}", Name = "FromName", Group = "ActiveAccount" },
                new EmailTempateVariable { Pid = 22, Code = "{{LinkActiveAccount}}", Name = "LinkActiveAccount", Group = "ActiveAccount" });

            builder.HasData(
                new EmailTempateVariable { Pid = 23, Code = "{{Logo}}", Name = "Logo", Group = "ForgotPassword" },
                new EmailTempateVariable { Pid = 24, Code = "{{Hotline}}", Name = "Hotline", Group = "ForgotPassword" },
                new EmailTempateVariable { Pid = 25, Code = "{{CompanyName}}", Name = "CompanyName", Group = "ForgotPassword" },
                new EmailTempateVariable { Pid = 26, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "ForgotPassword" },
                new EmailTempateVariable { Pid = 27, Code = "{{FromName}}", Name = "FromName", Group = "ForgotPassword" },
                new EmailTempateVariable { Pid = 28, Code = "{{LinkForgotPassword}}", Name = "LinkForgotPassword", Group = "ForgotPassword" });


            //recruitment email
            builder.HasData(
                new EmailTempateVariable { Pid = 29, Code = "{{Logo}}", Name = "Logo", Group = "RecruitToCustomer" },
                new EmailTempateVariable { Pid = 30, Code = "{{Hotline}}", Name = "Hotline", Group = "RecruitToCustomer" },
                new EmailTempateVariable { Pid = 31, Code = "{{CompanyName}}", Name = "CompanyName", Group = "RecruitToCustomer" },
                new EmailTempateVariable { Pid = 32, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "RecruitToCustomer" },
                new EmailTempateVariable { Pid = 33, Code = "{{FromName}}", Name = "FromName", Group = "RecruitToCustomer" },
                new EmailTempateVariable { Pid = 34, Code = "{{ClientName}}", Name = "ClientName", Group = "RecruitToCustomer" });


            builder.HasData(
                new EmailTempateVariable { Pid = 35, Code = "{{Logo}}", Name = "Logo", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 36, Code = "{{Hotline}}", Name = "Hotline", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 37, Code = "{{CompanyName}}", Name = "CompanyName", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 38, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 39, Code = "{{FromName}}", Name = "FromName", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 41, Code = "{{FullName}}", Name = "FullName", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 42, Code = "{{Email}}", Name = "Email", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 43, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 44, Code = "{{Job}}", Name = "Job", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 45, Code = "{{Title}}", Name = "Title", Group = "RecruitToAdmin" },
                new EmailTempateVariable { Pid = 46, Code = "{{Content}}", Name = "Content", Group = "RecruitToAdmin" });

            builder.HasData(
                new EmailTempateVariable { Pid = 47, Code = "{{Logo}}", Name = "Logo", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 48, Code = "{{Hotline}}", Name = "Hotline", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 49, Code = "{{CompanyName}}", Name = "CompanyName", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 50, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 51, Code = "{{FromName}}", Name = "FromName", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 52, Code = "{{FullName}}", Name = "FullName", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 53, Code = "{{Email}}", Name = "Email", Group = "CustomerOrder" },
                new EmailTempateVariable { Pid = 54, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "CustomerOrder" });

            builder.HasData(
                new EmailTempateVariable { Pid = 55, Code = "{{Logo}}", Name = "Logo", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 56, Code = "{{Hotline}}", Name = "Hotline", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 57, Code = "{{CompanyName}}", Name = "CompanyName", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 58, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 59, Code = "{{FromName}}", Name = "FromName", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 60, Code = "{{FullName}}", Name = "FullName", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 61, Code = "{{Email}}", Name = "Email", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 62, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 63, Code = "{{CompanyAddress}}", Name = "Address", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 64, Code = "{{FirstName}}", Name = "FirstName", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 65, Code = "{{LastName}}", Name = "LastName", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 66, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 67, Code = "{{Email}}", Name = "Email", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 68, Code = "{{CustomerAddress}}", Name = "CustomerAddress", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 69, Code = "{{State}}", Name = "State", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 70, Code = "{{IsPayment}}", Name = "IsPayment", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 71, Code = "{{PaymentMethod}}", Name = "PaymentMethod", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 72, Code = "{{ShipFee}}", Name = "ShipFee", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 73, Code = "{{Deposit}}", Name = "Deposit", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 74, Code = "{{Total}}", Name = "Total", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 75, Code = "{{Note}}", Name = "Note", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 79, Code = "{{TableProductList}}", Name = "TableProductList", Group = "AdminOrder" });

            builder.HasData(
                new EmailTempateVariable { Pid = 80, Code = "{{Logo}}", Name = "Logo", Group = "AdminVAT" },
                new EmailTempateVariable { Pid = 81, Code = "{{Hotline}}", Name = "Hotline", Group = "AdminVAT" },
                new EmailTempateVariable { Pid = 82, Code = "{{CompanyName}}", Name = "CompanyName", Group = "AdminVAT" },
                new EmailTempateVariable { Pid = 83, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "AdminVAT" },
                new EmailTempateVariable { Pid = 84, Code = "{{FromName}}", Name = "FromName", Group = "AdminVAT" },
                new EmailTempateVariable { Pid = 85, Code = "{{InvoiceCode}}", Name = "InvoiceCode", Group = "AdminVAT" });

            builder.HasData(
                new EmailTempateVariable { Pid = 86, Code = "{{InvoiceCode}}", Name = "InvoiceCode", Group = "AdminOrder" },
                new EmailTempateVariable { Pid = 87, Code = "{{TemporaryPrice}}", Name = "TemporaryPrice", Group = "AdminOrder" });

            builder.HasData(
                  new EmailTempateVariable { Pid = 88, Code = "{{Logo}}", Name = "Logo", Group = "EnquireToCustomer" },
                  new EmailTempateVariable { Pid = 89, Code = "{{Hotline}}", Name = "Hotline", Group = "EnquireToCustomer" },
                  new EmailTempateVariable { Pid = 90, Code = "{{CompanyName}}", Name = "CompanyName", Group = "EnquireToCustomer" },
                  new EmailTempateVariable { Pid = 91, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "EnquireToCustomer" },
                  new EmailTempateVariable { Pid = 92, Code = "{{FromName}}", Name = "FromName", Group = "EnquireToCustomer" },
                  new EmailTempateVariable { Pid = 93, Code = "{{ClientName}}", Name = "ClientName", Group = "EnquireToCustomer" });

            builder.HasData(
                new EmailTempateVariable { Pid = 94, Code = "{{Logo}}", Name = "Logo", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 95, Code = "{{Hotline}}", Name = "Hotline", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 96, Code = "{{CompanyName}}", Name = "CompanyName", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 97, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 98, Code = "{{FromName}}", Name = "FromName", Group = "EnquireToAdmin" },

                new EmailTempateVariable { Pid = 99, Code = "{{FullName}}", Name = "FullName", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 100, Code = "{{Email}}", Name = "Email", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 101, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 102, Code = "{{ServiceName}}", Name = "ServiceName", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 103, Code = "{{Content}}", Name = "Content", Group = "EnquireToAdmin" },
                new EmailTempateVariable { Pid = 104, Code = "{{EnquireDate}}", Name = "EnquireDate", Group = "EnquireToAdmin" });
            builder.HasData(
                new EmailTempateVariable { Pid = 105, Code = "{{Logo}}", Name = "Logo", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 106, Code = "{{Hotline}}", Name = "Hotline", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 107, Code = "{{CompanyName}}", Name = "CompanyName", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 108, Code = "{{DatetimeNow}}", Name = "DatetimeNow", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 109, Code = "{{FromName}}", Name = "FromName", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 110, Code = "{{FullName}}", Name = "FullName", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 111, Code = "{{Email}}", Name = "Email", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 112, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 113, Code = "{{CompanyAddress}}", Name = "Address", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 114, Code = "{{FirstName}}", Name = "FirstName", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 115, Code = "{{LastName}}", Name = "LastName", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 116, Code = "{{PhoneNumber}}", Name = "PhoneNumber", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 117, Code = "{{Email}}", Name = "Email", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 118, Code = "{{CustomerAddress}}", Name = "CustomerAddress", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 119, Code = "{{State}}", Name = "State", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 120, Code = "{{IsPayment}}", Name = "IsPayment", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 121, Code = "{{PaymentMethod}}", Name = "PaymentMethod", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 122, Code = "{{ShipFee}}", Name = "ShipFee", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 123, Code = "{{Deposit}}", Name = "Deposit", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 124, Code = "{{Total}}", Name = "Total", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 125, Code = "{{Note}}", Name = "Note", Group = "OrderToAdmin" },
                new EmailTempateVariable { Pid = 129, Code = "{{TableProductList}}", Name = "TableProductList", Group = "OrderToAdmin" });

        }
    }
}
