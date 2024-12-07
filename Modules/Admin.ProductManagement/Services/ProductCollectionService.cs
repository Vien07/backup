using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Repository;
using Admin.SEO;
using ComponentUILibrary.Models;
using Steam.Core.Base.Constant;
using Steam.Core.Common.SteamString;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using Admin.SEO.Services;
namespace Admin.ProductManagement.Services
{
    public class ProductCollectionService : IProductCollectionService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductManagementRepository<ProductCategory> _productCategoryRepository;
        private readonly IProductManagementRepository<ProductCategoryConfig> _productCategoryConfigRepository;

        private readonly ISEOService _srvSEO;
        public ProductCollectionService(
            IProductRepository productRepository, 
            IProductManagementRepository<ProductCategory> productCategoryRepository,
            IProductManagementRepository<ProductCategoryConfig> productCategoryConfigRepository,
            ISEOService seoRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productCategoryConfigRepository = productCategoryConfigRepository;

            _srvSEO = seoRepository;
        }

        public class Product_Item
        {
            public long Pid { get; set; }
            public string SKU { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string RootSlug { get; set; }
            public string Price { get; set; }
            public string Image { get; set; }
            public string ImagePath { get; set; }
            public string Font_Images { get; set; }
            public string Back_Images { get; set; }
            public string Alt_Font_Images { get; set; }
            public string Alt_Back_Images { get; set; }
            public string URL { get; set; }

        }
        public List<Product_Item> GetListProductByListSKU(string SKU)
        {
            List<Product_Item> rs = new List<Product_Item>();
            try
            {
                var RootSlug = _productCategoryConfigRepository.Query()
                            .Where(p => p.Key == ProductConstants.ConfigWebsite.PreSlug)
                            .FirstOrDefault().Value;

                var listProducts = _productRepository.Query().Where(p => ("," + SKU + ",").Contains(("," + p.Sku + ",")) && p.Enabled == true).ToList();
                var listSEO = _srvSEO.GetSEOsByModuleCode(ProductConstants.ModuleInfo.ModuleCode).ToList();
                var products = (from a in listProducts
                                join b in listSEO on a.Pid equals b.PostPid
                                select new Product_Item
                                {
                                    Pid = a.Pid,
                                    SKU = a.Sku,
                                    Title = a.Title,
                                    Slug = a.Slug,
                                    URL = @"/" + RootSlug + @"/" + a.Slug + @"/",
                                    RootSlug = RootSlug,
                                    Price = a.SellingPrice.FormatToMoney(),
                                    Image = a.FilePath + a.Images,
                                    ImagePath = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                                    Font_Images = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                                    Back_Images = SystemInfo.MedidaFileServer + a.FilePath + a.BackImages,
                                    Alt_Back_Images = a.BackImages_Alt,
                                    Alt_Font_Images = a.Images_Alt

                                }
                                ).ToList();
                rs = products;
            }
            catch (Exception ex)
            {


            }
            return rs;
        }
        public dynamic GetListProduct(dynamic search)
        {
            try
            {
                int pageIndex = Convert.ToInt32(search.PageIndex);
                string cateIds = search.CateIds ?? "";
                string keySearch = search.KeySearch ?? "";
                string listSKU = search.ChoosenProducts ?? "";
                var RootSlug = _productCategoryConfigRepository.Query()
                            .Where(p => p.Key == ProductConstants.ConfigWebsite.PreSlug)
                            .FirstOrDefault().Value;

                var listProducts = _productRepository.Query().Where(p => keySearch == "" || p.Title.Contains(@"/" + keySearch + @"/"))
                                    .Where(p => cateIds == "" || ("," + cateIds + "").Contains(("+" + p.CateID.ToString() + ","))).Where(p => p.Enabled == true)
                                    .Where(p => !("," + listSKU + ",").Contains(("," + p.Sku + ","))).ToList();

                var listSEO = _srvSEO.GetSEOsByModuleCode(ProductConstants.ModuleInfo.ModuleCode).ToList();
                var products = (from a in listProducts
                                join b in listSEO on a.Pid equals b.PostPid
                                select new Product_Item
                                {
                                    Pid = a.Pid,
                                    SKU = a.Sku,
                                    Title = a.Title,
                                    Slug = a.Slug,
                                    RootSlug = RootSlug,
                                    Price = a.SellingPrice.FormatToMoney(),
                                    Image = a.FilePath + a.Images,
                                    ImagePath = SystemInfo.MedidaFileServer + a.FilePath + a.Images

                                }
                                ).ToList();
                var productPaged = products.ToPagedList(pageIndex, 20);
                dynamic rs = new ExpandoObject();
                rs.ProductItems = productPaged;
                rs.PageNumber = productPaged.PageNumber;
                rs.PageSize = productPaged.PageSize;
                rs.PageCount = productPaged.PageCount;
                return rs;
            }
            catch (Exception ex)
            {

                return null;
            }
            //return rs;
        }
        public List<SelectControlData> GetProductCates()
        {
            var result = new List<SelectControlData>();
            try
            {
                var listCate = _productCategoryRepository.Query().Where(p => p.Enabled == true).ToList();
                result = listCate.Select(
                  row => new SelectControlData
                  {
                      Value = row.Pid.ToString(),
                      Name = row.Title,
                      ParrentID = row.ParentID,
                  }
                 ).ToList<SelectControlData>(); ;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
    }
}
