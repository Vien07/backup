using CMS.Areas.Contact.Models;
using DTO.Recruitment;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Services.EmailServices
{
    public interface IEmailServices
    {
        void SendRecoveryPassword(dynamic data);
        void SendNewPassword(dynamic data);
        void SendToAdmin(ContactList data);
        void SendToCus(ContactList data);
        void EnquireToAdmin(EnquireList data);
        void EnquireToCus(EnquireList data);
        Task<bool> SendMailActiveCustomer(string email);
        bool SendMailForgotPasswordCustomer(string email, string newPassword);
        void SendRecruitToAdmin(CVDto data);
        void SendRecruitToCus(CVDto data);
        Task SendMailOrderAdmin(long pid);
        Task SendMailVAT(long pid, string path);
        Task SendMailOrderCustomer(long pid);
        Task SendMailOrderToAdmin(long pid);
    }
}
