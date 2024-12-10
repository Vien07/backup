using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Promotion.Models;
using System.Collections.Generic;
using DTO.Common;
using DTO.Product;

namespace CMS.Areas.Promotion
{
    public interface IPromotionRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(PromotionDetail newsDetail, List<MultiLang_PromotionDetail> multiLangPromotionDetail,
                           IFormFile Images, List<Temp_Images> listImagesPromotion,
                           List<Temp_MultiLang_Images> listLangImagesPromotion, string listCates);
        dynamic Update(PromotionDetail newsDetail,
            List<MultiLang_PromotionDetail> multiLangPromotionDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesPromotion, List<Temp_MultiLang_Images> listLangImagesPromotion, string listCates);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Preview(string obj, string objDetail, IFormFile PicThumb);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
        bool SaveStatus(long pid, bool value, string type);
        List<ProductDto> GetAllProduct();
        List<PromoProduct> GetAllPromoProduct(List<long> listPid);
        List<PromoProduct> GetPromo(long pid);
        bool UpdatePromotionProduct(List<PromoProduct> data, long pid);
        string CheckValid(List<long> listPid, long pid);
        dynamic LoadProducts(SearchDto search);
    }
}