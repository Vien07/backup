using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.SaveModels
{
    public class ProductCategorySaveModel : ProductCategoryDto
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile file_Thumbnail { get; set; }
        public string FileStatus_Thumbnail { get; set; }
        public string FilePath_Thumbnail { get; set; }

        public IFormFile file_Banner { get; set; }
        public string FileStatus_Banner { get; set; }
        public string FilePath_Banner { get; set; }
    }

    public class ProductCategorySaveModelValidator : AbstractValidator<ProductCategorySaveModel>
    {
        public ProductCategorySaveModelValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }
    }
}
