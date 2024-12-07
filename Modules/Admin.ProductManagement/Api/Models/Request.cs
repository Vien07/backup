using MailKit.Search;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Admin.ProductManagement.Api.Models.Request
{
    public class GetProductsManagementBySlug
    {

    }
    #region cate
    public class GetProductBySlug
    {

        public string PostSlug { get; set; } = String.Empty;
    }
    public class GetListProductsByCateSlug
    {

        public string RootSlug { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
        public string? Color { get; set; } = String.Empty;
        public string? Size { get; set; } = String.Empty;
        public string? SortBy { get; set; } = String.Empty;
        public string? KeySearch { get; set; } = String.Empty;
        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = (value <= 0) ? 1 : value;
        }

        public int PageSize { get; set; } = 10;
        public bool FilterType(string typeProduct)
        {
            try
            {
                if (String.IsNullOrEmpty(typeProduct))
                {
                    return true;

                }
                var temp = typeProduct.Split(',');
                var tempSearch = Type.Split('-');
                bool searchTermExists = tempSearch.Any(t => temp.Contains(t));

                return searchTermExists;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool FilterSize(string tempSize)
        {
            try
            {
                if (String.IsNullOrEmpty(tempSize))
                {
                    return true;

                }
                var temp = tempSize.Split(',');
                var tempSearch = Size.Split('-');
                bool searchTermExists = tempSearch.Any(t => temp.Contains(t));

                return searchTermExists;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool FilterColor(string tempColor)
        {
            try
            {
                if (String.IsNullOrEmpty(tempColor))
                {
                    return true;

                }

                var temp = tempColor.Split(',');
                var tempSearch = Color.Split('-');
                bool searchTermExists = tempSearch.Any(t => temp.Contains(t));

                return searchTermExists;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
    public class GetListProductsBySlug
    {

        public string RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class GetListNewProductsByCateSlug
    {

        public string RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
        public int TakeItem { get; set; } = 8;

    }
    public class GetListRelateProductsByProductSlug
    {

        public string RootSlug { get; set; } = String.Empty;
        public string? PostSlug { get; set; } = String.Empty;
        public int TakeItem { get; set; } = 8;
    }
    public class GetListCateByCateSlug
    {

        public string? RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
    }
    public class GetCateDetail
    {

        public string? RootSlug { get; set; } = String.Empty;
        public string? CateSlug { get; set; } = String.Empty;
    }
    public class GetListColorProduct : Database.ProductSpecificaty
    {


    }
    public class GetListProductsByType
    {

        public string Type { get; set; } = String.Empty;
        public int TakeItem { get; set; } = 8;
    }
    #endregion

    public class CreateMisaOrder
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string District { get; set; }
        public string DistrictName { get; set; }
        public string Ward { get; set; }
        public string WardName { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CreateMisaOrder_Detail> Details { get; set; }
        public class CreateMisaOrder_Detail
        {
            public string Code { get; set; }
            public int Quantity { get; set; }
            public int SortOrder { get; set; }
        }
    }



}
