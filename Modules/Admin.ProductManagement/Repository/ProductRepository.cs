using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Repository
{
    public class ProductRepository : ProductManagementRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductManagementContext context) : base(context) { }

        public IQueryable<ProductDetailDto> GetProductDetail(long parentId)
        {
            var query = from a in Context.Set<ProductDetail>()
                        join b in Context.Set<ProductSpecificaty>() on a.ColorCode equals b.Code
                        into pd_ps
                        from c in pd_ps.DefaultIfEmpty()
                        where a.ParentPid == parentId
                        select new ProductDetailDto
                        {
                            Pid = a.Pid,
                            ParentPid = a.ParentPid,
                            Title = a.Title,
                            ColorValue = c.Value,
                            Sku = a.Sku,
                            Size = a.Size,
                            SellingPrice = a.SellingPrice,
                            ColorCode = a.ColorCode,
                            ColorName = a.Color,
                        };
            return query;
        }
    }
}
