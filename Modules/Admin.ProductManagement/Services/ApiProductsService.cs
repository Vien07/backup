using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using System.Dynamic;
using Steam.Core.Common;
using Microsoft.Extensions.Configuration;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Constants;
using static Admin.ProductManagement.Api.Models.Response.GetProductBySlug;
using Admin.ProductManagement.Api.Models.Response;

namespace Admin.ProductManagement.Services
{
    public class ApiProductsService : IApiProductsService
    {
        private readonly IConfiguration configuration;

        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        ProductManagementContext _db;
        SEOContext _dbSEO;

        public ApiProductsService(ProductManagementContext db,
            SEOContext dbSEO, IFileHelper fileHelper, ILoggerHelper logger)
        {
            _db = db;
            _dbSEO = dbSEO;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.ProductConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            //SystemInfo.MedidaFileServer = configuration["SystemConfig:SystemInfo.MedidaFileServer"].ToString();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //double limitRamGB = 0.11;
            //long UsedMemory = System.Diagnostics.Process.GetCurrentProcess().PagedMemorySize64;
            //double limitRamBytes = limitRamGB * 1024 * 1024 * 1024;
            //if (UsedMemory > limitRamBytes)
            //{
            //    GC.Collect(); // Collect all generations
            //      GC.Collect(2,GCCollectionMode.Forced);
            //}
        }
        public Response<dynamic> GetProductBySlug(Api.Models.Request.GetProductBySlug input)
        {
            Response<dynamic> rs = new Response<dynamic>();
            try
            {
                long postPid = 0;
                var tempSeo = _dbSEO.SEOs.Where(p => p.PostSlug == input.PostSlug && p.ModuleCode == "ProductPost").FirstOrDefault();
                if (tempSeo != null)
                {
                    tempSeo.CountView = tempSeo.CountView + 1;

                    postPid = tempSeo.PostPid;
                }
                Database.Product tempProduct = _db.Products.Where(p => p.Enabled == true).Where(p => p.Pid == postPid).FirstOrDefault();
                Api.Models.Response.GetProductBySlug product = new Api.Models.Response.GetProductBySlug();
                if (tempProduct != null)
                {
                    var ProductPolicyIds = tempProduct.ProductPolicyIds ?? "";
                    List<Database.ProductPolicy> tempPolicies = _db.ProductPolicies.Where(p => ("," + ProductPolicyIds + ",").Contains("," + p.Pid.ToString() + ",")).ToList();
                    var tempListProductChilds = (from a in _db.ProductDetails
                                                 join b in _db.ProductSpecificaties on a.ColorCode equals b.Code into product_color
                                                 from c in product_color.DefaultIfEmpty()
                                                 where a.ParentPid == tempProduct.Pid && a.Enabled == true
                                                 select new ProductDetail
                                                 {
                                                     Pid = a.Pid,
                                                     ParentPid = a.ParentPid,
                                                     Title = c.Name,
                                                     Color = c.Value,
                                                     Sku = a.Sku,
                                                     Size = a.Size,
                                                     SellingPrice = a.SellingPrice,
                                                     ColorCode = a.ColorCode,
                                                 }).ToList();
                    List<Database.Product_Files> tempListImages = _db.Product_Files.Where(p => p.MisaProductId == tempProduct.Pid).ToList();
                    List<ListPolicies> listPoli = tempPolicies.DeepClone<List<ListPolicies>>();
                    product = tempProduct.DeepClone<Api.Models.Response.GetProductBySlug>();
                    product.listPolicies = listPoli;
                    product.listProductChilds = tempListProductChilds;
                    product.listProductImages = tempListImages;
                    product.Slug = tempSeo.PostSlug;
                    //post.CateSlug= tempSeo.PostSlug;
                    product.Meta = tempSeo.Meta.ToRemoveBreakSympol();
                    product.ImagesPath = SystemInfo.MedidaFileServer + product.FilePath + product.Images;
                    product.BackFilePath = SystemInfo.MedidaFileServer + product.BackFilePath + product.BackImages;
                    product.Category = _db.ProductCategories.Where(x => x.Pid == product.CateID).FirstOrDefault();
                }
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = product;
                try { _dbSEO.SaveChanges(); } catch (Exception ex) { }
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        public ResponseList<dynamic> GetListProductsByCateSlug(Api.Models.Request.GetListProductsByCateSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();

            try
            {
                decimal priceFrom = 0;
                decimal priceTo = 0;
                if (!String.IsNullOrEmpty(input.Price))
                {
                    var tempPrice = input.Price.Split("_");
                    if (tempPrice.Length == 2)
                    {
                        priceFrom = Convert.ToDecimal(tempPrice[0]);
                        priceTo = Convert.ToDecimal(tempPrice[1]);
                    }
                    else
                    {
                        priceFrom = 0;
                        priceTo = Decimal.MaxValue;

                    }

                }
                long catePid = 0;
                var rootSlug = _config[ProductConstants.ConfigWebsite.PreSlug];
                if (input.RootSlug == _config[ProductConstants.ConfigWebsite.PreSlug])
                {
                    // nếu preslug là san-pham

                }
                else
                {
                    var tempRootCate = _db.ProductCategories.Where(p => p.Slug == input.RootSlug && p.ParentID == 0).FirstOrDefault();
                    if (tempRootCate != null)
                    {
                        catePid = tempRootCate.Pid;
                    }
                    if (!String.IsNullOrEmpty(input.CateSlug))
                    {
                        var cate = _db.ProductCategories.Where(p => p.Slug == input.CateSlug).FirstOrDefault();
                        var cateSEO = _dbSEO.SEOs.Where(p => p.PostSlug == input.CateSlug && p.ModuleCode == ProductCategoryConstants.ModuleInfo.ModuleCode).FirstOrDefault();
                        rs.Meta = cateSEO.Meta.ToRemoveBreakSympol();

                        catePid = cate.Pid;

                    }
                }

                var listProduct = _db.Products.Where(p => catePid == 0 || (p.CateID == catePid || ("," + p.SubCate + ",").Contains("," + catePid + ",")))
                                                            .AsEnumerable()   // tim danh mục
                                                                              // .Where(p=> input.Type == ""||( (","+p.TypeProduct+",").Contains(","+input.Type+","))) // tìm loại sản phẩm
                                                  .Where(p => input.Type == "" || input.FilterType(p.TypeProduct)) // tìm loại sản phẩm
                                                  .Where(p => String.IsNullOrEmpty(input.Price) || (p.SellingPrice >= priceFrom && p.SellingPrice <= priceTo))//tìm giá
                                                  .Where(p => String.IsNullOrEmpty(input.KeySearch)
                                                    || (p.Sku.RemoveSign4VietnameseString().ToLower().Trim().Contains(input.KeySearch.ToLower().Trim())
                                                    || p.Title.RemoveSign4VietnameseString().ToLower().Trim().Contains(input.KeySearch.ToLower().Trim())
                                                    || p.SellingPrice.ToString().Contains(input.KeySearch)
                                                    || p.SellingPrice.ToString().Contains(input.KeySearch)
                                                    ))
                                                  .ToList();
                if (input.SortBy == "newest")
                {
                    listProduct = listProduct.OrderByDescending(p => p.CreateDate).ToList();
                }
                else if (input.SortBy == "price_high_low")
                {
                    listProduct = listProduct.OrderByDescending(p => p.SellingPrice).ToList();
                }
                else if (input.SortBy == "price_high_low")
                {
                    listProduct = listProduct.OrderBy(p => p.SellingPrice).ToList();

                }

                var listChildProduct = _db.ProductDetails.ToList();
                var listSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == ProductConstants.ModuleInfo.ModuleCode)
                    .ToList();
                var posts = (
                        from a in listProduct
                        join b in listSeo on a.Pid equals b.PostPid
                        join c in listChildProduct.Where(p => (String.IsNullOrEmpty(input.Color) || input.FilterColor(p.Color))
                                                      && (String.IsNullOrEmpty(input.Size) || input.FilterSize(p.Size))

                          )
                        on a.Pid equals c.ParentPid
                        select new Api.Models.Response.GetListProductsByCateSlug
                        {
                            Title = a.Title,
                            Slug = b.PostSlug,
                            Description = a.Description,
                            RootSlug = rootSlug,
                            URL = rootSlug + a.Slug + "",
                            Price = a.SellingPrice.FormatToMoney(),
                            Font_Image_Path = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                            Back_Image_Path = SystemInfo.MedidaFileServer + a.FilePath + a.BackImages,
                            Font_Image_Alt = a.Images_Alt,
                            Back_Image_Alt = a.BackImages_Alt
                        }).DistinctBy(p => p.Slug).ToList().ToPagedList(input.PageIndex, input.PageSize);

                rs.IsError = false;

                rs.StatusCode = 200;
                rs.PageCount = posts.PageCount;
                rs.PageIndex = posts.PageNumber;
                rs.PageSize = posts.PageSize;
                rs.TotalItem = posts.LastItemOnPage;
                rs.Data = posts;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListNewProductsByCateSlug(Api.Models.Request.GetListNewProductsByCateSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();
            try
            {

                long catePid = 0;
                var tempRootCate = _db.ProductCategories.Where(p => p.Slug == input.RootSlug && p.ParentID == 0).FirstOrDefault();
                if (tempRootCate != null)
                {
                    catePid = tempRootCate.Pid;
                }
                if (!String.IsNullOrEmpty(input.CateSlug))
                {
                    var cate = _db.ProductCategories.Where(p => p.Slug == input.CateSlug).FirstOrDefault();
                    catePid = cate.Pid;

                }
                var listPost = _db.Products.Where(p => p.CateID == catePid && p.isNew == true).OrderBy(r => Guid.NewGuid()).Take(10).ToList();
                var listSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == "ProductPost").ToList();
                var posts = (
                       from a in listPost
                       join b in listSeo on a.Pid equals b.PostPid
                       where (a.CateID == catePid)
                       select new Api.Models.Response.GetListProductsBySlug
                       {
                           Title = a.Title,
                           Slug = b.PostSlug,
                           Description = a.Description,
                           Images = a.Images,
                           CateSlug = input.RootSlug,
                           ImagesPath = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                           Images_Alt = a.Images_Alt
                       }).ToList();
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = posts;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListRelateProductsByProductSlug(Api.Models.Request.GetListRelateProductsByProductSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();
            try
            {
                var listPost = new List<Database.Product>();

                long postPid = 0;
                var postSEO = _dbSEO.SEOs.Where(p => p.PostSlug == input.PostSlug && p.ModuleCode == "ProductPost").FirstOrDefault();
                if (postSEO != null)
                {
                    postPid = postSEO.PostPid;
                    var post = _db.Products.Where(p => p.Pid == postPid).FirstOrDefault();
                    var cateSub = post.SubCate;
                    long catePid = post.CateID;
                    if (!String.IsNullOrEmpty(cateSub))
                    {
                        var listCateSub = cateSub.Split(',');
                        var tempN = 0;
                        for (int i = 0; i <= input.TakeItem; i++)
                        {
                            var temp = false;
                            while (!temp)
                            {
                                var randomCate = listCateSub.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                                var randomPost = _db.Products.Where(p => p.Pid != postPid && ("," + p.SubCate + ",").Contains("," + randomCate + ",")).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                                var temppost = listPost.Where(p => p.Pid == randomPost.Pid).FirstOrDefault();
                                if (temppost == null)
                                {
                                    temp = true;

                                    listPost.Add(randomPost);
                                }
                                if (tempN >= input.TakeItem)
                                {
                                    temp = true;

                                }
                                tempN++;

                            }

                            if (tempN >= input.TakeItem)
                            {
                                break;

                            }
                        }
                    }
                    else
                    {
                        listPost = _db.Products.Where(p => p.CateID == catePid && p.Pid != postPid).OrderBy(x => Guid.NewGuid()).Take(input.TakeItem).ToList();


                    }
                    if (listPost.Count < input.TakeItem)
                    {
                        //var randomPost = _db.MisaProducts.Where(p => p.CateID == catePid && p.Pid != postPid).OrderBy(x => Guid.NewGuid()).Take(input.TakeItem).ToList();

                    }

                }
                else
                {



                }
                var listSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == "ProductPost").ToList();

                var posts = (
                    from a in listPost
                    join b in listSeo on a.Pid equals b.PostPid
                    select new Api.Models.Response.GetListProductsBySlug
                    {
                        Title = a.Title,
                        Slug = b.PostSlug,
                        Description = a.Description,
                        CateSlug = input.RootSlug,
                        SellingPrice = a.SellingPrice,
                        Images = a.Images,
                        ImagesPath = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                        BackImages = a.BackImages,
                        BackImagesPath = SystemInfo.MedidaFileServer + a.BackFilePath + a.BackImages,
                        Images_Alt = a.Images_Alt
                    }).ToList();
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = posts;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<List<Api.Models.Response.GetListProductsByType>> GetListProductsByType(Api.Models.Request.GetListProductsByType input)
        {
            ResponseList<List<Api.Models.Response.GetListProductsByType>> rs = new ResponseList<List<Api.Models.Response.GetListProductsByType>>();
            try
            {
                var RootSlug = _db.ProductCategoryConfigs
                            .Where(p => p.Key == ProductConstants.ConfigWebsite.PreSlug)
                            .FirstOrDefault().Value;
                var listProduct = new List<Database.Product>();
                listProduct = _db.Products.Where(p => p.Enabled == true
                            && (string.IsNullOrEmpty(input.Type) || ("," + p.TypeProduct + ",").Contains("," + input.Type + ",")))
                .OrderBy(arg => Guid.NewGuid()).Take(input.TakeItem).ToList();

                var listSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == Constants.ProductConstants.ModuleInfo.ModuleCode).ToList();
                var listCate = _db.ProductCategories.ToList();
                var products = (
                    from a in listProduct
                    join b in listSeo on a.Pid equals b.PostPid
                    join c in listCate on a.CateID equals c.Pid
                    select new Api.Models.Response.GetListProductsByType
                    {
                        Title = a.Title,
                        Slug = b.PostSlug,
                        Description = a.Description,
                        CateSlug = c.Slug,
                        URL = "/" + RootSlug + "/" + a.Slug + "/",
                        Price = a.SellingPrice.FormatToMoney(),
                        Font_Image_Path = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                        Back_Image_Path = SystemInfo.MedidaFileServer + a.FilePath + a.BackImages,
                        Font_Image_Alt = a.Images_Alt,
                        Back_Image_Alt = a.BackImages_Alt
                    }).ToList();
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = products;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListCateByCateSlug(Api.Models.Request.GetListCateByCateSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();

            try
            {
                var rootSlug = _db.ProductCategoryConfigs.Where(p => p.Key == ProductCategoryConstants.Config.Website.PreSlugCate).FirstOrDefault().Value;
                long catePid = 0;
                var listCateSlug = new List<SEO.Database.SEO>();
                var listCate = new List<Database.ProductCategory>();
                if (String.IsNullOrEmpty(input.CateSlug))
                {
                    listCate = _db.ProductCategories.Where(p => p.ParentID == 0 && p.Enabled == true).ToList();

                }
                else
                {
                    var tempCate = _db.ProductCategories.Where(p => p.Slug == input.CateSlug).FirstOrDefault();
                    if (tempCate != null)
                    {
                        listCate = _db.ProductCategories.Where(p => p.ParentID == tempCate.Pid && p.Enabled == true).ToList();

                    }

                }

                var cates = (
                       from a in listCate

                       select new Api.Models.Response.GetListCateByCateSlug
                       {
                           Title = a.Title,
                           RootSlug = rootSlug,
                           CateSlug = a.Slug,
                           URL = "/" + rootSlug + "/" + a.Slug + "/",
                           Images = a.Images,
                           Banner_Alt = SystemInfo.MedidaFileServer + a.FilePath + a.Images,
                           Images_Alt = a.Images_Alt
                       }).ToList();

                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = cates;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<dynamic> GetListTypeProduct(Api.Models.Request.GetListCateByCateSlug input)
        {
            ResponseList<dynamic> rs = new ResponseList<dynamic>();

            try
            {
                long catePid = 0;
                var listCateSlug = new List<SEO.Database.SEO>();
                var listCate = new List<Database.ProductCategory>();


                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = listCate;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<List<Api.Models.Response.GetListColorProduct>> GetListColorProduct()
        {
            ResponseList<List<Api.Models.Response.GetListColorProduct>> rs = new ResponseList<List<Api.Models.Response.GetListColorProduct>>();

            try
            {

                var listColorProduct = new List<Database.ProductSpecificaty>();
                listColorProduct = _db.ProductSpecificaties.Where(p => p.Enabled == true).ToList();
                var colors = (
                         from a in listColorProduct

                         select new Api.Models.Response.GetListColorProduct
                         {
                             Name = a.Name,
                             Code = a.Code,
                             Value = a.Value
                         }).ToList();



                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = colors;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public ResponseList<List<Api.Models.Response.GetListProductSpecificaties>> GetListProductSpecificaties(string group)
        {
            ResponseList<List<Api.Models.Response.GetListProductSpecificaties>> rs = new ResponseList<List<Api.Models.Response.GetListProductSpecificaties>>();

            try
            {

                var listColorProduct = new List<Database.ProductSpecificaty>();
                listColorProduct = _db.ProductSpecificaties.Where(p => p.Enabled == true && (string.IsNullOrEmpty(group) || p.Group == group)).ToList();
                var colors = (
                         from a in listColorProduct

                         select new Api.Models.Response.GetListProductSpecificaties
                         {
                             Name = a.Name,
                             Code = a.Code,
                             Value = a.Value,
                             Group = a.Group,
                         }).ToList();



                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = colors;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<List<Api.Models.Response.GetListProductTypes>> GetListProductTypes()
        {
            Response<List<Api.Models.Response.GetListProductTypes>> rs = new Response<List<Api.Models.Response.GetListProductTypes>>();
            List<Api.Models.Response.GetListProductTypes> productType = new List<Api.Models.Response.GetListProductTypes>();
            try
            {
                var listType = _db.ProductConfigs.Where(p => p.Key == ProductConstants.ConfigAdmin.ProductTypes).FirstOrDefault().Value;
                var listTemp = listType.Split(';');
                foreach (var item in listTemp)
                {
                    var tempType = item.Split(":");
                    if (tempType.Length == 2)
                    {
                        productType.Add(new GetListProductTypes() { Name = tempType[0], Value = tempType[1] });

                    }
                }

                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = productType;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<GetCateDetail> GetCateDetail(Api.Models.Request.GetCateDetail input)
        {
            Response<GetCateDetail> rs = new Response<GetCateDetail>();
            GetCateDetail productType = new GetCateDetail();
            try
            {
                var rootSlug = _db.ProductCategoryConfigs.Where(p => p.Key == ProductCategoryConstants.Config.Website.PreSlugCate).FirstOrDefault().Value;

                if (!String.IsNullOrEmpty(input.CateSlug))
                {
                    var temp = _db.ProductCategories.Where(p => p.Slug == input.CateSlug).FirstOrDefault();
                    var tempSEO = _dbSEO.SEOs.Where(p => p.ModuleCode == ProductCategoryConstants.ModuleInfo.ModuleCode).Where(p => p.PostSlug == input.CateSlug).FirstOrDefault();
                    productType.Slug = temp.Slug;
                    productType.Name = temp.Title;
                    productType.RootSlug = rootSlug;
                    productType.Meta = tempSEO.Meta.ToRemoveBreakSympol();
                    productType.URL = "/" + rootSlug + "/" + temp.Title + "/";
                    productType.Path = "";
                }
                else
                {
                    rootSlug = _db.ProductCategoryConfigs.Where(p => p.Key == ProductCategoryConstants.Config.Website.PreSlugDetail).FirstOrDefault().Value;

                    productType.Slug = "";
                    productType.Name = "Danh sách sản phẩm";
                    productType.URL = "/" + rootSlug + "/";
                    productType.Meta = "";
                    productType.Path = "";
                    productType.RootSlug = rootSlug;

                }

                rs.IsError = false;

                rs.StatusCode = 200;

                rs.Data = productType;
                return rs;

            }
            catch (Exception ex)
            {
                rs.Data = new GetCateDetail();
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        public Response<dynamic> GetListProductDetail()
        {
            Response<dynamic> rs = new Response<dynamic>();
            try
            {
                var colors = _db.ProductSpecificaties.Where(x => x.Group == "COLORS" && x.Enabled && !x.Deleted).ToList();
                var products = _db.Products.Where(x => x.Enabled && !x.Deleted).ToList();
                rs.IsError = false;
                rs.StatusCode = 200;
                rs.Data = new List<dynamic>();
                var model = _db.ProductDetails.Where(x => x.Enabled).ToList();
                foreach (var item in model)
                {
                    var product = products.Where(x => x.Pid == item.ParentPid).FirstOrDefault();
                    if (product is not null)
                    {
                        dynamic d = new ExpandoObject();
                        d.Sku = item.Sku;
                        d.ColorCode = item.ColorCode;
                        d.Color = colors.Where(x => x.Code == item.ColorCode).First().Value ?? "";
                        d.Size = item.Size;
                        d.SellingPrice = item.SellingPrice;
                        d.Title = product.Title;
                        d.Image = SystemInfo.MedidaFileServer + product.FilePath + product.Images;
                        rs.Data.Add(d);
                    }
                }
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
            }
            return rs;
        }

    }

}
