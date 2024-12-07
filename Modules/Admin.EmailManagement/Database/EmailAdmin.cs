
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.EmailManagement.Database
{
    public class EmailAdmin: BaseEntity
    {
        public string EmailAddress { get; set; }
        public string? Sender { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string Port { get; set; }
    }
    public class EmailAdminValidator : AbstractValidator<EmailAdmin>
    {
        public EmailAdminValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

