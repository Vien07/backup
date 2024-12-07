
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.EmailManagement.Database
{
    public class EmailTemplate: BaseEntity
    {
        [Required]
       public string EmailCode { get; set; }
       public string EmailName { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Header { get; set; }
        public string? Footer { get; set; }
        public string? Key { get; set; }
        public long? EmailAdminPid { get; set; }
    }
    public class EmailTemplateValidator : AbstractValidator<EmailTemplate>
    {
        public EmailTemplateValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

