using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.Product;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Repository
{
    public interface IProductRepository : IProductManagementRepository<Product>
    {
        IQueryable<ProductDetailDto> GetProductDetail(long parentId);
    }
}
