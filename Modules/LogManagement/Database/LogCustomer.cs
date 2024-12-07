
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LogManagement.Database
{
    public class LogCustomer : BaseDatabaseModel
    {
        public string Ip { get; set; }
        public string Device { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public bool Status { get; set; }
    }
    public class LogCustomerValidator : AbstractValidator<LogVisitor>
    {
        public LogCustomerValidator()
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

