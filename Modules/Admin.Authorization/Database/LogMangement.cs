
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class LogManagement: BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string ActionName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ActionUrl { get; set; }
        public string ActionData { get; set; }
        public bool Status { get; set; }
    }
    public class LogManagementValidator : AbstractValidator<LogManagement>
    {
        public LogManagementValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage(nameof(LogManagement.Title) + " - " + ErrorCode.NotEmpty);
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

