using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SaveModels
{
    public class ProductSpecificatySaveModel : ProductSpecificatyDto
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile Files { get; set; }
        public string FileStatus { get; set; }
        public string FilePath { get; set; }
    }

    public class ProductSpecificatySaveModelValidator : AbstractValidator<ProductSpecificatySaveModel>
    {
        public ProductSpecificatySaveModelValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }
    }
}
