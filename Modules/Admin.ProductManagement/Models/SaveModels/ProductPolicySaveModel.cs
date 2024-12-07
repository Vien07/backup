using Admin.ProductManagement.Database;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SaveModels
{
    public class ProductPolicySaveModel
    {
        public long Pid { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }
        public string? Content { get; set; }
        public double? Order { get; set; }
        public string? CreateUser { get; set; }
        public string? UpdateUser { get; set; }
    }

    public class ProductPolicySaveModelValidator : AbstractValidator<ProductPolicySaveModel>
    {
        public ProductPolicySaveModelValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }
    }
}
