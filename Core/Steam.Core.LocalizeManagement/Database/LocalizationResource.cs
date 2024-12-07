
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using Steam.Core.LocalizeManagement.Database;

namespace Steam.Core.LocalizeManagement.Database
{
    public class LocalizationResource
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long CultureID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public class LocalizationResourceValidator : AbstractValidator<LocalizationResource>
    {
        public LocalizationResourceValidator()
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

