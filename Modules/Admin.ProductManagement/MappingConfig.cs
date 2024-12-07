using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.MisaApiTracker;
using Admin.ProductManagement.DataTransferObjects.MisaReponse;
using Admin.ProductManagement.DataTransferObjects.OrderManagement;
using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using Admin.ProductManagement.DataTransferObjects.ProductPolicy;
using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.UpdateModels;
using AutoMapper;
using Steam.Core.Common.SteamString;

namespace Admin.ProductManagement
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                #region MisaApiTracker
                config.CreateMap<MisaApiTracker, MisaApiTrackerDto>();
                config.CreateMap<MisaApiTrackerDto, MisaApiTracker>();
                config.CreateMap<MisaApiConfig, MisaApiConfigDto>();
                config.CreateMap<MisaApiConfigDto, MisaApiConfig>();
                #endregion

                #region ProductPolicy
                config.CreateMap<ProductPolicyDto, ProductPolicy>();
                config.CreateMap<ProductPolicy, ProductPolicyDto>();
                config.CreateMap<ProductPolicySaveModel, ProductPolicy>();
                config.CreateMap<ProductPolicyConfig, ProductPolicyConfigDto>();
                config.CreateMap<ProductPolicyConfigDto, ProductPolicyConfig>();
                #endregion

                #region Product
                config.CreateMap<Product, ProductDto>();
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<ProductConfig, ProductConfigDto>();
                config.CreateMap<ProductConfigDto, ProductConfig>();
                config.CreateMap<ProductSaveModel, Product>()
                    .ForMember(des => des.Enabled, act => act.MapFrom(src => src.CheckBox == "on" ? true : false));
                config.CreateMap<ProductSaveModel, ProductUpdateModel>();
                config.CreateMap<ProductUpdateModel, Product>();

                config.CreateMap<MisaResponseProductDto, Product>()
                    .ForMember(des => des.Sku, act => act.MapFrom(src => src.Code))
                    .ForMember(des => des.Title, act => act.MapFrom(src => src.Name))
                    .ForMember(des => des.Slug, act => act.MapFrom(src => src.Name.ToSlug()))
                    .ForMember(des => des.PublishDate, act => act.MapFrom(src => DateTime.Now))
                    .ForMember(des => des.Content, act => act.MapFrom(src => src.Description))
                    .ForMember(des => des.UnitID, act => act.MapFrom(src => src.UnitId))
                    .ForMember(des => des.UpdateDate, act => act.MapFrom(src => src.ModifiedDate))
                    .ForMember(des => des.MisaProductID, act => act.MapFrom(src => src.Id))
                    .ForMember(des => des.CateID, act => act.MapFrom(src => 0));

                config.CreateMap<MisaResponseProductDetailDto, ProductDetail>()
                    .ForMember(des => des.Sku, act => act.MapFrom(src => src.Code))
                    .ForMember(des => des.Title, act => act.MapFrom(src => src.Name))
                    .ForMember(des => des.ColorCode, act => act.MapFrom(src => src.ColourCode))
                    .ForMember(des => des.ParentPid, act => act.MapFrom(src => DateTime.Now))
                    .ForMember(des => des.MisaProductID, act => act.MapFrom(src => src.Id));
                #endregion

                #region Product Spec
                config.CreateMap<ProductSpecificaty, ProductSpecificatyDto>();
                config.CreateMap<ProductSpecificatyDto, ProductSpecificaty>();
                config.CreateMap<ProductSpecificatyConfig, ProductSpecificatyConfigDto>();
                config.CreateMap<ProductSpecificatyConfigDto, ProductSpecificatyConfig>();

                config.CreateMap<ProductSpecificatySaveModel, ProductSpecificaty>();
                config.CreateMap<ProductSpecificatySaveModel, ProductSpecificatyUpdateModel>();
                config.CreateMap<ProductSpecificatyUpdateModel, ProductSpecificaty>();
                #endregion

                #region ProductCategory
                config.CreateMap<ProductCategory, ProductCategoryDto>();
                config.CreateMap<ProductCategoryDto, ProductCategory>();

                config.CreateMap<ProductCategoryConfig, ProductCategoryConfigDto>();
                config.CreateMap<ProductCategoryConfigDto, ProductCategoryConfig>();

                config.CreateMap<ProductCategorySaveModel, ProductCategory>();
                config.CreateMap<ProductCategorySaveModel, ProductCategoryUpdateModel>();
                config.CreateMap<ProductCategoryUpdateModel, ProductCategory>();
                #endregion

                #region OrderManagement
                config.CreateMap<OrderManagement, OrderManagementDto>();
                config.CreateMap<OrderManagementDto, OrderManagement>();

                config.CreateMap<OrderManagementConfig, OrderManagementConfigDto>();
                config.CreateMap<OrderManagementConfigDto, OrderManagementConfig>();
                #endregion
            });

            return mappingConfig;
        }
    }
}
