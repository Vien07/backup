using FluentValidation;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.SteamModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.EmailManagement.Database
{
    public class EmailMailBox: BaseEntity
    {
        public string? EmailTittle { get; set; }
        public string? EmailContent { get; set; }
        public string? EmailCode { get; set; }
        public bool? IsSendWithTemplate { get; set; } = false;
        public string EmailReceive { get; set; }
        public long? EmailAdminPid { get; set; }

        public bool? IsFirstSendSuccess { get; set; } = true;

        public string? ErrorDetail { get; set; }
        public int CountSendSuccess { get; set; } = 0;
        public int CountReSend { get; set; } = 0;

        public EmailMailBox()
        {

        }

        public EmailMailBox(string? title, string? content, string? emailReceive, bool? isSendWithTemplate, string? emailCode)
        {
            this.EmailTittle = title;
            this.EmailContent = content;
            this.EmailReceive = emailReceive;
            this.EmailCode = emailCode;
            this.IsSendWithTemplate = isSendWithTemplate;
        }
    }
    public class EmailMailBoxValidator : AbstractValidator<EmailMailBox>
    {
        public EmailMailBoxValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;

        }


    }
}
