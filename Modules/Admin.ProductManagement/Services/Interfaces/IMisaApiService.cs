using Steam.Core.Base.Models;
using Admin.ProductManagement.DTOs;
using Admin.ProductManagement.DataTransferObjects.MisaReponse;

namespace Admin.ProductManagement.Services
{
    public interface IMisaApiService
    {
        MisaResponseModel<MisaResponseAccessTokenDto> GenerateAccessToken();
        MisaResponseModel<List<MisaResponseCategoryDto>> GetCategoryList();
        MisaResponseModel<List<MisaResponseBranchDto>> GetBranchList();
        MisaResponseModel<List<MisaResponseProductDto>> GetProductList(DateTime lastSyncDate, int pageIndex, int pageSize);
        MisaResponseModel<List<MisaResponseAddressDto>> GetMisaAddress(string kind, string parentId);

        Task<Response<Api.Models.Response.GetMisaProductBySlug>> GetPostBySlug(string postSlug);
        Task<Response<bool>> CreateMisaOrder(Api.Models.Request.CreateMisaOrder request);
    }
}