using Steam.Core.Utilities.SteamModels;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface IMailHelper
    {
        void SendMail(string mail, string subject, string htmlBody, MailSettingModel setting);
        void SendMails(string listEmail, string subject, string htmlBody, MailSettingModel setting);
    }
}