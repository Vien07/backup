
using Admin.ProductManagement.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.ProductManagement.Api.Models.Response
{
    public class GetMisaProductBySlug
    {
        public Database.Product Parent { get; set; }
        public List<Database.ProductDetail> Children { get; set; }
        public List<SEO.Database.SEO> SEO { get; set; }
    }

    public class CreateMisaOrder
    {
    }
    public class GetAllListSEOActive
    {

        public string PostSlug { get; set; }
        public string CateSlug { get; set; }
        public long PostPid { get; set; }
        public long? CatePid { get; set; }
    }
    public class GetListCateByCateSlug : ProductCategory
    {
        public string CateSlug { get; set; }
        public string RootSlug { get; set; }
        public string URL { get; set; }

    }
    public class GetListProductsByCateSlug
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Font_Image_Path { get; set; }
        public string Back_Image_Path { get; set; }
        public string Font_Image_Alt { get; set; }
        public string Back_Image_Alt { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
        public string CateSlug { get; set; }
        public string RootSlug { get; set; }
        public string PublishDate { get; set; }
        public string Price { get; set; }
        public string URL { get; set; }
    }
    public class GetListProductsBySlug
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Images_Alt { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
        public string BackImages { get; set; }
        public string BackImagesPath { get; set; }
        public string CateSlug { get; set; }
        public string PublishDate { get; set; }
        public decimal SellingPrice { get; set; }
    }
    public class GetListProductsByType
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Font_Image_Path { get; set; }
        public string Back_Image_Path { get; set; }
        public string Font_Image_Alt { get; set; }
        public string Back_Image_Alt { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
        public string CateSlug { get; set; }
        public string PublishDate { get; set; }
        public string Price { get; set; }
        public string URL { get; set; }
    }
    public class GetListColorProduct : ProductSpecificaty
    {

    }
    public class GetListProductSpecificaties : ProductSpecificaty
    {

    }
    public class GetListProductTypes
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class GetCateDetail
    {
        public string Meta { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string RootSlug { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }
    }
    public class GetProductBySlug : Admin.ProductManagement.Database.Product
    {

        public string Slug { get; set; }
        public string Meta { get; set; }
        public string CateSlug { get; set; }
        public string ImagesPath { get; set; }
        public List<ListPolicies> listPolicies { get; set; }
        public List<ProductDetail> listProductChilds { get; set; }
        public List<Product_Files> listProductImages { get; set; }
        public ProductCategory Category { get; set; }
        public class ListPolicies
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string Group { get; set; }
        }
    }

    public class MisaAddress 
    {

        
    }
}
