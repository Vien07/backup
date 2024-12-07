using ComponentUILibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Admin.ProductManagement.Services.ProductCollectionService;

namespace Admin.ProductManagement.Services
{
    public interface IProductCollectionService
    {
        List<Product_Item> GetListProductByListSKU(string SKU);
        dynamic GetListProduct(dynamic search);
        List<SelectControlData> GetProductCates();
    }
}
