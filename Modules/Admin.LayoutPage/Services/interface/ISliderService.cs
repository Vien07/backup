using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface ISliderService
    {
         public Response<IPagedList<Database.Slider>> GetList(ParamSearch search);
        public Response<Database.Slider> GetById(int id);
        public Response<Database.SliderItem> GetChildById(int id);
        public Response<Database.Slider> Save(SliderModelEdit data );
        public Response<Database.SliderItem> SaveChild(SliderItemModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);
        public Response MoveChild(int fromId, int toId);

        public Response<List<Database.SliderItem>> GetChildList(long? Pid);
        public Response<string> GenerateSliderHtml(long pid);

    }
}