using Microsoft.AspNetCore.Http;
using Admin.Course.Models;
using Admin.Course.Database;
using X.PagedList;
using Admin.Common.Models;
using ComponentUILibrary.Models;

namespace Admin.Course
{
    public interface ICourseRepository
    {
        public Response<IPagedList<Database.Course>> GetList(ParamSearch search);
        public Response<Admin.Course.Database.Course> GetById(int id);
        public Response<string> GetCateById(int id);
        public Response<List<SelectControlData>> GetDataCategories();
        public Response<List<SelectControlData>> GetLectures();
        public Response<Database.Course> Save(Database.Course data, List<IFormFile> files, string categories);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id, double order);

        public Response<List<CourseConfig>> GetConfig();
        public Response SaveConfig(CourseConfigDto config);

        public Response<Root> GetLecture(int id);
        public Response SaveLecture(Root data);
    }
}