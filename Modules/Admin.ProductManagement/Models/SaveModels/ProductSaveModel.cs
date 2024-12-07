using Admin.ProductManagement.DataTransferObjects.Product;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SaveModels
{
    public class ProductSaveModel : ProductDto
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile Files { get; set; }
        public string FileStatus { get; set; }
        public IFormFile BackFiles { get; set; }
        public string? BackFileStatus { get; set; }
        public string? BackFilePath { get; set; }
    }

    public class ProductSaveModelValidator : AbstractValidator<ProductSaveModel>
    {
        public ProductSaveModelValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }
    }
}
