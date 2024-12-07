
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.SEO.Database
{
    public class SEO : BaseEntity
    {
        public string? Description { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        [Required(ErrorMessage = "Required")]
        public string PostSlug { get; set; } = String.Empty;
        public string PostTitle { get; set; } = String.Empty;
        public string? TagKeys { get; set; } = String.Empty;
        public string? MetaDescription { get; set; } = String.Empty;
        public string? Meta { get; set; } = String.Empty;
        [Required(ErrorMessage = "Required")]
        public long PostPid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? ModuleCode { get; set; }
        public string? ExtraMeta { get; set; }
        public string? OgType { get; set; } = "article";
        public int CountView
        {
            get; set;


        }
        public class SEOValidator : AbstractValidator<SEO>
        {
            public SEOValidator()
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
}

