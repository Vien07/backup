
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Database
{
    public class LocalizeManagement: BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Key { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = String.Empty;
        public string? Value { get; set; } = String.Empty;
        public string? Group { get; set; } = String.Empty;
        public bool isSystemKey { get; set; } 
        public bool isMedia { get; set; } 
        public string? MediaPath { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
    }
    public class LocalizeManagementValidator : AbstractValidator<LocalizeManagement>
    {
        public LocalizeManagementValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage(nameof(LocalizeManagement.Title) + " - " + ErrorCode.NotEmpty);
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

