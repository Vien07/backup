
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Admin.MemberManagement.Database
{
    public partial class Feedback : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SKU { get; set; }
        public string? Rating { get; set; } = String.Empty;
        public string? Content { get; set; } = String.Empty;
        public string? Images { get; set; } = String.Empty;
        public string? FilePath { get; set; } = String.Empty;
        public string? Images_Caption { get; set; } = String.Empty;
        public string? Images_Description { get; set; } = String.Empty;
        public string? Images_Alt { get; set; } = String.Empty;
        public string? Link { get; set; } = String.Empty;
        public string? Position { get; set; } = String.Empty;
        public long CateID { get; set; }
        public string? SubCate { get; set; }
        public bool isNew { get; set; } = false;
        public bool Approval { get; set; } = false;


    }
    public class FeedbackValidator : AbstractValidator<Feedback>
    {
        public FeedbackValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage(nameof(Feedback.Title) + " - " + ErrorCode.NotEmpty); 
            //RuleFor(x => x.Description).NotEmpty().WithMessage(nameof(Feedback.Description) + " - " + ErrorCode.NotEmpty); 
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

