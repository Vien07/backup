using Admin.Collection.Database;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Admin.Collection.Constants;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Admin.PostsCategory.Database;
using Admin.Collection.Api.Models.Response;
using Steam.Core.Common;
using static Admin.Collection.Api.Models.Response.GetDetailCollection;
using Admin.ProductManagement.Services;

namespace Admin.Collection
{
    public class ApiCollectionRepository : IApiCollectionRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        CollectionContext _db;
        SEOContext _dbSEO;
        IProductCollectionService _repMSProduct;
        PostsCategoryContext _dbCate;
        public ApiCollectionRepository(CollectionContext db, PostsCategoryContext dbCate,
            SEOContext dbSEO, IFileHelper fileHelper, IProductCollectionService repMSProduct, ILoggerHelper logger)
        {
            _repMSProduct = repMSProduct;
            _db = db;
            _dbSEO = dbSEO;
            _dbCate = dbCate;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _db.CollectionConfigs.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);

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
        public ResponseList<List<GetDetailCollection>> GetListProductWithCollection()
        {
            ResponseList<List<GetDetailCollection>> rs = new ResponseList<List<GetDetailCollection>>();
            List<GetDetailCollection> collections = new List<GetDetailCollection>();
            try
            {
                List<Product_Item> listProducts = new List<Product_Item>();

                var tempCollection = _db.Collections.Where(p => p.Enabled == true).ToList();
                foreach (var item in tempCollection)
                {
                    var tempSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == CollectionConstants.ModuleInfo.ModuleCode && p.PostPid == item.Pid).FirstOrDefault();
                    if (tempSeo != null)
                    {
                        collections.Add(GetDetailCollection(tempSeo.PostSlug).Data);


                    }
                }



                rs.Data = collections;
            }
            catch (Exception ex)
            {

            }
            return rs;
        }
        public Response<List<GetListCollectionName>> GetListCollectionName()
        {
            Response<List<GetListCollectionName>> rs = new Response<List<GetListCollectionName>>();

            try
            {
                var collections = _db.Collections.Where(p => p.Enabled == true).ToList();
                var config = _db.CollectionConfigs.Where(p => p.Key == CollectionConstants.Config.Website.PreSlug).FirstOrDefault();
                var listSeos = _dbSEO.SEOs.Where(p => p.ModuleCode == CollectionConstants.ModuleInfo.ModuleCode).ToList();
                var rsCol = (from a in collections
                             join b in listSeos on a.Pid equals b.PostPid
                             select new GetListCollectionName
                             {
                                 Title = a.Title,
                                 RootSlug = config.Value,
                                 Order = a.Order,
                                 CateSlug = b.PostSlug
                             }).ToList();
                rs.Data = rsCol;
            }
            catch (Exception ex)
            {

            }
            return rs;
        }
        public Response<GetDetailCollection> GetDetailCollection(string slug)
        {
            Response<GetDetailCollection> rs = new Response<GetDetailCollection>();

            try
            {
                List<Product_Item> listProducts = new List<Product_Item>();
                var tempSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == CollectionConstants.ModuleInfo.ModuleCode).FirstOrDefault();

                if (!string.IsNullOrEmpty(slug))
                {
                    tempSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == CollectionConstants.ModuleInfo.ModuleCode).Where(p => p.PostSlug == slug).FirstOrDefault();

                }
                if (tempSeo != null)
                {
                    var tempCollection = _db.Collections.Where(p => p.Enabled == true && p.Pid == tempSeo.PostPid).FirstOrDefault();
                    listProducts = _repMSProduct.GetListProductByListSKU(tempCollection.ProductIDs).DeepClone<List<Product_Item>>();

                    var listImage = _db.Collection_Files.Where(p => p.CollectionId == tempCollection.Pid).ToList();
                    GetDetailCollection data = new GetDetailCollection();
                    data.Title = tempCollection.Title;
                    data.ImagePath = SystemInfo.MedidaFileServer + tempCollection.FilePath + tempCollection.Images;
                    data.Images_Alt = tempCollection.Images_Alt;
                    data.Meta = tempSeo.Meta.ToRemoveBreakSympol();
                    data.ListImages = listImage;
                    data.Pid = tempCollection.Pid;
                    data.ListProducts = listProducts;


                    rs.Data = data;
                }
            }
            catch (Exception ex)
            {

            }
            return rs;
        }
    }

}
