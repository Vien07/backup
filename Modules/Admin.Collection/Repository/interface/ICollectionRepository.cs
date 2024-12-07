using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Collection.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using ComponentUILibrary.Models;

namespace Admin.Collection
{
    public interface ICollectionRepository
    {
        public Response<Collection_List> GetList(CollectionModel.ParamSearch search);
        public Response<CollectionModel.CollectionDetail> GetById(int id);
        public Response<Database.Collection> Save(CollectionModelEdit data);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<Database.CollectionConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<Database.CollectionConfig>> GetAllConfigs();
        public Response<Database.CollectionConfig> GetConfigByKey(string key);
        public List<SelectControlData> GetCatesForCollection();
        public List<Admin.PostsCategory.Database.PostsCategory> GetChildrenPostCategory(long parentId);
        public List<Collection_Product_Item> GetProductOfCollection(string sku);
        public IPagedList<Collection_Product_Item> GetListProductOfCollection(long pid);
        public Collection_Product_List GetListProducts(Collection_Product_List.ParamSearch input);

    }
}