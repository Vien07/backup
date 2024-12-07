
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.WebSetting.Database
{
    public class WebsiteConfiguration : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
    }

    public class ConfigurationValidator : AbstractValidator<WebsiteConfiguration>
    {
        public ConfigurationValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }
}

