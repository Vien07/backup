
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LogManagement.Database
{
    public class LogVisitor : BaseDatabaseModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int AccessTimes{ get; set; }
    }
    public class LogVisitorValidator : AbstractValidator<LogVisitor>
    {
        public LogVisitorValidator()
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

