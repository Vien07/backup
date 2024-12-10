using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Product.Models;
using System.Collections.Generic;
using DTO.Common;
using DTO.Product;

namespace CMS.Areas.Product
{
    public interface IProductRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(ProductDetail ProductDetail, List<MultiLang_ProductDetail> multiLangProductDetail,
                           IFormFile Images, List<Temp_Images> listImagesProduct,
                           List<Temp_MultiLang_Images> listLangImagesProduct, string listCates, string productOptionList, string listColors, string listTypes, List<ProductPrice> listProductPrice);
        dynamic Update(ProductDetail ProductDetail,
            List<MultiLang_ProductDetail> multiLangProductDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesProduct, List<Temp_MultiLang_Images> listLangImagesProduct, string listCates, string productOptionList, string listColors, string listTypes, List<ProductPrice> listProductPrice);
        bool Count(int code);
        bool Preview(string obj, string objDetail, IFormFile Images);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
        string GetProductOption();
        bool SaveStatus(long pid, bool value, string type);
        string LoadProductCode();
        dynamic GetProductType(string lang);
    }
}