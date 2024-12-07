
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Admin.TemplatePage.Database
{
    public class TemplatePage: BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Url { get; set; }
        public string? Description { get; set; } = String.Empty;
        public string? PageType  { get; set; } 
        public string? Parameters { get; set; } 
        public string? Image { get; set; } 
        public string? Image_Alt { get; set; } 
        public string? Image_Description { get; set; } 
        public string? Image_Caption { get; set; } 
        public string? FilePath { get; set; } 
    }
    public class TemplatePageValidator : AbstractValidator<TemplatePage>
    {
        public TemplatePageValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage(nameof(TemplatePage.Title) + " - " + ErrorCode.NotEmpty);
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

