
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Steam.Core.LocalizeManagement.Database
{
    public class LocalizeCulture : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public string LangCode { get; set; }
        
        public string ShortCode { get; set; }  
        
        public string Name { get; set; }

        public string SlugKey { get; set; }
    }
    public class LocalizeCultureValidator : AbstractValidator<LocalizeCulture>
    {
        public LocalizeCultureValidator()
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

