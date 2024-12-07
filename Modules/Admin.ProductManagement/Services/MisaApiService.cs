using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using System.Text.Json;
using Admin.ProductManagement.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Admin.ProductManagement.DTOs;
using Microsoft.EntityFrameworkCore;
using Admin.SEO.Database;
using System.Dynamic;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Models.ViewModels.MisaApiTracker;
using Admin.ProductManagement.DataTransferObjects.MisaReponse;

namespace Admin.ProductManagement.Services
{
    public class MisaApiService : IMisaApiService
    {
        private ILoggerHelper _logger;
        IMisaRestHelper _restHelper;

        Dictionary<string, string> _config;
        ProductManagementContext _db;

        SEOContext _dbSEO;

        public MisaApiService(ProductManagementContext db, IFileHelper fileHelper, ILoggerHelper logger, IMisaRestHelper restHelper, IMisaApiTrackerHelper trackHelper)
        {
            _db = db;
            _logger = logger;
            _restHelper = restHelper;

            _config = _db.MisaApiConfigs.Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t => t.Value);
        }
        public string computeHmacSha256(string message, string secretKey)
        {
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(secretKeyBytes))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
        public MisaResponseModel<MisaResponseAccessTokenDto> GenerateAccessToken()
        {
            try
            {
                var appId = _config[MisaApiTrackerConstants.AppID];
                var secrectKey = _config[MisaApiTrackerConstants.SecretKey];
                var domain = _config[MisaApiTrackerConstants.Domain];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];

                var loginTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");

                //setup param login info
                var loginInfo = new Dictionary<string, string>();
                loginInfo.Add("AppID", appId);
                loginInfo.Add("Domain", domain);
                loginInfo.Add("LoginTime", loginTime);

                //get signature info
                var message = JsonSerializer.Serialize(loginInfo);
                var signatureInfo = computeHmacSha256(message, secrectKey);

                var dictParam = new Dictionary<string, string>();
                dictParam.Add("AppID", appId);
                dictParam.Add("Domain", domain);
                dictParam.Add("SignatureInfo", signatureInfo);
                dictParam.Add("LoginTime", loginTime);

                MisaResponseModel<MisaResponseAccessTokenDto> resp = _restHelper.POST<MisaResponseAccessTokenDto>("GetAccessToken", baseUrl, "/auth/api/account/login", null, dictParam);

                if (resp.Success)
                {
                    //save config

                    var newConfig = new MisaApiConfigViewModel()
                    {
                        SignatureInfo = signatureInfo,
                        AccessToken = resp.Data.AccessToken,
                        LoginTime = loginTime,
                        CompanyCode = resp.Data.CompanyCode,
                        Environment = resp.Data.Environment,
                    };
                    foreach (PropertyInfo propertyInfo in newConfig.GetType().GetProperties())
                    {
                        Database.MisaApiConfig MisaProductConfig = _db.MisaApiConfigs.Where(p => p.Key.Equals(propertyInfo.Name)).FirstOrDefault();
                        if (MisaProductConfig is not null)
                        {
                            var value = propertyInfo.GetValue(newConfig, null);
                            if (value is not null)
                            {
                                MisaProductConfig.Value = value.ToString();
                                MisaProductConfig.UpdateDate = DateTime.Now;
                                MisaProductConfig.UpdateUser = "admin";
                                _db.SaveChanges();
                            }
                        }
                    }

                }

                return resp;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return null;
            }
        }
        public MisaResponseModel<List<MisaResponseCategoryDto>> GetCategoryList()
        {
            try
            {
                var environment = _config[MisaApiTrackerConstants.Environment];
                var accessToken = _config[MisaApiTrackerConstants.AccessToken];
                var companyCode = _config[MisaApiTrackerConstants.CompanyCode];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];

                var dictHeader = new Dictionary<string, string>()
                {
                    {"CompanyCode",companyCode },
                    {"Authorization",string.Format("Bearer {0}", accessToken) }
                };

                var dictParam = new Dictionary<string, string>();
                dictParam.Add("includeInactive", "true");

                var urlRequest = string.Format("/{0}/api/v1/categories/list", environment);

                MisaResponseModel<List<MisaResponseCategoryDto>> resp = _restHelper.GET<List<MisaResponseCategoryDto>>("GetCategoryList", baseUrl, urlRequest, dictHeader, dictParam);

                return resp;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return null;
            }
        }
        public MisaResponseModel<List<MisaResponseBranchDto>> GetBranchList()
        {
            try
            {
                var environment = _config[MisaApiTrackerConstants.Environment];
                var accessToken = _config[MisaApiTrackerConstants.AccessToken];
                var companyCode = _config[MisaApiTrackerConstants.CompanyCode];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];

                var dictHeader = new Dictionary<string, string>()
                {
                    {"CompanyCode",companyCode },
                    {"Authorization",string.Format("Bearer {0}", accessToken) }
                };

                var dictParam = new Dictionary<string, string>();
                dictParam.Add("includeInactive", "true");
                dictParam.Add("IsIncludeChainOfBranch", "false");

                var urlRequest = string.Format("/{0}/api/v1/branchs/all", environment);

                MisaResponseModel<List<MisaResponseBranchDto>> resp = _restHelper.POST<List<MisaResponseBranchDto>>("GetBranchList", baseUrl, urlRequest, dictHeader, dictParam);

                if (resp.Code == (int)HttpStatusCode.OK)
                {
                    foreach (var item in resp.Data)
                    {
                        var model = _db.MisaBranchs.FirstOrDefault(x => x.MisaBranchID == item.Id);
                        if (model is null)
                        {
                            var newBranch = new Database.MisaBranch();
                            newBranch.MisaBranchID = item.Id;
                            newBranch.IsChainBranch = item.IsChainBranch;
                            newBranch.IsBaseDepot = item.IsBaseDepot;
                            newBranch.Code = item.Code;
                            newBranch.Title = item.Name;
                            newBranch.Address = item.Address;
                            newBranch.CommuneAddr = item.CommuneAddr;
                            newBranch.DistrictAddr = item.DistrictAddr;
                            newBranch.ProvinceAddr = item.ProvinceAddr;
                            newBranch.CreateUser = "admin";
                            newBranch.UpdateUser = "admin";
                            _db.MisaBranchs.Add(newBranch);
                            _db.SaveChanges();
                        }
                        else
                        {
                            model.IsChainBranch = item.IsChainBranch;
                            model.IsBaseDepot = item.IsBaseDepot;
                            model.Code = item.Code;
                            model.Title = item.Name;
                            model.Address = item.Address;
                            model.CommuneAddr = item.CommuneAddr;
                            model.DistrictAddr = item.DistrictAddr;
                            model.ProvinceAddr = item.ProvinceAddr;
                            _db.SaveChanges();
                        }
                    }
                }

                return resp;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return null;
            }
        }
        public MisaResponseModel<List<MisaResponseProductDto>> GetProductList(DateTime lastSyncDate, int pageIndex, int pageSize)
        {
            try
            {
                var environment = _config[MisaApiTrackerConstants.Environment];
                var accessToken = _config[MisaApiTrackerConstants.AccessToken];
                var companyCode = _config[MisaApiTrackerConstants.CompanyCode];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];

                var dictHeader = new Dictionary<string, string>()
                {
                    {"CompanyCode",companyCode },
                    {"Authorization",string.Format("Bearer {0}", accessToken) }
                };

                var dictParam = new Dictionary<string, string>();
                dictParam.Add("Page", pageIndex.ToString());
                dictParam.Add("Limit", pageSize.ToString());
                dictParam.Add("SortField", "Code");
                dictParam.Add("SortType", "1");
                dictParam.Add("IncludeInventory", "true");
                //dictParam.Add("InventoryItemCategoryID", "dec3e6b9-57fb-4084-903d-c7772d240395");
                dictParam.Add("LastSyncDate", lastSyncDate.ToString("yyyy-MM-dd HH:mm:ss"));

                var urlRequest = string.Format("/{0}/api/v1/inventoryitems/pagingwithdetail", environment);

                MisaResponseModel<List<MisaResponseProductDto>> resp = _restHelper.POST<List<MisaResponseProductDto>>("GetProductList", baseUrl, urlRequest, dictHeader, dictParam);

                return resp;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return null;
            }
        }
        public async Task<Response<Api.Models.Response.GetMisaProductBySlug>> GetPostBySlug(string postSlug)
        {
            Response<Api.Models.Response.GetMisaProductBySlug> rs = new Response<Api.Models.Response.GetMisaProductBySlug>();
            rs.Data = new Api.Models.Response.GetMisaProductBySlug();
            try
            {
                var model = await _db.Products.Where(p => p.Slug.Equals(postSlug) && !p.Deleted && p.Enabled).FirstOrDefaultAsync();

                if (model is null)
                {
                    rs.IsError = true;
                    rs.StatusCode = 404;
                    rs.Message = "No resources found";
                    return rs;
                }
                rs.Data.Parent = model;
                rs.Data.Children = await (from a in _db.ProductDetails
                                          join b in _db.ProductSpecificaties on a.ColorCode equals b.Code into product_color
                                          from c in product_color.DefaultIfEmpty()
                                          where a.ParentPid == model.Pid
                                          select new Database.ProductDetail
                                          {
                                              Pid = a.Pid,
                                              Title = a.Title,
                                              Color = c.Value,
                                              Sku = a.Sku,
                                              Size = a.Size,
                                              SellingPrice = a.SellingPrice,
                                              ColorCode = a.ColorCode,
                                          }).ToListAsync();

                rs.IsError = false;
                rs.StatusCode = 200;
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
        public async Task<Response<bool>> CreateMisaOrder(Api.Models.Request.CreateMisaOrder request)
        {
            Response<bool> rs = new Response<bool>();

            try
            {

                dynamic param = new ExpandoObject();
                param.OrderDate = request.OrderDate.ToString("yyyy-MM-ddTHH:mm:sszzz");
                param.ToDistrictId = request.District;
                param.ToDistrictName = request.DistrictName;
                param.ToWardOrCommuneId = request.Ward;
                param.ToWardOrCommuneName = request.WardName;
                param.ToProvinceOrCityId = request.City;
                param.ToProvinceOrCityName = request.CityName;
                param.ScopeOfApplication = 1;
                param.EditMode = 1;
                var getBranchDefault = _db.MisaBranchs.FirstOrDefault();
                param.BranchId = getBranchDefault.MisaBranchID;
                param.OrderDelivery = new ExpandoObject();
                param.OrderDelivery.Receiver = new ExpandoObject();
                param.OrderDelivery.Receiver.Name = request.FullName;
                param.OrderDelivery.Receiver.Tel = request.PhoneNumber;
                param.OrderDelivery.Receiver.Email = request.Email;
                param.OrderDelivery.Receiver.Address = request.Address;
                param.OrderDetails = new List<dynamic>();
                var stt = 1;
                decimal totalAmount = 0;
                foreach (var orderDetail in request.Details)
                {
                    var product = await _db.ProductDetails.Where(x => !x.Deleted && x.Sku == orderDetail.Code).FirstOrDefaultAsync();
                    var productParent = await _db.Products.Where(x => x.Pid == product.ParentPid).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        decimal total = Math.Round(orderDetail.Quantity * product.SellingPrice, 2);
                        param.OrderDetails.Add(new
                        {
                            ProductId = product.MisaProductID,
                            Quantity = orderDetail.Quantity,
                            ProductCode = orderDetail.Code,
                            ProductName = product.Title,
                            SellingPrice = product.SellingPrice,
                            ProductType = 1,
                            Amount = total,
                            DiscountAmount = 0M,
                            DiscountRate = 0M,
                            UnitId = productParent.UnitID,
                            SortOrder = stt,
                        });
                        totalAmount += total;
                        stt++;
                    }
                }
                param.TotalAmount = totalAmount;

                var orderManagement = new OrderManagement();
                orderManagement.OrderNo = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                orderManagement.OrderDate = DateTime.Now;
                orderManagement.FullName = request.FullName;
                orderManagement.OrderStatus = "Pending";
                orderManagement.PaymentStatus = "Pending";
                orderManagement.Email = request.Email;
                orderManagement.PhoneNumber = request.PhoneNumber;
                orderManagement.Address = $"{request.Address}, {request.WardName}, {request.DistrictName}, {request.CityName}";
                orderManagement.Provider = "Misa";
                orderManagement.Amount = totalAmount;
                orderManagement.Data = JsonSerializer.Serialize(param);
                orderManagement.CreateUser = "admin";
                orderManagement.UpdateUser = "admin";
                await _db.OrderManagements.AddAsync(orderManagement);
                await _db.SaveChangesAsync();

                foreach (var orderDetail in request.Details)
                {
                    var product = await _db.ProductDetails.Where(x => !x.Deleted && x.Sku == orderDetail.Code).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        var productParent = await _db.Products.Where(x => x.Pid == product.ParentPid).FirstOrDefaultAsync();
                        var newOrderDetail = new OrderDetail();
                        newOrderDetail.OrderManagementPid = orderManagement.Pid;
                        newOrderDetail.Sku = orderDetail.Code;
                        newOrderDetail.ProductName = product.Title;
                        newOrderDetail.Price = product.SellingPrice;
                        newOrderDetail.Quantity = orderDetail.Quantity;
                        newOrderDetail.DiscountPrice = 0M;
                        newOrderDetail.DiscountRate = 0M;
                        newOrderDetail.CreateUser = "admin";
                        newOrderDetail.UpdateUser = "admin";
                        await _db.OrderDetails.AddAsync(newOrderDetail);
                        await _db.SaveChangesAsync();
                    }
                }

                var environment = _config[MisaApiTrackerConstants.Environment];
                var accessToken = _config[MisaApiTrackerConstants.AccessToken];
                var companyCode = _config[MisaApiTrackerConstants.CompanyCode];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];
                var dictHeader = new Dictionary<string, string>()
                {
                    {"CompanyCode",companyCode },
                    {"Authorization",string.Format("Bearer {0}", accessToken) }
                };

                var urlRequest = string.Format("/{0}/api/v1/invoices", environment);
                MisaResponseModel<dynamic> resp = _restHelper.POST("CreateOrder", baseUrl, urlRequest, dictHeader, param);
                rs.IsError = false;
                rs.Data = resp.Success;
                rs.StatusCode = 200;
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
        public MisaResponseModel<List<MisaResponseAddressDto>> GetMisaAddress(string kind, string parentId)
        {
            try
            {
                var environment = _config[MisaApiTrackerConstants.Environment];
                var accessToken = _config[MisaApiTrackerConstants.AccessToken];
                var companyCode = _config[MisaApiTrackerConstants.CompanyCode];
                var baseUrl = _config[MisaApiTrackerConstants.BaseUrl];

                var dictHeader = new Dictionary<string, string>()
                {
                    {"CompanyCode",companyCode },
                    {"Authorization",string.Format("Bearer {0}", accessToken) }
                };
                var urlRequest = $"{baseUrl}/{environment}/api/v1/locations/bykindandparentid?kind={kind}&parentId={parentId}";

                MisaResponseModel<List<MisaResponseAddressDto>> resp = _restHelper.GET<List<MisaResponseAddressDto>>("GetMisaAddress", baseUrl, urlRequest, dictHeader, null);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");
                return null;
            }
        }
    }
}
