using Microsoft.AspNetCore.Http;
using Admin.Course.Models;
using X.PagedList;
using Admin.Common.Models;
using Admin.Course.Database;
using ComponentUILibrary.Models;

namespace Admin.Course
{
    public interface IMemberRepository
    {
        public Response<IPagedList<Database.Member>> GetList(ParamSearch search);
        public Response<Database.Member> Save(Database.Member data, List<IFormFile> files);
        public Response<Database.Member> GetById(int id);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id, double order);
        public Response<List<Database.MemberConfig>> GetConfig();
        public Response SaveConfig(MemberConfigDto config);

        public Response<List<SelectControlData>> GetCourses();
        public Response<List<Models.CourseMemberViewModel>> GetListCourseMember(int memberPid);
        public Response<Database.Course_Member> SaveCourseMember(Database.Course_Member data);
        public Response<Database.Course_Member> GetCourseMember(int courseMemberPid);
        public Response DeleteCourseMember(int courseMemberPid);
        public Response SendMailCourseMember(int courseMemberPid);
    }
}