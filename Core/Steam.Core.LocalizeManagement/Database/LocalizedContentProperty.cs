
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using Steam.Infrastructure.Models;

namespace Steam.Core.LocalizeManagement.Database
{
    public class LocalizedContentProperty: EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long EntityID { get; set; }
        public string CultureID { get; set; }
        public string ProperyName { get; set; }
        public string Value { get; set; }
        public string EntityType { get; set; }

    }
    public class LocalizedContentPropertyValidator : AbstractValidator<LocalizedContentProperty>
    {
        public LocalizedContentPropertyValidator()
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

