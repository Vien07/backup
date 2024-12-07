using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Api.Controllers
{
    public partial class ApiProductsController : Controller
    {

        [HttpGet("GetListProductHomePage")]
        //[CustomAuthorization]
        public Response<List<Dictionary<string, string>>> GetListProductHomePage(string? type, int takeitem)
        {
            Api.Models.Request.GetListProductsByType input = new Api.Models.Request.GetListProductsByType();
            input.Type = type;
            input.TakeItem = takeitem;
            Response<List<Dictionary<string, string>>> rs = new Response<List<Dictionary<string, string>>>();
            try
            {
                var listData = new List<Dictionary<string, string>>();
                var listProduct = _rep.GetListProductsByType(input).Data;
                foreach (var item in listProduct)
                {
                    Dictionary<string, string> tempItem = new Dictionary<string, string>()
                   {
                       {"{{SECTION_ITEM_NAME}}",item.Title},
                       {"{{SECTION_ITEM_IMAGE_FONT}}",item.Font_Image_Path},
                       {"{{SECTION_ITEM_IMAGE_BACK}}",item.Back_Image_Path},
                       {"{{SECTION_ITEM_IMAGE_BACK_ALT}}",item.Back_Image_Alt},
                       {"{{SECTION_ITEM_IMAGE_FONT_ALT}}",item.Font_Image_Alt},
                       {"{{SECTION_ITEM_PRICE}}",item.Price},
                       {"{{SECTION_ITEM_URL}}",item.URL}
                   };
                    listData.Add(tempItem);
                }
                #region sample
                //Dictionary<string, string> listItem1 = new Dictionary<string, string>()
                //   {
                //       {"{{SECTION_ITEM_NAME}}","Sản phẩm 1"},
                //       {"{{SECTION_ITEM_IMAGE}}",""},
                //       {"{{SECTION_ITEM_PRICE}}","1.790.000"},
                //   };
                //Dictionary<string, string> listItem2 = new Dictionary<string, string>()
                //   {
                //       {"{{SECTION_ITEM_NAME}}","Sản phẩm 2"},
                //       {"{{SECTION_ITEM_Image}}",""},
                //       {"{{SECTION_ITEM_PRICE}}","790.000"},
                //   };

                //listData.Add(listItem1);
                //listData.Add(listItem2);
                #endregion
                rs.Data = listData;
                rs.IsError = false;
                rs.StatusCode = 200;
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.StatusCode = 500;
                rs.Message = "Error Message: " + ex.ToString();
                return rs;
            }

            return rs;
        }
    }

}
