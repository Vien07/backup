
using Admin.Collection.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Collection.Api.Models.Response
{
    public class GetAllListSEOActive
    {

        public string PostSlug { get; set; }
        public string CateSlug { get; set; } 
        public long PostPid { get; set; } 
        public long? CatePid { get; set; }
    }
    public class GetListCollectionName
    {

        public string RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
        public string? Title { get; set; } 
        public double? Order { get; set; }
    }
    public class GetDetailCollection:Database.Collection
    {

        public string RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
        public string? ImagePath { get; set; } = String.Empty;
        public string? Title { get; set; } = String.Empty;
        public string? Meta { get; set; } = String.Empty;
        public List<Collection_Files> ListImages { get; set; }
        public List<Product_Item> ListProducts { get; set; }
        public class Product_Item
        {
            public string Id { get; set; }
            public string SKU { get; set; }
            public string Title { get; set; }
            public string? URL { get; set; }
            public string Slug { get; set; }
            public string RootSlug { get; set; }
            public string Price { get; set; }
            public string Font_Images { get; set; }
            public string Back_Images { get; set; }
            public string Alt_Font_Images { get; set; }
            public string Alt_Back_Images { get; set; }
                
        }
    }
    public class GetListProductWithCollection
    {

        public string TabName { get; set; }
        public string Description { get; set; } 
        public List<Image> Images { get; set; } 
        public List<Product> Products { get; set; } 
        public class Image
        {
            public string ImagePath { get; set; }
            public string ImageAlt { get; set; }
        }
        public class Product
        {

            public string Slug { get; set; }
            public string Title { get; set; }
            public string CateSlug { get; set; }
            public string Price { get; set; }
            public Image ImagesAfter { get; set; }
            public Image ImagesBefore { get; set; }
        }
    }
}
