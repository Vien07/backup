
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.EmailManagement.Api.Models.Request
{
    public class Request_GetPostBySlug
    {

        public string PostSlug { get; set; }=String.Empty;
    }  
    public class SendEmail
    {

        public string EmailCode { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string[] EmailReceive { get; set; }
    }

    public class SendEmailWithTemplate : SendEmail
    {

        public Dictionary<string, string>? ListKey { get; set; }
    }

}
