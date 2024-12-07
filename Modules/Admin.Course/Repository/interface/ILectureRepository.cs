using Microsoft.AspNetCore.Http;
using Admin.Course.Models;
using X.PagedList;
using Admin.Common.Models;

namespace Admin.Course
{
    public interface ILectureRepository
    {
        public Response<IPagedList<Database.Lecture>> GetList(ParamSearch search);
        public Response<Database.Lecture> GetById(int id);
        public Response<Database.Lecture> Save(Database.Lecture data, List<IFormFile> files);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id, double order);
        public Response<List<Database.LectureConfig>> GetConfig();
        public Response SaveConfig(LectureConfigDto config);
    }
}