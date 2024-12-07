using Microsoft.EntityFrameworkCore;

namespace Steam.Core.Utilities.SteamModels
{
    public class MailSettingModel
    {
        public string SmtpSender { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpPort { get; set; }
        public string EmailAdmin { get; set; }

        public MailSettingModel()
        {
            
        }

        public MailSettingModel(string smtpSender, string smtpServer, string smtpUser, string smtpPassword, string smtpPort, string emailAdmin)
        {
            this.SmtpServer = smtpServer;
            this.SmtpUser = smtpUser;
            this.SmtpPassword = smtpPassword;
            this.SmtpPort = smtpPort;
            this.EmailAdmin = emailAdmin;
            this.SmtpSender = smtpSender;
        }
    }


}