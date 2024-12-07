namespace Admin.MemberManagement.Api.Models.Request
{
    public class LoginAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterAccount : LoginAccount
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailCode { get; set; }
        public Dictionary<string, string>? ListKey { get; set; }
    }

    public class ChangePasswordAccount : LoginAccount
    {
        public string NewPassword { get; set; }
    }

    public class FeedbackRequest 
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SKU { get; set; }
        public string Rating { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
