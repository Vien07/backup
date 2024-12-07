using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.MemberManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using ComponentUILibrary.Models;

namespace Admin.MemberManagement.Services
{
    public interface IFeedbackService
    {
        public Response<Feedback_List> GetList(FeedbackModel.ParamSearch search);
        public Response<FeedbackModel.FeedbackDetail> GetById(int id);
        public Response<Database.Feedback> Save(FeedbackModelEdit data);
        public Response Delete(List<int> ids);

    }
}