
using Admin.Collection.Api.Models;
using Admin.Collection.Api.Models.Response;
using Admin.Collection.Models;
using Microsoft.AspNetCore.Mvc;

using NLog;
using Steam.Core.Base.Models;
using System.Dynamic;

namespace Admin.Collection.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiCollectionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IApiCollectionRepository _rep;
        public ApiCollectionController(IApiCollectionRepository rep)
        {
            _rep = rep;
        }


        [HttpGet("GetListProductWithCollectionHomePage")]
        //[CustomAuthorization]
        public Response<dynamic> GetListProductWithCollection()
        {
            Response<dynamic> rs = new Response<dynamic>();
            try
            {
                var collections = new List<dynamic>();
                var listCollection = _rep.GetListProductWithCollection().Data;
                foreach (var item in listCollection)
                {
                    var listProducts = new List<Dictionary<string, string>>();

                    dynamic collection_Item = new ExpandoObject();
                    collection_Item.TabName = item.Title;
                    collection_Item.ImagePath = item.ImagePath;
                    collection_Item.Images_Alt = item.Images_Alt;
                    collection_Item.Pid = item.Pid;
                    foreach (var productItem in item.ListProducts)
                    {
                        Dictionary<string, string> product_Item = new Dictionary<string, string>()
                   {
                       {"{{SECTION_ITEM_NAME}}",productItem.Title},
                       {"{{SECTION_ITEM_IMAGE}}",productItem.Font_Images},
                       {"{{SECTION_ITEM_URL}}",productItem.URL},
                       {"{{SECTION_ITEM_PRICE}}",productItem.Price },
                       {"{{SECTION_ITEM_IMAGE_FONT}}",productItem.Font_Images},
                       {"{{SECTION_ITEM_IMAGE_BACK}}",productItem.Back_Images},
                       {"{{SECTION_ITEM_IMAGE_FONT_ALT}}",productItem.Alt_Font_Images},
                       {"{{SECTION_ITEM_IMAGE_BACK_ALT}}",productItem.Alt_Back_Images}
                   };
                        listProducts.Add(product_Item);
                    }
                    collection_Item.ListItems = listProducts;
                    collections.Add(collection_Item);   
                }
                //var listData = new List<Dictionary<string, string>>();
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
                //var listCate = new[]
                // {
                //     new
                //     {
                //         Cate = "Baby face",
                //         ListItem =listData
                //     },
                //     new
                //     {
                //         Cate = "Art work",
                //         ListItem =listData

                //     },
                //     new
                //     {
                //         Cate = "Logo Play",
                //         ListItem =listData

                //     },
                //     new
                //     {
                //         Cate = "Teddyb Bear",
                //         ListItem =listData

                //     }
                //  };

                rs.Data = collections;
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

        [HttpGet("GetListCollectionName")]
        //[CustomAuthorization]
        public Response<List<GetListCollectionName>> GetListCollectionName()
        {
            Response<List<GetListCollectionName>> rs = new Response<List<GetListCollectionName>>();
            try
            {
                rs = _rep.GetListCollectionName();
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
        [HttpGet("GetDetailCollection")]
        //[CustomAuthorization]
        public Response<GetDetailCollection> GetDetailCollection(string? slug)
        {
            Response<GetDetailCollection> rs = new Response<GetDetailCollection>();
            try
            {

                rs = _rep.GetDetailCollection(slug);
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
