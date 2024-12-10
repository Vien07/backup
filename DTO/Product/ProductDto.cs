using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductColorDto
    {
        public long Pid { get; set; }
        public string Title { get; set; }
        public string PicThumb { get; set; }
        public string Code { get; set; }
    }
    public class ProductDto
    {
        public long Pid { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public ProductCateDto ProductCate { get; set; }
        public string TitleSEO { get; set; }
        public string Description { get; set; }
        public string DescriptionSEO { get; set; }
        public string Tiki { get; set; }
        public string Lazada { get; set; }
        public string Shopee { get; set; }
        public string Origin { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public string PriceString { get; set; }
        public decimal PriceDiscount { get; set; }
        public string PriceDiscountString { get; set; }
        public string Code { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Content2 { get; set; }
        public string Material { get; set; }
        public string Size { get; set; }
        public string PicThumb { get; set; }
        public string PicFull { get; set; }
        public string ImageMeta { get; set; }
        public string PublishDate { get; set; }
        public string SlugTagKey { get; set; }
        public string TagKey { get; set; } = string.Empty;
        public bool Enabled { get; set; }
        public bool IsHot { get; set; }
        public bool IsNew { get; set; }
        public string CateName { get; set; }
        public string CateSlug { get; set; }
        public List<string> ImageList { get; set; }

        public List<ProductColorDto> Colors { get; set; }
        public List<ProductOptionDto> Options { get; set; }
        public List<ProductCommentDto> Comments { get; set; }
    }
}
