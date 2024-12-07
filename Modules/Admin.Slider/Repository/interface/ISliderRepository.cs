using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Slider.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.Slider
{
    public interface ISliderRepository
    {
         public Response<IPagedList<Database.Slider>> GetList(ParamSearch search);
        public Response<SliderDetail> GetById(int id);
        public Response<Slider.Database.Slider> Save(SliderModelEdit input);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<Slider.Database.SliderConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<Slider.Database.SliderConfig>> GetAllConfigs();
        public Response<Slider.Database.SliderConfig> GetConfigByKey(string key);

    }
}